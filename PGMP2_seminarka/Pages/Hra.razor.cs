using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PGMP2_seminarka.Models;
using System.Reflection;

namespace PGMP2_seminarka.Pages
{
    public partial class Hra
    {
        string zprava = "";
        string konecnaHlaska = "";

        List<Postava> postavy = new List<Postava>
    {
        new Postava(JmenoPostavy.Prevoznik, "../img/prevoznik.png", false, Breh.Levy ),
        new Postava(JmenoPostavy.Zeli, "../img/zeli.png", false, Breh.Levy ),
        new Postava(JmenoPostavy.Koza, "../img/koza.png", false, Breh.Levy ),
        new Postava(JmenoPostavy.Vlk, "../img/vlk.png", false, Breh.Levy ),
        };

        Lod lod = new Lod { KapacitaLodi = 2 };
        List<Stav> stavy = new List<Stav>();
        Stav AktualniStav { get; set; }

        bool konecHry = false;
        bool vyhra = false;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            // Nastaví základní styl pro všechny postavy
            for (int i = 0; i < postavy.Count; i++)
            {
                NastavStylPostavy(postavy[i], i);
            }

            // Inicializujte seznam stavů s počátečním stavem, kde jsou všechny postavy na levém břehu.
            stavy = new List<Stav>
                {
                    new Stav
                    {
                        PostavyNaLevemBrehu = postavy.Select(p => p.Jmeno).ToList(),
                        PostavyNaPravemBrehu = new List<JmenoPostavy>(),
                        LodJeNaPravemBrehu = false
                    }
                };
            AktualniStav = stavy.First();
        }

        void AktualizovatHru()
        {
            for (int i = 0; i < postavy.Count; i++)
            {
                NastavStylPostavy(postavy[i], i);
            }

            var postavyNaLevemBrehu = postavy.Where(p => p.Breh == Breh.Levy).ToList();
            var postavyNaPravemBrehu = postavy.Where(p => p.Breh == Breh.Pravy).ToList();

            if ((postavyNaLevemBrehu.Any(p => p.Jmeno == JmenoPostavy.Vlk) && postavyNaLevemBrehu.Any(p => p.Jmeno == JmenoPostavy.Koza) && !postavyNaLevemBrehu.Any(p => p.Jmeno == JmenoPostavy.Prevoznik)) ||
                (postavyNaPravemBrehu.Any(p => p.Jmeno == JmenoPostavy.Vlk) && postavyNaPravemBrehu.Any(p => p.Jmeno == JmenoPostavy.Koza) && !postavyNaPravemBrehu.Any(p => p.Jmeno == JmenoPostavy.Prevoznik)))
            {
                konecnaHlaska = "Hra končí, vlk sežral kozu";
                konecHry = true;
            }

            if ((postavyNaLevemBrehu.Any(p => p.Jmeno == JmenoPostavy.Koza) && postavyNaLevemBrehu.Any(p => p.Jmeno == JmenoPostavy.Zeli) && !postavyNaLevemBrehu.Any(p => p.Jmeno == JmenoPostavy.Prevoznik)) ||
                (postavyNaPravemBrehu.Any(p => p.Jmeno == JmenoPostavy.Koza) && postavyNaPravemBrehu.Any(p => p.Jmeno == JmenoPostavy.Zeli) && !postavyNaPravemBrehu.Any(p => p.Jmeno == JmenoPostavy.Prevoznik)))
            {
                konecnaHlaska = "Hra končí, koza sežrala obří zelí";
                konecHry = true;
            }

            if (postavy.All(p => p.Breh == Breh.Pravy) && postavy.All(p => !p.JeNaLodi))
            {
                konecnaHlaska = "Gratulujeme, vyhrál jste!";
                vyhra = true;
                konecHry = true;
            }
            if (konecHry)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    await RestartovatHru();
                });
            }
        }

        async Task RestartovatHru()
        {
            string message = $"{konecnaHlaska}\nChcete restartovat hru?";
            bool confirm = await JSRuntime.InvokeAsync<bool>("confirm", message);
            if (confirm)
            {
                await JSRuntime.InvokeVoidAsync("eval", "location.reload()");
            }
        }


        void NastavStylPostavy(Postava postava, int index)
        {
            // Nastaví styl postavy na základě jejího stavu
            postava.Style = postava.JeNaLodi
                ? $"left: {(lod.Breh == Breh.Pravy ? 600 + lod.PostavyNaLodi.IndexOf(postava) * 60 : 380 + lod.PostavyNaLodi.IndexOf(postava) * 60)}px; transform: translate(0, -10px);"
                : $"left: {(postava.Breh == Breh.Pravy ? 800 + index * 60 : 80 + index * 60)}px;";
        }

        void PresunNaLod(Postava postava)
        {
            if (postava.JeNaLodi)
            {
                // Pokud je postava již na lodi, odstraní ji z lodi
                postava.JeNaLodi = false;
                lod.PostavyNaLodi.Remove(postava);

                // Pokud je loď na opačném břehu, změníme stav
                if ((lod.Breh == Breh.Levy && AktualniStav.PostavyNaPravemBrehu.Contains(postava.Jmeno)) ||
                    (lod.Breh == Breh.Pravy && AktualniStav.PostavyNaLevemBrehu.Contains(postava.Jmeno)))
                {
                    postava.Breh = lod.Breh;
                    ZmenitStav(postava);
                }
            }
            else if (lod.PostavyNaLodi.Count < lod.KapacitaLodi && postava.Breh == lod.Breh)
            {
                // Pokud postava není na lodi, loď není plná a postava je na stejném břehu jako loď, přidá postavu na loď
                postava.JeNaLodi = true;
                lod.PostavyNaLodi.Add(postava);
            }
            else
            {
                zprava = "Loď je plná nebo postava je na opačném břehu!";
            }

            AktualizovatHru();
        }

        void ZmenitStav(Postava postava)
        {
            if (AktualniStav != null)
            {
                var novyStav = new Stav
                {
                    PostavyNaLevemBrehu = new List<JmenoPostavy>(AktualniStav.PostavyNaLevemBrehu),
                    PostavyNaPravemBrehu = new List<JmenoPostavy>(AktualniStav.PostavyNaPravemBrehu),
                    LodJeNaPravemBrehu = lod.Breh == Breh.Pravy ? true : false
                };

                if (postava.Breh == Breh.Levy)
                {
                    novyStav.PostavyNaPravemBrehu.Remove(postava.Jmeno);
                    novyStav.PostavyNaLevemBrehu.Add(postava.Jmeno);
                }
                else
                {
                    novyStav.PostavyNaLevemBrehu.Remove(postava.Jmeno);
                    novyStav.PostavyNaPravemBrehu.Add(postava.Jmeno);
                }

                // Přidejte nový stav do seznamu stavů.
                stavy.Add(novyStav);

                // Nastavte nový stav jako aktuální stav.
                AktualniStav = novyStav;
            }
        }
       private void VyploutLodi()
        {
            if (lod.JePrevoznikNaLodi())
            {
                // Přesune loď na druhý břeh
                if (lod.Breh == Breh.Levy)
                {
                    lod.Breh = Breh.Pravy;
                    lod.Style = "transform: scaleX(1);";
                }
                else
                {
                    lod.Breh = Breh.Levy;
                    lod.Style = "";
                }

                // Přesune všechny postavy na lodi na druhý břeh
                foreach (var postava in postavy)
                {
                    if (postava.JeNaLodi)
                    {
                        postava.Breh = lod.Breh;
                    }
                }
                zprava = "Loď vyplula!";
            }
            else
            {
                zprava = "Na lodi musí být převozník, aby mohla vyplout!";
            }

            AktualizovatHru();
        }
        public string GetCeskeJmeno(JmenoPostavy jmeno)
        {
            switch (jmeno)
            {
                case JmenoPostavy.Vlk:
                    return "Vlk";
                case JmenoPostavy.Koza:
                    return "Koza";
                case JmenoPostavy.Zeli:
                    return "Obří zelí";
                case JmenoPostavy.Prevoznik:
                    return "Převozník";
                default:
                    throw new ArgumentException("Neznámé jméno postavy");
            }
        }
    }

}
