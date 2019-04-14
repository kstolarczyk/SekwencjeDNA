using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bioinformatyka
{
    class Graf
    {
        public string[] Vertices { get; set; }
        public Dictionary<string, Dictionary<string, byte>> Connections { get; set; }

        public byte Przesuniecie(string Source, string Target)
        {
            byte end = (byte)Target.Length;
            byte max = end;
            while(Source.Substring(end - max, max) != Target.Substring(0, max))
            {
                if (max-- == 0) break;
            }
            return max;
        }

        public void CountConnections(string Source, string[] Targets)
        {
            foreach(string Target in Targets)
            {
                if(Target != Source)
                {
                    this.Connections[Source].Add(Target, this.Przesuniecie(Source, Target));
                }
            }
        }

        public Graf(string[] Spectrum)
        {
            this.Vertices = Spectrum;
            int Len = Spectrum.Length;
            this.Connections = new Dictionary<string, Dictionary<string, byte>>(Len);
            foreach(string OliNuk in Spectrum)
            {
                this.Connections.Add(OliNuk, new Dictionary<string, byte>(Len));
                this.CountConnections(OliNuk, Spectrum);
            }
        }

        public override string ToString()
        {
            string print = "";
            foreach(string S in this.Vertices)
            {
                foreach(var pair in this.Connections[S])
                {
                    print += S + " -> " + pair.Key + " przesunięcie: " + pair.Value + "\n";
                }
            }
            return print;
        }
    }
}
