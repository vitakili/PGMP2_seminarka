namespace PGMP2_seminarka.Models
{
    public class Postava
    {
        public string Jmeno { get; set; }
        public string CestaObrazku { get; set; }
        public bool JeNaLodi { get; set; }
        public string Breh { get; set; }

        public string Style { get; set; } = "";

        public Postava(string jmeno, string cestaObrazku, bool jeNaLodi, string breh)
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