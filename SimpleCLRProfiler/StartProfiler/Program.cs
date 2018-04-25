using System.Diagnostics;

namespace StartProfiler
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = @"C:\Users\samadala\Desktop\CustomProfiler\StartProfiler\CLRProfiler\ProfileMe\bin\Debug\ProfileMe.exe",
                UseShellExecute = false
            };
            processStartInfo.Environment["COR_ENABLE_PROFILING"] = "1";
            processStartInfo.Environment["COR_PROFILER_PATH"] =
                @"C:\Users\samadala\Desktop\CustomProfiler\StartProfiler\CLRProfiler\Debug\CLRProfiler.dll";
            processStartInfo.Environment["COR_PROFILER"] = "{27AD456B-386A-499F-A0D9-165CDD7A2C31}";

            var proc = new Process()
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };

            proc.Start();
            proc.WaitForExit();
        }
    }
}
