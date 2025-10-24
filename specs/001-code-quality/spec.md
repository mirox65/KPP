# Feature Specification: Code Quality & Maintainability Enhancement# Feature Specification: Code Quality & Maintainability Enhancement# Feature Specification: Code Quality & Maintainability Enhancement# Feature Specification: [FEATURE NAME]



**Feature Branch**: `001-code-quality`  

**Created**: 2025-10-24  

**Status**: Draft  **Feature Branch**: `001-code-quality`  



**Input**: User description: "Comprehensive code review and quality improvement for the existing production asset management system"**Created**: 2025-10-24  



## User Scenarios & Testing *(mandatory)***Status**: Draft  **Feature Branch**: `001-code-quality`  **Feature Branch**: `[###-feature-name]`  



### User Story 1 - Reliable Error Handling (Priority: P1)**Input**: User description: "Comprehensive code review and quality improvement for the existing production asset management system"



As a system administrator or user, when I interact with any form in the KPP application, I should receive clear, actionable error messages in Croatian when something goes wrong, and the system should gracefully recover from errors without crashing or losing my data.**Created**: 2025-10-24  **Created**: [DATE]  



**Why this priority**: Production stability is critical. Unhandled exceptions cause crashes, data loss, and user frustration. This is the foundation for all other improvements.## User Scenarios & Testing *(mandatory)*



**Independent Test**: Can be fully tested by deliberately triggering error conditions (database unavailable, invalid input, network failures) across all forms and verifying that users receive appropriate Croatian error messages and the system remains stable.**Status**: Draft  **Status**: Draft  



**Acceptance Scenarios**:### User Story 1 - Reliable Error Handling (Priority: P1)



1. **Given** a user is entering employee data, **When** the database connection fails during save, **Then** the user sees "Greška prilikom spremanja: Baza podataka trenutno nije dostupna. Pokušajte ponovno." and their input is preserved**Input**: User description: "Comprehensive code review and quality improvement for the existing production asset management system"**Input**: User description: "$ARGUMENTS"

2. **Given** a user attempts to assign already-assigned equipment, **When** they click save, **Then** they see "Oprema je već zadužena. Odaberite drugu stavku." and can correct their selection

3. **Given** an unexpected exception occurs in any form, **When** the error is caught, **Then** the error details are logged internally and the user sees a friendly message without technical jargonAs a system administrator or user, when I interact with any form in the KPP application, I should receive clear, actionable error messages in Croatian when something goes wrong, and the system should gracefully recover from errors without crashing or losing my data.

4. **Given** a user fills out a form incorrectly, **When** validation fails, **Then** invalid fields are highlighted in pink and a clear message explains what needs to be corrected



---

**Why this priority**: Production stability is critical. Unhandled exceptions cause crashes, data loss, and user frustration. This is the foundation for all other improvements.

### User Story 2 - Consistent Input Validation (Priority: P1)

## User Scenarios & Testing *(mandatory)*## User Scenarios & Testing *(mandatory)*

As a user entering data into any form, I should immediately see which fields are required and receive clear feedback when my input doesn't meet the expected format, so I can correct errors before attempting to save.

**Independent Test**: Can be fully tested by deliberately triggering error conditions (database unavailable, invalid input, network failures) across all forms and verifying that users receive appropriate Croatian error messages and the system remains stable.

**Why this priority**: Inconsistent validation leads to bad data, which causes downstream errors and reduces trust in the system. This directly impacts data integrity.



**Independent Test**: Can be fully tested by attempting to submit incomplete or invalid data across all forms (Djelatnici, Oprema, Vozila, etc.) and verifying that validation rules are consistently enforced with pink highlight feedback.

**Acceptance Scenarios**:

**Acceptance Scenarios**:

### User Story 1 - Reliable Error Handling (Priority: P1)<!--

1. **Given** a user is on the Djelatnici form, **When** they leave required fields (Ime, Prezime) empty and click save, **Then** those fields turn pink and they see "Ćelije ne smiju biti prazne!"

2. **Given** a user enters an invalid OIB format, **When** they attempt to save, **Then** the OIB field turns pink and they see "OIB mora imati 11 znamenaka"1. **Given** a user is entering employee data, **When** the database connection fails during save, **Then** the user sees "Greška prilikom spremanja: Baza podataka trenutno nije dostupna. Pokušajte ponovno." and their input is preserved

3. **Given** a user selects a termination date before employment date, **When** they attempt to save, **Then** they see "Datum otkaza mora biti nakon datuma zaposlenja"

4. **Given** a user enters a duplicate serial number for equipment, **When** they attempt to save, **Then** they see "Serijski broj već postoji u bazi"2. **Given** a user attempts to assign already-assigned equipment, **When** they click save, **Then** they see "Oprema je već zadužena. Odaberite drugu stavku." and can correct their selection  IMPORTANT: User stories should be PRIORITIZED as user journeys ordered by importance.

5. **Given** all validation passes, **When** user clicks save, **Then** pink highlights are cleared and data is saved successfully

3. **Given** an unexpected exception occurs in any form, **When** the error is caught, **Then** the error details are logged internally and the user sees a friendly message without technical jargon

---

4. **Given** a user fills out a form incorrectly, **When** validation fails, **Then** invalid fields are highlighted in pink and a clear message explains what needs to be correctedAs a system administrator or user, when I interact with any form in the KPP application, I should receive clear, actionable error messages in Croatian when something goes wrong, and the system should gracefully recover from errors without crashing or losing my data.  Each user story/journey must be INDEPENDENTLY TESTABLE - meaning if you implement just ONE of them,

### User Story 3 - Reduced Code Duplication (Priority: P2)



As a developer maintaining the codebase, when I need to add validation logic or database operations, I should be able to use centralized helper functions instead of copying and pasting code, reducing bugs and making the codebase easier to understand.

---  you should still have a viable MVP (Minimum Viable Product) that delivers value.

**Why this priority**: Code duplication makes maintenance harder and increases the risk of inconsistent behavior. Centralizing logic improves long-term maintainability.



**Independent Test**: Can be fully tested by code review analyzing helper class usage across all forms and verifying that common operations (validation, database queries, error handling) use shared functions from DbClass and EditClass.

### User Story 2 - Consistent Input Validation (Priority: P1)**Why this priority**: Production stability is critical. Unhandled exceptions cause crashes, data loss, and user frustration. This is the foundation for all other improvements.  

**Acceptance Scenarios**:



1. **Given** multiple forms need to validate required fields, **When** reviewing the code, **Then** all forms use `EditClass.NullOrWhite()` and `EditClass.BojaPozadineZaPrazneCeliji()` consistently

2. **Given** forms need to execute database queries, **When** reviewing the code, **Then** all forms use `DbClass.ExcecuteNonQuery()` and `DbClass.Select()` instead of inline database codeAs a user entering data into any form, I should immediately see which fields are required and receive clear feedback when my input doesn't meet the expected format, so I can correct errors before attempting to save.  Assign priorities (P1, P2, P3, etc.) to each story, where P1 is the most critical.

3. **Given** forms need to display error messages, **When** reviewing the code, **Then** all forms use `EditClass.MessageException()`, `EditClass.MessageDBErrorInsert()`, etc. for user feedback

4. **Given** new validation logic is needed, **When** adding it to EditClass, **Then** all forms can immediately use it without code duplication



---**Why this priority**: Inconsistent validation leads to bad data, which causes downstream errors and reduces trust in the system. This directly impacts data integrity.**Independent Test**: Can be fully tested by deliberately triggering error conditions (database unavailable, invalid input, network failures) across all forms and verifying that users receive appropriate Croatian error messages and the system remains stable.  Think of each story as a standalone slice of functionality that can be:



### User Story 4 - Improved Audit Trail (Priority: P2)



As a system administrator reviewing data changes, I should be able to see who made each change and when it was made, ensuring accountability and enabling investigation of data issues.**Independent Test**: Can be fully tested by attempting to submit incomplete or invalid data across all forms (Djelatnici, Oprema, Vozila, etc.) and verifying that validation rules are consistently enforced with pink highlight feedback.  - Developed independently



**Why this priority**: Audit trails are essential for compliance, troubleshooting, and user accountability. This supports the constitution's Data Integrity principle.



**Independent Test**: Can be fully tested by making changes to records across all forms and verifying that korisnikId and azurirano/dateEdited fields are correctly populated with current user and timestamp.**Acceptance Scenarios**:**Acceptance Scenarios**:  - Tested independently



**Acceptance Scenarios**:



1. **Given** a user creates a new employee record, **When** the record is saved, **Then** korisnikId is set to the current user's ID and azurirano is set to today's date1. **Given** a user is on the Djelatnici form, **When** they leave required fields (Ime, Prezime) empty and click save, **Then** those fields turn pink and they see "Ćelije ne smiju biti prazne!"  - Deployed independently

2. **Given** a user updates equipment information, **When** the record is saved, **Then** korisnikId and dateEdited are updated to reflect the current user and timestamp

3. **Given** an administrator reviews a record, **When** viewing the data grid, **Then** they can see who last modified each record and when2. **Given** a user enters an invalid OIB format, **When** they attempt to save, **Then** the OIB field turns pink and they see "OIB mora imati 11 znamenaka"

4. **Given** audit fields are missing from a form, **When** code review occurs, **Then** the form is updated to include audit field population

3. **Given** a user selects a termination date before employment date, **When** they attempt to save, **Then** they see "Datum otkaza mora biti nakon datuma zaposlenja"1. **Given** a user is entering employee data, **When** the database connection fails during save, **Then** the user sees "Greška prilikom spremanja: Baza podataka trenutno nije dostupna. Pokušajte ponovno." and their input is preserved  - Demonstrated to users independently

---

4. **Given** a user enters a duplicate serial number for equipment, **When** they attempt to save, **Then** they see "Serijski broj već postoji u bazi"

### User Story 5 - Standardized Null Handling (Priority: P3)

5. **Given** all validation passes, **When** user clicks save, **Then** pink highlights are cleared and data is saved successfully2. **Given** a user attempts to assign already-assigned equipment, **When** they click save, **Then** they see "Oprema je već zadužena. Odaberite drugu stavku." and can correct their selection-->

As a developer, when I work with data that might be null or empty, I should have consistent helper methods that prevent null reference exceptions and ensure predictable behavior across all forms.



**Why this priority**: Null reference exceptions are a common source of crashes. Standardizing null handling prevents these errors proactively.

---3. **Given** an unexpected exception occurs in any form, **When** the error is caught, **Then** the error details are logged internally and the user sees a friendly message without technical jargon

**Independent Test**: Can be fully tested by code review identifying all places where null checks are needed and verifying they use standardized helper methods, plus testing with null/empty data scenarios.



**Acceptance Scenarios**:

### User Story 3 - Reduced Code Duplication (Priority: P2)4. **Given** a user fills out a form incorrectly, **When** validation fails, **Then** invalid fields are highlighted in pink and a clear message explains what needs to be corrected### User Story 1 - [Brief Title] (Priority: P1)

1. **Given** a form retrieves data from the database, **When** a field might be null, **Then** the code uses null-coalescing operators or helper methods to provide safe defaults

2. **Given** a user doesn't enter data in an optional field, **When** the form processes the data, **Then** no null reference exception occurs

3. **Given** a dropdown has no selection, **When** the form accesses the selected value, **Then** the code safely handles the null case

4. **Given** autocomplete suggestions might be empty, **When** displaying the list, **Then** the code handles empty collections gracefullyAs a developer maintaining the codebase, when I need to add validation logic or database operations, I should be able to use centralized helper functions instead of copying and pasting code, reducing bugs and making the codebase easier to understand.



---



### User Story 6 - Enhanced Logging (Priority: P3)**Why this priority**: Code duplication makes maintenance harder and increases the risk of inconsistent behavior. Centralizing logic improves long-term maintainability.---[Describe this user journey in plain language]



As a system administrator troubleshooting issues, I should be able to review detailed logs that capture errors, user actions, and system events, enabling faster problem resolution.



**Why this priority**: Good logging accelerates troubleshooting and reduces downtime. This supports operational efficiency.**Independent Test**: Can be fully tested by code review analyzing helper class usage across all forms and verifying that common operations (validation, database queries, error handling) use shared functions from DbClass and EditClass.



**Independent Test**: Can be fully tested by triggering various error scenarios and verifying that appropriate log entries are created with sufficient detail for diagnosis.



**Acceptance Scenarios**:**Acceptance Scenarios**:### User Story 2 - Consistent Input Validation (Priority: P1)**Why this priority**: [Explain the value and why it has this priority level]



1. **Given** an exception occurs in any form, **When** caught by try-catch, **Then** a log entry is created with timestamp, user, form name, exception message, and stack trace

2. **Given** a database operation fails, **When** the error is caught, **Then** the SQL query and parameters are logged (excluding sensitive data)

3. **Given** a user performs a critical action (delete, major update), **When** the action completes, **Then** an audit log entry is created1. **Given** multiple forms need to validate required fields, **When** reviewing the code, **Then** all forms use `EditClass.NullOrWhite()` and `EditClass.BojaPozadineZaPrazneCeliji()` consistently

4. **Given** an administrator needs to investigate an issue, **When** reviewing logs, **Then** they can filter by date, user, form, or error type

2. **Given** forms need to execute database queries, **When** reviewing the code, **Then** all forms use `DbClass.ExcecuteNonQuery()` and `DbClass.Select()` instead of inline database code

---

3. **Given** forms need to display error messages, **When** reviewing the code, **Then** all forms use `EditClass.MessageException()`, `EditClass.MessageDBErrorInsert()`, etc. for user feedbackAs a user entering data into any form, I should immediately see which fields are required and receive clear feedback when my input doesn't meet the expected format, so I can correct errors before attempting to save.**Independent Test**: [Describe how this can be tested independently - e.g., "Can be fully tested by [specific action] and delivers [specific value]"]

### Edge Cases

4. **Given** new validation logic is needed, **When** adding it to EditClass, **Then** all forms can immediately use it without code duplication

- What happens when a user has a session open during a code deployment or database update?

- How does the system handle partial validation failures (some fields valid, others not)?

- What happens when the database is read-only or in maintenance mode?

- How does the system behave when network latency is high?---

- What happens if two users try to edit the same record simultaneously?

- How are validation rules enforced when importing bulk data?**Why this priority**: Inconsistent validation leads to bad data, which causes downstream errors and reduces trust in the system. This directly impacts data integrity.**Acceptance Scenarios**:

- What happens when a foreign key reference is deleted while a form is open?

- How does the system handle very long text input in fields with length limits?### User Story 4 - Improved Audit Trail (Priority: P2)



## Requirements *(mandatory)*



### Constitution Compliance Check *(KPP-specific)*As a system administrator reviewing data changes, I should be able to see who made each change and when it was made, ensuring accountability and enabling investigation of data issues.



Before proceeding, verify this feature aligns with project constitution:**Independent Test**: Can be fully tested by attempting to submit incomplete or invalid data across all forms (Djelatnici, Oprema, Vozila, etc.) and verifying that validation rules are consistently enforced with pink highlight feedback.1. **Given** [initial state], **When** [action], **Then** [expected outcome]



- [x] **Stability First**: Does not degrade existing production functionality - all improvements are additive and backward compatible**Why this priority**: Audit trails are essential for compliance, troubleshooting, and user accountability. This supports the constitution's Data Integrity principle.

- [x] **Backward Compatibility**: Preserves legacy data and workflows - no schema changes, only code improvements

- [x] **Data Integrity**: Maintains audit trails (korisnikId, azurirano fields) - explicitly strengthens audit field population2. **Given** [initial state], **When** [action], **Then** [expected outcome]

- [x] **Validation Culture**: Required field validation with pink highlight feedback - explicitly enhances validation consistency

- [x] **Security**: Respects role-based permissions (Administrator/User) - no changes to security model**Independent Test**: Can be fully tested by making changes to records across all forms and verifying that korisnikId and azurirano/dateEdited fields are correctly populated with current user and timestamp.

- [x] **Code Quality**: Follows MVC pattern and uses helper classes - explicitly improves helper class usage and reduces duplication

- [x] **Consistent UX**: Uses standard form layout (DataGridView + search/filter + CRUD buttons) - no UI changes, only behavior improvements**Acceptance Scenarios**:

- [x] **Database Changes**: If schema changes required, includes migration script and version bump - NO database schema changes in this feature

**Acceptance Scenarios**:

*Reference: `.specify/memory/constitution.md`*

---

### Functional Requirements

1. **Given** a user creates a new employee record, **When** the record is saved, **Then** korisnikId is set to the current user's ID and azurirano is set to today's date

**Error Handling & Stability**

2. **Given** a user updates equipment information, **When** the record is saved, **Then** korisnikId and dateEdited are updated to reflect the current user and timestamp1. **Given** a user is on the Djelatnici form, **When** they leave required fields (Ime, Prezime) empty and click save, **Then** those fields turn pink and they see "Ćelije ne smiju biti prazne!"

- **FR-001**: System MUST wrap all database operations in try-catch blocks with appropriate error handling

- **FR-002**: System MUST display user-friendly Croatian error messages for all error conditions3. **Given** an administrator reviews a record, **When** viewing the data grid, **Then** they can see who last modified each record and when

- **FR-003**: System MUST log all exceptions with timestamp, user context, form name, and stack trace

- **FR-004**: System MUST preserve user input when validation fails or errors occur4. **Given** audit fields are missing from a form, **When** code review occurs, **Then** the form is updated to include audit field population2. **Given** a user enters an invalid OIB format, **When** they attempt to save, **Then** the OIB field turns pink and they see "OIB mora imati 11 znamenaka"### User Story 2 - [Brief Title] (Priority: P2)

- **FR-005**: System MUST prevent null reference exceptions through consistent null checking



**Validation & Data Integrity**

---3. **Given** a user selects a termination date before employment date, **When** they attempt to save, **Then** they see "Datum otkaza mora biti nakon datuma zaposlenja"

- **FR-006**: System MUST preserve existing authentication via LoginHelper (username/password with role-based access)

- **FR-007**: System MUST highlight invalid fields with pink background color

- **FR-008**: System MUST validate data format (OIB, dates, numbers) according to business rules

- **FR-009**: System MUST enforce unique constraints (serial numbers, usernames, registration numbers)### User Story 5 - Standardized Null Handling (Priority: P3)4. **Given** a user enters a duplicate serial number for equipment, **When** they attempt to save, **Then** they see "Serijski broj već postoji u bazi"[Describe this user journey in plain language]

- **FR-010**: System MUST validate foreign key references before saving

- **FR-011**: System MUST enforce logical date constraints (termination after employment, etc.)

- **FR-012**: System MUST clear validation highlights when fields become valid

As a developer, when I work with data that might be null or empty, I should have consistent helper methods that prevent null reference exceptions and ensure predictable behavior across all forms.5. **Given** all validation passes, **When** user clicks save, **Then** pink highlights are cleared and data is saved successfully

**Audit & Tracking**



- **FR-013**: System MUST populate korisnikId field with current user's ID on all insert and update operations

- **FR-014**: System MUST populate azurirano/dateEdited field with current timestamp on all insert and update operations. *Note: Timestamp field names vary by table per existing schema (azurirano, Ažurirano, DateEdited, EditedOn). See Current Audit Field State section and data-model.md audit field summary. Implementation must use correct field name per entity.***Why this priority**: Null reference exceptions are a common source of crashes. Standardizing null handling prevents these errors proactively.**Why this priority**: [Explain the value and why it has this priority level]

- **FR-015**: System MUST ensure audit fields exist on all business entity tables



**Code Quality & Maintainability**

**Independent Test**: Can be fully tested by code review identifying all places where null checks are needed and verifying they use standardized helper methods, plus testing with null/empty data scenarios.---

- **FR-016**: System MUST use centralized helper methods from EditClass for validation operations

- **FR-017**: System MUST use centralized helper methods from DbClass for database operations

- **FR-018**: System MUST follow consistent naming conventions (Croatian property names matching database columns)

- **FR-019**: System MUST document business rules in code comments where logic is not self-evident**Acceptance Scenarios**:**Independent Test**: [Describe how this can be tested independently]

- **FR-020**: System MUST organize methods logically within each form class

- **FR-021**: System MUST eliminate duplicate validation and database logic across forms



**User Experience**1. **Given** a form retrieves data from the database, **When** a field might be null, **Then** the code uses null-coalescing operators or helper methods to provide safe defaults### User Story 3 - Reduced Code Duplication (Priority: P2)



- **FR-022**: System MUST maintain existing keyboard shortcuts (F2, F3, F4) and menu functionality2. **Given** a user doesn't enter data in an optional field, **When** the form processes the data, **Then** no null reference exception occurs

- **FR-023**: System MUST preserve existing form layouts and visual design

- **FR-024**: System MUST maintain existing search and filter functionality3. **Given** a dropdown has no selection, **When** the form accesses the selected value, **Then** the code safely handles the null case**Acceptance Scenarios**:

- **FR-025**: System MUST display Croatian error messages using standardized text from EditClass

4. **Given** autocomplete suggestions might be empty, **When** displaying the list, **Then** the code handles empty collections gracefully

**Logging & Retention**

As a developer maintaining the codebase, when I need to add validation logic or database operations, I should be able to use centralized helper functions instead of copying and pasting code, reducing bugs and making the codebase easier to understand.

- **FR-026**: System MUST retain error logs for 365 days and automatically purge older logs on application startup to prevent disk space issues

---

### Key Entities *(code quality focus)*

1. **Given** [initial state], **When** [action], **Then** [expected outcome]

This feature improves code quality across existing entities without modifying data structures:

### User Story 6 - Enhanced Logging (Priority: P3)

- **Forms**: All form classes (Djelatnici, Oprema, Vozila, Korisnici, ZaRa, etc.) - improved error handling, validation, and helper method usage

- **Models**: All model classes in Models/ folder - no changes expected, already well-structured**Why this priority**: Code duplication makes maintenance harder and increases the risk of inconsistent behavior. Centralizing logic improves long-term maintainability.

- **Controllers**: All controller classes - improved error handling and audit field population

- **Helper Classes**: As a system administrator troubleshooting issues, I should be able to review detailed logs that capture errors, user actions, and system events, enabling faster problem resolution.

  - **DbClass**: Enhanced with additional helper methods for common database operations

  - **EditClass**: Enhanced with additional validation helper methods---

  - **LoginHelper**: No changes expected, already serves its purpose

**Why this priority**: Good logging accelerates troubleshooting and reduces downtime. This supports operational efficiency.

### Current Audit Field State *(documented 2025-10-24)*

**Independent Test**: Can be fully tested by code review analyzing helper class usage across all forms and verifying that common operations (validation, database queries, error handling) use shared functions from DbClass and EditClass.

**All 14 business entity tables currently have audit tracking fields:**

**Independent Test**: Can be fully tested by triggering various error scenarios and verifying that appropriate log entries are created with sufficient detail for diagnosis.

| Table | User Field | Timestamp Field | Naming Convention |

|-------|-----------|----------------|-------------------|### User Story 3 - [Brief Title] (Priority: P3)

| Djelatnici | KorisnikId | Azurirano | Croatian (standard) |

| Korisnici | KorisnikId | Azurirano | Croatian (standard) |**Acceptance Scenarios**:

| Odjeli | KorisnikId | Azurirano | Croatian (standard) |

| Mjesta | KorisnikId | Azurirano | Croatian (standard) |**Acceptance Scenarios**:

| Partneri | KorisnikId | Azurirano | Croatian (standard) |

| Posiljatelji | KorisnikId | Azurirano | Croatian (standard) |1. **Given** an exception occurs in any form, **When** caught by try-catch, **Then** a log entry is created with timestamp, user, form name, exception message, and stack trace

| Knjiga | KorisnikId | Azurirano | Croatian (standard) |

| Poslane | KorisnikId | Azurirano | Croatian (standard) |2. **Given** a database operation fails, **When** the error is caught, **Then** the SQL query and parameters are logged (excluding sensitive data)[Describe this user journey in plain language]

| DataCards | KorisnikId | Ažurirano | Croatian (with diacritic) |

| Enc | KorisnikId | Ažurirano | Croatian (with diacritic) |3. **Given** a user performs a critical action (delete, major update), **When** the action completes, **Then** an audit log entry is created

| Oprema | KorisnikId | DateEdited | **English (inconsistent)** |

| Vozila | KorisnikId | DateEdited | **English (inconsistent)** |4. **Given** an administrator needs to investigate an issue, **When** reviewing logs, **Then** they can filter by date, user, form, or error type1. **Given** multiple forms need to validate required fields, **When** reviewing the code, **Then** all forms use `EditClass.NullOrWhite()` and `EditClass.BojaPozadineZaPrazneCeliji()` consistently

| KarticeUlaz | KorisnikId | DateEdited | **English (inconsistent)** |

| ZaRa | KorisnikId | EditedOn | **English (inconsistent)** |



**Key Finding**: Inconsistent timestamp field naming - 10 tables use Croatian naming (`Azurirano` or `Ažurirano`), but 4 tables use English naming (`DateEdited` or `EditedOn`).---2. **Given** forms need to execute database queries, **When** reviewing the code, **Then** all forms use `DbClass.ExcecuteNonQuery()` and `DbClass.Select()` instead of inline database code**Why this priority**: [Explain the value and why it has this priority level]



**Implementation Impact**: 

- All Models already have audit fields defined with defaults (`LoginHelper.StaticId` and `DateTime.Now.Date`)

- No database schema changes needed### Edge Cases3. **Given** forms need to display error messages, **When** reviewing the code, **Then** all forms use `EditClass.MessageException()`, `EditClass.MessageDBErrorInsert()`, etc. for user feedback

- Controllers must ensure these fields are populated during INSERT/UPDATE operations

- Naming inconsistency is acceptable (preserves backward compatibility) - no standardization required for this feature



### Data Retention Policy- What happens when a user has a session open during a code deployment or database update?4. **Given** new validation logic is needed, **When** adding it to EditClass, **Then** all forms can immediately use it without code duplication**Independent Test**: [Describe how this can be tested independently]



**Note**: Data retention for business records (employees, equipment, vehicles, etc.) is managed outside this feature scope. All database entries that are linked as foreign keys will be retained indefinitely. Database records cannot be deleted if they have active references in other tables. A comprehensive data retention and archival policy will be addressed in a future code refactoring effort when business requirements are formalized.- How does the system handle partial validation failures (some fields valid, others not)?



### Assumptions- What happens when the database is read-only or in maintenance mode?



- Current error messages in EditClass are appropriate and don't need rewording (only need consistent usage)- How does the system behave when network latency is high?

- Existing database schema is correct and doesn't need changes

- Current MVC architecture is sound and should be preserved- What happens if two users try to edit the same record simultaneously?---**Acceptance Scenarios**:

- User workflows are well-established and shouldn't be disrupted

- Development will be done incrementally, one form or improvement area at a time- How are validation rules enforced when importing bulk data?

- Testing will be done in non-production environment before deployment

- Logging framework decision deferred - can use simple file logging or existing mechanism- What happens when a foreign key reference is deleted while a form is open?



## Success Criteria *(mandatory)*- How does the system handle very long text input in fields with length limits?



### Measurable Outcomes### User Story 4 - Improved Audit Trail (Priority: P2)1. **Given** [initial state], **When** [action], **Then** [expected outcome]



**Stability & Reliability**## Requirements *(mandatory)*



- **SC-001**: Production runtime errors (unhandled exceptions) reduced by 90% within 30 days of deployment

- **SC-002**: Zero data corruption incidents related to validation failures after deployment

- **SC-003**: All forms complete save operations successfully when valid data is entered (100% success rate)### Constitution Compliance Check *(KPP-specific)*

- **SC-004**: System recovers gracefully from all database connection failures without data loss

As a system administrator reviewing data changes, I should be able to see who made each change and when it was made, ensuring accountability and enabling investigation of data issues.---

**Data Quality & Integrity**

Before proceeding, verify this feature aligns with project constitution:

- **SC-005**: 100% of new and updated records include correct korisnikId and timestamp in audit fields

- **SC-006**: Invalid data entry attempts are prevented before reaching database (100% validation coverage on required fields)

- **SC-007**: Duplicate serial numbers, usernames, and registration numbers are blocked at UI level (0% duplicates accepted)

- **SC-008**: All date logic validations prevent illogical dates (termination before employment, replacement before purchase, etc.)- [x] **Stability First**: Does not degrade existing production functionality - all improvements are additive and backward compatible



**User Experience**- [x] **Backward Compatibility**: Preserves legacy data and workflows - no schema changes, only code improvements**Why this priority**: Audit trails are essential for compliance, troubleshooting, and user accountability. This supports the constitution's Data Integrity principle.[Add more user stories as needed, each with an assigned priority]



- **SC-009**: Users receive clear Croatian error messages for 100% of validation failures- [x] **Data Integrity**: Maintains audit trails (korisnikId, azurirano fields) - explicitly strengthens audit field population

- **SC-010**: Users can identify which fields need correction without reading error messages (pink highlight visible immediately)

- **SC-011**: User input is preserved 100% of the time when validation fails or errors occurs- [x] **Validation Culture**: Required field validation with pink highlight feedback - explicitly enhances validation consistency

- **SC-012**: No changes to existing keyboard shortcuts, menu items, or form layouts (0% workflow disruption)

- [x] **Security**: Respects role-based permissions (Administrator/User) - no changes to security model

**Code Quality**

- [x] **Code Quality**: Follows MVC pattern and uses helper classes - explicitly improves helper class usage and reduces duplication**Independent Test**: Can be fully tested by making changes to records across all forms and verifying that korisnikId and azurirano/dateEdited fields are correctly populated with current user and timestamp.### Edge Cases

- **SC-013**: Code duplication reduced by 60% through centralized helper method usage

- **SC-014**: 100% of database operations use DbClass helper methods (no inline SQL connection handling in forms)- [x] **Consistent UX**: Uses standard form layout (DataGridView + search/filter + CRUD buttons) - no UI changes, only behavior improvements

- **SC-015**: 100% of validation operations use EditClass helper methods

- **SC-016**: New developer onboarding time reduced by 40% due to clearer, more consistent codebase- [x] **Database Changes**: If schema changes required, includes migration script and version bump - NO database schema changes in this feature

- **SC-017**: Code review findings related to error handling, validation, or duplication reduced by 80%



**Operational Efficiency**

*Reference: `.specify/memory/constitution.md`***Acceptance Scenarios**:<!--

- **SC-018**: Support tickets related to unclear error messages reduced by 70%

- **SC-019**: Time to diagnose and fix production issues reduced by 50% through enhanced logging

- **SC-020**: Maintenance development time for adding new forms or fields reduced by 40% through reusable patterns

### Functional Requirements  ACTION REQUIRED: The content in this section represents placeholders.

## Scope & Boundaries



### In Scope

**Error Handling & Stability**1. **Given** a user creates a new employee record, **When** the record is saved, **Then** korisnikId is set to the current user's ID and azurirano is set to today's date  Fill them out with the right edge cases.

- Review and improve all existing forms (Djelatnici, Oprema, Vozila, Korisnici, ZaRa, Odjeli, Mjesta, Partneri, Posiljatelji, Poslane, Knjiga, KeyCards, DataCards, Enc, Ovlastenja, ExportData)

- Enhance DbClass and EditClass helper classes with additional utility methods

- Standardize error handling across all controllers

- Improve validation consistency across all forms- **FR-001**: System MUST wrap all database operations in try-catch blocks with appropriate error handling2. **Given** a user updates equipment information, **When** the record is saved, **Then** korisnikId and dateEdited are updated to reflect the current user and timestamp-->

- Strengthen audit field population in all controllers

- Enhance code documentation and organization- **FR-002**: System MUST display user-friendly Croatian error messages for all error conditions

- Implement comprehensive logging for troubleshooting

- **FR-003**: System MUST log all exceptions with timestamp, user context, form name, and stack trace3. **Given** an administrator reviews a record, **When** viewing the data grid, **Then** they can see who last modified each record and when

### Out of Scope

- **FR-004**: System MUST preserve user input when validation fails or errors occur

- Database schema changes or migrations

- New feature development (new forms or major functionality)- **FR-005**: System MUST prevent null reference exceptions through consistent null checking4. **Given** audit fields are missing from a form, **When** code review occurs, **Then** the form is updated to include audit field population- What happens when [boundary condition]?

- UI redesign or visual changes beyond validation highlights

- Performance optimization (unless directly related to error handling)

- Migration to new technology stack (stays in current C# WinForms)

- Changes to authentication/authorization logic**Validation & Data Integrity**- How does system handle [error scenario]?

- Modifications to business rules or workflow logic (preserve existing behavior)



## Dependencies & Constraints

- **FR-006**: System MUST validate all required fields before allowing save operations---

### Dependencies

- **FR-007**: System MUST highlight invalid fields with pink background color

- Access to complete source code repository

- Access to production database schema documentation- **FR-008**: System MUST validate data format (OIB, dates, numbers) according to business rules## Requirements *(mandatory)*

- Access to sample/test data for validation testing

- Non-production environment for testing changes- **FR-009**: System MUST enforce unique constraints (serial numbers, usernames, registration numbers)

- Constitution compliance verification before implementation

- **FR-010**: System MUST validate foreign key references before saving### User Story 5 - Standardized Null Handling (Priority: P3)

### Constraints

- **FR-011**: System MUST enforce logical date constraints (termination after employment, etc.)

- All changes must be backward compatible with existing data

- No downtime allowed for production system during incremental rollout- **FR-012**: System MUST clear validation highlights when fields become valid<!--

- Changes must not alter existing user workflows or keyboard shortcuts

- Must maintain Croatian language for all user-facing messages

- Must preserve existing MVC architecture and helper class patterns

- Each improvement should be testable independently**Audit & Tracking**As a developer, when I work with data that might be null or empty, I should have consistent helper methods that prevent null reference exceptions and ensure predictable behavior across all forms.  ACTION REQUIRED: The content in this section represents placeholders.

- Deployment must support rollback in case of issues



## Notes

- **FR-013**: System MUST populate korisnikId field with current user's ID on all insert and update operations  Fill them out with the right functional requirements.

- This is a code quality improvement initiative, not a feature addition

- Success depends on thorough testing in non-production before each deployment- **FR-014**: System MUST populate azurirano/dateEdited field with current timestamp on all insert and update operations

- Incremental rollout recommended: start with one form, verify stability, then proceed

- Consider pairing this work with creation of regression test suite for long-term quality- **FR-015**: System MUST ensure audit fields exist on all business entity tables**Why this priority**: Null reference exceptions are a common source of crashes. Standardizing null handling prevents these errors proactively.-->

- Documentation updates should accompany code changes where appropriate

- May identify opportunities for future enhancements, but those should be separate specifications


**Code Quality & Maintainability**



- **FR-016**: System MUST use centralized helper methods from EditClass for validation operations**Independent Test**: Can be fully tested by code review identifying all places where null checks are needed and verifying they use standardized helper methods, plus testing with null/empty data scenarios.### Constitution Compliance Check *(KPP-specific)*

- **FR-017**: System MUST use centralized helper methods from DbClass for database operations

- **FR-018**: System MUST follow consistent naming conventions (Croatian property names matching database columns)

- **FR-019**: System MUST document business rules in code comments where logic is not self-evident

- **FR-020**: System MUST organize methods logically within each form class**Acceptance Scenarios**:Before proceeding, verify this feature aligns with project constitution:

- **FR-021**: System MUST eliminate duplicate validation and database logic across forms



**User Experience**

1. **Given** a form retrieves data from the database, **When** a field might be null, **Then** the code uses null-coalescing operators or helper methods to provide safe defaults- [ ] **Stability First**: Does not degrade existing production functionality

- **FR-022**: System MUST maintain existing keyboard shortcuts (F2, F3, F4) and menu functionality

- **FR-023**: System MUST preserve existing form layouts and visual design2. **Given** a user doesn't enter data in an optional field, **When** the form processes the data, **Then** no null reference exception occurs- [ ] **Backward Compatibility**: Preserves legacy data and workflows

- **FR-024**: System MUST maintain existing search and filter functionality

- **FR-025**: System MUST display Croatian error messages using standardized text from EditClass3. **Given** a dropdown has no selection, **When** the form accesses the selected value, **Then** the code safely handles the null case- [ ] **Data Integrity**: Maintains audit trails (korisnikId, azurirano fields)



### Key Entities *(code quality focus)*4. **Given** autocomplete suggestions might be empty, **When** displaying the list, **Then** the code handles empty collections gracefully- [ ] **Validation Culture**: Required field validation with pink highlight feedback



This feature improves code quality across existing entities without modifying data structures:- [ ] **Security**: Respects role-based permissions (Administrator/User)



- **Forms**: All form classes (Djelatnici, Oprema, Vozila, Korisnici, ZaRa, etc.) - improved error handling, validation, and helper method usage---- [ ] **Code Quality**: Follows MVC pattern and uses helper classes

- **Models**: All model classes in Models/ folder - no changes expected, already well-structured

- **Controllers**: All controller classes - improved error handling and audit field population- [ ] **Consistent UX**: Uses standard form layout (DataGridView + search/filter + CRUD buttons)

- **Helper Classes**: 

  - **DbClass**: Enhanced with additional helper methods for common database operations### User Story 6 - Enhanced Logging (Priority: P3)- [ ] **Database Changes**: If schema changes required, includes migration script and version bump

  - **EditClass**: Enhanced with additional validation helper methods

  - **LoginHelper**: No changes expected, already serves its purpose



### AssumptionsAs a system administrator troubleshooting issues, I should be able to review detailed logs that capture errors, user actions, and system events, enabling faster problem resolution.*Reference: `.specify/memory/constitution.md`*



- Current error messages in EditClass are appropriate and don't need rewording (only need consistent usage)

- Existing database schema is correct and doesn't need changes

- Current MVC architecture is sound and should be preserved**Why this priority**: Good logging accelerates troubleshooting and reduces downtime. This supports operational efficiency.### Functional Requirements

- User workflows are well-established and shouldn't be disrupted

- Development will be done incrementally, one form or improvement area at a time

- Testing will be done in non-production environment before deployment

- Logging framework decision deferred - can use simple file logging or existing mechanism**Independent Test**: Can be fully tested by triggering various error scenarios and verifying that appropriate log entries are created with sufficient detail for diagnosis.- **FR-001**: System MUST [specific capability, e.g., "allow users to create accounts"]



## Success Criteria *(mandatory)*- **FR-002**: System MUST [specific capability, e.g., "validate email addresses"]  



### Measurable Outcomes**Acceptance Scenarios**:- **FR-003**: Users MUST be able to [key interaction, e.g., "reset their password"]



**Stability & Reliability**- **FR-004**: System MUST [data requirement, e.g., "persist user preferences"]



- **SC-001**: Production runtime errors (unhandled exceptions) reduced by 90% within 30 days of deployment1. **Given** an exception occurs in any form, **When** caught by try-catch, **Then** a log entry is created with timestamp, user, form name, exception message, and stack trace- **FR-005**: System MUST [behavior, e.g., "log all security events"]

- **SC-002**: Zero data corruption incidents related to validation failures after deployment

- **SC-003**: All forms complete save operations successfully when valid data is entered (100% success rate)2. **Given** a database operation fails, **When** the error is caught, **Then** the SQL query and parameters are logged (excluding sensitive data)

- **SC-004**: System recovers gracefully from all database connection failures without data loss

3. **Given** a user performs a critical action (delete, major update), **When** the action completes, **Then** an audit log entry is created*Example of marking unclear requirements:*

**Data Quality & Integrity**

4. **Given** an administrator needs to investigate an issue, **When** reviewing logs, **Then** they can filter by date, user, form, or error type

- **SC-005**: 100% of new and updated records include correct korisnikId and timestamp in audit fields

- **SC-006**: Invalid data entry attempts are prevented before reaching database (100% validation coverage on required fields)- **FR-006**: System MUST authenticate users via [NEEDS CLARIFICATION: auth method not specified - email/password, SSO, OAuth?]

- **SC-007**: Duplicate serial numbers, usernames, and registration numbers are blocked at UI level (0% duplicates accepted)

- **SC-008**: All date logic validations prevent illogical dates (termination before employment, replacement before purchase, etc.)---- **FR-007**: System MUST retain user data for [NEEDS CLARIFICATION: retention period not specified]



**User Experience**



- **SC-009**: Users receive clear Croatian error messages for 100% of validation failures### Edge Cases### Key Entities *(include if feature involves data)*

- **SC-010**: Users can identify which fields need correction without reading error messages (pink highlight visible immediately)

- **SC-011**: User input is preserved 100% of the time when validation fails or errors occur

- **SC-012**: No changes to existing keyboard shortcuts, menu items, or form layouts (0% workflow disruption)

- What happens when a user has a session open during a code deployment or database update?- **[Entity 1]**: [What it represents, key attributes without implementation]

**Code Quality**

- How does the system handle partial validation failures (some fields valid, others not)?- **[Entity 2]**: [What it represents, relationships to other entities]

- **SC-013**: Code duplication reduced by 60% through centralized helper method usage

- **SC-014**: 100% of database operations use DbClass helper methods (no inline SQL connection handling in forms)- What happens when the database is read-only or in maintenance mode?

- **SC-015**: 100% of validation operations use EditClass helper methods

- **SC-016**: New developer onboarding time reduced by 40% due to clearer, more consistent codebase- How does the system behave when network latency is high?## Success Criteria *(mandatory)*

- **SC-017**: Code review findings related to error handling, validation, or duplication reduced by 80%

- What happens if two users try to edit the same record simultaneously?

**Operational Efficiency**

- How are validation rules enforced when importing bulk data?<!--

- **SC-018**: Support tickets related to unclear error messages reduced by 70%

- **SC-019**: Time to diagnose and fix production issues reduced by 50% through enhanced logging- What happens when a foreign key reference is deleted while a form is open?  ACTION REQUIRED: Define measurable success criteria.

- **SC-020**: Maintenance development time for adding new forms or fields reduced by 40% through reusable patterns

- How does the system handle very long text input in fields with length limits?  These must be technology-agnostic and measurable.

## Scope & Boundaries

-->

### In Scope

## Requirements *(mandatory)*

- Review and improve all existing forms (Djelatnici, Oprema, Vozila, Korisnici, ZaRa, Odjeli, Mjesta, Partneri, Posiljatelji, Poslane, Knjiga, KeyCards, DataCards, Enc, Ovlastenja, ExportData)

- Enhance DbClass and EditClass helper classes with additional utility methods### Measurable Outcomes

- Standardize error handling across all controllers

- Improve validation consistency across all forms### Constitution Compliance Check *(KPP-specific)*

- Strengthen audit field population in all controllers

- Enhance code documentation and organization- **SC-001**: [Measurable metric, e.g., "Users can complete account creation in under 2 minutes"]

- Implement comprehensive logging for troubleshooting

Before proceeding, verify this feature aligns with project constitution:- **SC-002**: [Measurable metric, e.g., "System handles 1000 concurrent users without degradation"]

### Out of Scope

- **SC-003**: [User satisfaction metric, e.g., "90% of users successfully complete primary task on first attempt"]

- Database schema changes or migrations

- New feature development (new forms or major functionality)- [x] **Stability First**: Does not degrade existing production functionality - all improvements are additive and backward compatible- **SC-004**: [Business metric, e.g., "Reduce support tickets related to [X] by 50%"]

- UI redesign or visual changes beyond validation highlights

- Performance optimization (unless directly related to error handling)- [x] **Backward Compatibility**: Preserves legacy data and workflows - no schema changes, only code improvements

- Migration to new technology stack (stays in current C# WinForms)- [x] **Data Integrity**: Maintains audit trails (korisnikId, azurirano fields) - explicitly strengthens audit field population

- Changes to authentication/authorization logic- [x] **Validation Culture**: Required field validation with pink highlight feedback - explicitly enhances validation consistency

- Modifications to business rules or workflow logic (preserve existing behavior)- [x] **Security**: Respects role-based permissions (Administrator/User) - no changes to security model

- [x] **Code Quality**: Follows MVC pattern and uses helper classes - explicitly improves helper class usage and reduces duplication

## Dependencies & Constraints- [x] **Consistent UX**: Uses standard form layout (DataGridView + search/filter + CRUD buttons) - no UI changes, only behavior improvements

- [x] **Database Changes**: If schema changes required, includes migration script and version bump - NO database schema changes in this feature

### Dependencies

*Reference: `.specify/memory/constitution.md`*

- Access to complete source code repository

- Access to production database schema documentation### Functional Requirements

- Access to sample/test data for validation testing

- Non-production environment for testing changes**Error Handling & Stability**

- Constitution compliance verification before implementation

- **FR-001**: System MUST wrap all database operations in try-catch blocks with appropriate error handling

### Constraints- **FR-002**: System MUST display user-friendly Croatian error messages for all error conditions

- **FR-003**: System MUST log all exceptions with timestamp, user context, form name, and stack trace

- All changes must be backward compatible with existing data- **FR-004**: System MUST preserve user input when validation fails or errors occur

- No downtime allowed for production system during incremental rollout- **FR-005**: System MUST prevent null reference exceptions through consistent null checking

- Changes must not alter existing user workflows or keyboard shortcuts

- Must maintain Croatian language for all user-facing messages**Validation & Data Integrity**

- Must preserve existing MVC architecture and helper class patterns

- Each improvement should be testable independently- **FR-006**: System MUST validate all required fields before allowing save operations

- Deployment must support rollback in case of issues- **FR-007**: System MUST highlight invalid fields with pink background color

- **FR-008**: System MUST validate data format (OIB, dates, numbers) according to business rules

## Notes- **FR-009**: System MUST enforce unique constraints (serial numbers, usernames, registration numbers)

- **FR-010**: System MUST validate foreign key references before saving

- This is a code quality improvement initiative, not a feature addition- **FR-011**: System MUST enforce logical date constraints (termination after employment, etc.)

- Success depends on thorough testing in non-production before each deployment- **FR-012**: System MUST clear validation highlights when fields become valid

- Incremental rollout recommended: start with one form, verify stability, then proceed

- Consider pairing this work with creation of regression test suite for long-term quality**Audit & Tracking**

- Documentation updates should accompany code changes where appropriate

- May identify opportunities for future enhancements, but those should be separate specifications- **FR-013**: System MUST populate korisnikId field with current user's ID on all insert and update operations
- **FR-014**: System MUST populate azurirano/dateEdited field with current timestamp on all insert and update operations
- **FR-015**: System MUST ensure audit fields exist on all business entity tables

**Code Quality & Maintainability**

- **FR-016**: System MUST use centralized helper methods from EditClass for validation operations
- **FR-017**: System MUST use centralized helper methods from DbClass for database operations
- **FR-018**: System MUST follow consistent naming conventions (Croatian property names matching database columns)
- **FR-019**: System MUST document business rules in code comments where logic is not self-evident
- **FR-020**: System MUST organize methods logically within each form class
- **FR-021**: System MUST eliminate duplicate validation and database logic across forms

**User Experience**

- **FR-022**: System MUST maintain existing keyboard shortcuts (F2, F3, F4) and menu functionality
- **FR-023**: System MUST preserve existing form layouts and visual design
- **FR-024**: System MUST maintain existing search and filter functionality
- **FR-025**: System MUST display Croatian error messages using standardized text from EditClass

### Key Entities *(code quality focus)*

This feature improves code quality across existing entities without modifying data structures:

- **Forms**: All form classes (Djelatnici, Oprema, Vozila, Korisnici, ZaRa, etc.) - improved error handling, validation, and helper method usage
- **Models**: All model classes in Models/ folder - no changes expected, already well-structured
- **Controllers**: All controller classes - improved error handling and audit field population
- **Helper Classes**: 
  - **DbClass**: Enhanced with additional helper methods for common database operations
  - **EditClass**: Enhanced with additional validation helper methods
  - **LoginHelper**: No changes expected, already serves its purpose

### Current Audit Field State *(documented 2025-10-24)*

**All 14 business entity tables currently have audit tracking fields:**

| Table | User Field | Timestamp Field | Naming Convention |
|-------|-----------|----------------|-------------------|
| Djelatnici | KorisnikId | Azurirano | Croatian (standard) |
| Korisnici | KorisnikId | Azurirano | Croatian (standard) |
| Odjeli | KorisnikId | Azurirano | Croatian (standard) |
| Mjesta | KorisnikId | Azurirano | Croatian (standard) |
| Partneri | KorisnikId | Azurirano | Croatian (standard) |
| Posiljatelji | KorisnikId | Azurirano | Croatian (standard) |
| Knjiga | KorisnikId | Azurirano | Croatian (standard) |
| Poslane | KorisnikId | Azurirano | Croatian (standard) |
| DataCards | KorisnikId | Ažurirano | Croatian (with diacritic) |
| Enc | KorisnikId | Ažurirano | Croatian (with diacritic) |
| Oprema | KorisnikId | DateEdited | **English (inconsistent)** |
| Vozila | KorisnikId | DateEdited | **English (inconsistent)** |
| KarticeUlaz | KorisnikId | DateEdited | **English (inconsistent)** |
| ZaRa | KorisnikId | EditedOn | **English (inconsistent)** |

**Key Finding**: Inconsistent timestamp field naming - 10 tables use Croatian naming (`Azurirano` or `Ažurirano`), but 4 tables use English naming (`DateEdited` or `EditedOn`).

**Implementation Impact**: 
- All Models already have audit fields defined with defaults (`LoginHelper.StaticId` and `DateTime.Now.Date`)
- No database schema changes needed
- Controllers must ensure these fields are populated during INSERT/UPDATE operations
- Naming inconsistency is acceptable (preserves backward compatibility) - no standardization required for this feature

### Assumptions

- Current error messages in EditClass are appropriate and don't need rewording (only need consistent usage)
- Existing database schema is correct and doesn't need changes
- Current MVC architecture is sound and should be preserved
- User workflows are well-established and shouldn't be disrupted
- Development will be done incrementally, one form or improvement area at a time
- Testing will be done in non-production environment before deployment
- Logging framework decision deferred - can use simple file logging or existing mechanism

## Success Criteria *(mandatory)*

### Measurable Outcomes

**Stability & Reliability**

- **SC-001**: Production runtime errors (unhandled exceptions) reduced by 90% within 30 days of deployment
- **SC-002**: Zero data corruption incidents related to validation failures after deployment
- **SC-003**: All forms complete save operations successfully when valid data is entered (100% success rate)
- **SC-004**: System recovers gracefully from all database connection failures without data loss

**Data Quality & Integrity**

- **SC-005**: 100% of new and updated records include correct korisnikId and timestamp in audit fields
- **SC-006**: Invalid data entry attempts are prevented before reaching database (100% validation coverage on required fields)
- **SC-007**: Duplicate serial numbers, usernames, and registration numbers are blocked at UI level (0% duplicates accepted)
- **SC-008**: All date logic validations prevent illogical dates (termination before employment, replacement before purchase, etc.)

**User Experience**

- **SC-009**: Users receive clear Croatian error messages for 100% of validation failures
- **SC-010**: Users can identify which fields need correction without reading error messages (pink highlight visible immediately)
- **SC-011**: User input is preserved 100% of the time when validation fails or errors occur
- **SC-012**: No changes to existing keyboard shortcuts, menu items, or form layouts (0% workflow disruption)

**Code Quality**

- **SC-013**: Code duplication reduced by 60% through centralized helper method usage
- **SC-014**: 100% of database operations use DbClass helper methods (no inline SQL connection handling in forms)
- **SC-015**: 100% of validation operations use EditClass helper methods
- **SC-016**: New developer onboarding time reduced by 40% due to clearer, more consistent codebase
- **SC-017**: Code review findings related to error handling, validation, or duplication reduced by 80%

**Operational Efficiency**

- **SC-018**: Support tickets related to unclear error messages reduced by 70%
- **SC-019**: Time to diagnose and fix production issues reduced by 50% through enhanced logging
- **SC-020**: Maintenance development time for adding new forms or fields reduced by 40% through reusable patterns

## Scope & Boundaries

### In Scope

- Review and improve all existing forms (Djelatnici, Oprema, Vozila, Korisnici, ZaRa, Odjeli, Mjesta, Partneri, Posiljatelji, Poslane, Knjiga, KeyCards, DataCards, Enc, Ovlastenja, ExportData)
- Enhance DbClass and EditClass helper classes with additional utility methods
- Standardize error handling across all controllers
- Improve validation consistency across all forms
- Strengthen audit field population in all controllers
- Enhance code documentation and organization
- Implement comprehensive logging for troubleshooting

### Out of Scope

- Database schema changes or migrations
- New feature development (new forms or major functionality)
- UI redesign or visual changes beyond validation highlights
- Performance optimization (unless directly related to error handling)
- Migration to new technology stack (stays in current C# WinForms)
- Changes to authentication/authorization logic
- Modifications to business rules or workflow logic (preserve existing behavior)

## Dependencies & Constraints

### Dependencies

- Access to complete source code repository
- Access to production database schema documentation
- Access to sample/test data for validation testing
- Non-production environment for testing changes
- Constitution compliance verification before implementation

### Constraints

- All changes must be backward compatible with existing data
- No downtime allowed for production system during incremental rollout
- Changes must not alter existing user workflows or keyboard shortcuts
- Must maintain Croatian language for all user-facing messages
- Must preserve existing MVC architecture and helper class patterns
- Each improvement should be testable independently
- Deployment must support rollback in case of issues

## Notes

- This is a code quality improvement initiative, not a feature addition
- Success depends on thorough testing in non-production before each deployment
- Incremental rollout recommended: start with one form, verify stability, then proceed
- Consider pairing this work with creation of regression test suite for long-term quality
- Documentation updates should accompany code changes where appropriate
- May identify opportunities for future enhancements, but those should be separate specifications
