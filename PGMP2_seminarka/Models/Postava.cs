namespace PGMP2_seminarka.Models
{
    public class Postava
    {
        public JmenoPostavy Jmeno { get; set; }
        public string CestaObrazku { get; set; }
        public bool JeNaLodi { get; set; }
        public Breh Breh { get; set; }

        public string Style { get; set; } = "";

        public Postava(JmenoPostavy jmeno, string cestaObrazku, bool jeNaLodi, Breh breh)
        {
            Jmeno = jmeno;
            CestaObrazku = cestaObrazku;
            JeNaLodi = jeNaLodi;
            Breh = breh;
        }

        public bool JeNaStejnemMiste(Postava jinaPostava)
        {
            return this.Breh == jinaPostava.Breh;
        }
    }
}