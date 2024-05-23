namespace PGMP2_seminarka.Models
{
    public class Lod
    {
        public int KapacitaLodi { get; set; }
        public List<Postava> PostavyNaLodi { get; set; } = new List<Postava>();
        public Breh Breh { get; set; } = Breh.Levy;
        public string Style { get; set; } = "";

        public bool JePlna()
        {
            return PostavyNaLodi.Count >= KapacitaLodi;
        }

        public bool JePrevoznikNaLodi()
        {
            return PostavyNaLodi.Any(p => p.Jmeno == JmenoPostavy.Prevoznik);
        }

    }

}
