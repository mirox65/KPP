<!--
Sync Impact Report - v1.0.0 (Initial Constitution)
────────────────────────────────────────────────────
Version Change: [TEMPLATE] → 1.0.0
Action: Initial constitution creation for KPP asset management system

Modified Principles:
  - All principles newly defined (initial version)

Added Sections:
  ✅ Core Principles (7 principles)
  ✅ Data & Database Standards
  ✅ Security & Compliance
  ✅ Code Quality Standards
  ✅ User Interface Standards
  ✅ Feature Development Process
  ✅ Testing & Deployment
  ✅ Governance

Templates Requiring Updates:
  ⚠ .specify/templates/plan-template.md - pending review
  ⚠ .specify/templates/spec-template.md - pending review
  ⚠ .specify/templates/tasks-template.md - pending review

Follow-up TODOs:
  - Establish baseline regression test suite
  - Document database schema versioning procedure
  - Create security audit checklist
  - Define release process workflow
────────────────────────────────────────────────────
-->

# KPP Application Constitution

**Purpose**: Maintain and improve the company's production-critical asset management system while ensuring reliability, data integrity, and user efficiency.

## Core Principles

### I. Stability First (NON-NEGOTIABLE)

Production stability is paramount. No changes may degrade existing functionality, corrupt data, or disrupt active users. All modifications MUST preserve backward compatibility with existing database schemas, business logic, and user workflows.

**Rationale**: KPP manages critical company assets (employees, equipment, vehicles, documents) in active production. Data loss or workflow disruption directly impacts business operations.

**Non-negotiable rules**:
- Zero tolerance for data corruption or loss
- Existing forms and functionality MUST continue to work as-is
- Changes tested in non-production environment before deployment
- Rollback plan required for every production change

### II. Data Integrity & Auditability

All data modifications MUST maintain referential integrity, preserve audit trails, and enforce validation rules. Database schema changes require explicit versioning and migration planning.

**Rationale**: Historical data is legally and operationally significant. Audit trails enable accountability and compliance.

**Mandatory practices**:
- All tables MUST include: `korisnikId` (creator/modifier), `azurirano` (last updated date)
- Foreign key relationships MUST be validated before record creation
- Required field validation MUST be enforced at UI and business logic layers
- Unique constraints (serial numbers, usernames) MUST be verified before insert/update
- Soft deletes via status flags preferred over physical record deletion

### III. Backward Compatibility

New features and enhancements MUST preserve legacy data structures, existing workflows, and established UI patterns. Breaking changes are forbidden without explicit versioning and migration strategy.

**Rationale**: Users rely on consistent behavior. Database contains years of historical data that cannot be restructured without significant risk.

**Requirements**:
- Access database schema (.accdb) changes require version bump and migration script
- Existing table columns cannot be removed (add new columns, deprecate old ones)
- Form layouts follow established patterns (DataGridView + search/filter + CRUD buttons)
- API contracts (if introduced) MUST maintain backward compatibility

### IV. Validation Culture

User input MUST be validated comprehensively before database operations. Validation errors MUST provide clear, actionable feedback in Croatian language.

**Rationale**: Invalid data causes downstream errors, support burden, and operational issues. Clear validation prevents user frustration.

**Validation requirements**:
- Required fields: Pink background highlight + error message on empty submission
- Data type validation: Dates, numbers, OIB format checks
- Business rule validation: Date logic (termination > employment), equipment availability
- Foreign key existence: Autocomplete ensures valid department/employee/equipment references
- Unique constraint checking: Prevent duplicate serial numbers, usernames, registration plates

**Standard error messages** (Croatian):
- Empty fields: "Ćelije ne smiju biti prazne!"
- Insert failure: "Unos nije napravljen!"
- Update failure: "Izmjena nije izvršena!"
- Key missing: "ID stavke nije pronađen! Provjeri polja na greške."

### V. Security & Least Privilege

User access MUST respect role-based permissions (Administrator/User). Sensitive data MUST be protected according to security best practices.

**Rationale**: Unauthorized access or data exposure creates legal, operational, and privacy risks.

**Security requirements**:
- Password storage: MUST use salted hash (never plain text)
- Role enforcement: `LoginHelper.StaticUloga` controls form/feature access
- Session management: Current user context tracked via `LoginHelper.StaticId`
- Audit logging: All modifications record which user made the change
- Database password protection: Access .accdb uses password `SunceJeLijepo1#`

### VI. Code Quality & Maintainability

Code MUST be modular, well-documented, and follow established patterns from the existing codebase. Duplication is minimized through helper classes.

**Rationale**: Maintainable code reduces bugs, speeds development, and enables knowledge transfer.

**Quality standards**:
- **Modular architecture**: Model-View-Controller pattern (Models/, Controller/, Forms)
- **Helper classes**: `DbClass` for database ops, `EditClass` for validation, `LoginHelper` for auth
- **Consistent naming**: Croatian property names match database columns exactly
- **Error handling**: Try-catch with user-friendly messages via `EditClass.MessageException()`
- **Comments**: Document business rules that aren't obvious from code
- **No duplication**: Extract shared validation/database logic into helpers

### VII. Consistent User Experience

All forms MUST follow established UI/UX patterns to maintain user familiarity and operational efficiency.

**Rationale**: Consistent interface reduces training time, prevents errors, and increases user productivity.

**UI standards**:
- **DataGridView configuration**: Alternating row colors (white/light green), auto-size columns, read-only
- **Search functionality**: `txt_pretrazivanje` real-time filter on all list forms
- **Status filtering**: `ComBoxFilter` dropdown for status-based filtering
- **CRUD buttons**: Insert (F2), Edit (F3), Clear (F4) with keyboard shortcuts
- **Menu structure**: File → Add (Ctrl+N), Save (Ctrl+S), Clear (Ctrl+R)
- **Validation feedback**: Pink background on invalid fields, MessageBox dialogs
- **Autocomplete**: Foreign key fields use autocomplete from lookup tables

## Data & Database Standards

**Database Technology**: Microsoft Access (.accdb) with OleDb provider  
**Connection String**: `Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\KPP.accdb; Jet OLEDB:Database Password=SunceJeLijepo1#;`

### Schema Change Policy

1. All schema changes MUST be documented with migration scripts
2. Version number MUST increment following semantic versioning
3. Test migration on backup database before production deployment
4. Schema changes require administrator approval

### Data Integrity Rules

- **Primary Keys**: Auto-increment integer `id` on all tables
- **Foreign Keys**: Validated via dictionary lookups before insert/update
- **Required Fields**: Enforced at business logic layer (pink highlight pattern)
- **Unique Constraints**: Serial numbers, usernames, registration plates
- **Soft Deletes**: Use `aktivan` or `status` flags instead of DELETE operations
- **Audit Fields**: `korisnikId`, `azurirano`/`dateEdited` mandatory on all business tables

## Security & Compliance

### Authentication & Authorization

- **Login Required**: All users MUST authenticate via username/password
- **Role-Based Access**: Administrator vs User permissions enforced
- **Password Policy**: Salted hash storage (no plain text)
- **Session Tracking**: `LoginHelper` maintains current user context

### Data Protection

- **Sensitive Data**: OIB (tax IDs), employee data, user credentials
- **Access Control**: Role-based feature access via `LoginHelper.StaticUloga`
- **Audit Trail**: All modifications log user ID and timestamp
- **Backup Policy**: Regular .accdb backups required (external to application)

## Code Quality Standards

### Architecture Patterns

- **MVC Structure**: Models/ (data), Controller/ (business logic), Forms (UI)
- **Helper Classes**: Shared utilities in HelperClasses/ folder
- **Separation of Concerns**: Database ops in DbClass, validation in EditClass

### Naming Conventions

- **C# Properties**: PascalCase (e.g., `DatumZaduženja`)
- **Database Columns**: Croatian camelCase (e.g., `datZaduženja`)
- **Form Controls**: Prefix + descriptive name (e.g., `Txt_Ime`, `Cmb_Aktivan`)
- **Tables**: Croatian lowercase (e.g., `djelatnici`, `ictOprema`)

### Error Handling

- **User-Facing**: Croatian language messages via `EditClass` methods
- **Internal**: Try-catch blocks with stack trace logging
- **Database Errors**: Graceful degradation with rollback where applicable

## User Interface Standards

### Form Layout

All data entry forms MUST include:
- Label + Input field pairs with consistent alignment
- DataGridView for record display (alternating colors)
- Search textbox (`txt_pretrazivanje`) at top
- Filter combobox for status filtering
- Button panel: Insert, Edit, Clear (with F-key shortcuts)
- MenuStrip with File menu and shortcuts

### Visual Feedback

- **Validation Errors**: Pink background (`Color.LightPink`) on empty required fields
- **Success**: MessageBox with confirmation
- **Errors**: MessageBox with specific error message
- **Loading**: Consider progress indication for slow operations (future enhancement)

### Keyboard Navigation

- **F2**: Dodaj novi unos (Insert new record)
- **F3**: Spremi izmjene (Save changes)
- **F4**: Očisti polja (Clear form)
- **Ctrl+N**: New record (menu shortcut)
- **Ctrl+S**: Save changes (menu shortcut)
- **Ctrl+R**: Reset form (menu shortcut)

## Feature Development Process

### Incremental Delivery

- Ship small, safe improvements regularly
- Prioritize user-requested features based on operational impact
- Avoid large-scale rewrites; evolve existing codebase incrementally

### Development Workflow

1. **Requirements**: Document user need and acceptance criteria
2. **Design**: Verify alignment with constitution principles
3. **Implementation**: Follow existing code patterns and standards
4. **Testing**: Verify core workflows still function (regression testing)
5. **Documentation**: Update relevant docs and release notes
6. **Deployment**: Test in non-production first, then production with rollback plan

### Change Categories

- **Low Risk**: UI text changes, new optional fields, report enhancements
- **Medium Risk**: New forms, business logic additions, performance improvements
- **High Risk**: Database schema changes, authentication changes, deletion logic

High-risk changes require explicit review and staged rollout.

## Testing & Deployment

### Regression Testing

Before every production deployment, verify:
- Core workflows: Login, employee CRUD, equipment assignment/return
- Data integrity: Foreign key validation, required field checks
- User permissions: Admin vs User access restrictions
- Search/filter functionality on main forms

### Release Management

- **Version Format**: MAJOR.MINOR.PATCH (e.g., 1.2.3)
- **Release Notes**: Document all changes, new features, known issues
- **Database Backups**: Mandatory before schema changes
- **Rollback Plan**: Document steps to revert to previous version

### Deployment Checklist

- [ ] Regression tests passed
- [ ] Database backup created
- [ ] Release notes prepared
- [ ] Rollback plan documented
- [ ] User notification sent (if disruptive changes)
- [ ] Deployment window scheduled
- [ ] Post-deployment verification completed

## Governance

### Constitution Authority

This constitution supersedes all other development practices and guidelines. When conflicts arise, constitution principles take precedence.

### Amendment Process

1. **Proposal**: Document proposed change with rationale
2. **Impact Analysis**: Identify affected code, workflows, users
3. **Review**: Administrator approval required
4. **Migration Plan**: Document how existing code/data will adapt
5. **Version Bump**: Update constitution version per semantic versioning
6. **Communication**: Notify development team of changes

### Version Bump Rules

- **MAJOR**: Backward-incompatible principle removals or redefinitions
- **MINOR**: New principles added or existing principles materially expanded
- **PATCH**: Clarifications, wording improvements, typo fixes

### Compliance Verification

All code changes MUST be verified against these principles:
- Does it preserve stability and backward compatibility?
- Does it maintain data integrity and audit trails?
- Does it follow validation culture standards?
- Does it respect security and authorization rules?
- Does it maintain code quality and consistency?
- Does it follow established UI/UX patterns?

Development guidance documents and templates MUST align with this constitution.

---

**Version**: 1.0.0 | **Ratified**: 2025-10-24 | **Last Amended**: 2025-10-24
