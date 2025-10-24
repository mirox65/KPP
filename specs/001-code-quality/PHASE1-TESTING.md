# Phase 1 Testing Instructions (T016, T020, T022)

## Overview
Phase 1 helper classes have been implemented. This document explains how to verify the implementation.

## Files Created/Modified

### New Files
1. **KPP_Alpha1/HelperClasses/LogHelper.cs** (T001-T004)
   - LogError, LogInfo, CleanOldLogs methods
   - File-based logging to Logs/ directory
   - 365-day retention policy

2. **KPP_Alpha1/HelperTests.cs** (T016, T020 test harness)
   - Temporary test form to validate all helper methods
   - Tests EditClass validation methods
   - Tests DbClass query methods
   - Tests LogHelper functionality
   - **NOTE**: This file should be removed after Phase 1 validation

### Enhanced Files
3. **KPP_Alpha1/HelperClasses/EditClass.cs** (T006-T015)
   - Added 10 static validation methods:
     - ValidateRequired
     - ValidateComboBox
     - ValidateOib
     - ValidateNumeric
     - ValidateDateRange
     - ValidateNotFutureDate
     - SafeString
     - SafeInt
     - SafeDateTime
     - LogException

4. **KPP_Alpha1/HelperClasses/DbClass.cs** (T017-T019)
   - Added 3 static helper methods:
     - TestConnection
     - ExecuteScalar
     - RecordExists

5. **KPP_Alpha1/Program.cs** (T021)
   - Added LogHelper.CleanOldLogs(365) on startup
   - Temporarily launches HelperTests form instead of Form1
   - **TODO**: Restore original login flow after validation

6. **KPP_Alpha1/KPP_Alpha1.csproj**
   - Added LogHelper.cs and HelperTests.cs to compilation

## Testing Procedure

### Option 1: Run from Visual Studio (Recommended)
1. Open `KPP_Alpha1.sln` in Visual Studio
2. Build the solution (Ctrl+Shift+B)
3. Verify 0 errors, 0 warnings
4. Run the application (F5)
5. The HelperTests form will launch automatically
6. Check the message box showing test completion
7. Review the log file at `KPP_Alpha1/bin/Debug/Logs/KPP_2025-10-24.log`

### Option 2: Run from VS Code
1. Press F5 or use "Run > Start Debugging"
2. The application will build and launch
3. Follow steps 5-7 from Option 1

### Option 3: Manual Build and Run
1. Open Developer Command Prompt for Visual Studio
2. Navigate to the project directory:
   ```cmd
   cd C:\Users\vranicm\Projects\KPP
   ```
3. Build the solution:
   ```cmd
   msbuild KPP_Alpha1.sln /p:Configuration=Debug
   ```
4. Run the executable:
   ```cmd
   KPP_Alpha1\bin\Debug\KPP.exe
   ```
5. Follow steps 5-7 from Option 1

## Expected Test Results

### T016: EditClass Validation Tests
Check the log file for these entries:
- ✅ ValidateRequired: Fails for empty textbox, passes for filled
- ✅ ValidateComboBox: Fails for no selection, passes for selected
- ✅ ValidateOib: Fails for invalid format, passes for 11 digits
- ✅ ValidateNumeric: Fails for non-digits, passes for numeric
- ✅ ValidateDateRange: Fails when end < start, passes when end >= start
- ✅ ValidateNotFutureDate: Fails for future dates, passes for past
- ✅ SafeString: Returns default for null/DBNull, returns value otherwise
- ✅ SafeInt: Returns default for null/DBNull, returns value otherwise
- ✅ SafeDateTime: Returns null for null/DBNull, returns value otherwise
- ✅ LogException: Creates error log entry with exception details

### T020: DbClass Tests
Check the log file for these entries:
- ✅ TestConnection: Returns true if database accessible
- ✅ ExecuteScalar: Returns count of Odjeli records
- ✅ ExecuteScalar with params: Returns filtered count
- ✅ RecordExists: Returns true for existing record
- ✅ RecordExists: Returns false for non-existing value
- ✅ RecordExists with excludeId: Correctly excludes specified ID

### T005: LogHelper Tests
- ✅ Log file created at `Logs/KPP_2025-10-24.log`
- ✅ LogInfo entries have [INFO] prefix
- ✅ LogError entries have [ERROR] prefix with exception details
- ✅ All log entries have timestamp and source

## Validation Checklist

- [ ] T005: Log file created successfully
- [ ] T016: All EditClass validation methods tested and logged
- [ ] T020: All DbClass methods tested and logged
- [ ] T021: LogHelper.CleanOldLogs(365) runs on startup
- [ ] T022: Solution builds with 0 errors

## After Validation

Once all tests pass:

1. **Restore Program.cs to normal login flow**:
   ```csharp
   static void Main()
   {
       LogHelper.CleanOldLogs(365);  // Keep this
       Application.EnableVisualStyles();
       Application.SetCompatibleTextRenderingDefault(false);
       Application.Run(new Form1());  // Restore this
   }
   ```

2. **Remove test files**:
   - Delete `KPP_Alpha1/HelperTests.cs`
   - Remove `<Compile Include="HelperTests.cs">` from KPP_Alpha1.csproj

3. **Mark tasks complete in tasks.md**:
   - [X] T005, T016, T020, T021, T022

4. **Proceed to Phase 2**:
   - Begin US1 implementation on Odjeli form (T023-T035)

## Troubleshooting

### Build Errors
- Ensure .NET Framework 4.8 is installed
- Check that all using statements are correct
- Verify KPP.accdb database exists in bin/Debug/ directory

### Database Connection Fails
- Check App.config for correct connection string
- Verify database password: `SunceJeLijepo1#`
- Ensure database file is in `|DataDirectory|` location

### Log File Not Created
- Check write permissions on project directory
- Verify Logs/ folder can be created
- Check for antivirus blocking file creation

## Notes
- All helper methods follow contracts in `specs/001-code-quality/contracts/helper-contracts.md`
- Croatian error messages used throughout
- Existing instance methods in EditClass/DbClass preserved
- New static methods added for future form refactoring
