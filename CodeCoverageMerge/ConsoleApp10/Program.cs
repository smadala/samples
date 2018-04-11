using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Coverage.Analysis;

namespace ConsoleApp10
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 0;
            while (true)
            {
                var cmd = Console.ReadLine();
                if (cmd.StartsWith("q"))
                {
                    break;
                }

                Stopwatch stopwatch = Stopwatch.StartNew();
                var covFilesdir = @"C:\Users\samadala\src\vstest\cc2";
                string[] covFiles = Directory.GetFiles(covFilesdir);
                Console.WriteLine("Total files: " + covFiles.Length);
                string resultedFile = null;
                if (cmd.StartsWith("d"))
                {
                    resultedFile = OnDiskMerge(covFilesdir, covFiles);
                }
                else if (cmd.StartsWith("x"))
                {
                    resultedFile = OnDiskMergeInParallel(covFilesdir, covFiles);
                }
                else if (cmd.StartsWith("m"))
                {
                    resultedFile = InMemoryMerge(covFilesdir, covFiles, false);
                }
                else
                {
                    resultedFile = InMemoryMerge(covFilesdir, covFiles, true);
                }
                Console.WriteLine("Resulted file: " + resultedFile);
                stopwatch.Stop();
                var timeTaken = stopwatch.Elapsed.ToString().Replace(":", "-");
                Console.WriteLine("Operation: "+ cmd +"Time taken: " + timeTaken);
                File.Create(resultedFile + "-" + timeTaken).Close();
            }
        }

        public static string OnDiskMerge(string covFilesdir, string[] covFiles)
        {
            var resultedFile = Path.Combine(Path.GetDirectoryName(covFilesdir), "OnDisk-" + covFiles.Length + "-" + Guid.NewGuid().ToString().Substring(0, 5)+".coverage");
            if (covFiles.Length < 2)
            {
                Console.WriteLine("To merge give atleast two cov files");
                return "";
            }
            string first = covFiles[0], second = covFiles[1];
            for (int i = 1; i < covFiles.Length; i++)
            {
                var tmpFile =  Path.Combine(Path.GetDirectoryName(resultedFile), Path.GetFileNameWithoutExtension(resultedFile) + "-"+ i + Path.GetExtension(resultedFile));
                if (i == covFiles.Length - 1)
                {
                    tmpFile = resultedFile;
                }
                Console.WriteLine("Creating merge file: "+ tmpFile);
                CoverageInfo.MergeCoverageFiles(first, second, tmpFile, true);
                first = tmpFile;
                if (i != covFiles.Length - 1)
                {
                    second = covFiles[i + 1];
                }
            }
            CoverageSummary.PrintCoverageInfo(resultedFile);
            return resultedFile;
        }

        public static string OnDiskMergeInParallel(string covFilesdir, string[] covFiles)
        {
            ConcurrentQueue<string> covFilesQueue = new ConcurrentQueue<string>(covFiles);
            var resultedFile = Path.Combine(Path.GetDirectoryName(covFilesdir), "OnDisk-" + covFiles.Length + "-result-" + Guid.NewGuid().ToString().Substring(0, 5) + ".coverage");
            var parallelLevel = GetNumOfCores();
            Task[] tasks = new Task[parallelLevel];
            for (int i = 0; i < parallelLevel; i++)
            {
                Console.WriteLine("Creating task: " + i);
                tasks[i] = Task.Run(() => OnDiskWork(covFilesQueue, covFilesdir, covFiles.Length));
            }
            Task.WaitAll(tasks);
            if (covFilesQueue.Count > 1)
            {
                Console.WriteLine("Number remaining files in queue:" + covFilesQueue.Count);
                OnDiskWork(covFilesQueue, covFilesdir, covFiles.Length);
            }

            Debug.Assert(covFilesQueue.Count == 1, "Only one cov file should be there");
            string finalFile;
            covFilesQueue.TryDequeue(out finalFile);
            File.Move(finalFile, resultedFile);
            CoverageSummary.PrintCoverageInfo(resultedFile);
            return resultedFile;
        }
        private static void OnDiskWork(ConcurrentQueue<string> covFilesQueue, string covFilesdir, int numOfCovFiles)
        {
            var threadId = "ThreadId " + Thread.CurrentThread.ManagedThreadId +" ";
            var resultedFile = Path.Combine(Path.GetDirectoryName(covFilesdir), "OnDisk-" + numOfCovFiles + "-" + Guid.NewGuid().ToString().Substring(0, 5) + ".coverage");
            if (numOfCovFiles < 2)
            {
                Console.WriteLine(threadId + "To merge give atleast two cov files");
            }

            string first=null, second=null;
            covFilesQueue.TryDequeue(out first);
            covFilesQueue.TryDequeue(out second);
            while(first != null && second != null)
            {
                var tmpFile = Path.Combine(Path.GetDirectoryName(resultedFile), Path.GetFileNameWithoutExtension(resultedFile) + "-" + Guid.NewGuid().ToString().Substring(0, 5) + Path.GetExtension(resultedFile));
                Console.WriteLine(threadId + "Creating merge file: " + tmpFile);
                CoverageInfo.MergeCoverageFiles(first, second, tmpFile, true);
                first = tmpFile;
                covFilesQueue.TryDequeue(out second);
            }
            if(first != null)
            {
                covFilesQueue.Enqueue(first);
            }
            if(second != null)
            {
                covFilesQueue.Enqueue(second);
            }
        }
        public static string InMemoryMerge(string covFilesdir, string[] covFiles, bool parallel)
        {
            CoverageInfo _covinfo = null;
            if (!parallel)
            {
                _covinfo = CoverageInfo.CreateFromFile(covFiles[0], null, null);
                int batchmergefiles = covFiles.Length - 1;
                for (int i = 0; i < batchmergefiles; i++)
                {
                    Console.WriteLine("Merging file " + covFiles[i + 1]);
                    CoverageInfo covinfo2 = CoverageInfo.CreateFromFile(covFiles[i + 1], null, null);
                    CoverageInfo cs3 = CoverageInfo.Join(_covinfo, covinfo2);
                    _covinfo.Dispose();
                    covinfo2.Dispose();
                    _covinfo = cs3;
                }
            }
            else
            {
                _covinfo = InMemoryParallel(covFiles);
            }
            CoverageSummary.PrintCoverageInfo(_covinfo);
            var resultedFile = Path.Combine(Path.GetDirectoryName(covFilesdir), "InMem-"+ covFiles.Length + "-" + Guid.NewGuid().ToString().Substring(0, 5) + "-is-paralel-" + parallel);
            return resultedFile ;
        }

        public static CoverageInfo InMemoryParallel(string[] covFiles)
        {
            InMemoryCoverageFilesMerger merger = new InMemoryCoverageFilesMerger(covFiles);
            CoverageInfo coverageInfo = merger.MergeAllFiles();
            return coverageInfo;
        }

        internal class InMemoryCoverageFilesMerger
        {
            private readonly string[] coverageFiles;
            private readonly int parallelLevel;
            private int numOfCoverageFilesLoaded;
            private readonly ConcurrentQueue<CoverageInfo> coverageInfosCache;
            private bool isCanceled = false;
            public InMemoryCoverageFilesMerger(string[] coverageFiles, int parallelLevel = 4)
            {
                this.coverageFiles = coverageFiles;
                this.parallelLevel = parallelLevel;
                this.coverageInfosCache = new ConcurrentQueue<CoverageInfo>();
                this.numOfCoverageFilesLoaded = 0;
            }

            public CoverageInfo MergeAllFiles()
            {
                Task[] tasks = new Task[this.parallelLevel];
                for (int i = 0; i < this.parallelLevel; i++)
                {
                    Console.WriteLine("Creating task: "+ i);
                    tasks[i]=Task.Run(() => MergeTask());
                }
                Task.WaitAll(tasks);

                if (coverageInfosCache.Count > 1)
                {
                    Console.WriteLine("Merging CovInfos from Cache, Cache size:" + coverageInfosCache.Count);
                    MergeTask();
                }

                if (this.isCanceled)
                {
                    Console.WriteLine("Merging has been cancelled.");
                    throw new Exception("Mergering has been cancelled.");
                }
                else
                {
                    Debug.Assert(coverageInfosCache.Count == 1,
                        $"Only one CovInfo should be in CovInfoCache, CovInfoCache size:{coverageInfosCache.Count}");
                    CoverageInfo coverageInfo;
                    coverageInfosCache.TryDequeue(out coverageInfo);
                    return coverageInfo;
                }
            }

            private void MergeTask()
            {
                try
                {
                    if (this.isCanceled)
                    {
                        return;
                    }

                    var first = GetNextCoverageInfo();
                    if (first == null)
                    {
                        return;
                    }
                    var second = GetNextCoverageInfo();
                    while (second != null && !this.isCanceled)
                    {
                        var third = CoverageInfo.Join(first, second);
                        first.Dispose();
                        second.Dispose();
                        first = third;
                        second = GetNextCoverageInfo();
                    }
                    coverageInfosCache.Enqueue(first);
                    Debug.Assert(coverageInfosCache.Count <= parallelLevel,
                        "Cache size can't be more than parallel level");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("MergeTask: failure in CovergaInfo join:" + ex);
                    this.isCanceled = true;
                }
            }

            private CoverageInfo GetNextCoverageInfo()
            {
                if (this.isCanceled)
                {
                    return null;
                }

                CoverageInfo coverageInfo = null;

                // Make sure one file provied to only one thread and all files are processed.
                var currentFilePos = this.GetAndIncrement(ref this.numOfCoverageFilesLoaded, this.coverageFiles.Length);


                if (currentFilePos < this.coverageFiles.Length)
                {
                    // Get coverage info from file.
                    coverageInfo = CoverageInfo.CreateFromFile(coverageFiles[currentFilePos]);
                    Console.WriteLine("Loading CovInfo from file:" + coverageFiles[currentFilePos]);
                }
                else
                {
                    // Get coverage info from cache.
                    coverageInfosCache.TryDequeue(out coverageInfo);
                    if (coverageInfo != null)
                    {
                        Console.WriteLine("Getting covInfo from cache:");
                    }
                }

                return coverageInfo;
            }

            private object syncObject = new object();
            private int GetAndIncrement(ref int counter, int max)
            {
               int org = int.MaxValue;
                lock (syncObject)
                {
                    if (counter < max)
                    {
                        org = counter;
                        counter++;
                    }
                    else
                    {
                        org = max;
                    }
                }
                return org;
            }

        }

        private static int GetNumOfCores()
        {
            int coreCount = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }
            return coreCount;
        }
    }
}
