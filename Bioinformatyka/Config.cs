using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bioinformatyka
{
    class Config
    {
        public static readonly double ALFA = 2; // istotność feromonów (jakość trasy)
        public static readonly double BETA = 40; // jakość pokrycia oligonukleotydów
        public static readonly byte OLIGONUKLEOTYD_LEN = 10; // długość oligonukleotydu
        public static readonly double QF = 50; // bazowa ilość rozprowadzanego feromonu
        public static readonly string FILE_NAME = "test.txt"; // nazwa pliku instancji
        public static readonly double PAROWANIE = 0.5;
        public static readonly int LICZBA_MROWEK = 50;
        public static readonly int MAX_TIMEOUT = 30000; // max working time in miliseconds
    }
}
