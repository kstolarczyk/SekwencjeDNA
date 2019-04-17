using System;
using System.Collections.Generic;

namespace Bioinformatyka
{
    internal class Ant
    {
        public Graf graf { get; set; }
        private int n; // dlugosc sekwencji
        private int len; // dlugosc oligonukleotydu
        private string start; // wierzcholek startowy
        private double powtorzenia; // szansa na powtorzenie oligonukleotydu w sekwencji
        private List<string> trasa;
        private Dictionary<string, bool> visited;
        private Random rnd;
        public Ant(Graf G, int dlugoscSekwencji, int dlugoscOligo, string wierzchStart, double powtorzenia)
        {
            this.graf = G;
            this.n = dlugoscSekwencji;
            this.len = dlugoscOligo;
            this.start = wierzchStart;
            this.powtorzenia = powtorzenia;
            this.visited = new Dictionary<string, bool>(graf.Vertices.Length);
            this.trasa = new List<string>(this.n - this.len + 1);
            foreach(var v in graf.Vertices)
            {
                this.visited.Add(v, false);
            }
            this.rnd = new Random();
        }

        public double[] Probabilty(string curr, Pokrycie[] somsiedzi)
        {
            int length = somsiedzi.Length;
            double[] prob = new double[length];
            double sum = 0.0;
            for(int i = 0; i < length; i++)
            {    
                prob[i] = Math.Pow(somsiedzi[i].f, Config.ALFA) * Math.Pow(somsiedzi[i].len, Config.BETA);
                if (visited[somsiedzi[i].id]) prob[i] *= this.powtorzenia;
                sum += prob[i];
            }
            prob[0] /= sum;
            for(int i = 1; i < length; i++)
            {
                prob[i] /= sum;
                prob[i] += prob[i - 1];
            }
            prob[length - 1] = 1;
            return prob;
        }

        public void updateFeromons(int count)
        {
            double feromon = Config.QF * graf.BestResult / count;
            for(int i = 0; i < this.trasa.Count - 1; i++)
            {
                var elem = graf.Connections[this.trasa[i]][this.trasa[i + 1]];
                lock(elem)
                {
                    elem.f = feromon / (this.trasa.Count - 1);
                }
            }
        }

        public void Run()
        {
            string result = start;
            string curr = start;
            this.visited[curr] = true;
            this.trasa[0] = start;
            int curLen = this.len;
            int count = 1;
            Pokrycie[] somsiedzi = new Pokrycie[graf.Vertices.Length - 1];
            while (true)
            {
                while (curLen < this.n)
                {
                    graf.Connections[curr].Values.CopyTo(somsiedzi, 0);
                    double[] p = this.Probabilty(curr, somsiedzi);
                    double r = rnd.NextDouble();
                    int left = 0, k;
                    int right = somsiedzi.Length;
                    while (true)
                    {
                        k = (left + right) / 2;
                        if (r <= p[k])
                        {
                            if (k == left) break;
                            right = k;
                        }
                        else
                        {
                            if (k == left) { k = right; break; }
                            left = k;
                        }
                    }
                    result += somsiedzi[k].diff;
                    curLen += this.len - somsiedzi[k].len;
                    curr = somsiedzi[k].id;
                    this.visited[curr] = true;
                    this.trasa[count++] = curr;

                }

                if (count > graf.BestResult)
                {
                    lock (graf.BRLock)
                    {
                        graf.BestResult = count;
                    }
                    Console.WriteLine(result);
                    Console.WriteLine(count);
                }

                this.updateFeromons(count);

                count = 1;
                result = start;
                curLen = this.len;
            }
        }
    }
}
