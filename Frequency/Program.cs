using System;
using System.IO;
using Analyst = TextFromFileAnalysis.TextFromFileAnalyst;

namespace Frequency
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfo fi = new FileInfo("HarryPotterText.txt");
            if (fi.Exists)
            {
                foreach (var pair in Analyst.FindMostFrequentBigramsInTextFromFile(fi))
                {
                    Console.WriteLine(pair.Key + " : " + pair.Value);
                }
            }
        }
    }
}