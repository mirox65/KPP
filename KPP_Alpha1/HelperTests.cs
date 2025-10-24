using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KPP_Alpha1.HelperClasses;

namespace KPP_Alpha1
{
    /// <summary>
    /// Temporary test form for validating Phase 1 helper methods (T016, T020)
    /// This form can be deleted after Phase 1 verification is complete
    /// </summary>
    public partial class HelperTests : Form
    {
        public HelperTests()
        {
            InitializeComponent();
        }

        private void HelperTests_Load(object sender, EventArgs e)
        {
            TestEditClassMethods();
            TestDbClassMethods();
            TestLogHelper();
            
            MessageBox.Show("All helper tests completed! Check the output and Logs folder.", 
                "Test Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// T016: Test all EditClass validation methods
        /// </summary>
        private void TestEditClassMethods()
        {
            LogHelper.LogInfo("HelperTests", "=== Testing EditClass Methods (T016) ===");

            // Test ValidateRequired
            TextBox txtTest = new TextBox();
            txtTest.Text = "";
            string errorMsg;
            bool result = EditClass.ValidateRequired(txtTest, "Test Field", out errorMsg);
            LogHelper.LogInfo("HelperTests", $"ValidateRequired (empty): {result} - {errorMsg}");

            txtTest.Text = "Valid Value";
            result = EditClass.ValidateRequired(txtTest, "Test Field", out errorMsg);
            LogHelper.LogInfo("HelperTests", $"ValidateRequired (filled): {result}");

            // Test ValidateComboBox
            ComboBox cmbTest = new ComboBox();
            cmbTest.SelectedIndex = -1;
            result = EditClass.ValidateComboBox(cmbTest, "Test Combo", out errorMsg);
            LogHelper.LogInfo("HelperTests", $"ValidateComboBox (not selected): {result} - {errorMsg}");

            cmbTest.Items.Add("Item 1");
            cmbTest.SelectedIndex = 0;
            result = EditClass.ValidateComboBox(cmbTest, "Test Combo", out errorMsg);
            LogHelper.LogInfo("HelperTests", $"ValidateComboBox (selected): {result}");

            // Test ValidateOib
            result = EditClass.ValidateOib("12345678901", out errorMsg);
            LogHelper.LogInfo("HelperTests", $"ValidateOib (valid 11 digits): {result}");

            result = EditClass.ValidateOib("123", out errorMsg);
            LogHelper.LogInfo("HelperTests", $"ValidateOib (invalid length): {result} - {errorMsg}");

            result = EditClass.ValidateOib("1234567890a", out errorMsg);
            LogHelper.LogInfo("HelperTests", $"ValidateOib (contains letters): {result} - {errorMsg}");

            // Test ValidateNumeric
            result = EditClass.ValidateNumeric("12345", "Test Number", out errorMsg);
            LogHelper.LogInfo("HelperTests", $"ValidateNumeric (valid): {result}");

            result = EditClass.ValidateNumeric("123abc", "Test Number", out errorMsg);
            LogHelper.LogInfo("HelperTests", $"ValidateNumeric (invalid): {result} - {errorMsg}");

            // Test ValidateDateRange
            DateTime start = new DateTime(2025, 1, 1);
            DateTime end = new DateTime(2025, 12, 31);
            result = EditClass.ValidateDateRange(start, end, "Start Date", "End Date", out errorMsg);
            LogHelper.LogInfo("HelperTests", $"ValidateDateRange (valid): {result}");

            result = EditClass.ValidateDateRange(end, start, "Start Date", "End Date", out errorMsg);
            LogHelper.LogInfo("HelperTests", $"ValidateDateRange (invalid order): {result} - {errorMsg}");

            // Test ValidateNotFutureDate
            DateTime past = new DateTime(2024, 1, 1);
            result = EditClass.ValidateNotFutureDate(past, "Past Date", out errorMsg);
            LogHelper.LogInfo("HelperTests", $"ValidateNotFutureDate (past): {result}");

            DateTime future = DateTime.Now.AddDays(10);
            result = EditClass.ValidateNotFutureDate(future, "Future Date", out errorMsg);
            LogHelper.LogInfo("HelperTests", $"ValidateNotFutureDate (future): {result} - {errorMsg}");

            // Test SafeString
            string safeStr = EditClass.SafeString(null, "default");
            LogHelper.LogInfo("HelperTests", $"SafeString (null): '{safeStr}'");

            safeStr = EditClass.SafeString(DBNull.Value, "default");
            LogHelper.LogInfo("HelperTests", $"SafeString (DBNull): '{safeStr}'");

            safeStr = EditClass.SafeString("actual value", "default");
            LogHelper.LogInfo("HelperTests", $"SafeString (value): '{safeStr}'");

            // Test SafeInt
            int safeInt = EditClass.SafeInt(null, -1);
            LogHelper.LogInfo("HelperTests", $"SafeInt (null): {safeInt}");

            safeInt = EditClass.SafeInt(DBNull.Value, -1);
            LogHelper.LogInfo("HelperTests", $"SafeInt (DBNull): {safeInt}");

            safeInt = EditClass.SafeInt(42, -1);
            LogHelper.LogInfo("HelperTests", $"SafeInt (value): {safeInt}");

            // Test SafeDateTime
            DateTime? safeDate = EditClass.SafeDateTime(null);
            LogHelper.LogInfo("HelperTests", $"SafeDateTime (null): {(safeDate.HasValue ? safeDate.Value.ToString() : "null")}");

            safeDate = EditClass.SafeDateTime(DBNull.Value);
            LogHelper.LogInfo("HelperTests", $"SafeDateTime (DBNull): {(safeDate.HasValue ? safeDate.Value.ToString() : "null")}");

            safeDate = EditClass.SafeDateTime(new DateTime(2025, 10, 24));
            LogHelper.LogInfo("HelperTests", $"SafeDateTime (value): {(safeDate.HasValue ? safeDate.Value.ToString() : "null")}");

            // Test LogException
            try
            {
                throw new InvalidOperationException("Test exception for logging");
            }
            catch (Exception ex)
            {
                EditClass.LogException("HelperTests", ex);
                LogHelper.LogInfo("HelperTests", "LogException test completed - check logs for exception entry");
            }

            LogHelper.LogInfo("HelperTests", "=== EditClass Tests Complete ===");
        }

        /// <summary>
        /// T020: Test DbClass enhancements
        /// </summary>
        private void TestDbClassMethods()
        {
            LogHelper.LogInfo("HelperTests", "=== Testing DbClass Methods (T020) ===");

            // Test TestConnection
            string errorMsg;
            bool connected = DbClass.TestConnection(out errorMsg);
            LogHelper.LogInfo("HelperTests", $"TestConnection: {connected} - {errorMsg}");

            if (connected)
            {
                try
                {
                    // Test ExecuteScalar - Count records in Odjeli table
                    object countResult = DbClass.ExecuteScalar("SELECT COUNT(*) FROM Odjeli", null);
                    LogHelper.LogInfo("HelperTests", $"ExecuteScalar (COUNT): {countResult}");

                    // Test ExecuteScalar with parameters
                    var parameters = new Dictionary<string, object>
                    {
                        { "@naziv", "Test" }
                    };
                    object paramResult = DbClass.ExecuteScalar("SELECT COUNT(*) FROM Odjeli WHERE Naziv LIKE '%' + @naziv + '%'", parameters);
                    LogHelper.LogInfo("HelperTests", $"ExecuteScalar (with params): {paramResult}");

                    // Test RecordExists - Check if any Odjeli exists
                    // First, get a value to test with
                    object existingNaziv = DbClass.ExecuteScalar("SELECT TOP 1 Naziv FROM Odjeli", null);
                    if (existingNaziv != null)
                    {
                        bool exists = DbClass.RecordExists("Odjeli", "Naziv", existingNaziv);
                        LogHelper.LogInfo("HelperTests", $"RecordExists (existing record): {exists}");
                    }

                    // Test RecordExists with non-existing value
                    bool notExists = DbClass.RecordExists("Odjeli", "Naziv", "NonExistentTestValue12345");
                    LogHelper.LogInfo("HelperTests", $"RecordExists (non-existing): {notExists}");

                    // Test RecordExists with excludeId
                    object existingId = DbClass.ExecuteScalar("SELECT TOP 1 Id FROM Odjeli", null);
                    if (existingId != null && existingNaziv != null)
                    {
                        int id = Convert.ToInt32(existingId);
                        bool existsExcluded = DbClass.RecordExists("Odjeli", "Naziv", existingNaziv, id);
                        LogHelper.LogInfo("HelperTests", $"RecordExists (with excludeId={id}): {existsExcluded}");
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError("HelperTests", "DbClass test error", ex);
                }
            }

            LogHelper.LogInfo("HelperTests", "=== DbClass Tests Complete ===");
        }

        /// <summary>
        /// Test LogHelper functionality (validates T001-T005)
        /// </summary>
        private void TestLogHelper()
        {
            LogHelper.LogInfo("HelperTests", "=== Testing LogHelper ===");
            LogHelper.LogInfo("HelperTests", "LogInfo test message");
            
            try
            {
                throw new InvalidOperationException("Test exception for LogHelper");
            }
            catch (Exception ex)
            {
                LogHelper.LogError("HelperTests", "LogError test message", ex);
            }

            LogHelper.LogInfo("HelperTests", "=== LogHelper Tests Complete ===");
            LogHelper.LogInfo("HelperTests", "Check Logs/KPP_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log file for log entries");
        }
    }

    // Designer code
    partial class HelperTests
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // HelperTests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.Name = "HelperTests";
            this.Text = "Helper Classes Test (T016, T020)";
            this.Load += new System.EventHandler(this.HelperTests_Load);
            this.ResumeLayout(false);
        }
    }
}
