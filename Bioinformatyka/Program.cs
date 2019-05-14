using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Bioinformatyka
{
    internal class Program
    {
        public static Barrier barrier = new Barrier(Config.LICZBA_MROWEK);
        private static void Main(string[] args)
        {
            try
            {

                // wybor opcji instancji
              
                Console.WriteLine("Wczytywanie sekwencji -wybierz opcje (1-2):");
                Console.WriteLine("1. Wczytaj instancję (sekwencje) z pliku");
                Console.WriteLine("2. Wygeneruj losową sekwencje (stałe losowe prawdopodobieństwo nukleotydów)");
                Console.WriteLine("3. Wygeneruj losową sekwencje (zmienne losowe prawdopobieństwo nukleotydów)");
                Console.WriteLine("4. Wygeneruj losową sekwencje (stałe ręcznie wpisane prawdopodobieństwo nukleotydów)");

                string sekwencja;
                uint n;
                switch (Console.ReadKey().KeyChar)
                {
                    case '1':
                        Console.Write("\nWpisz nazwę pliku instancji: ");
                        string fileName = Console.ReadLine();
                        StreamReader reader = new StreamReader(fileName);
                        sekwencja = reader.ReadLine();
                        break;
                    case '2':
                        Console.Write("\nWpisz rozmiar n sekwencji: ");
                        
                        if (!UInt32.TryParse(Console.ReadLine(), out n))
                        {
                            throw new Exception("\nWpisano niepoprawną wartość, program zostanie zamknięty");
                        }
                        sekwencja = Instancja.GenerujInstancje(n);
                        break;
                    case '3':
                        Console.Write("\nWpisz rozmiar n sekwencji: ");
                        if (!UInt32.TryParse(Console.ReadLine(), out n))
                        {
                            throw new Exception("\nWpisano niepoprawną wartość, program zostanie zamknięty");
                        }
                        uint k;
                        Console.Write("\nWpisz krok k zmienności prawdopodobieństwa: ");
                        if (!UInt32.TryParse(Console.ReadLine(), out k))
                        {
                            throw new Exception("\nWpisano niepoprawną wartość, program zostanie zamknięty");
                        }
                        sekwencja = Instancja.GenerujInstancje(n,k);
                        break;
                    case '4':
                        Console.Write("\nWpisz rozmiar n sekwencji: ");
                        if (!UInt32.TryParse(Console.ReadLine(), out n))
                        {
                            throw new Exception("\nWpisano niepoprawną wartość, program zostanie zamknięty");
                        }
                        double A, C, G, T;
                        Console.Write("\nWpisz prawdopodobieństwo dla A: ");
                        if (!Double.TryParse(Console.ReadLine().Replace('.', ','), out A))
                        {
                            throw new Exception("\nWpisano niepoprawną wartość, program zostanie zamknięty");
                        }
                        Console.Write("\nWpisz prawdopodobieństwo dla C: ");
                        if (!Double.TryParse(Console.ReadLine().Replace('.', ','), out C))
                        {
                            throw new Exception("\nWpisano niepoprawną wartość, program zostanie zamknięty");
                        }
                        Console.Write("\nWpisz prawdopodobieństwo dla G: ");
                        if (!Double.TryParse(Console.ReadLine().Replace('.', ','), out G))
                        {
                            throw new Exception("\nWpisano niepoprawną wartość, program zostanie zamknięty");
                        }
                        Console.Write("\nWpisz prawdopodobieństwo dla T: ");
                        if (!Double.TryParse(Console.ReadLine().Replace('.', ','), out T))
                        {
                            throw new Exception("\nWpisano niepoprawną wartość, program zostanie zamknięty");
                        }
                        sekwencja = Instancja.GenerujInstancje(n, A, C, G, T);
                        break;
                    default:
                        throw new Exception("\nNie wybrano żadnej opcji, program zostanie zamknięty");

                }

                Console.WriteLine("\nKonfiguracja parametrów - wybierz opcje (1-2):");
                Console.WriteLine("1. Konfiguruj parametry (długość oligonukleotydu - k, błędy negatywne - p1, błędy pozytywne - p2, czas wykonywania algorytmu - t)");
                Console.WriteLine("2. Pomiń konfigurację (domyślne parametry: k = 10; p1 = 0.05; p2 = 0.02; t = 30;");

                switch (Console.ReadKey().KeyChar)
                {
                    case '1':

                        // pobranie dlugosci oligonukleotydu w spektrum

                        uint k;
                        Console.Write("\nWpisz długość k oligonukleotydu (8 <= k <= 10): ");

                        if (!UInt32.TryParse(Console.ReadLine(), out k) || k < 8 || k > 10)
                        {
                            throw new Exception("\nWpisano niepoprawną wartość, program zostanie zamknięty");
                        }

                        Config.OLIGONUKLEOTYD_LEN = (byte)k;

                        // pobranie informacji o możliwych błędach w spektrum

                        double p;

                        Console.Write("\nPodaj prawdopodobieństwo p1 wystąpienia błędu negatywnego (powtórzenia oligonukleotydu) w spektrum (0 <= p <= 1): ");
                        if (!Double.TryParse(Console.ReadLine().Replace('.', ','), out p) || p < 0 || p > 1)
                        {
                            throw new Exception("\nWpisano niepoprawną wartość, program zostanie zamknięty");
                        }
                        Config.POWTORZENIA = p;


                        Console.Write("\nPodaj prawdopodobieństwo p2 wystąpienia błędu pozytywnego (nadmiarowego oligonukleotydu) w spektrum (0 <= p <= 1): ");
                        if (!Double.TryParse(Console.ReadLine().Replace('.', ','), out p) || p < 0 || p > 1)
                        {
                            throw new Exception("\nWpisano niepoprawną wartość, program zostanie zamknięty");
                        }
                        Config.POZYTYWNE = p;

                        // Ustawienie przyblizonego czasu wykonywania algorytmu

                        uint t;
                        Console.Write("\nPodaj czas t wykonywania algorytmu w sekundach: ");
                        if (!UInt32.TryParse(Console.ReadLine(), out t))
                        {
                            throw new Exception("\nWpisano niepoprawną wartość, program zostanie zamknięty");
                        }

                        Config.MAX_TIMEOUT = t * 1000;
                        break;
                    case '2':
                        break;
                    default:
                        throw new Exception("\nNie wybrano żadnej opcji, program zostanie zamknięty");

                }
                Console.WriteLine("\n");

                Instancja inst = new Instancja(sekwencja, Config.OLIGONUKLEOTYD_LEN); // utworzenie rzeczywistej instancji (spektrum oligonukleotydow w losowej kolejnosci)

                Console.WriteLine("Trwa działanie algorytmu...");

                Graf graf = new Graf(inst.Spectrum.ToArray());
                Thread[] threads = new Thread[Config.LICZBA_MROWEK];
                Ant[] Ants = new Ant[Config.LICZBA_MROWEK - 1];
                for (int i = 0; i < Ants.Length; i++)
                {
                    Ants[i] = new Ant(graf, inst.dlugoscSekwencji, Config.OLIGONUKLEOTYD_LEN, inst.start, Config.POWTORZENIA);
                    threads[i] = new Thread(new ThreadStart(Ants[i].Run));
                }
                SpecialAnt main = new SpecialAnt(graf, inst.dlugoscSekwencji, Config.OLIGONUKLEOTYD_LEN, inst.start, Config.POWTORZENIA);
                threads[Config.LICZBA_MROWEK - 1] = new Thread(new ThreadStart(main.Run));
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                //Console.WriteLine(graf);
                for (int i = 0; i < threads.Length; i++)
                {
                    threads[i].Priority = ThreadPriority.AboveNormal;
                    threads[i].Start();
                }

                Thread.Sleep((int)Config.MAX_TIMEOUT);
                for (int i = 0; i < threads.Length; i++)
                {
                    threads[i].Abort();
                }
                foreach (string result in graf.Results.Keys)
                {
                    Console.WriteLine("result: {0}\nwith matching score: {1}%", result, SequenceAlignment.Score(inst.Sekwencja, result) * 100);
                }
                Console.WriteLine("{0} number of results with {1} vertices", graf.Results.Count, graf.BestResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();

        }
    }
}
