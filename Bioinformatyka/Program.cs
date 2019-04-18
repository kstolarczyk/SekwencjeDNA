using System;
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
                Instancja inst = new Instancja(Config.FILE_NAME, Config.OLIGONUKLEOTYD_LEN);
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
                    threads[i].Priority = ThreadPriority.Highest;
                    threads[i].Start();
                }
                for (int i = 0; i < threads.Length; i++)
                {
                    //threads[i].Join(1);
                }
                Thread.Sleep(Config.MAX_TIMEOUT);
                for (int i = 0; i < threads.Length; i++)
                {
                    threads[i].Abort();
                }
                foreach (string result in graf.Results)
                {
                    Console.WriteLine(result);
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
