namespace Bioinformatyka
{
    class SpecialAnt : Ant
    {
        public SpecialAnt(Graf graf, int dlugoscSekwencji, int dlugoscOligo, string wierzchStart, double powtorzenia) : base(graf, dlugoscSekwencji, dlugoscOligo, wierzchStart, powtorzenia)
        {

        }

        public new void updateFeromons(int count)
        {
            base.updateFeromons(count);
            this.parowanie();
        }

        public void parowanie()
        {
            foreach (var Source in graf.Vertices)
            {
                foreach (var Target in graf.Vertices)
                {
                    if (Source == Target)
                    {
                        continue;
                    }

                    graf.Connections[Source][Target].f *= Config.PAROWANIE;
                }
            }
        }
    }
}
