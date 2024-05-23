using PGMP2_seminarka.Models;

namespace PGMP2_seminarka.Pages
{
    public partial class Hra
    {
        string zprava = "";

        List<Postava> postavy = new List<Postava>
    {
        new Postava("Převozník", "../img/prevoznik.png", false, "levý" ),
        new Postava("Zelí", "../img/zeli.png", false, "levý" ),
        new Postava("Koza", "../img/koza.png", false, "levý" ),
        new Postava("Vlk", "../img/vlk.png", false, "levý" ),
        };

        Lod lod = new Lod { KapacitaLodi = 2};

        bool jePlna;

        void AktualizovatHru()
        {
            // Zde můžete provést potřebné změny ve hře...

            // Nastavte styl pro všechny postavy
            for (int i = 0; i < postavy.Count; i++)
            {
                NastavStylPostavy(postavy[i], i);
            }
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            jePlna = lod.JePlna();
            // Nastavte základní styl pro všechny postavy
            for (int i = 0; i < postavy.Count; i++)
            {
                NastavStylPostavy(postavy[i], i);
            }
        }

        void NastavStylPostavy(Postava postava, int index)
        {
            // Nastavte styl postavy na základě jejího stavu
            postava.Style = postava.JeNaLodi
                ? $"left: {(lod.Breh == "pravý" ? 500 + lod.PostavyNaLodi.IndexOf(postava) * 60 : 310 + lod.PostavyNaLodi.IndexOf(postava) * 60)}px"
                : $"left: {(postava.Breh == "pravý" ? 800 + index * 60 : 80 + index * 60)}px";
        }

        void PresunNaLod(Postava postava)
        {
            if (postava.JeNaLodi)
            {
                // Pokud je postava již na lodi, odstraňte ji z lodi
                postava.JeNaLodi = false;
                lod.PostavyNaLodi.Remove(postava);
            }
            else if (lod.PostavyNaLodi.Count < lod.KapacitaLodi && postava.Breh == lod.Breh)
            {
                // Pokud postava není na lodi, loď není plná a postava je na stejném břehu jako loď, přidejte postavu na loď
                postava.JeNaLodi = true;
                lod.PostavyNaLodi.Add(postava);
            }
            else
            {
                zprava = "Loď je plná nebo postava je na opačném břehu!";
            }
            AktualizovatHru();

        }
        void VyplutLodi()
        {
            if (lod.JePrevoznikNaLodi())
            {
                // Přesuňte loď na druhý břeh
                lod.Breh = lod.Breh == "levý" ? "pravý" : "levý";

                // Přesuňte všechny postavy na lodi na druhý břeh
                foreach (var postava in postavy)
                {
                    if (postava.JeNaLodi)
                    {
                        postava.Breh = lod.Breh;
                    }
                }

                // Aktualizujte stav hry
                // ...
                zprava = "Loď vyplula!";
            }
            else
            {
                zprava = "Na lodi musí být převozník, aby mohla vyplout!";
            }
            AktualizovatHru();
        }
    }
}
