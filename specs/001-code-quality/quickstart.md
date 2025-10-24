# Quick Start: Code Quality Refactoring

**Feature**: 001-code-quality  
**Purpose**: Get developers started quickly on refactoring forms following established patterns

## Prerequisites

1. Visual Studio 2019+ with .NET Framework 4.7.2+
2. Access to KPP.accdb database (password: `SunceJeLijepo1#`)
3. Non-production environment for testing
4. Feature branch checked out: `001-code-quality`

## Implementation Order

Follow this sequence to build confidence incrementally:

**Week 1-2**: Helper Classes & First Form
1. Add LogHelper class → Test logging
2. Enhance EditClass with validation helpers → Unit test each helper
3. Refactor **Odjeli** form (simplest) → Full CRUD testing
4. Deploy Odjeli to production → Monitor for 1 week

**Week 3-4**: Reference Data Forms
5. Refactor **Mjesta** → Test & deploy
6. Refactor **Partneri** → Test & deploy
7. Refactor **Posiljatelji** → Test & deploy

**Week 5-6**: Simple Asset Forms
8. Refactor **Enc** → Test & deploy
9. Refactor **KeyCards** → Test & deploy
10. Refactor **DataCards** → Test & deploy

**Week 7-10**: Complex Asset Forms
11. Refactor **Oprema** → Extensive testing
12. Refactor **Vozila** → Extensive testing

**Week 11-14**: Core Business Forms (highest value, highest risk)
13. Refactor **Djelatnici** → Comprehensive testing
14. Refactor **Korisnici** → Security-focused testing
15. Refactor **ZaRa** → Complex business logic testing

**Week 15-17**: Document Tracking
16. Refactor **Knjiga** → Test & deploy
17. Refactor **Poslane** → Test & deploy

**Week 18-20**: Special Forms
18. Refactor **Ovlastenja** → Test & deploy
19. Refactor **ExportData** → Test & deploy

## Step-by-Step: Refactoring a Form

### Phase 1: Create LogHelper (ONE TIME)

```bash
# Create new file: KPP_Alpha1/HelperClasses/LogHelper.cs
# Copy implementation from specs/001-code-quality/research.md
# Build and test logging with simple program:
```

```csharp
// Test in Program.cs Main method
LogHelper.LogInfo("Application", "KPP Application started");
LogHelper.LogError("Test", "Test error", new Exception("Test exception"));
// Check that Logs/KPP_YYYY-MM-DD.log file is created
```

### Phase 2: Enhance EditClass (ONE TIME)

```bash
# Open KPP_Alpha1/HelperClasses/EditClass.cs
# Add methods from specs/001-code-quality/contracts/helper-contracts.md
# Test each new method:
```

```csharp
// Quick test in a form Load event (remove after testing)
private void TestEditClassEnhancements()
{
    // Test required field validation
    bool valid = EditClass.ValidateRequired(txtTest, "Test Field", out string err);
    MessageBox.Show($"Valid: {valid}, Error: {err}");
    
    // Test OIB validation
    valid = EditClass.ValidateOib("12345678901", out err);
    MessageBox.Show($"Valid: {valid}, Error: {err}");
    
    // Test null-safe helpers
    string safe = EditClass.SafeString(null, "default");
    MessageBox.Show($"Safe string: {safe}");
}
```

### Phase 3: Enhance DbClass (ONE TIME, Optional)

```bash
# Open KPP_Alpha1/HelperClasses/DbClass.cs
# Add RecordExists method and ExecuteScalar from contracts
# Test RecordExists:
```

```csharp
// Quick test
bool exists = DbClass.RecordExists("Odjeli", "Naziv", "Test Department");
MessageBox.Show($"Record exists: {exists}");
```

### Phase 4: Refactor a Controller

**Example: OdjelController.cs**

```csharp
// BEFORE (typical existing code)
public void Insert(OdjelModel model)
{
    string sql = "INSERT INTO Odjeli (Naziv, korisnikId, azurirano) VALUES (@p0, @p1, @p2)";
    var parameters = new Dictionary<string, object>
    {
        {"@p0", model.Naziv},
        {"@p1", model.KorisnikId},
        {"@p2", model.Azurirano}
    };
    DbClass.ExcecuteNonQuery(sql, parameters);
}

// AFTER (refactored with error handling + logging)
public void Insert(OdjelModel model)
{
    try
    {
        // Explicitly set audit fields (even though model has defaults)
        model.KorisnikId = LoginHelper.StaticId;
        model.Azurirano = DateTime.Now.Date;
        
        string sql = "INSERT INTO Odjeli (Naziv, korisnikId, azurirano) VALUES (@p0, @p1, @p2)";
        var parameters = new Dictionary<string, object>
        {
            {"@p0", model.Naziv},
            {"@p1", model.KorisnikId},
            {"@p2", model.Azurirano}
        };
        
        DbClass.ExecuteNonQuery(sql, parameters);
        
        // Log successful operation
        LogHelper.LogInfo("OdjelController.Insert", 
            $"Inserted department '{model.Naziv}' by user {LoginHelper.StaticUsername}");
    }
    catch (OleDbException ex)
    {
        // Log database error
        LogHelper.LogError("OdjelController.Insert", 
            $"Database error inserting department '{model.Naziv}'", ex);
        
        // Throw business-friendly exception
        throw new ApplicationException("Greška prilikom spremanja odjela u bazu!");
    }
    catch (Exception ex)
    {
        // Log unexpected error
        LogHelper.LogError("OdjelController.Insert", 
            $"Unexpected error inserting department '{model.Naziv}'", ex);
        throw;
    }
}
```

**Repeat for Update and Delete methods** following the same pattern.

### Phase 5: Refactor a Form

**Example: Odjeli.cs**

#### Step 5.1: Enhance Insert Button Event

```csharp
// BEFORE (typical existing code - might vary)
private void btnInsert_Click(object sender, EventArgs e)
{
    if (txtNaziv.Text.Trim() == "")
    {
        MessageBox.Show("Naziv ne smije biti prazan!");
        return;
    }
    
    var model = new OdjelModel
    {
        Naziv = txtNaziv.Text.Trim()
    };
    
    controller.Insert(model);
    RefreshGrid();
}

// AFTER (refactored with comprehensive error handling)
private void btnInsert_Click(object sender, EventArgs e)
{
    try
    {
        // Step 1: Validate required fields using helper
        if (!EditClass.ValidateRequired(txtNaziv, "Naziv odjela", out string errorMsg))
        {
            EditClass.MessageException(errorMsg);
            return;
        }
        
        // Step 2: Check for duplicates
        if (DbClass.RecordExists("Odjeli", "Naziv", txtNaziv.Text.Trim()))
        {
            EditClass.MessageException("Odjel s tim nazivom već postoji!");
            txtNaziv.BackColor = Color.Pink;
            return;
        }
        
        // Step 3: Create model and insert
        var model = new OdjelModel
        {
            Naziv = txtNaziv.Text.Trim()
        };
        
        controller.Insert(model);
        
        // Step 4: Success feedback
        RefreshGrid();
        ClearForm();
        MessageBox.Show("Odjel uspješno spremljen!", "Uspjeh", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    catch (ApplicationException ex)
    {
        // Business exception with user-friendly message
        EditClass.MessageException(ex.Message);
    }
    catch (Exception ex)
    {
        // Unexpected exception - log and show generic message
        EditClass.LogException("Odjeli.btnInsert_Click", ex);
        EditClass.MessageException("Nepoznata greška! Kontaktirajte administratora.");
    }
}
```

#### Step 5.2: Enhance Update Button Event

```csharp
private void btnUpdate_Click(object sender, EventArgs e)
{
    try
    {
        // Step 1: Verify record is selected
        if (selectedOdjelId == 0)
        {
            EditClass.MessageDBErrorPrimaryKeyNotFound();
            return;
        }
        
        // Step 2: Validate required fields
        if (!EditClass.ValidateRequired(txtNaziv, "Naziv odjela", out string errorMsg))
        {
            EditClass.MessageException(errorMsg);
            return;
        }
        
        // Step 3: Check for duplicates (exclude current record)
        if (DbClass.RecordExists("Odjeli", "Naziv", txtNaziv.Text.Trim(), selectedOdjelId))
        {
            EditClass.MessageException("Odjel s tim nazivom već postoji!");
            txtNaziv.BackColor = Color.Pink;
            return;
        }
        
        // Step 4: Create model and update
        var model = new OdjelModel
        {
            Id = selectedOdjelId,
            Naziv = txtNaziv.Text.Trim()
        };
        
        controller.Update(model);
        
        // Step 5: Success feedback
        RefreshGrid();
        ClearForm();
        MessageBox.Show("Odjel uspješno ažuriran!", "Uspjeh", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    catch (ApplicationException ex)
    {
        EditClass.MessageException(ex.Message);
    }
    catch (Exception ex)
    {
        EditClass.LogException("Odjeli.btnUpdate_Click", ex);
        EditClass.MessageException("Greška prilikom ažuriranja! Pokušajte ponovno.");
    }
}
```

#### Step 5.3: Enhance Delete Button Event

```csharp
private void btnDelete_Click(object sender, EventArgs e)
{
    try
    {
        // Step 1: Verify record is selected
        if (selectedOdjelId == 0)
        {
            EditClass.MessageDBErrorPrimaryKeyNotFound();
            return;
        }
        
        // Step 2: Confirm deletion
        var confirmResult = MessageBox.Show(
            "Jeste li sigurni da želite obrisati ovaj odjel?",
            "Potvrda brisanja",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (confirmResult != DialogResult.Yes)
            return;
        
        // Step 3: Delete
        controller.Delete(selectedOdjelId);
        
        // Step 4: Success feedback
        RefreshGrid();
        ClearForm();
        MessageBox.Show("Odjel uspješno obrisan!", "Uspjeh", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    catch (ApplicationException ex)
    {
        EditClass.MessageException(ex.Message);
    }
    catch (Exception ex)
    {
        EditClass.LogException("Odjeli.btnDelete_Click", ex);
        EditClass.MessageException("Greška prilikom brisanja! Možda postoje povezani zapisi.");
    }
}
```

### Phase 6: Testing Checklist

Before deploying refactored form to production:

#### Functional Testing
- [ ] **Insert**: Creates new record with correct audit fields (korisnikId, timestamp)
- [ ] **Update**: Modifies existing record with updated audit fields
- [ ] **Delete**: Removes record (or sets inactive if soft delete)
- [ ] **Search**: Filter functionality still works
- [ ] **Keyboard shortcuts**: F2 (Insert), F3 (Edit), F4 (Clear) still work
- [ ] **Menu items**: Ctrl+N, Ctrl+S, Ctrl+R still work

#### Validation Testing
- [ ] **Required fields**: Pink highlight appears when empty
- [ ] **Unique constraints**: Duplicate values are rejected with clear message
- [ ] **Format validation**: Invalid formats (e.g., OIB) are rejected
- [ ] **Business rules**: Date logic, foreign keys validated correctly
- [ ] **Error messages**: All messages in Croatian, user-friendly

#### Error Scenario Testing
- [ ] **Database unavailable**: Graceful error message, no crash
- [ ] **Constraint violation**: Clear message explains issue
- [ ] **Null reference**: No crashes from null values
- [ ] **Network timeout**: Appropriate error handling

#### Audit Testing
- [ ] **Insert operation**: Check database - korisnikId and timestamp populated
- [ ] **Update operation**: Check database - korisnikId and timestamp updated
- [ ] **Correct user**: Verify korisnikId matches current logged-in user
- [ ] **Correct timestamp**: Verify timestamp is today's date

#### Logging Testing
- [ ] **Log file created**: Check Logs/KPP_YYYY-MM-DD.log exists
- [ ] **Successful operations**: Info messages logged
- [ ] **Errors logged**: Exception details captured with stack trace
- [ ] **User context**: Log entries include current username

### Phase 7: Deployment

```bash
# 1. Backup current production .exe
copy KPP_Alpha1.exe KPP_Alpha1.exe.backup

# 2. Build Release version
# In Visual Studio: Build > Build Solution (Release mode)

# 3. Deploy to production location
copy bin\Release\KPP_Alpha1.exe [production_path]\KPP_Alpha1.exe

# 4. Monitor logs for 1 week
# Check Logs\ folder daily for errors
# Ask users for feedback on form behavior
```

## Common Patterns Reference

### Pattern 1: Multi-Field Validation

```csharp
List<string> errors = new List<string>();

if (!EditClass.ValidateRequired(txtIme, "Ime", out string err1))
    errors.Add(err1);

if (!EditClass.ValidateRequired(txtPrezime, "Prezime", out string err2))
    errors.Add(err2);

if (!EditClass.ValidateOib(txtOib.Text, out string err3))
    errors.Add(err3);

if (errors.Count > 0)
{
    EditClass.MessageException(string.Join("\n", errors));
    return;
}
```

### Pattern 2: ComboBox Foreign Key Validation

```csharp
if (!EditClass.ValidateComboBox(cmbOdjel, "Odjel", out string errorMsg))
{
    EditClass.MessageException(errorMsg);
    return;
}

int odjelId = Convert.ToInt32(cmbOdjel.SelectedValue);
```

### Pattern 3: Date Range Validation

```csharp
if (dtpOtkaz.Value != DateTime.MinValue)
{
    if (!EditClass.ValidateDateRange(
        dtpZaposlen.Value, 
        dtpOtkaz.Value,
        "Datum zaposlenja", 
        "Datum otkaza", 
        out string errorMsg))
    {
        EditClass.MessageException(errorMsg);
        return;
    }
}
```

### Pattern 4: Safe DataGridView Cell Reading

```csharp
// Reading from selected row
if (dgvData.SelectedRows.Count > 0)
{
    var row = dgvData.SelectedRows[0];
    int id = EditClass.SafeInt(row.Cells["Id"].Value);
    string naziv = EditClass.SafeString(row.Cells["Naziv"].Value);
    DateTime? datum = EditClass.SafeDateTime(row.Cells["Datum"].Value);
}
```

## Troubleshooting

### Issue: "Ćelije ne smiju biti prazne!" appears even when field has value

**Cause**: Whitespace-only input  
**Solution**: EditClass.NullOrWhite() trims and checks - ensure user entered actual content

### Issue: Audit fields not populating in database

**Cause**: Controller not explicitly setting fields OR SQL query missing audit columns  
**Solution**: 
1. Verify Controller sets `model.KorisnikId = LoginHelper.StaticId`
2. Verify SQL INSERT/UPDATE includes korisnikId and timestamp columns
3. Check data-model.md for correct timestamp field name (azurirano vs DateEdited vs EditedOn)

### Issue: Application crashes on database error

**Cause**: Missing try-catch in Form event handler  
**Solution**: Wrap all Controller calls in try-catch blocks per pattern above

### Issue: Log file not created

**Cause**: Permissions issue or LogHelper not initialized  
**Solution**: 
1. Ensure Logs\ directory created in static constructor
2. Run application as Administrator for first run to create directory
3. Check Application.Ev entLog for permission errors

## Next Steps

After completing all 19 forms:
1. Review aggregate metrics (Success Criteria from spec.md)
2. Gather user feedback on stability improvements
3. Consider adding automated regression tests
4. Document lessons learned for future refactoring efforts

## Support

- **Specification**: `specs/001-code-quality/spec.md`
- **Research Decisions**: `specs/001-code-quality/research.md`
- **Data Model**: `specs/001-code-quality/data-model.md`
- **Coding Contracts**: `specs/001-code-quality/contracts/helper-contracts.md`
- **Constitution**: `.specify/memory/constitution.md`
