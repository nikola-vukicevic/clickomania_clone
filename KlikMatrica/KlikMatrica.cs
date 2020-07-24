using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace KlikMatrica
{
    class KlikMatrica
    {
        private Int32 BrojRedova, BrojKolona, SirinaDugmeta, VisinaDugmeta,
                      HorizontalniRazmak, VertikalniRazmak,
                      GornjiCosakX, GornjiCosakY, LevaGranicaProvere, DesnaGranicaProvere, BrojPreostalih;
        public Boolean IgraUToku;
        private Polje PomocnoPolje;
        private List<Panel> Dugmad;
        private List<Polje> Polja;
        Queue<Polje> RedZaBrisanje;
        private Form NosecaForma;
        private Random GeneratorSlucajnihBrojeva;

        public KlikMatrica(Int32 BrojRedovaMatrice, Int32 BrojKolonaMatrice, Int32 SirinaDugmetaMatrice,
                           Int32 VisinaDugmetaMatrice, Int32 RazmakHor, Int32 RazmakVer,
                           Form FormaZaDugmad)
        {
            BrojRedova                = BrojRedovaMatrice;
            BrojKolona                = BrojKolonaMatrice;
            SirinaDugmeta             = SirinaDugmetaMatrice;
            VisinaDugmeta             = VisinaDugmetaMatrice;
            HorizontalniRazmak        = RazmakHor;
            VertikalniRazmak          = RazmakVer;
            GornjiCosakX              = 4;
            GornjiCosakY              = 4;
            BrojPreostalih            = BrojRedova * BrojKolona;
            IgraUToku                 = false;
            NosecaForma               = FormaZaDugmad;
            Dugmad                    = new List<Panel>();
            Polja                     = new List<Polje>();
            RedZaBrisanje             = new Queue<Polje>();
            GeneratorSlucajnihBrojeva = new Random();    
        }

        private void ProveraKlika(object sender, EventArgs e)
        {
            Panel D = (Panel)sender;
            String s = D.Name.Substring(5);
            Int32 indeksPolja  = Convert.ToInt32(s);

            BrisanjeDobrihSuseda(indeksPolja);
            KonsolidacijaKolona(LevaGranicaProvere, DesnaGranicaProvere);
            PomeranjePoljaUlevo();
            
            ///*
            if (!MozeLiSeDaljeIgrati())
            {
                if (BrojPreostalih == 0)
                {
                    MessageBox.Show("POBEDA!");
                }
                else
                {
                    String poruka = "Igra je gotova.\r\nPreostali broj polja: " + BrojPreostalih.ToString();
                    MessageBox.Show(poruka);
                }
            }
            //*/
        }

        public void NapraviDugme(String Ime, Int32 X, Int32 Y, Int32 Sirina, Int32 Visina)
        {
            Panel D                     = new Panel();
            D.Name                      = Ime;
            D.Text                      = "";
            D.Location                  = new Point(X, Y);
            D.Width                     = Sirina;
            D.Height                    = Visina;
            D.Click                    += new EventHandler(ProveraKlika);
            NosecaForma.Controls.Add(D);
            Dugmad.Add(D);
        }

        public void InicijalizacijaMatricePolja()
        {
            Int32 i, j, X, Y, indeks;
            String ime = "";

            NosecaForma.Controls.Clear();
            Dugmad.Clear();
            Polja.Clear();

            Panel D     = new Panel();
            PomocnoPolje = new Polje(0, 0, D, GeneratorSlucajnihBrojeva);
            Dugmad.Add(D);
            Polja.Add(PomocnoPolje);

            for (i = 1; i <= BrojRedova; i++)
            {
                for (j = 1; j <= BrojKolona; j++)
                {
                    indeks  = (i - 1) * BrojKolona + j;
                    ime     = "dugme" + indeks.ToString();
                    X       = GornjiCosakX + (j - 1) * SirinaDugmeta + (j - 2) * HorizontalniRazmak;
                    Y       = GornjiCosakY + (i - 1) * VisinaDugmeta + (i - 2) * VertikalniRazmak;
                    NapraviDugme(ime, X, Y, SirinaDugmeta, VisinaDugmeta);
                    Polje P = new Polje(i, j, Dugmad[indeks], GeneratorSlucajnihBrojeva);
                    Polja.Add(P);
                }
            }

            NosecaForma.Width  = 4 + BrojKolona * SirinaDugmeta + (BrojKolona - 1) * HorizontalniRazmak + 18;
            NosecaForma.Height = 4 + BrojRedova * VisinaDugmeta + (BrojRedova - 1) * VertikalniRazmak   + 40;
            NosecaForma.MinimumSize = new Size(NosecaForma.Width, NosecaForma.Height);
            NosecaForma.MaximumSize = new Size(NosecaForma.Width, NosecaForma.Height);
            NosecaForma.MaximizeBox = false;
            NosecaForma.Text = "KlikMatrica (" + BrojRedova.ToString() + " x " + BrojKolona.ToString() +
                               ") - " + BrojPreostalih.ToString();
        }

        public void NovaMatrica()
        {
            Int32 i, j, indeks;

            for (i = 1; i <= BrojRedova; i++)
            {
                for (j = 1; j <= BrojKolona; j++)
                {
                    indeks = (i - 1) * BrojKolona + j;
                    Polje P = Polja[indeks];
                    P.Reset();
                }
            }

            IgraUToku    = false;
            BrojPreostalih = BrojRedova * BrojKolona;
            NosecaForma.Text = "KlikMatrica (" + BrojRedova.ToString() + " x " + BrojKolona.ToString() +
                               ") - " + BrojPreostalih.ToString();
        }

        private void ObradaSusednogPolja(Int32 Red, Int32 Kolona, Int32 PrethodnaBoja)
        {
            if (Red >= 1 && Red <= BrojRedova && Kolona >= 1 && Kolona <= BrojKolona)
            {
                Int32 Indeks = (Red - 1) * BrojKolona + Kolona;
                Polje P = Polja[Indeks];

                if (P.Boja == PrethodnaBoja && !P.Reseno)
                {
                    P.Reseno = true;
                    P.Dugme.Visible = false;
                    P.Dugme.Enabled = false;
                    if (P.Kolona < LevaGranicaProvere)  LevaGranicaProvere  = P.Kolona;
                    if (P.Kolona > DesnaGranicaProvere) DesnaGranicaProvere = P.Kolona;
                    RedZaBrisanje.Enqueue(P);
                    BrojPreostalih--;
                    NosecaForma.Text = "KlikMatrica (" + BrojRedova.ToString() + " x " + BrojKolona.ToString() +
                               ") - " + BrojPreostalih.ToString();
                }
            }
        }

        private void BrisanjeDobrihSuseda(Int32 Indeks)
        {
            Polje Pom = Polja[Indeks];
            RedZaBrisanje.Enqueue(Pom);
            LevaGranicaProvere = DesnaGranicaProvere = Pom.Kolona;

            while (RedZaBrisanje.Count > 0)
            {
                Polje P = RedZaBrisanje.Dequeue();
                Int32 red = P.Red, kol = P.Kolona, red_pom, kol_pom;

                red_pom = red - 1; kol_pom = kol;
                ObradaSusednogPolja(red_pom, kol_pom, P.Boja);

                red_pom = red; kol_pom = kol - 1;
                ObradaSusednogPolja(red_pom, kol_pom, P.Boja);

                red_pom = red; kol_pom = kol + 1;
                ObradaSusednogPolja(red_pom, kol_pom, P.Boja);

                red_pom = red + 1; kol_pom = kol;
                ObradaSusednogPolja(red_pom, kol_pom, P.Boja);
            }
        }

        private void DelimicnaZamenaDvaPolja(Polje P1, Polje P2)
        {
            PomocnoPolje.Boja          = P1.Boja;
            PomocnoPolje.Dugme.Visible = P1.Dugme.Visible;
            PomocnoPolje.Dugme.Enabled = P1.Dugme.Enabled;
            PomocnoPolje.Reseno        = P1.Reseno;

            P1.Boja                    = P2.Boja;
            P1.Dugme.Visible           = P2.Dugme.Visible;
            P1.Dugme.Enabled           = P2.Dugme.Enabled;
            P1.Reseno                  = P2.Reseno;

            P2.Boja                    = PomocnoPolje.Boja;
            P2.Dugme.Visible           = PomocnoPolje.Dugme.Visible;
            P2.Dugme.Enabled           = PomocnoPolje.Dugme.Enabled;
            P2.Reseno                  = PomocnoPolje.Reseno;

            P1.BojenjeDugmeta(); P2.BojenjeDugmeta();
        }

        private void DelimicnaZamenaDveKolone(Int32 LevaKolona, Int32 DesnaKolona)
        {
            for (Int32 i = BrojRedova; i >= 1; i--)
            {
                Polje PL = Polja[(i - 1) * BrojKolona + LevaKolona];
                Polje PD = Polja[(i - 1) * BrojKolona + DesnaKolona];
                DelimicnaZamenaDvaPolja(PL, PD);
            }
        }

        private void KonsolidacijaKolone(Int32 Kolona)
        {
            Int32 i, g;
            Stack<Polje> stekPolja = new Stack<Polje>();
            for (i = 1; i <= BrojRedova; i++)
            {
                Polje P = Polja[(i - 1) * BrojKolona + Kolona];
                if (!P.Reseno)
                {
                    stekPolja.Push(P);
                }
            }
            
            g = BrojRedova - stekPolja.Count;
            
            for (i = BrojRedova; i > g; i--)
            {
                Polje P1 = stekPolja.Pop();
                Polje P2 = Polja[(i - 1) * BrojKolona + Kolona];
                DelimicnaZamenaDvaPolja(P1, P2);
            }
            ///*
            for (i = 1; i <= g; i++)
            {
                Polje P = Polja[(i - 1) * BrojKolona + Kolona];
                P.Reseno = true;
                P.Dugme.Visible = P.Dugme.Enabled = false;
            }
            //*/
        }

        private Boolean KolonaPrazna(Int32 Kolona)
        {

            Int32 indeksPolja = (BrojRedova - 1) * BrojKolona + Kolona;
            Polje P = Polja[indeksPolja];
            return P.Reseno;
        }

        private void PomeranjePoljaUlevoZaJednoMesto(Int32 LevaKolona)
        {
            for (Int32 i = LevaKolona; i < BrojKolona; i++)
            {
                DelimicnaZamenaDveKolone(i, i + 1);
            }
 
        }

        private void PomeranjePoljaUlevo()
        {
            if (BrojPreostalih == 0) return;
            
            for (Int32 i = LevaGranicaProvere; i <= DesnaGranicaProvere; i++)
            {
                Int32 brojac = 1, granica = DesnaGranicaProvere - LevaGranicaProvere + 1;
                while (KolonaPrazna(i) && brojac <= granica)
                {
                    PomeranjePoljaUlevoZaJednoMesto(i);
                    brojac++;
                }
            }
        }

        public void KonsolidacijaKolona(Int32 LevaKolona, Int32 DesnaKolona)
        {
            for (Int32 i = LevaKolona; i <= DesnaKolona; i++)
            {
                KonsolidacijaKolone(i);
            }
        }

        private Boolean ImaLiDobrihSuseda(Polje P1)
        {
            Polje P2;
            Int32 red_p, kol_p, ind_p;

            if (P1.Reseno) return false;

            red_p = P1.Red - 1; kol_p = P1.Kolona; ind_p = (red_p - 1) * BrojKolona + kol_p;
            if (red_p >= 1 && red_p <= BrojRedova && kol_p >= 1 && kol_p <= BrojKolona)
            {
                P2 = Polja[ind_p];
                if (P1.Boja == P2.Boja && !P2.Reseno) return true;
            }
                        
            red_p = P1.Red; kol_p = P1.Kolona - 1; ind_p = (red_p - 1) * BrojKolona + kol_p;
            if (red_p >= 1 && red_p <= BrojRedova && kol_p >= 1 && kol_p <= BrojKolona)
            {
                P2 = Polja[ind_p];
                if (P1.Boja == P2.Boja && !P2.Reseno) return true;
            }
                        
            red_p = P1.Red; kol_p = P1.Kolona + 1; ind_p = (red_p - 1) * BrojKolona + kol_p;
            if (red_p >= 1 && red_p <= BrojRedova && kol_p >= 1 && kol_p <= BrojKolona)
            {
                P2 = Polja[ind_p];
                if (P1.Boja == P2.Boja && !P2.Reseno) return true;
            }
            
            red_p = P1.Red + 1; kol_p = P1.Kolona; ind_p = (red_p - 1) * BrojKolona + kol_p;
            if (red_p >= 1 && red_p <= BrojRedova && kol_p >= 1 && kol_p <= BrojKolona)
            {
                P2 = Polja[ind_p];
                if (P1.Boja == P2.Boja && !P2.Reseno) return true;
            }

            return false;
        }

        public Int32 PrebrojPoljaSaDobrimSusedima()
        {
            Int32 psds = 0;
            foreach (Polje P in Polja)
            {
                if (ImaLiDobrihSuseda(P))
                {
                    psds++;
                }
            }
            return psds;
        }

        private Boolean MozeLiSeDaljeIgrati()
        {
            return PrebrojPoljaSaDobrimSusedima() > 0;
        }
    }
}
