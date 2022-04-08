using System;
using System.IO;
using System.Collections.Generic;

namespace TextFromFileAnalysis
{
    public static class TextFromFileAnalyst
    {
        public static string ReadFromFile(FileInfo fi)
        {
            string result = string.Empty;
            fi.Refresh();
            StreamReader fstr_in = new StreamReader(fi.OpenRead());
            try
            {
                while (!fstr_in.EndOfStream)
                {
                    result += fstr_in.ReadLine();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString() + ":\n" + exception.Message);
            }
            finally
            {
                fstr_in.Dispose();
            }
            return result;
        }

        static string AddWord(string word, List<string> lst)
        {
            if (word != string.Empty)
                lst.Add(word);
            return string.Empty;
        }

        public static List<List<string>> ParseSentences(string text)
        {
            var sentencesList = new List<List<string>>();
            string temp = string.Empty;
            string[] sentences = text.Split(new char[] { '.', '?', '!', ';', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < sentences.Length; ++i)
            {
                List<string> tempList = new List<string>(); 
                for (int k = 0; k < sentences[i].Length; ++k)
                {
                    if (char.IsLetter(sentences[i][k]) || sentences[i][k] == '\'' || sentences[i][k] == '-')
                        temp += char.ToLower(sentences[i][k]);
                    else temp = AddWord(temp, tempList);

                    if (k == sentences[i].Length - 1)
                    {
                        temp = AddWord(temp, tempList);
                        if (tempList.Count != 0)
                            sentencesList.Add(tempList);
                    }
                }
            }
            return sentencesList;
        }

        public static Dictionary<string, Dictionary<string, int>> GetAllPhrases(List<List<string>> text)
        {
            var frequency = new Dictionary<string, Dictionary<string, int>>();

            foreach(var sentence in text)
            {
                if (sentence.Count > 1)
                    for (int i = 0; i < sentence.Count - 1; ++i)
                    {
                        if (frequency.ContainsKey(sentence[i]))
                        {
                            if (frequency[sentence[i]].ContainsKey(sentence[i + 1]))
                                ++frequency[sentence[i]][sentence[i + 1]];
                            else frequency[sentence[i]].Add(sentence[i + 1], 1);
                        }
                        else
                        {
                            frequency.Add(sentence[i], new Dictionary<string, int>());
                            frequency[sentence[i]].Add(sentence[i + 1], 1);
                        }
                    }
            }
            return frequency;
        }

        public static Dictionary<string, string> FindMostFrequent(Dictionary<string, Dictionary<string, int>> frequency)
        {
            var result = new Dictionary<string, string>();
            int tempMax = 0;
            string tempMaxStr = string.Empty;
            foreach (var phrase in frequency)
            {
                foreach (var word in phrase.Value)
                {
                    if (word.Value > tempMax)
                    {
                        tempMax = word.Value;
                        tempMaxStr = word.Key;
                    }
                }
                result.Add(phrase.Key, tempMaxStr);
                tempMax = 0;
            }
            return result;
        }

        public static Dictionary<string, string> FindMostFrequentBigramsInTextFromFile(FileInfo fi)
        {
            return FindMostFrequent(GetAllPhrases(ParseSentences(ReadFromFile(fi))));
        }
    }
}