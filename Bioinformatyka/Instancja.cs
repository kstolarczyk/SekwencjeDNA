using System;
using System.Collections.Generic;
using System.IO;
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
        private Random Rnd = new Random();

        public Instancja(string FileName, int Dlugosc)
        {
            StreamReader reader = new StreamReader(FileName);
            Spectrum = new HashSet<string>();
            Sekwencja = reader.ReadLine();
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
                if(Config.POZYTYWNE > 0 && Rnd.NextDouble() < Config.POZYTYWNE)
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
            for(int i = 0; i < dlugosc; i++)
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
