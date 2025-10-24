using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using KPP_Alpha1.HelperClasses;

namespace KPP_Alpha1
{
    class EditClass
    {
        /// <summary>
        /// EditClasss služi za poruke korisnicima.
        /// Metode za provjeru textBoxova koji su prazni
        /// i uloge korisnika aplikacije.
        /// </summary>

        // Stringovi s kojima se rade poruke
        public string PraznaCelija = "Ćelije ne smiju biti prazne!";
        public string CelijaNazivUpozorenje = "Upozorenje";
        public string CelijaNazivObavjest = "Obavjest";
        public string IzmjenaError = "Izmjena nije izvršena!";
        public string UnosError = "Unos nije napravljen!";
        public string IdError = "ID stavke nije pronađen!\n\nProvjeri polja na greške u malim i velikim slovima.\n\n" +
            "Ako željeni naziv ili korisnik nije ponuđen dodaj ga u bazu.";
        public string ExError = "Dogodila se greška:\n\n";

        #region USER MESSAGES
        // Poruke korisniku
        // Prazna ćelija upozorava da ista ne smije biti prazna
        internal void PorukaPraznaCelija()
        {
            MessageBox.Show(PraznaCelija, CelijaNazivUpozorenje);
        }
        // INSERT u bazu nije napravljen
        internal void MessageDBErrorInsert()
        {
            MessageBox.Show(UnosError, CelijaNazivUpozorenje);
        }
        // UPDATE nije napravljen
        internal void MessageDBErrorUpdate()
        {
            MessageBox.Show(IzmjenaError, CelijaNazivUpozorenje);
        }
        // EX error poruka koristi se u try-catch bloku
        internal void MessageException(Exception ex)
        {
            MessageBox.Show(ExError + ex.Message + $"\n\n Stack Trace:" + ex.StackTrace, CelijaNazivUpozorenje);
        }
        // Dictionary KEY ako nije nađen koji se koristi kao forigne key u tablicama
        internal void MessageErrorKeyMissing()
        {
            MessageBox.Show(IdError, CelijaNazivUpozorenje);
        }
        // Poruka korisniku da nije pronađen u aplikaciji kod logiranja ili da nema prava pristupa
        internal void MessageAccessDenied()
        {
            MessageBox.Show("Korisničko ime ili lozinka nisu točni. Pokušaj ponovno!\n" +
                        "Ili nisi ovlašten za korištenje aplikacije.", CelijaNazivUpozorenje);
        }
        internal void MessagePasswordMissmatch()
        {
            MessageBox.Show("Lozinke se ne podudaraju!", CelijaNazivUpozorenje);
        }

        internal void MessagePasswordChanged()
        {
            MessageBox.Show("Lozinka je promjenjena.\n\nPrijavite se sa novom lozinkom", CelijaNazivObavjest);
        }

        #endregion

        #region METODE ZA PROVJERU PRAZNIH ČELIJA
        // Metoda za promjenu pozdaniske boje da se ukaže korisniku koji textBox mora biti ispunjen
        internal void BojaPozadineZaPrazneCeliji(TextBox textBox)
        {
            if (textBox.Text == "")
            {
                textBox.BackColor = Color.LightPink;
            }
            else
            {
                textBox.BackColor = Color.White;
            }
        }
        // Metoda za prvjeru prazne čelije koristi se prije metode za promjenu boje i svaki put kad se žele vrati boja u bijelo,
        // ako je uvijet zadovoljen
        internal bool NullOrWhite(TextBox textBox)
        {
            return string.IsNullOrWhiteSpace(textBox.Text);
        }
        #endregion

        #region NEW STATIC VALIDATION METHODS (T002-T015)
        
        /// <summary>
        /// Validates that a TextBox contains a value. Highlights field pink if empty.
        /// </summary>
        /// <param name="txt">TextBox to validate</param>
        /// <param name="fieldName">Croatian name of field for error message</param>
        /// <param name="errorMsg">Output: error message if validation fails</param>
        /// <returns>True if valid, false if empty</returns>
        public static bool ValidateRequired(TextBox txt, string fieldName, out string errorMsg)
        {
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                txt.BackColor = Color.LightPink;
                errorMsg = $"{fieldName} je obavezno polje!";
                return false;
            }
            txt.BackColor = Color.White;
            errorMsg = string.Empty;
            return true;
        }

        /// <summary>
        /// Validates that a ComboBox has a selection. Highlights pink if not.
        /// </summary>
        public static bool ValidateComboBox(ComboBox cmb, string fieldName, out string errorMsg)
        {
            if (cmb.SelectedValue == null || cmb.SelectedValue.ToString() == "0")
            {
                cmb.BackColor = Color.Pink;
                errorMsg = $"{fieldName} je obavezno polje!";
                return false;
            }
            cmb.BackColor = Color.White;
            errorMsg = string.Empty;
            return true;
        }

        /// <summary>
        /// Validates OIB (Croatian tax ID) format: exactly 11 digits.
        /// </summary>
        public static bool ValidateOib(string oib, out string errorMsg)
        {
            if (string.IsNullOrWhiteSpace(oib))
            {
                errorMsg = "OIB je obavezan!";
                return false;
            }
            
            if (oib.Length != 11 || !oib.All(char.IsDigit))
            {
                errorMsg = "OIB mora imati točno 11 znamenaka!";
                return false;
            }
            
            errorMsg = string.Empty;
            return true;
        }

        /// <summary>
        /// Validates that a string contains only digits.
        /// </summary>
        public static bool ValidateNumeric(string value, string fieldName, out string errorMsg)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.All(char.IsDigit))
            {
                errorMsg = $"{fieldName} mora sadržavati samo brojeve!";
                return false;
            }
            
            errorMsg = string.Empty;
            return true;
        }

        /// <summary>
        /// Validates that end date is after start date.
        /// </summary>
        public static bool ValidateDateRange(DateTime startDate, DateTime endDate, 
            string startLabel, string endLabel, out string errorMsg)
        {
            if (endDate < startDate)
            {
                errorMsg = $"{endLabel} mora biti nakon {startLabel}!";
                return false;
            }
            
            errorMsg = string.Empty;
            return true;
        }

        /// <summary>
        /// Validates that a date is not in the future.
        /// </summary>
        public static bool ValidateNotFutureDate(DateTime date, string fieldName, out string errorMsg)
        {
            if (date > DateTime.Now)
            {
                errorMsg = $"{fieldName} ne može biti u budućnosti!";
                return false;
            }
            
            errorMsg = string.Empty;
            return true;
        }

        /// <summary>
        /// Safely converts object to string, handling null and DBNull.
        /// </summary>
        public static string SafeString(object value, string defaultValue = "")
        {
            return value == null || value == DBNull.Value 
                ? defaultValue 
                : value.ToString().Trim();
        }

        /// <summary>
        /// Safely converts object to int, handling null and DBNull.
        /// </summary>
        public static int SafeInt(object value, int defaultValue = 0)
        {
            if (value == null || value == DBNull.Value)
                return defaultValue;
            
            if (int.TryParse(value.ToString(), out int result))
                return result;
            
            return defaultValue;
        }

        /// <summary>
        /// Safely converts object to DateTime, handling null and DBNull.
        /// </summary>
        public static DateTime? SafeDateTime(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;
            
            if (DateTime.TryParse(value.ToString(), out DateTime result))
                return result;
            
            return null;
        }

        /// <summary>
        /// Logs exception details for debugging while displaying user-friendly message.
        /// </summary>
        public static void LogException(string source, Exception ex)
        {
            LogHelper.LogError(source, $"User: {LoginHelper.StaticUsername}", ex);
        }

        #endregion
    }
}
