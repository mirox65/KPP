# Research: Code Quality & Maintainability Enhancement

**Feature**: 001-code-quality  
**Date**: 2025-10-24  
**Purpose**: Resolve technical unknowns and establish best practices for refactoring existing C# Windows Forms application

## Research Tasks

### 1. Error Handling Patterns in Windows Forms Applications

**Question**: What are the best practices for comprehensive error handling in Windows Forms with Access database backend?

**Decision**: Use layered try-catch strategy with centralized error message formatting

**Rationale**: 
- **Controller layer**: Catch database exceptions (OleDbException), log technical details, throw business-friendly exceptions
- **Form layer**: Catch all exceptions, display user-friendly Croatian messages via EditClass helpers, preserve user input
- **Helper layer**: Centralized exception logging to enable troubleshooting without exposing technical details to users

**Alternatives considered**:
- Global exception handler (Application.ThreadException) - Rejected: loses local context needed for specific error recovery
- No exception handling in forms - Rejected: violates Stability First principle, causes application crashes
- Database-level error handling only - Rejected: doesn't provide user-friendly feedback or preserve input

**Implementation approach**:
```csharp
// Controller pattern
public void Insert(DjelatnikModel model)
{
    try
    {
        DbClass.ExecuteNonQuery(sql, parameters);
    }
    catch (OleDbException ex)
    {
        EditClass.LogException("DjelatnikController.Insert", ex);
        throw new ApplicationException("Greška prilikom spremanja djelatnika");
    }
}

// Form pattern
private void btnInsert_Click(object sender, EventArgs e)
{
    try
    {
        // Validation
        if (!ValidateInput()) return;
        
        // Business logic
        controller.Insert(CreateModel());
        
        // Success feedback
        RefreshGrid();
        ClearForm();
    }
    catch (ApplicationException ex)
    {
        EditClass.MessageException(ex.Message);
    }
    catch (Exception ex)
    {
        EditClass.LogException("Djelatnici.btnInsert_Click", ex);
        EditClass.MessageException("Nepoznata greška. Kontaktirajte administratora.");
    }
}
```

---

### 2. Validation Helper Method Design

**Question**: How should validation helper methods be structured to maximize reusability without creating excessive abstraction?

**Decision**: Create focused, single-purpose validation helpers in EditClass with clear naming

**Rationale**:
- **Focused methods**: Each validator checks one thing (null, format, business rule)
- **Composability**: Complex validation built by calling multiple simple validators
- **Clear naming**: Method names describe what they validate (e.g., `IsRequiredFieldFilled`, `IsValidOibFormat`, `IsDateRangeValid`)
- **Consistent return pattern**: Return bool for success/failure, use out parameter for error message

**Alternatives considered**:
- Fluent validation library - Rejected: adds external dependency, overkill for this scope
- Attribute-based validation - Rejected: requires model changes, doesn't fit WinForms pattern
- Single "Validate" method with reflection - Rejected: reduces clarity, harder to debug

**Implementation approach**:
```csharp
// EditClass additions
public static class EditClass
{
    // Existing methods preserved
    public static bool NullOrWhite(string value) { /* existing */ }
    public static void BojaPozadineZaPrazneCeliji(TextBox txt) { /* existing */ }
    
    // NEW: Required field validation with feedback
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
    
    // NEW: OIB format validation
    public static bool ValidateOib(string oib, out string errorMsg)
    {
        if (string.IsNullOrWhiteSpace(oib) || oib.Length != 11 || !oib.All(char.IsDigit))
        {
            errorMsg = "OIB mora imati točno 11 znamenaka!";
            return false;
        }
        errorMsg = string.Empty;
        return true;
    }
    
    // NEW: Date range validation
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
    
    // NEW: Null-safe string getter
    public static string SafeString(object value, string defaultValue = "")
    {
        return value == null || value == DBNull.Value 
            ? defaultValue 
            : value.ToString();
    }
}
```

---

### 3. Audit Field Population Strategy

**Question**: Should audit fields be populated in Models, Controllers, or both? How to handle inconsistent naming?

**Decision**: Populate in Controllers just before INSERT/UPDATE, accept naming inconsistency

**Rationale**:
- **Controller responsibility**: Business logic layer is correct place for audit trail enforcement
- **Verify Model defaults**: Models already set `LoginHelper.StaticId` and `DateTime.Now.Date` as defaults, which is good fallback
- **Explicit over implicit**: Controllers should explicitly set these fields to ensure they're not forgotten
- **Naming inconsistency acceptable**: Changing column names (Azurirano vs DateEdited) would violate Backward Compatibility principle

**Alternatives considered**:
- Standardize column names - Rejected: requires database schema change, migration risk
- Database triggers - Rejected: Access doesn't support robust triggers, reduces code transparency
- Aspect-oriented programming - Rejected: excessive complexity for this use case

**Implementation approach**:
```csharp
// Controller pattern for INSERT
public void Insert(DjelatnikModel model)
{
    // Explicitly set audit fields (redundant with model defaults, but explicit)
    model.KorisnikId = LoginHelper.StaticId;
    model.Azurirano = DateTime.Now.Date;
    
    string sql = @"INSERT INTO Djelatnici (PerNum, Ime, Prezime, Oib, OdjelId, 
                   DatZaposlen, DatOtkaz, Aktivan, korisnikId, azurirano) 
                   VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)";
    
    var parameters = new Dictionary<string, object>
    {
        {"@p0", model.PerNum},
        {"@p1", model.Ime},
        {"@p2", model.Prezime},
        {"@p3", model.Oib},
        {"@p4", model.OdjelId},
        {"@p5", model.DatZaposlen},
        {"@p6", model.DatOtkaz},
        {"@p7", model.Aktivan},
        {"@p8", model.KorisnikId},
        {"@p9", model.Azurirano}
    };
    
    DbClass.ExecuteNonQuery(sql, parameters);
}

// Audit field checklist by table (reference for implementation)
// Djelatnici: korisnikId + Azurirano
// Korisnici: korisnikId + Azurirano
// Odjeli: korisnikId + Azurirano
// Mjesta: korisnikId + Azurirano
// Partneri: korisnikId + Azurirano
// Posiljatelji: korisnikId + Azurirano
// Knjiga: korisnikId + Azurirano
// Poslane: korisnikId + Azurirano
// DataCards: korisnikId + Ažurirano (note diacritic)
// Enc: korisnikId + Ažurirano (note diacritic)
// Oprema: korisnikId + DateEdited (English name)
// Vozila: korisnikId + DateEdited (English name)
// KarticeUlaz: korisnikId + DateEdited (English name)
// ZaRa: korisnikId + EditedOn (English name)
```

---

### 4. Logging Implementation Without External Dependencies

**Question**: How to implement effective logging without adding NuGet dependencies or complex frameworks?

**Decision**: Simple file-based logging with daily log rotation using built-in System.IO

**Rationale**:
- **Zero dependencies**: Uses only .NET Framework built-in classes
- **Simple implementation**: Single static class in HelperClasses folder
- **Daily rotation**: Prevents log files from growing too large (365-day retention)
- **Structured format**: Timestamp | Level | Source | Message | Exception
- **Async-safe**: Uses lock to prevent file access conflicts
- **Automatic cleanup**: LogHelper.CleanOldLogs(365) called on application startup removes files older than 365 days

**Alternatives considered**:
- NLog/log4net - Rejected: adds external dependency, overkill for single-user desktop app
- Windows Event Log - Rejected: requires admin permissions, harder for users to access
- Database logging - Rejected: complicates debugging database issues
- No logging - Rejected: violates operational efficiency goals (SC-019)

**Implementation approach**:
```csharp
// New file: KPP_Alpha1/HelperClasses/LogHelper.cs
using System;
using System.IO;
using System.Text;

namespace KPP_Alpha1.HelperClasses
{
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
        
        public static void LogError(string source, string message, Exception ex = null)
        {
            Log("ERROR", source, message, ex);
        }
        
        public static void LogInfo(string source, string message)
        {
            Log("INFO", source, message, null);
        }
        
        private static void Log(string level, string source, string message, Exception ex)
        {
            lock (_lock)
            {
                try
                {
                    string logFile = Path.Combine(_logDirectory, 
                        $"KPP_{DateTime.Now:yyyy-MM-dd}.log");
                    
                    var sb = new StringBuilder();
                    sb.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {level} | {source}");
                    sb.AppendLine($"  Message: {message}");
                    
                    if (ex != null)
                    {
                        sb.AppendLine($"  Exception: {ex.GetType().Name}");
                        sb.AppendLine($"  Details: {ex.Message}");
                        sb.AppendLine($"  Stack: {ex.StackTrace}");
                    }
                    
                    sb.AppendLine(new string('-', 80));
                    
                    File.AppendAllText(logFile, sb.ToString());
                }
                catch
                {
                    // Swallow logging errors to prevent cascading failures
                }
            }
        }
        
        public static void CleanOldLogs(int daysToKeep = 365)
        {
            try
            {
                var cutoffDate = DateTime.Now.AddDays(-daysToKeep);
                var files = Directory.GetFiles(_logDirectory, "KPP_*.log");
                
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTime < cutoffDate)
                        fileInfo.Delete();
                }
            }
            catch
            {
                // Swallow cleanup errors
            }
        }
    }
}

// Usage in EditClass
public static void LogException(string source, Exception ex)
{
    LogHelper.LogError(source, $"User: {LoginHelper.StaticUsername}", ex);
}
```

---

### 5. Incremental Rollout Strategy

**Question**: What is the safest order to refactor forms to minimize risk while building confidence?

**Decision**: Start with low-risk reference data forms, then progress to high-value transactional forms

**Rationale**:
- **Phase 1 (Low risk)**: Reference data forms (Odjeli, Mjesta, Partneri, Posiljatelji) - simpler logic, fewer fields, less frequent use
- **Phase 2 (Medium risk)**: Asset tracking forms (Oprema, Vozila, KeyCards, DataCards, Enc) - more complex but well-isolated
- **Phase 3 (High value)**: Core business forms (Djelatnici, Korisnici, ZaRa, Knjiga, Poslane) - most critical, highest usage
- **Phase 4 (Special)**: System forms (Ovlastenja, ExportData) - unique functionality, requires careful testing

**Rollout order**:
1. **Odjeli** (Departments) - Simplest form, good proof of concept
2. **Mjesta** (Locations) - Similar to Odjeli, builds pattern
3. **Partneri** (Partners) - Reference data, low complexity
4. **Posiljatelji** (Senders) - Reference data with foreign key
5. **Enc** - Simple equipment type with minimal fields
6. **KeyCards** - Simple asset tracking
7. **DataCards** - Similar to KeyCards
8. **Oprema** (Equipment) - Complex asset tracking, high value
9. **Vozila** (Vehicles) - Complex asset tracking with contracts
10. **Djelatnici** (Employees) - Core HR functionality
11. **Korisnici** (Users) - Security-critical, requires extra testing
12. **ZaRa** (Assignment/Return) - Complex business logic
13. **Knjiga** (Registry) - Document tracking
14. **Poslane** (Sent items) - Document tracking
15. **Ovlastenja** (Permissions) - System administration
16. **ExportData** - Reporting functionality

**Per-form checklist**:
- [ ] Review existing code for error handling gaps
- [ ] Identify validation rules to standardize
- [ ] Update Controller to use enhanced DbClass methods
- [ ] Add try-catch blocks in Form event handlers
- [ ] Replace inline validation with EditClass helpers
- [ ] Verify audit field population
- [ ] Test all CRUD operations in non-production
- [ ] Test validation scenarios (empty fields, invalid formats, duplicates)
- [ ] Test error scenarios (database unavailable, constraint violations)
- [ ] Deploy to production
- [ ] Monitor for 1 week before proceeding to next form

**Alternatives considered**:
- Big bang deployment - Rejected: too risky, violates Incremental Delivery principle
- Random order - Rejected: doesn't build confidence systematically
- Alphabetical order - Rejected: doesn't account for complexity or risk

---

## Summary of Research Findings

**Ready to proceed**: All technical unknowns resolved. No external dependencies required. Implementation can use existing .NET Framework capabilities and established patterns from codebase.

**Key decisions**:
1. Layered try-catch with centralized error formatting
2. Focused validation helpers in EditClass
3. Controller-level audit field population
4. Simple file-based logging (new LogHelper class)
5. Incremental rollout starting with reference data forms

**Next phase**: Generate data model documentation and establish coding contracts for refactored components.
