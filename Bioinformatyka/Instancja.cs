using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bioinformatyka
{
    class Instancja
    {
        public HashSet<string> Spectrum;
        private string Sekwencja;
        private Random Rnd = new Random();
        public Instancja(string FileName, int Dlugosc)
        {
            StreamReader reader = new StreamReader(FileName);
            Spectrum = new HashSet<string>();
            this.Sekwencja = reader.ReadLine();
            if(!this.Sekwencja.All((character) =>
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
            for(int i = 0; i <= this.Sekwencja.Length - Dlugosc; i++)
            {
                Spectrum.Add(this.Sekwencja.Substring(i, Dlugosc)); 
            }
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
            string[] Shuffled = this.Spectrum.ToArray();
            for (int i = this.Spectrum.Count - 1; i >= 0; i--)
            {
                int r = Rnd.Next(i);
                Swap(ref Shuffled[i], ref Shuffled[r]);
            }
            return Shuffled;
        }

        public override string ToString()
        {
            return String.Join("\n", this.Spectrum);
        }
    }
}
