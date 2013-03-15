using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using SciSharp.Collections;
using SciSharp.Probabilities;


namespace SciSharp.Examples.BayesianTextGenerator
{
    internal static class Program
    {
        private static readonly DefaultDictionary<Tuple<String, String>, int> PairsCount = new DefaultDictionary<Tuple<string, string>, int>();
        private static readonly DefaultDictionary<Tuple<String, String, String>, int> TriplesCount = new DefaultDictionary<Tuple<string, string, string>, int>();
        private static readonly DefaultDictionary<String, int> WordsCount = new DefaultDictionary<string, int>();
        private static readonly HashSet<string> Words = new HashSet<string>();

        private static readonly RandomEx Random = new RandomEx();

        private static IEnumerable<string> Walk(string root, string pattern)
        {
            foreach (string file in Directory.GetFiles(root, pattern))
                yield return file;

            foreach (string directory in Directory.GetDirectories(root))
                foreach (string file in Walk(directory, pattern))
                    yield return file;
        }

        private static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Walking '{0}' for pattern '{1}'", args[0], args[1]);

                foreach (string file in Walk(args[0], args[1]))
                    Train(LoadFile(file));
            }
            else if (args.Length > 0)
            {
                Console.WriteLine("Loading file '{0}'", args[0]);

                Train(LoadFile(args[0]));
            }
            else
            {
                Console.WriteLine("Manual training. Enter a filename to train. Enter an empty line to end training.");

                string s = Console.ReadLine();

                while (!string.IsNullOrEmpty(s))
                {
                    Train(LoadFile(s));
                    s = Console.ReadLine();
                }
            }

            Console.WriteLine();
            Console.WriteLine("Learned {0} words, {1} word pairs, {2} word triples.", Words.Count, PairsCount.Count, TriplesCount.Count);
            Console.WriteLine();

            Generate();
        }

        private static void Generate(string start = "")
        {
            string previous1 = start;
            string previous2 = start;

            Console.Write(start);

            do
            {
                string next = Sample(previous1, previous2);

                if (string.IsNullOrEmpty(next))
                    next = Sample(previous2);

                previous1 = previous2;
                previous2 = next;

                if (next.Length > 0 || !char.IsPunctuation(next, 0))
                    Console.Write(" ");

                Console.Write(next);
            } while (!string.IsNullOrEmpty(previous2));
        }

        private static IEnumerable<string> LoadFile(string filename)
        {
            var stream = new StreamReader(filename, Encoding.UTF8);

            while (!stream.EndOfStream)
            {
                string[] line = stream.ReadLine().Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);

                foreach (string str in line)
                    yield return str;

                //var list = new LinkedList<string>(line);

                //while (list.Count > 0)
                //{
                //    var str = list.First.Value;
                //    list.RemoveFirst();

                //    if (str.All(char.IsLetter))
                //        yield return str;
                //    else
                //    {
                //        char ch = str.First(c => !char.IsLetter(c));
                //        int p = str.IndexOf(ch);

                //        if (p > 0)
                //            yield return str.Substring(0, p).ToLower();

                //        if (char.IsPunctuation(str, p))
                //            yield return str.Substring(p, 1);

                //        if (p < str.Length - 1)
                //            list.AddFirst(str.Substring(p + 1));
                //    }
                //}

                yield return "\n";
            }
        }

        private static void Train(IEnumerable<string> corpus)
        {
            string previous1 = string.Empty;
            string previous2 = string.Empty;

            foreach (string word in corpus)
            {
                Words.Add(word);
                WordsCount[word]++;
                PairsCount[Tuple.Create(previous2, word)]++;
                TriplesCount[Tuple.Create(previous1, previous2, word)]++;

                previous1 = previous2;
                previous2 = word;

                if (Words.Count%1000 == 0)
                    Console.WriteLine(word);
            }
        }

        private static string Sample(string w1, string w2)
        {
            var words = new List<string>();
            var probabilites = new List<double>();
            int wordCount = PairsCount[Tuple.Create(w1, w2)];

            if (wordCount == 0)
                return string.Empty;

            foreach (string w in Words)
            {
                double tripleCount = TriplesCount[Tuple.Create(w1, w2, w)];

                if (tripleCount > 0)
                {
                    words.Add(w);
                    probabilites.Add(tripleCount/wordCount);
                }
            }

            if (words.Count <= 1)
                return string.Empty;

            return Random.Roulette(words.ToArray(), probabilites.ToArray());
        }

        private static string Sample(string word)
        {
            var words = new List<string>();
            var probabilites = new List<double>();
            int wordCount = WordsCount[word];

            foreach (string w in Words)
            {
                double pairCount = PairsCount[Tuple.Create(word, w)];

                if (pairCount > 0)
                {
                    words.Add(w);
                    probabilites.Add(pairCount/wordCount);
                }
            }

            if (words.Count == 0)
                return string.Empty;

            return Random.Roulette(words.ToArray(), probabilites.ToArray());
        }
    }
}
