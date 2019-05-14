using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bioinformatyka
{
    internal class Instancja
    {
        public HashSet<string> Spectrum { get; set; }
        public string Sekwencja { get; set; }
        public int dlugoscSekwencji;
        public string start;
        private static Random Rnd = new Random();

        public static string GenerujInstancje(uint n) // n - dlugosc sekwencji;
        {
            StringBuilder inst = new StringBuilder("", (int)n);
            double probA = Rnd.NextDouble();
            double probT = Rnd.NextDouble();
            double probG = Rnd.NextDouble();
            double probC = Rnd.NextDouble();
            double sum = probA + probT + probG + probC;
            probA /= sum;
            probT = probA + (probT / sum);
            probG = probT + (probG / sum);
            probC = probG + (probC / sum);
            for (int i = 0; i < n; i++)
            {
                double r = Rnd.NextDouble();
                if (r <= probA) inst.Append("A");
                else if (r <= probT) inst.Append("T");
                else if (r <= probG) inst.Append("G");
                else inst.Append("C");
            }
            return inst.ToString();
        }

        public static string GenerujInstancje(uint n, double probA, double probC, double probG, double probT) // n - dlugosc sekwencji; probA, probC, probG, probT - prawdopodobieństwo każdego nukleotydu
        {
            StringBuilder inst = new StringBuilder("", (int)n);

            double sum = probA + probT + probG + probC;
            probA /= sum;
            probT = probA + (probT / sum);
            probG = probT + (probG / sum);
            probC = probG + (probC / sum);

            for (int i = 0; i < n; i++)
            {
                double r = Rnd.NextDouble();
                if (r <= probA) inst.Append("A");
                else if (r <= probT) inst.Append("T");
                else if (r <= probG) inst.Append("G");
                else inst.Append("C");
            }
            return inst.ToString();
        }

        public static string GenerujInstancje(uint n, uint k) // n - dlugosc sekwencji; k - krok zmienności prawdopodobieństw
        {
            StringBuilder inst = new StringBuilder("", (int)n);
            double probA = Rnd.NextDouble();
            double probT = Rnd.NextDouble();
            double probG = Rnd.NextDouble();
            double probC = Rnd.NextDouble();

            double sum = probA + probT + probG + probC;
            probA /= sum;
            probT = probA + (probT / sum);
            probG = probT + (probG / sum);
            probC = probG + (probC / sum);

            for (int i = 0; i < n; i++)
            {
                if(i % k == k-1)
                {
                    probA = Rnd.NextDouble();
                    probT = Rnd.NextDouble();
                    probG = Rnd.NextDouble();
                    probC = Rnd.NextDouble();
                    sum = probA + probT + probG + probC;
                    probA /= sum;
                    probT = probA + (probT / sum);
                    probG = probT + (probG / sum);
                    probC = probG + (probC / sum);
                }
                double r = Rnd.NextDouble();
                if (r <= probA) inst.Append("A");
                else if (r <= probT) inst.Append("T");
                else if (r <= probG) inst.Append("G");
                else inst.Append("C");
            }
            return inst.ToString();
        }

        public static double Powtorzenia(string instancja, int k)
        {
            HashSet<string> oligo = new HashSet<string>();
            for (int i = 0; i < instancja.Length - k; i++)
            {
                oligo.Add(instancja.Substring(i, k));
            }
            int max = instancja.Length - k + 1;

            return (max - oligo.Count) / (double)max;
        }


        public Instancja(string sequence, int Dlugosc)
        {
            Spectrum = new HashSet<string>();
            Sekwencja = sequence;
            this.dlugoscSekwencji = Sekwencja.Length;
            if (!Sekwencja.All((character) =>
             {
                 if (character != 'A' && character != 'C' && character != 'T' && character != 'G')
                 {
                     return false;
                 }
                 return true;
             }))
            {
                throw new Exception("Wykryto nieprawidłowe elementy sekwencji");
            }
            for (int i = 0; i <= Sekwencja.Length - Dlugosc; i++)
            {
                Spectrum.Add(Sekwencja.Substring(i, Dlugosc));
                if (Config.POZYTYWNE > 0 && Rnd.NextDouble() < Config.POZYTYWNE)
                {
                    Spectrum.Add(this.GenerujOligo(Dlugosc));
                }
            }
            this.start = Spectrum.First();
            Spectrum = new HashSet<string>(Shuffle());

        }

        private string GenerujOligo(int dlugosc)
        {
            double probA = Rnd.NextDouble();
            double probT = Rnd.NextDouble();
            double probG = Rnd.NextDouble();
            double probC = Rnd.NextDouble();
            double sum = probA + probT + probG + probC;
            probA /= sum;
            probT = probA + (probT / sum);
            probG = probT + (probG / sum);
            probC = probG + (probC / sum);
            StringBuilder result = new StringBuilder(dlugosc);
            for (int i = 0; i < dlugosc; i++)
            {
                double r = Rnd.NextDouble();
                if (r <= probA) result.Append("A");
                else if (r <= probT) result.Append("T");
                else if (r <= probG) result.Append("G");
                else result.Append("C");
            }
            Console.WriteLine("wygenerowano błąd pozytywny: {0}", result.ToString());
            return result.ToString();
        }

        public void Swap(ref string a, ref string b)
        {
            string tmp = a;
            a = b;
            b = tmp;
        }

        public string[] Shuffle()
        {
            string[] Shuffled = Spectrum.ToArray();
            for (int i = Spectrum.Count - 1; i >= 0; i--)
            {
                int r = Rnd.Next(i);
                Swap(ref Shuffled[i], ref Shuffled[r]);
            }
            return Shuffled;
        }

        public override string ToString()
        {
            return String.Join("\n", Spectrum);
        }
    }
}
