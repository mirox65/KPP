# Data Model: Code Quality Enhancement

**Feature**: 001-code-quality  
**Date**: 2025-10-24  
**Purpose**: Document existing data entities and their audit field requirements

## Overview

This feature does **NOT** modify database schema. This document serves as a reference for understanding which audit fields exist on each table and how they should be populated during refactoring.

## Entity Audit Field Reference

### Core Business Entities

#### 1. Djelatnici (Employees)

**Table Name**: `Djelatnici`  
**Audit Fields**: `korisnikId` (int), `azurirano` (DateTime)  
**Model**: `DjelatnikModel.cs`  
**Controller**: `DjelatnikController.cs`

**Fields**:
- `Id` (PK, AutoNumber)
- `PerNum` (int) - Personnel number
- `Ime` (string) - First name [REQUIRED]
- `Prezime` (string) - Last name [REQUIRED]
- `Oib` (string) - Tax ID [REQUIRED, 11 digits]
- `OdjelId` (int, FK → Odjeli) - Department [REQUIRED]
- `DatZaposlen` (DateTime) - Employment date [REQUIRED]
- `DatOtkaz` (DateTime) - Termination date [OPTIONAL]
- `Aktivan` (string) - Active status ("DA"/"NE") [REQUIRED]
- `korisnikId` (int) - Last modifier [AUDIT]
- `azurirano` (DateTime) - Last modified date [AUDIT]

**Validation Rules**:
- Ime, Prezime cannot be empty
- OIB must be exactly 11 digits
- DatOtkaz must be after DatZaposlen (if provided)
- OdjelId must exist in Odjeli table

---

#### 2. Oprema (Equipment)

**Table Name**: `Oprema`  
**Audit Fields**: `korisnikId` (int), `DateEdited` (DateTime) ⚠️ *English name*  
**Model**: `OpremaModel.cs`  
**Controller**: `OpremaController.cs`

**Fields**:
- `Id` (PK, AutoNumber)
- `Naziv` (string) - Equipment name [REQUIRED]
- `SerijskiBroj` (string) - Serial number [REQUIRED, UNIQUE]
- `InventarniBroj` (string) - Inventory number [DEFAULT: "Novo"]
- `StatusOpreme` (string) - Status ("Novo"/"Zaduženo"/"Razduže no"/"Otpisano") [REQUIRED]
- `DatumKupovine` (DateTime) - Purchase date [REQUIRED]
- `VijekTrajanja` (int) - Lifespan in years [REQUIRED]
- `DatumZamjene` (DateTime) - Replacement date (calculated) [REQUIRED]
- `korisnikId` (int) - Last modifier [AUDIT]
- `DateEdited` (DateTime) - Last modified date [AUDIT]

**Validation Rules**:
- SerijskiBroj must be unique
- StatusOpreme must be one of allowed values
- DatumZamjene should be after DatumKupovine

---

#### 3. Vozila (Vehicles)

**Table Name**: `Vozila`  
**Audit Fields**: `korisnikId` (int), `DateEdited` (DateTime) ⚠️ *English name*  
**Model**: `VozilaModel.cs`  
**Controller**: `VozilaController.cs`

**Fields**:
- `Id` (PK, AutoNumber)
- `Proizvodac` (string) - Manufacturer [REQUIRED]
- `Model` (string) - Model [REQUIRED]
- `Opis` (string) - Description [OPTIONAL]
- `BrSasije` (string) - Chassis number [REQUIRED, UNIQUE]
- `RegOznaka` (string) - Registration plate [REQUIRED, UNIQUE]
- `BrUgovora` (string) - Contract number [OPTIONAL]
- `korisnikId` (int) - Last modifier [AUDIT]
- `DateEdited` (DateTime) - Last modified date [AUDIT]

**Validation Rules**:
- BrSasije must be unique
- RegOznaka must be unique
- Proizvodac and Model cannot be empty

---

#### 4. ZaRa (Equipment Assignment/Return)

**Table Name**: `ZaRa`  
**Audit Fields**: `korisnikId` (int), `EditedOn` (DateTime) ⚠️ *English name variant*  
**Model**: `ZaRaModel.cs`  
**Controller**: `ZaRaController.cs`

**Fields**:
- `Id` (PK, AutoNumber)
- `DjelatnikId` (int, FK → Djelatnici) - Employee [REQUIRED]
- `NazivZaRaTablice` (string) - Equipment table name ("Oprema"/"Vozila"/"KeyCards"/etc.) [REQUIRED]
- `OpremaId` (int) - Equipment ID (references vary by NazivZaRaTablice) [REQUIRED]
- `DatumZaduženja` (DateTime) - Assignment date [REQUIRED]
- `DatumRazduženja` (DateTime) - Return date [OPTIONAL]
- `korisnikId` (int) - Last modifier [AUDIT]
- `EditedOn` (DateTime) - Last modified date [AUDIT]

**Validation Rules**:
- DjelatnikId must exist in Djelatnici
- NazivZaRaTablice must be valid table name
- OpremaId must exist in specified table
- DatumRazduženja must be after DatumZaduženja (if provided)
- Cannot assign already-assigned equipment (check existing records where DatumRazduženja is null)

---

### Reference Data Entities

#### 5. Odjeli (Departments)

**Table Name**: `Odjeli`  
**Audit Fields**: `korisnikId` (int), `azurirano` (DateTime)  
**Model**: `OdjelModel.cs`  
**Controller**: `OdjelController.cs`

**Fields**:
- `Id` (PK, AutoNumber)
- `Naziv` (string) - Department name [REQUIRED, UNIQUE]
- `korisnikId` (int) - Last modifier [AUDIT]
- `azurirano` (DateTime) - Last modified date [AUDIT]

**Validation Rules**:
- Naziv cannot be empty
- Naziv must be unique

---

#### 6. Mjesta (Locations)

**Table Name**: `Mjesta`  
**Audit Fields**: `korisnikId` (int), `azurirano` (DateTime)  
**Model**: `MjestoModel.cs`  
**Controller**: `MjestoController.cs`

**Fields**:
- `Id` (PK, AutoNumber)
- `Ptt` (string) - Postal code [REQUIRED]
- `Mjesto` (string) - City/town name [REQUIRED]
- `IdZupanije` (int) - County ID [REQUIRED]
- `korisnikId` (int) - Last modifier [AUDIT]
- `azurirano` (DateTime) - Last modified date [AUDIT]

**Validation Rules**:
- Ptt and Mjesto cannot be empty
- IdZupanije must be valid (reference data)

---

#### 7. Partneri (Partners)

**Table Name**: `Partneri`  
**Audit Fields**: `korisnikId` (int), `azurirano` (DateTime)  
**Model**: `PartnerModel.cs`  
**Controller**: `PartnerController.cs`

**Fields**:
- `Id` (PK, AutoNumber)
- `Naziv` (string) - Partner name [REQUIRED, UNIQUE]
- `korisnikId` (int) - Last modifier [AUDIT]
- `azurirano` (DateTime) - Last modified date [AUDIT]

**Validation Rules**:
- Naziv cannot be empty
- Naziv should be unique (business rule)

---

#### 8. Posiljatelji (Senders)

**Table Name**: `Posiljatelji`  
**Audit Fields**: `korisnikId` (int), `azurirano` (DateTime)  
**Model**: `PosiljateljModel.cs`  
**Controller**: `PosiljateljController.cs`

**Fields**:
- `Id` (PK, AutoNumber)
- `Naziv` (string) - Sender name [REQUIRED]
- `IdMjesto` (int, FK → Mjesta) - Location [REQUIRED]
- `korisnikId` (int) - Last modifier [AUDIT]
- `azurirano` (DateTime) - Last modified date [AUDIT]

**Validation Rules**:
- Naziv cannot be empty
- IdMjesto must exist in Mjesta table

---

### Document Tracking Entities

#### 9. Knjiga (Registry Book)

**Table Name**: `Knjiga`  
**Audit Fields**: `korisnikId` (int), `azurirano` (DateTime)  
**Model**: `KnjigaModel.cs`  
**Controller**: `KnjigaController.cs`

**Fields**:
- `Id` (PK, AutoNumber)
- `DatumPrimitka` (DateTime) - Receipt date [REQUIRED]
- `Pismeno` (string) - Written reference [OPTIONAL]
- `Brojcano` (string) - Numeric reference [OPTIONAL]
- `PosiljateljId` (int, FK → Posiljatelji) - Sender [REQUIRED]
- `Datum` (DateTime) - Document date [REQUIRED]
- `OdjelId` (int, FK → Odjeli) - Department [REQUIRED]
- `korisnikId` (int) - Last modifier [AUDIT]
- `azurirano` (DateTime) - Last modified date [AUDIT]

**Validation Rules**:
- DatumPrimitka and Datum cannot be empty
- PosiljateljId must exist
- OdjelId must exist

---

#### 10. Poslane (Sent Items)

**Table Name**: `Poslane`  
**Audit Fields**: `korisnikId` (int), `azurirano` (DateTime)  
**Model**: `PoslaneModel.cs`  
**Controller**: `PoslaneController.cs`

**Fields**:
- `Id` (PK, AutoNumber)
- `Datum` (DateTime) - Date sent [REQUIRED]
- `PosiljateljId` (int, FK → Posiljatelji) - Sender [REQUIRED]
- `DjelanikId` (int, FK → Djelatnici) - Employee *[Note: typo in model, likely "DjelatnikId"]* [REQUIRED]
- `Naziv` (string) - Description [REQUIRED]
- `Napomena` (string) - Notes [OPTIONAL]
- `PartnerId` (int, FK → Partneri) - Partner [REQUIRED]
- `korisnikId` (int) - Last modifier [AUDIT]
- `azurirano` (DateTime) - Last modified date [AUDIT]

**Validation Rules**:
- Datum and Naziv cannot be empty
- All foreign keys must exist in respective tables

---

### Access Control Entities

#### 11. Korisnici (Users)

**Table Name**: `Korisnici`  
**Audit Fields**: `korisnikId` (int), `azurirano` (DateTime)  
**Model**: `KorisnikModel.cs`  
**Controller**: `KorisnikController.cs`

**Fields**:
- `Id` (PK, AutoNumber)
- `DjelatnikId` (int, FK → Djelatnici) - Employee [REQUIRED, UNIQUE]
- `KorisnickoIme` (string) - Username [REQUIRED, UNIQUE]
- `Lozinka` (string) - Password hash [REQUIRED]
- `Sol` (string) - Password salt [REQUIRED]
- `Uloga` (string) - Role ("Administrator"/"User") [REQUIRED]
- `Aktivan` (string) - Active status ("DA"/"NE") [REQUIRED, DEFAULT: "DA"]
- `korisnikId` (int) - Last modifier [AUDIT]
- `azurirano` (DateTime) - Last modified date [AUDIT]

**Validation Rules**:
- KorisnickoIme must be unique
- DjelatnikId must be unique (one user per employee)
- Lozinka must be salted hash (never plain text)
- Uloga must be "Administrator" or "User"

---

### Asset Tracking Entities (Simple)

#### 12. KeyCards (KarticeUlaz)

**Table Name**: `KarticeUlaz`  
**Audit Fields**: `korisnikId` (int), `DateEdited` (DateTime) ⚠️ *English name*  
**Model**: `KarticeUlazModel.cs`  
**Controller**: `KarticeUlazController.cs`

**Fields**:
- `Id` (PK, AutoNumber)
- `Naziv` (string) - Card name/identifier [REQUIRED]
- `Napomena` (string) - Notes [OPTIONAL]
- `Aktivno` (string) - Active status ("DA"/"NE") [REQUIRED]
- `korisnikId` (int) - Last modifier [AUDIT]
- `DateEdited` (DateTime) - Last modified date [AUDIT]

---

#### 13. DataCards

**Table Name**: `DataCards`  
**Audit Fields**: `korisnikId` (int), `Ažurirano` (DateTime) ⚠️ *Croatian with diacritic*  
**Model**: `DataCardModel.cs`  
**Controller**: `DataCardsController.cs`

**Fields**:
- `Id` (PK, AutoNumber)
- `Imei` (string) - IMEI number [REQUIRED, UNIQUE]
- `SerBr` (string) - Serial number [REQUIRED, UNIQUE]
- `korisnikId` (int) - Last modifier [AUDIT]
- `Ažurirano` (DateTime) - Last modified date [AUDIT]

**Validation Rules**:
- Imei must be unique
- SerBr must be unique

---

#### 14. Enc

**Table Name**: `Enc`  
**Audit Fields**: `korisnikId` (int), `Ažurirano` (DateTime) ⚠️ *Croatian with diacritic*  
**Model**: `EncModel.cs`  
**Controller**: `EncController.cs`

**Fields**:
- `Id` (PK, AutoNumber)
- `Naziv` (string) - ENC item name [REQUIRED]
- `korisnikId` (int) - Last modifier [AUDIT]
- `Ažurirano` (DateTime) - Last modified date [AUDIT]

---

## Audit Field Naming Summary

| Naming Pattern | Count | Tables |
|---------------|-------|--------|
| `azurirano` (standard Croatian) | 8 | Djelatnici, Korisnici, Odjeli, Mjesta, Partneri, Posiljatelji, Knjiga, Poslane |
| `Ažurirano` (Croatian with diacritic) | 2 | DataCards, Enc |
| `DateEdited` (English) | 3 | Oprema, Vozila, KarticeUlaz |
| `EditedOn` (English variant) | 1 | ZaRa |

**Implementation Note**: All Controllers must use the **exact field name** from their respective table when constructing INSERT/UPDATE queries. Do not attempt to standardize naming (violates Backward Compatibility).

## Entity Relationships

```
Djelatnici
  ├─→ Odjeli (OdjelId)
  ├─→ Korisnici (referenced by DjelatnikId, 1:1)
  ├─→ ZaRa (referenced by DjelatnikId, 1:N)
  └─→ Poslane (referenced by DjelanikId, 1:N)

Oprema
  └─→ ZaRa (referenced by OpremaId when NazivZaRaTablice="Oprema")

Vozila
  └─→ ZaRa (referenced by OpremaId when NazivZaRaTablice="Vozila")

KeyCards
  └─→ ZaRa (referenced by OpremaId when NazivZaRaTablice="KeyCards")

DataCards
  └─→ ZaRa (referenced by OpremaId when NazivZaRaTablice="DataCards")

Enc
  └─→ ZaRa (referenced by OpremaId when NazivZaRaTablice="Enc")

Mjesta
  └─→ Posiljatelji (referenced by IdMjesto, 1:N)

Posiljatelji
  ├─→ Knjiga (referenced by PosiljateljId, 1:N)
  └─→ Poslane (referenced by PosiljateljId, 1:N)

Partneri
  └─→ Poslane (referenced by PartnerId, 1:N)

Odjeli
  ├─→ Djelatnici (referenced by OdjelId, 1:N)
  └─→ Knjiga (referenced by OdjelId, 1:N)
```

## Validation Summary by Priority

### High Priority (P1) - Required Field & Format Validation

Forms that MUST have comprehensive validation:
- **Djelatnici**: Ime, Prezime, OIB (11 digits), DatZaposlen, OdjelId, date logic
- **Korisnici**: KorisnickoIme (unique), DjelatnikId (unique), password rules, role
- **Oprema**: Naziv, SerijskiBroj (unique), StatusOpreme, dates
- **Vozila**: Proizvodac, Model, BrSasije (unique), RegOznaka (unique)
- **ZaRa**: All foreign keys, date logic, assignment availability

### Medium Priority (P2) - Business Rule Validation

Forms with important but less critical validation:
- **Knjiga**: Foreign keys, date fields
- **Poslane**: Foreign keys, required text fields
- **DataCards**: IMEI and SerBr uniqueness

### Low Priority (P3) - Simple Validation

Reference data forms with basic validation:
- **Odjeli**: Naziv uniqueness
- **Mjesta**: Required fields
- **Partneri**: Naziv uniqueness
- **Posiljatelji**: Foreign key validation
- **KeyCards**: Required fields
- **Enc**: Required fields

---

**Summary**: 14 tables, all with audit fields. No schema changes needed. Focus on ensuring Controllers populate audit fields correctly and Forms validate consistently before database operations.
