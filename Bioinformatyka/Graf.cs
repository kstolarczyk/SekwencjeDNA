using System.Collections.Generic;
using System.Text;

namespace Bioinformatyka
{
    internal class Pokrycie
    {
        public byte len { get; set; } // dlugosc pokrycia
        public string diff { get; set; } // niepokrywjaca sie czesc
        public double f { get; set; } // feromony
        public double p { get; set; }
        public string id { get; set; }
        public Pokrycie(byte l, string d, string v)
        {
            len = l;
            diff = d;
            f = 0.5f;
            p = 0;
            id = v;
        }
    }

    internal class Graf
    {
        public string[] Vertices { get; set; }
        public Dictionary<string, Dictionary<string, Pokrycie>> Connections { get; set; }
        public int BestResult { get; set; }
        public readonly object BRLock = new object();
        public Dictionary<string, bool> Results { get; set; }

        public Pokrycie Przesuniecie(string Source, string Target)
        {
            byte end = (byte)Target.Length;
            byte max = end;
            while (Source.Substring(end - max, max) != Target.Substring(0, max))
            {
                if (max-- == 0)
                {
                    break;
                }
            }
            return new Pokrycie(max, Target.Substring(max, end - max), Target);
        }

        public void CountConnections(string Source, string[] Targets)
        {
            foreach (string Target in Targets)
            {
                if (Target != Source)
                {
                    Connections[Source].Add(Target, Przesuniecie(Source, Target));
                }
            }
        }

        public Graf(string[] Spectrum)
        {
            Vertices = Spectrum;
            int Len = Spectrum.Length;
            Results = new Dictionary<string, bool>(1024);
            Connections = new Dictionary<string, Dictionary<string, Pokrycie>>(Len);
            foreach (string OliNuk in Spectrum)
            {
                Connections.Add(OliNuk, new Dictionary<string, Pokrycie>(Len));
                CountConnections(OliNuk, Spectrum);
            }
        }

        public override string ToString()
        {
            StringBuilder print = new StringBuilder("");
            foreach (string S in Vertices)
            {
                foreach (var pair in Connections[S])
                {
                    print.Append(S).Append(" -> ").Append(pair.Key).Append(" przesunięcie: ").Append(pair.Value.len).Append("\n");
                }
            }
            return print.ToString();
        }
    }
}
