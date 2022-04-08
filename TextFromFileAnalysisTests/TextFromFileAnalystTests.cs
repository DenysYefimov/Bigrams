using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using TextFromFileAnalysis;

namespace TextFromFileAnalysisTests
{
    public class Tests
    {
        bool Compare(Dictionary<string, Dictionary<string, int>> a, Dictionary<string, Dictionary<string, int>> b)
        {
            if (a.Count != b.Count)
                return false;
            foreach (var pair in a)
            {
                if (b.ContainsKey(pair.Key))
                    foreach (var pairint in pair.Value)
                    {
                        if (b[pair.Key].ContainsKey(pairint.Key))
                        {
                            var t = b[pair.Key][pairint.Key];
                            if (pairint.Value != t)
                                return false;
                        }
                        else return false;
                    }
                else return false;
            }
            return true;
        }
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReadFromFile_TestFile_Text()
        {
            string expexted = "a b c d. b c d. e b c a d.";
            string actual = TextFromFileAnalyst.ReadFromFile(new FileInfo("TestFile.txt"));

            Assert.AreEqual(expexted, actual);
        }

        [Test]
        public void ParseSentences_TestText_ListOfSentences()
        {
            List<List<string>> expected = new List<List<string>> 
            { 
                new List<string> { "a", "b", "c", "d" }, 
                new List<string> { "b", "c", "d" }, 
                new List<string> { "e", "b", "c", "a", "d" } 
            };
            List<List<string>> actual = TextFromFileAnalyst.ParseSentences("a b c d. b c d. e b c a d.");
            CollectionAssert.AreEqual(expected, actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAllPhrases_TestListOfSentences_DictionaryOfPhrases()
        {
            Dictionary<string, Dictionary<string, int>> expected = new Dictionary<string, Dictionary<string, int>>
            {
                {
                "a", new Dictionary<string, int>
                {
                    { "b", 1 }, { "d", 1}
                }
                },

                {
                "b", new Dictionary<string, int>
                {
                    { "c", 3 }
                }
                },

                {
                "c", new Dictionary<string, int>
                {
                    { "d", 2 }, { "a", 1}
                }
                },

                {
                "e", new Dictionary<string, int>
                {
                    { "b", 1 }
                }
                }
            };
            Dictionary<string, Dictionary<string, int>> actual = TextFromFileAnalyst.GetAllPhrases(new List<List<string>>
            {
                new List<string> { "a", "b", "c", "d" },
                new List<string> { "b", "c", "d" },
                new List<string> { "e", "b", "c", "a", "d" }
            });
            Assert.IsTrue(Compare(expected, actual));
        }

        [Test]
        public void FindMostFrequent_TestDictionaryOfPhrases_Dictionary()
        {
            Dictionary<string, string> expected = new Dictionary<string, string> { { "a", "b" }, { "b", "c"}, { "c", "d" }, { "e", "b" } };
            Dictionary<string, string> actual = TextFromFileAnalyst.FindMostFrequent(new Dictionary<string, Dictionary<string, int>>
            {
            {
                "a", new Dictionary<string, int>
                {
                    { "b", 1 }, { "d", 1}
                }
                },

                {
                "b", new Dictionary<string, int>
                {
                    { "c", 3 }
                }
                },

                {
                "c", new Dictionary<string, int>
                {
                    { "d", 2 }, { "a", 1}
                }
                },

                {
                "e", new Dictionary<string, int>
                {
                    { "b", 1 }
                }
                }
            });
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}