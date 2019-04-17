using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bioinformatyka
{
    class Config
    {
        public static readonly double ALFA = 1.0f; // istotność feromonów (jakość trasy)
        public static readonly double BETA = 2.0f; // jakość pokrycia oligonukleotydów
        public static readonly byte OLIGONUKLEOTYD_LEN = 4; // długość oligonukleotydu
        public static readonly double QF = 100.0f; // bazowa ilość rozprowadzanego feromonu
        public static readonly string FILE_NAME = "test.txt"; // nazwa pliku instancji
        public static readonly double PAROWANIE = 0.5;
        public static readonly int LICZBA_MROWEK = 10;
    }
}
