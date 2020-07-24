using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace KlikMatrica
{
    class Polje
    {
        public Int32 Red, Kolona, Boja;
        public Boolean Reseno, ImaDobreSusede;
        public Panel Dugme;
        List<Polje> DobriSusedi;
        Random Randomizator;

        public Polje(Int32 RedPolja, Int32 KolonaPolja, Panel DugmePolja, Random RandomizatorPolja)
        {
            Red             = RedPolja;
            Kolona          = KolonaPolja;
            Reseno          = false;
            ImaDobreSusede  = false;
            Dugme           = DugmePolja;
            DobriSusedi     = new List<Polje>();
            Randomizator    = RandomizatorPolja;
            ZadavanjeRandomBoje();
        }

        public void Reset()
        {
            Dugme.Visible  = true;
            Dugme.Enabled  = true;
            Reseno         = false;
            ImaDobreSusede = true;
            DobriSusedi.Clear();
            ZadavanjeRandomBoje();
        }

        public void BojenjeDugmeta()
        {
            switch (Boja)
            {
                case 1: Dugme.BackColor = Color.FromArgb(240, 20 ,  20); break;
                case 2: Dugme.BackColor = Color.FromArgb(80 , 220,  80); break;
                case 3: Dugme.BackColor = Color.FromArgb(100, 120, 220); break;
                case 4: Dugme.BackColor = Color.FromArgb(255, 180,  40); break;
                case 5: Dugme.BackColor = Color.FromArgb(255, 240, 100); break;
                default: break;
            }
        }

        public void ZadavanjeRandomBoje()
        {
            Boja = Randomizator.Next(1, 6);
            BojenjeDugmeta();
        }
    }
}
