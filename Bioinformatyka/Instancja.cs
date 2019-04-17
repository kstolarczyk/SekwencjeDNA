using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bioinformatyka
{
    internal class Instancja
    {
        public HashSet<string> Spectrum { get; set; }
        private string Sekwencja;
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
            }
            this.start = Spectrum.First();
            Spectrum = new HashSet<string>(Shuffle());

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
