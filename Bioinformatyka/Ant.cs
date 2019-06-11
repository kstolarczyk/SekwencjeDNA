using System;
using System.Collections.Generic;
using System.Text;

namespace Bioinformatyka
{
    internal class Ant
    {
        public Graf graf { get; set; }
        private int n; // dlugosc sekwencji
        private int len; // dlugosc oligonukleotydu
        private string start; // wierzcholek startowy
        private double powtorzenia; // szansa na powtorzenie oligonukleotydu w sekwencji
        private Dictionary<string, bool> visited;
        private List<string> trasa;
        public Ant(Graf G, int dlugoscSekwencji, int dlugoscOligo, string wierzchStart, double powtorzenia)
        {
            this.graf = G;
            this.n = dlugoscSekwencji;
            this.len = dlugoscOligo;
            this.start = wierzchStart;
            this.powtorzenia = powtorzenia;
            this.trasa = new List<string>(this.n - this.len + 1);
            this.visited = new Dictionary<string, bool>(graf.Vertices.Length);
            foreach (var v in graf.Vertices)
            {
                this.visited.Add(v, false);
            }
        }


        public double[] Probabilty(string curr, Pokrycie[] somsiedzi)
        {
            int length = somsiedzi.Length;
            double[] prob = new double[length];
            double sum = 0.0;
            for (int i = 0; i < length; i++)
            {
                prob[i] = Math.Pow(somsiedzi[i].f, Config.ALFA) * Math.Pow(somsiedzi[i].len, Config.BETA);
                if (visited[somsiedzi[i].id])
                {
                    prob[i] *= this.powtorzenia;
                }

                sum += prob[i];
            }
            prob[0] /= sum;
            for (int i = 1; i < length; i++)
            {
                prob[i] /= sum;
                prob[i] += prob[i - 1];
            }
            prob[length - 1] = 1;
            return prob;
        }

        public void resetVisited()
        {
            foreach (var key in graf.Vertices)
            {
                this.visited[key] = false;
            }
        }

        public void updateFeromons(int count)
        {
            double feromon = Config.QF * graf.BestResult / count;
            for (int i = 0; i < this.trasa.Count - 1; i++)
            {
                var elem = graf.Connections[this.trasa[i]][this.trasa[i + 1]];
                lock (elem)
                {
                    elem.f = feromon * elem.len / Config.OLIGONUKLEOTYD_LEN;
                }
            }
        }

        public void Run()
        {

            Pokrycie[] somsiedzi = new Pokrycie[graf.Vertices.Length - 1];
            Program.barrier.SignalAndWait();
            StringBuilder result = new StringBuilder(this.start, this.n - this.len + 1);

            while (true)
            {
                string curr = this.start;
                int curLen = this.len;
                int count = 1;
                this.visited[curr] = true;
                this.trasa.Add(start);
                while (curLen < this.n)
                {
                    graf.Connections[curr].Values.CopyTo(somsiedzi, 0);
                    double[] p = this.Probabilty(curr, somsiedzi);
                    double r = RandomGen.NextDouble();
                    int left = 0, k;
                    int right = somsiedzi.Length;
                    while (true)
                    {
                        k = (left + right) / 2;
                        if (r <= p[k])
                        {
                            if (k == left)
                            {
                                break;
                            }

                            right = k;
                        }
                        else
                        {
                            if (k == left) { k = right; break; }
                            left = k;
                        }
                    }
                    result.Append(somsiedzi[k].diff);
                    curLen += this.len - somsiedzi[k].len;
                    curr = somsiedzi[k].id;
                    this.trasa.Add(curr);
                    this.visited[curr] = true;
                    count++;

                }
                if (curLen == this.n)
                {
                    if (count >= graf.BestResult)
                    {
                        if (count > graf.BestResult)
                        {
                            lock (graf.BRLock)
                            {
                                if (count > graf.BestResult)
                                {
                                    graf.Results.Clear();
                                    graf.BestResult = count;
                                    Console.WriteLine("Best result: {0} vertices", count);

                                }
                            }
                        }
                        string res = result.ToString();
                        if (!graf.Results.ContainsKey(res))
                        {
                            lock (graf.BRLock)
                            {
                                if (!graf.Results.ContainsKey(res))
                                    graf.Results.Add(res,true);
                            }
                        }
                    }

                    this.updateFeromons(count);
                }

                this.resetVisited();
                this.trasa.Clear(); 
                result.Clear().Append(start);
            }
        }
    }
}
