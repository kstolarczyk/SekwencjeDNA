namespace Bioinformatyka
{
    class Config
    {
        public static readonly double ALFA = 2; // istotność feromonów (jakość trasy)
        public static readonly double BETA = 40; // jakość pokrycia oligonukleotydów
        public static byte OLIGONUKLEOTYD_LEN = 10; // długość oligonukleotydu
        public static readonly double QF = 50; // bazowa ilość rozprowadzanego feromonu
        public static readonly double PAROWANIE = 0.5;
        public static readonly int LICZBA_MROWEK = 30;
        public static uint MAX_TIMEOUT = 30000; // max working time in miliseconds
        public static double POWTORZENIA = 0.05; // szansa na powtorzenie
        public static double POZYTYWNE = 0.02; // szansa na blad pozytywny (nadmiarowy oligonukleotyd w spektrum)
        
    }
}
