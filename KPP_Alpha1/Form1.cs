using KPP_Alpha1.HelperClasses;
using System;
using System.Windows.Forms;

namespace KPP_Alpha1
{
    public partial class Form1 : Form
    {
        readonly FormLogin LogFrm = new FormLogin();

        //O aplikaciji
        string MB_NazivAplikacija = "O aplikaciji KPP";
        string appName = "Knjiga primljenih pošiljki (KPP)";
        string appVersion = "Version 4.0.0.0";
        string appAuthor = "Miroslav Vranić";
        string aboutApp = "Aplikcaija Knjiga primljenih pošiljki je napravaljena kao projekt na predmetu Objektno orijentirano programiranje 2 " +
            "na TVZ-u godina 2017/2018";

        public Form1()
        {
            InitializeComponent();
        }

        #region Nove forme
        private void FormaKPP()
        {
            FormKPP form = new FormKPP();
            Forma(form);
        }
        private void FormaKorisnici()
        {
            FormKorisnici form = new FormKorisnici();
            Forma(form);
        }
        private void FormaOdjeli()
        {
            FormOdjeli form = new FormOdjeli();
            Forma(form);
        }
        private void FormaPosiljatelji()
        {
            FormPosiljatelji form = new FormPosiljatelji();
            Forma(form);
        }
        private void FormaMjesta()
        {
            FormMjesta form = new FormMjesta();
            Forma(form);
        }
        private void FormaIzvoz()
        {
            FormExportData form = new FormExportData();
            Forma(form);
        }

        private void FormaDjelatnici()
        {
            FormDjelatnici form = new FormDjelatnici();
            Forma(form);
        }

        private void FormaPartneri()
        {
            FormPartneri form = new FormPartneri();
            Forma(form);
        }

        private void FormaPoslane()
        {
            FormPoslane form = new FormPoslane();
            Forma(form);
        }
        
        private void FormaOvlastenja()
        {
            FormOvlastenja form = new FormOvlastenja();
            Forma(form);
        }

        private void Forma(Form form)
        {
            form.Show();
            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
        }
        #endregion

        #region Tool strip menu item click events
        private void otvoriKPPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
                FormaKPP();
            }
            else
            {
                FormaKPP();
            }
        }
        private void dodajKorisnikaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
                FormaKorisnici();
            }
            else
            {
                FormaKorisnici();
            }
        }
        private void dodajOdjelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
                FormaOdjeli();
            }
            else
            {
                FormaOdjeli();
            }
        }
        private void pošiljateljiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
                FormaPosiljatelji();
            }
            else
            {
                FormaPosiljatelji();
            }
        }
        private void mjestaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
                FormaMjesta();
            }
            else
            {
                FormaMjesta();
            }
        }
        private void izvozPodatakaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
                FormaIzvoz();
            }
            else
            {
                FormaIzvoz();
            }
        }

        private void djelatniciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
                FormaDjelatnici();
            }
            else
            {
                FormaDjelatnici();
            }
        }

        private void partneriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
                FormaPartneri();
            }
            else
            {
                FormaPartneri();
            }
        }

        private void knjigaPoslanihPošiljkiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
                FormaPoslane();
            }
            else
            {
                FormaPoslane();
            }
        }

        private void ovlastenjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
                FormaOvlastenja();
            }
            else
            {
                FormaOvlastenja();
            }
        }
        private void iTOpremaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormOprema();
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
                Forma(form);
            }
            else
            {
                Forma(form);
            }
        }

        private void zaRaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new FormZaRa();
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
                Forma(form);
            }
            else
            {
                Forma(form);
            }
        }
        private void vozilaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormVozila();
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
                Forma(form);
            }
            else
            {
                Forma(form);
            }
        }

        private void podatkovneKarticeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormDataCards();
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
                Forma(form);
            }
            else
            {
                Forma(form);
            }
        }
        private void karticeZaUlazToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormKeyCards();
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
                Forma(form);
            }
            else
            {
                Forma(form);
            }
        }

        private void eNCUređajiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormEnc();
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
                Forma(form);
            }
            else
            {
                Forma(form);
            }
        }

        private void izađiIzAplikacijeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void zatvoriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild == null)
            {
                this.Close();
            }
            else
            {
                ActiveMdiChild.Close();
            }
        }
        private void oAplikacijiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Aplikacija:\t\t" + appName + "\nVerzija aplikacije:\t" + appVersion +
                "\nAutor aplikacije:\t" + appAuthor + "\n\nO aplikaciji:\n" + aboutApp, MB_NazivAplikacija);
        }
        #endregion

        // Metoda load prekida otvaranje aplikacije i poziva formu za prijavu/autentifikaciju korisnika
        // prava korisnika i prikazivanje elemenata po pravima
        private void form_Main_Load(object sender, EventArgs e)
        {
            using (FormLogin LogForm = new FormLogin())
            {
                if (LogFrm.ShowDialog() != DialogResult.OK)
                {
                    Application.Exit();
                    return;
                }
                else
                {
                    if (LoginHelper.StaticUloga == "Administrator")
                    {
                        dodajKorisnikaToolStripMenuItem.Visible = true;                        
                    }
                    tssl_korisnik.Text = LoginHelper.StaticKorisnik;
                }
            }
        }

        
    }
}
