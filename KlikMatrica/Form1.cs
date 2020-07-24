/* -------------------------------------------------------------------------- */
/* Clickomania clone (projekat za III godinu)                                 */
/* Autor: Nikola Vukicevic                                                    */
/* 10.05.2017.                                                                */
/* -------------------------------------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KlikMatrica
{
    public partial class Form1 : Form
    {
        KlikMatrica matricaDugmica;

        public Form1()
        {
            InitializeComponent();
            matricaDugmica = new KlikMatrica(16, 10, 40, 40, 1, 1, this);
            matricaDugmica.InicijalizacijaMatricePolja();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                matricaDugmica.NovaMatrica(); 
            }
        }
    }
}
