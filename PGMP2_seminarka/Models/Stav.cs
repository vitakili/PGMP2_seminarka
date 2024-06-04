namespace PGMP2_seminarka.Models
{
    public class Stav
    {
        public string Nazev { get; set; }
        public List<JmenoPostavy> PostavyNaLevemBrehu { get; set; }
        public List<JmenoPostavy> PostavyNaPravemBrehu { get; set; }
        public bool LodJeNaPravemBrehu { get; set; }
    }
}
