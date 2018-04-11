using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Coverage.Analysis;

namespace ConsoleApp10
{
    class CoverageSummary
    {
        public static void PrintCoverageInfo(string coverageFilePath)
        {
            PrintCoverageInfo(CoverageInfo.CreateFromFile(coverageFilePath));
        }
        public static void PrintCoverageInfo(CoverageInfo finalCovInfo)
        {
            CoverageDS coverageDS = finalCovInfo.BuildDataSet();

            //Get All Source files from coverage file
            List<string> sourceFiles = coverageDS.GetSourceFiles();

            foreach (string sourceFile in sourceFiles)
            {
                //No need for doing this anymore
                /*List<CoverageDSPriv.LinesRow> allLines = DataRowToList(coverageDS, sourceFile);

                IList<LineCoverageInfo> mergedInfos = CoverageDS.MergeFileLines(allLines);
                */

                //this is the new API which you can use for each source to get correct coverage info.

                IList<LineCoverageInfo> sortInfo = CoverageDS.GetSourceFileLineCoverageInfo(coverageDS, sourceFile);
                AnalyzeMergedLines(sortInfo);
            }
        }

        // Not Required anymore
        /*public static List<CoverageDSPriv.LinesRow> DataRowToList(CoverageDS coverageDS, string sourceFile)
        {
            uint fileId = coverageDS.FindSourceFileId(sourceFile);
            string filter = string.Format(CultureInfo.InvariantCulture, "SourceFileID = {0}", fileId);
            DataRow[] rows = coverageDS.Lines.Select(filter);

            List<CoverageDSPriv.LinesRow> allLines = new List<CoverageDSPriv.LinesRow>();
            foreach (DataRow dataRow in rows)
            {
                if (dataRow.RowState == DataRowState.Deleted || dataRow.RowState == DataRowState.Detached)
                    continue;

                CoverageDSPriv.LinesRow row = dataRow as CoverageDSPriv.LinesRow;

                allLines.Add(row);
            }

            return allLines;
        }*/

        //this is just manipulation of LineCoverageInfo
        //Each LineCoverageInfo is a range of lines

        /*for example:
         *              startLine   EndLine   Status
         * range 1          13          17      C
         * range 2          13          17      N
         * range 3          13          17      C
         * 
         * after pasrsign 1st range we will say covered, then after parssing 2nd we will say not coverred,
         * finally after parsing 3rd range it will say covered.
         * 
         * it's weird but like I said, either we are getting extremely lucky and it might screw us later, 
         * or I'm missing something somewhere in code
         */

        public static void AnalyzeMergedLines(IList<LineCoverageInfo> lineInfo)
        {
            //first we remove dumplicates by using map, after this each line is only present once, no more line ranges.
            Dictionary<uint, CoverageStatus> lineMap = new Dictionary<uint, CoverageStatus>();
            foreach (LineCoverageInfo info in lineInfo)
            {
                for (uint lineNum = info.LineBegin; lineNum <= info.LineEnd; lineNum++)
                    lineMap[lineNum] = info.CoverageStatus;
            }

            HashSet<uint> coveredLines = new HashSet<uint>();
            HashSet<uint> notCoveredLines = new HashSet<uint>();
            HashSet<uint> partiallyCoveredLines = new HashSet<uint>();

            //on getting this Map, add data to list(s) of covered, notcovered and partially covered
            // Note: not iterating over ranges here
            foreach (KeyValuePair<uint, CoverageStatus> kvp in lineMap)
            {
                CoverageStatus status = kvp.Value;
                switch (status)
                {
                    case CoverageStatus.Covered:
                        coveredLines.Add(kvp.Key);
                        break;

                    case CoverageStatus.NotCovered:
                        notCoveredLines.Add(kvp.Key);
                        break;

                    case CoverageStatus.PartiallyCovered:
                        partiallyCoveredLines.Add(kvp.Key);
                        break;
                }
            }

            Console.WriteLine($"CoveredLines: {string.Join(",", coveredLines)}");
            Console.WriteLine($"Not covered lines: {string.Join(",", notCoveredLines)}");
            Console.WriteLine($"Partially covered lines: {string.Join(",", partiallyCoveredLines)}");
        }
    }
}