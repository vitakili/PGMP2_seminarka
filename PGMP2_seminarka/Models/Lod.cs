namespace PGMP2_seminarka.Models
{
    public class Lod
    {
        public int KapacitaLodi { get; set; }
        public List<Postava> PostavyNaLodi { get; set; } = new List<Postava>();
        public string Breh { get; set; } = "levý";

        public bool JePlna()
        {
            return PostavyNaLodi.Count >= KapacitaLodi;
        }

        public bool JePrevoznikNaLodi()
        {
            return PostavyNaLodi.Any(p => p.Jmeno == "Převozník");
        }

    }

}
