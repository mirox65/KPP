# Helper Class Contracts

**Purpose**: Define the interface contracts for enhanced helper classes to ensure consistency across all form refactoring.

## EditClass Enhancements

### Existing Methods (DO NOT MODIFY)

```csharp
public static class EditClass
{
    // Validation helpers
    public static bool NullOrWhite(string value);
    public static void BojaPozadineZaPrazneCeliji(TextBox txt);
    
    // Error messages
    public static void MessageException(string message);
    public static void MessageDBErrorInsert();
    public static void MessageDBErrorUpdate();
    public static void MessageDBErrorDelete();
    public static void MessageDBErrorPrimaryKeyNotFound();
    public static void MessageDeleteConfirmation();
}
```

### New Methods (ADD during refactoring)

#### Required Field Validation

```csharp
/// <summary>
/// Validates that a TextBox contains a value. Highlights field pink if empty.
/// </summary>
/// <param name="txt">TextBox to validate</param>
/// <param name="fieldName">Croatian name of field for error message</param>
/// <param name="errorMsg">Output: error message if validation fails</param>
/// <returns>True if valid, false if empty</returns>
public static bool ValidateRequired(TextBox txt, string fieldName, out string errorMsg)
{
    if (NullOrWhite(txt.Text))
    {
        BojaPozadineZaPrazneCeliji(txt);
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
```

#### Format Validation

```csharp
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
```

#### Business Logic Validation

```csharp
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
```

#### Null-Safe Helpers

```csharp
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
```

#### Exception Logging

```csharp
/// <summary>
/// Logs exception details for debugging while displaying user-friendly message.
/// </summary>
public static void LogException(string source, Exception ex)
{
    LogHelper.LogError(source, $"User: {LoginHelper.StaticUsername}", ex);
}
```

---

## DbClass Enhancements

### Existing Methods (DO NOT MODIFY)

```csharp
public static class DbClass
{
    public static void ExcecuteNonQuery(string sql, Dictionary<string, object> parameters);
    public static DataTable Select(string sql, Dictionary<string, object> parameters);
}
```

### New Methods (ADD during refactoring)

#### Connection Management

```csharp
/// <summary>
/// Tests database connectivity. Returns true if connection successful.
/// </summary>
public static bool TestConnection(out string errorMsg)
{
    try
    {
        using (var conn = new OleDbConnection(connectionString))
        {
            conn.Open();
            errorMsg = string.Empty;
            return true;
        }
    }
    catch (Exception ex)
    {
        errorMsg = "Baza podataka nije dostupna: " + ex.Message;
        LogHelper.LogError("DbClass.TestConnection", errorMsg, ex);
        return false;
    }
}
```

#### Query Helpers

```csharp
/// <summary>
/// Executes scalar query and returns single value.
/// </summary>
public static object ExecuteScalar(string sql, Dictionary<string, object> parameters)
{
    try
    {
        using (var conn = new OleDbConnection(connectionString))
        using (var cmd = new OleDbCommand(sql, conn))
        {
            if (parameters != null)
            {
                foreach (var param in parameters)
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
            }
            
            conn.Open();
            return cmd.ExecuteScalar();
        }
    }
    catch (Exception ex)
    {
        LogHelper.LogError("DbClass.ExecuteScalar", sql, ex);
        throw;
    }
}

/// <summary>
/// Checks if a record with specified field value exists.
/// </summary>
public static bool RecordExists(string tableName, string fieldName, object fieldValue, int? excludeId = null)
{
    string sql = $"SELECT COUNT(*) FROM {tableName} WHERE {fieldName} = @value";
    
    if (excludeId.HasValue)
        sql += " AND Id <> @excludeId";
    
    var parameters = new Dictionary<string, object>
    {
        { "@value", fieldValue }
    };
    
    if (excludeId.HasValue)
        parameters.Add("@excludeId", excludeId.Value);
    
    int count = Convert.ToInt32(ExecuteScalar(sql, parameters));
    return count > 0;
}
```

#### Transaction Support (Optional - for complex operations)

```csharp
/// <summary>
/// Executes multiple non-query statements in a transaction.
/// </summary>
public static void ExecuteTransaction(List<(string sql, Dictionary<string, object> parameters)> commands)
{
    using (var conn = new OleDbConnection(connectionString))
    {
        conn.Open();
        using (var transaction = conn.BeginTransaction())
        {
            try
            {
                foreach (var (sql, parameters) in commands)
                {
                    using (var cmd = new OleDbCommand(sql, conn, transaction))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                                cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                        
                        cmd.ExecuteNonQuery();
                    }
                }
                
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
```

---

## LogHelper (NEW CLASS)

```csharp
using System;
using System.IO;
using System.Text;

namespace KPP_Alpha1.HelperClasses
{
    /// <summary>
    /// Simple file-based logging for error tracking and debugging.
    /// Logs are written to /Logs directory with daily rotation.
    /// </summary>
    public static class LogHelper
    {
        private static readonly object _lock = new object();
        private static readonly string _logDirectory = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Logs");
        
        static LogHelper()
        {
            if (!Directory.Exists(_logDirectory))
                Directory.CreateDirectory(_logDirectory);
        }
        
        /// <summary>
        /// Logs an error with exception details.
        /// </summary>
        public static void LogError(string source, string message, Exception ex = null);
        
        /// <summary>
        /// Logs an informational message (for debugging).
        /// </summary>
        public static void LogInfo(string source, string message);
        
        /// <summary>
        /// Deletes log files older than specified days (default: 30).
        /// Call this on application startup.
        /// </summary>
        public static void CleanOldLogs(int daysToKeep = 30);
        
        // Private implementation details omitted - see research.md
    }
}
```

---

## Controller Contract Pattern

All Controllers MUST follow this pattern for INSERT and UPDATE operations:

```csharp
public class [Entity]Controller
{
    /// <summary>
    /// Inserts new record with audit field population.
    /// </summary>
    public void Insert([Entity]Model model)
    {
        try
        {
            // REQUIRED: Explicitly set audit fields
            model.KorisnikId = LoginHelper.StaticId;
            model.[TimestampField] = DateTime.Now.Date;  // Use correct field name for this table!
            
            // Construct parameterized query
            string sql = @"INSERT INTO [TableName] (..., korisnikId, [timestampField]) 
                          VALUES (..., @korisnikId, @timestamp)";
            
            var parameters = new Dictionary<string, object>
            {
                // ... all fields including:
                {"@korisnikId", model.KorisnikId},
                {"@timestamp", model.[TimestampField]}
            };
            
            DbClass.ExecuteNonQuery(sql, parameters);
            LogHelper.LogInfo($"{this.GetType().Name}.Insert", 
                $"Inserted {typeof([Entity]Model).Name} by user {LoginHelper.StaticUsername}");
        }
        catch (OleDbException ex)
        {
            LogHelper.LogError($"{this.GetType().Name}.Insert", 
                $"Database error inserting {typeof([Entity]Model).Name}", ex);
            throw new ApplicationException("Greška prilikom spremanja u bazu podataka!");
        }
        catch (Exception ex)
        {
            LogHelper.LogError($"{this.GetType().Name}.Insert", 
                $"Unexpected error inserting {typeof([Entity]Model).Name}", ex);
            throw;
        }
    }
    
    /// <summary>
    /// Updates existing record with audit field population.
    /// </summary>
    public void Update([Entity]Model model)
    {
        try
        {
            // REQUIRED: Update audit fields
            model.KorisnikId = LoginHelper.StaticId;
            model.[TimestampField] = DateTime.Now.Date;
            
            string sql = @"UPDATE [TableName] SET ..., korisnikId = @korisnikId, 
                          [timestampField] = @timestamp WHERE Id = @id";
            
            var parameters = new Dictionary<string, object>
            {
                // ... all fields including:
                {"@korisnikId", model.KorisnikId},
                {"@timestamp", model.[TimestampField]},
                {"@id", model.Id}
            };
            
            DbClass.ExecuteNonQuery(sql, parameters);
            LogHelper.LogInfo($"{this.GetType().Name}.Update", 
                $"Updated {typeof([Entity]Model).Name} Id={model.Id} by user {LoginHelper.StaticUsername}");
        }
        catch (OleDbException ex)
        {
            LogHelper.LogError($"{this.GetType().Name}.Update", 
                $"Database error updating {typeof([Entity]Model).Name}", ex);
            throw new ApplicationException("Greška prilikom ažuriranja!");
        }
        catch (Exception ex)
        {
            LogHelper.LogError($"{this.GetType().Name}.Update", 
                $"Unexpected error updating {typeof([Entity]Model).Name}", ex);
            throw;
        }
    }
}
```

---

## Form Event Handler Contract Pattern

All Form CRUD event handlers MUST follow this pattern:

```csharp
public partial class [Entity]Form : Form
{
    private void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            // Step 1: Validate all required fields
            List<string> errors = new List<string>();
            
            if (!EditClass.ValidateRequired(txtField1, "Polje 1", out string err1))
                errors.Add(err1);
            
            if (!EditClass.ValidateRequired(txtField2, "Polje 2", out string err2))
                errors.Add(err2);
            
            // ... additional validations
            
            if (errors.Count > 0)
            {
                EditClass.MessageException(string.Join("\n", errors));
                return;
            }
            
            // Step 2: Check business rules (e.g., uniqueness)
            if (DbClass.RecordExists("TableName", "FieldName", txtField.Text))
            {
                EditClass.MessageException("Vrijednost već postoji!");
                txtField.BackColor = Color.Pink;
                return;
            }
            
            // Step 3: Create model and call controller
            var model = CreateModelFromForm();
            controller.Insert(model);
            
            // Step 4: Success feedback
            RefreshDataGrid();
            ClearForm();
            EditClass.MessageException("Uspješno spremljeno!");
        }
        catch (ApplicationException ex)
        {
            // Business logic exception with user-friendly message
            EditClass.MessageException(ex.Message);
        }
        catch (Exception ex)
        {
            // Unexpected exception - log and display generic message
            EditClass.LogException($"{this.GetType().Name}.btnInsert_Click", ex);
            EditClass.MessageException("Greška prilikom spremanja! Pokušajte ponovno.");
        }
    }
    
    private void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            // Similar pattern to Insert but check for selected record first
            if (selectedId == 0)
            {
                EditClass.MessageDBErrorPrimaryKeyNotFound();
                return;
            }
            
            // ... validation ...
            // ... controller.Update(model) ...
            // ... success feedback ...
        }
        catch (ApplicationException ex)
        {
            EditClass.MessageException(ex.Message);
        }
        catch (Exception ex)
        {
            EditClass.LogException($"{this.GetType().Name}.btnUpdate_Click", ex);
            EditClass.MessageException("Greška prilikom ažuriranja! Pokušajte ponovno.");
        }
    }
}
```

---

## Summary

**Critical contracts**:
1. All Controllers MUST populate `korisnikId` and timestamp field (use correct name for each table)
2. All Forms MUST validate input before calling Controllers
3. All database operations MUST be wrapped in try-catch
4. All exceptions MUST be logged via LogHelper
5. All user-facing errors MUST use Croatian messages from EditClass

**Reference tables for timestamp field names**:
- **azurirano**: Djelatnici, Korisnici, Odjeli, Mjesta, Partneri, Posiljatelji, Knjiga, Poslane
- **Ažurirano**: DataCards, Enc
- **DateEdited**: Oprema, Vozila, KarticeUlaz
- **EditedOn**: ZaRa
