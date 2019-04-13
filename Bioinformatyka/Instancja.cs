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
        public Instancja(string FileName, int Dlugosc)
        {
            StreamReader reader = new StreamReader(FileName);
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
            for(int i = 0; i < this.Sekwencja.Length - Dlugosc; i++)
            {
                Spectrum.Add(this.Sekwencja.Substring(i, Dlugosc)); 
            }

        }
        public override string ToString()
        {
            return String.Join("\n", this.Sekwencja);
        }
    }
}
