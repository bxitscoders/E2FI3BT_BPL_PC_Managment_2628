# 🖥️ PC-Verwaltungssystem (E2FI3BT BPL)

Ein modulares, modernes Desktop-Managementsystem für PC-Hardwarekomponenten, individuelle Systemkonfigurationen, automatische IP-Netzwerkadressierung und persistente SQLite-Datenbankanbindung.

Entwickelt mit **.NET 8 (C#)**, **WPF (XAML)** und **Entity Framework Core (SQLite)**.

---

## 🌟 Funktionsumfang & User Stories

### 1. ⚙️ Hardwarekomponenten erfassen & verwalten ([Issue #1](https://github.com/bxitscoders/E2FI3BT_BPL_PC_Managment_2628/issues/1))
- **Komponententypen:** Erfassung und Pflege von **CPU**, **Mainboard**, **RAM**, **SSD** und **Gehäuse**.
- **Objektorientiertes Datenmodell:** Saubere Vererbungshierarchie basierend auf der abstrakten Basisklasse `HardwareKomponente` (OOP-Generalisierung).
- **Validierung:** Robuste Prüfung von Pflichtfeldern und numerischen Werten (EK-Preis, VK-Preis, Taktraten, Kernanzahlen, Kapazitäten).
- **Übersicht:** Tabellarische Ansichten mit Live-Suchfiltern und Bestandsüberblick in `AllListsView`.

### 2. 🛠️ PC-System konfigurieren & prüfen ([Issue #2](https://github.com/bxitscoders/E2FI3BT_BPL_PC_Managment_2628/issues/2))
- **Interaktiver PC-Builder:** Intuitive Auswahl von Gehäuse, Mainboard, Prozessor, Arbeitsspeicher und Festplatte via `PcBuilderView`.
- **Integrierte Kompatibilitätsprüfung:**
  - **Formfaktor-Validierung:** Erkennt Unstimmigkeiten zwischen Mainboard (z. B. ATX) und Gehäuse (z. B. Mini-ITX).
  - **Sockel-Validierung:** Stellt sicher, dass CPU und Mainboard denselben Sockel nutzen (z. B. AM5 ↔ AM5).
  - **Visuelles Feedback:** Live-Statusanzeige mit Erfolgsmeldung oder Warnhinweisen.
- **Wirtschaftliche Kalkulation:** Automatische Live-Berechnung von **Gesamt-EK**, **Gesamt-VK** und der resultierenden **Handelsmarge**.
- **Systemdetails:** Eindeutige Namensvergabe mit Duplikatsprüfung.

### 3. 🌐 Automatische IP-Adress-Vorkonfiguration ([Issue #3](https://github.com/bxitscoders/E2FI3BT_BPL_PC_Managment_2628/issues/3))
- **Netzwerk-Vorkonfiguration:** Automatische Zuweisung einer gültigen IPv4-Adresse für jedes konfigurierte System.
- **Logische Vergabe:** Das erste System erhält die erste freie Host-Adresse nach dem Router (z. B. `.2`).
- **Fortlaufende Inkrementierung:** Folgesysteme erhalten automatisch aufsteigende IP-Adressen innerhalb des Subnetzes.

### 4. 💾 Persistente Datenspeicherung (SQLite & EF Core)
- **Relationale Datenbank:** Speicherung sämtlicher Hardwarekomponenten und PC-Konfigurationen in einer lokalen SQLite-Datenbank (`Data/pc_management.db`).
- **Entity Framework Core:** Automatisches Schema-Management und Initialisierung beim Programmstart.
- **Sitzungsübergreifend:** Komponenten und konfigurierte PCs bleiben nach Schließen der Anwendung vollständig erhalten.

---

## 🏗️ Projekt- & Ordnerstruktur

```text
produktiv/
├── PC_Management/                      <-- Hauptanwendung (.NET 8 WPF)
│   ├── Classes/                        <-- Domänenmodell & OOP-Klassen
│   │   ├── HardwareKomponente.cs       <-- Abstrakte Basisklasse
│   │   ├── Case.cs                     <-- Gehäuse-Modell
│   │   ├── CPU.cs                      <-- Prozessor-Modell
│   │   ├── Mainboard.cs                <-- Mainboard-Modell
│   │   ├── Ram.cs                      <-- Arbeitsspeicher-Modell
│   │   ├── SSD.cs                      <-- Massenspeicher-Modell
│   │   ├── PC.cs                       <-- Zusammengestelltes System
│   │   └── Enums.cs                    <-- Typisierungen (Sockel, Formfaktor, RAM)
│   ├── Data/                           <-- Datenpersistenz (EF Core / SQLite)
│   │   ├── AppDbContext.cs             <-- Datenbankkontext
│   │   └── pc_management.db            <-- Lokale SQLite-Datenbankdatei
│   ├── Images/                         <-- Bildressourcen & Icons
│   ├── AllListsView.xaml/.cs           <-- Tabellarische Gesamtübersicht
│   ├── PcBuilderView.xaml/.cs          <-- Konfigurator mit Kompatibilitätsprüfung
│   ├── MainView.xaml/.cs               <-- Start- & Hauptansicht
│   ├── CpuView.xaml/.cs                <-- Erfassungsmaske CPU
│   ├── MainboardView.xaml/.cs          <-- Erfassungsmaske Mainboard
│   ├── RamView.xaml/.cs                <-- Erfassungsmaske RAM
│   ├── SsdView.xaml/.cs                <-- Erfassungsmaske SSD
│   ├── GehaeuseView.xaml/.cs           <-- Erfassungsmaske Gehäuse
│   ├── App.xaml/.cs                    <-- Anwendungs-Lebenszyklus & Dateninitialisierung
│   ├── PCVerwaltung.csproj             <-- Projektdatei mit Abhängigkeiten
│   └── PCVerwaltung.sln                <-- Visual Studio Solution
├── .gitignore                          <-- Bereinigungsregeln für .NET/C#
└── README.md                           <-- Diese Dokumentation
```

---

## 🚀 Erste Schritte & Ausführung

### Voraussetzungen
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (mit Workload *Desktopentwicklung mit .NET*) oder [Visual Studio Code](https://code.visualstudio.com/)

### Starten via Visual Studio
1. Die Projektmappe [`PC_Management/PCVerwaltung.sln`](PC_Management/PCVerwaltung.sln) in Visual Studio öffnen.
2. Mit <kbd>F5</kbd> oder `Starten` kompilieren und ausführen.

### Starten via Terminal / PowerShell
```powershell
# In das Projektverzeichnis wechseln:
cd produktiv/PC_Management

# Abhängigkeiten wiederherstellen und kompilieren:
dotnet build

# Anwendung starten:
dotnet run
```

---

## 🌿 Branching-Modell & Arbeitsstände

| Branch | Status | Beschreibung |
|---|:---:|---|
| `main` | 🟢 Stabil | Bereinigte Ausgangsbasis mit `.gitignore`, vollständigen Klassen und Ressourcen. |
| [`feature/US-01-hardware-verwaltung`](https://github.com/bxitscoders/E2FI3BT_BPL_PC_Managment_2628/tree/feature/US-01-hardware-verwaltung) | ✅ Gemergt ([PR #8](https://github.com/bxitscoders/E2FI3BT_BPL_PC_Managment_2628/pull/8)) | Erfassungsmasken & OOP-Modell für alle Hardwarekomponenten. |
| [`feature/US-02-pc-konfiguration`](https://github.com/bxitscoders/E2FI3BT_BPL_PC_Managment_2628/tree/feature/US-02-pc-konfiguration) | 🟡 In Review ([PR #9](https://github.com/bxitscoders/E2FI3BT_BPL_PC_Managment_2628/pull/9)) | PC-Konfigurator mit Kompatibilitätsprüfung & Margenberechnung. |
| [`feature/US-03-ip-adress-verwaltung`](https://github.com/bxitscoders/E2FI3BT_BPL_PC_Managment_2628/tree/feature/US-03-ip-adress-verwaltung) | 🟡 In Review ([PR #10](https://github.com/bxitscoders/E2FI3BT_BPL_PC_Managment_2628/pull/10)) | Automatische IP-Adress-Zuweisung für konfigurierte Systeme. |
| [`feature/sqlite-datenbank-persistenz`](https://github.com/bxitscoders/E2FI3BT_BPL_PC_Managment_2628/tree/feature/sqlite-datenbank-persistenz) | 🟡 In Review ([PR #11](https://github.com/bxitscoders/E2FI3BT_BPL_PC_Managment_2628/pull/11)) | Vollständige SQLite-Persistenz mit EF Core & VS Solution. |

---

## 👥 Beteiligte & Kontext
- **Team / Organisation:** [bxitscoders](https://github.com/bxitscoders)
- **Projekt:** E2FI3BT BPL PC-Managementsystem 2628
