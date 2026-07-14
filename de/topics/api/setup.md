# Einrichten der Umgebung

## Anforderungen

Für die Arbeit mit StockSharp benötigen Sie:

- **.NET 10** (SDK und Runtime) - [Herunterladen](https://dotnet.microsoft.com/download/dotnet/10.0)
- **IDE** - Visual Studio 2022+, JetBrains Rider oder VS Code
- **NuGet** - Paketmanager (in Visual Studio und Rider integriert)

Prüfen Sie, ob das SDK installiert ist:

```bash
dotnet --version
```

## Projekt erstellen

### Visual Studio 2022+

1. **Datei -> Neu -> Projekt**
2. Vorlage **Konsolen-App** oder **WPF-Anwendung** auswählen
3. Zielframework auf **.NET 10** setzen

### JetBrains Rider

1. **Datei -> Neue Lösung**
2. **.NET / .NET Core -> Konsolenanwendung** auswählen
3. **Zielframework: net10.0** festlegen

### Befehlszeile (CLI)

```bash
# Konsolenanwendung
dotnet new console -n MyTradingApp --framework net10.0
cd MyTradingApp

# WPF-Anwendung (nur Windows)
dotnet new wpf -n MyTradingGui --framework net10.0-windows
```

## NuGet-Paketkatalog

StockSharp wird über NuGet verteilt. Unten finden Sie den vollständigen Paketkatalog nach Kategorien geordnet.

### Core

| Paket | Beschreibung |
|-------|--------------|
| [StockSharp.Messages](https://www.nuget.org/packages/StockSharp.Messages/) | Basisnachrichten und Verträge. Fundament des gesamten Frameworks |
| [StockSharp.BusinessEntities](https://www.nuget.org/packages/StockSharp.BusinessEntities/) | Handelsentitäten: Security, Order, Trade, Portfolio usw. |
| [StockSharp.Algo](https://www.nuget.org/packages/StockSharp.Algo/) | Algorithmischer Handel im Kern, Connector, Abonnements, Kerzen |
| [StockSharp.Configuration](https://www.nuget.org/packages/StockSharp.Configuration/) | Konfigurationsverwaltung, Verbindungseinstellungen |
| [StockSharp.Localization](https://www.nuget.org/packages/StockSharp.Localization/) | Lokalisierungssystem (standardmäßig Englisch) |

### Strategien und Indikatoren

| Paket | Beschreibung |
|-------|--------------|
| [StockSharp.Algo.Strategies](https://www.nuget.org/packages/StockSharp.Algo.Strategies/) | Strategie-Framework - Basisklasse Strategy, Positionen, PnL |
| [StockSharp.Algo.Indicators](https://www.nuget.org/packages/StockSharp.Algo.Indicators/) | 100+ technische Indikatoren (SMA, EMA, RSI, MACD, Bollinger usw.) |

### Tests

| Paket | Beschreibung |
|-------|--------------|
| [StockSharp.Algo.Testing](https://www.nuget.org/packages/StockSharp.Algo.Testing/) | Rücktests auf historischen Daten, Trade-Emulation |

### Speicherung und Daten

| Paket | Beschreibung |
|-------|--------------|
| [StockSharp.Algo.Export](https://www.nuget.org/packages/StockSharp.Algo.Export/) | Marktdatenexport (CSV, Excel, JSON usw.) |
| [StockSharp.Algo.Import](https://www.nuget.org/packages/StockSharp.Algo.Import/) | Datenimport aus externen Formaten |

### Analytics und Berechnung

| Paket | Beschreibung |
|-------|--------------|
| [StockSharp.Algo.Analytics](https://www.nuget.org/packages/StockSharp.Algo.Analytics/) | Interfaces für Analytics-Skripte |
| [StockSharp.Algo.Compilation](https://www.nuget.org/packages/StockSharp.Algo.Compilation/) | Codekompilierung zur Laufzeit |
| [StockSharp.Algo.Gpu](https://www.nuget.org/packages/StockSharp.Algo.Gpu/) | GPU-beschleunigte Indikatorberechnungen (CUDA über ILGPU) |

### GUI-Komponenten (nur Windows)

| Paket | Beschreibung |
|-------|--------------|
| [StockSharp.Xaml](https://www.nuget.org/packages/StockSharp.Xaml/) | WPF-Steuerelemente: Instrument-, Portfolio- und Ordertabellen |
| [StockSharp.Xaml.Charting](https://www.nuget.org/packages/StockSharp.Xaml.Charting/) | Candlestick-Charts, Indikatoren, Equity-Kurven |
| [StockSharp.Charting.Interfaces](https://www.nuget.org/packages/StockSharp.Charting.Interfaces/) | Interfaces der Chart-Komponenten |
| [StockSharp.Alerts.Interfaces](https://www.nuget.org/packages/StockSharp.Alerts.Interfaces/) | Interfaces des Alert-Systems |
| [StockSharp.Diagram.Core](https://www.nuget.org/packages/StockSharp.Diagram.Core/) | Kern des visuellen Strategiedesigners |

### Connectors (Börsen und Broker)

Jeder Connector ist ein separates NuGet-Paket. Wichtige Connectors:

| Paket | Börse/Broker |
|-------|--------------|
| `StockSharp.Binance` | Binance |
| `StockSharp.InteractiveBrokers` | Interactive Brokers |
| `StockSharp.Fix` | FIX-Protokoll (universell) |
| `StockSharp.Connectors.Coinbase` | Coinbase |
| `StockSharp.Connectors.BitStamp` | Bitstamp |
| `StockSharp.Connectors.Bittrex` | Bittrex |

> [!NOTE]
> Die vollständige Liste der Connectors finden Sie im Abschnitt [Konnektoren](connectors.md). Einige Connectors sind nur über den [privaten NuGet-Server](#private-nuget-server) verfügbar.

### Lokalisierung

Das Basispaket `StockSharp.Localization` enthält Englisch. Zusätzliche Sprachen werden als separate Pakete installiert:

| Paket | Sprache |
|-------|---------|
| `StockSharp.Localization.ru` | Russisch |
| `StockSharp.Localization.zh` | Chinesisch |
| `StockSharp.Localization.de` | Deutsch |
| `StockSharp.Localization.es` | Spanisch |
| `StockSharp.Localization.ja` | Japanisch |
| `StockSharp.Localization.ko` | Koreanisch |
| `StockSharp.Localization.All` | Alle Sprachen (Metapaket) |

Ebenfalls verfügbar: `ar`, `bn`, `ca`, `cs`, `da`, `el`, `fa`, `fi`, `fr`, `he`, `hi`, `hu`, `it`, `jv`, `ms`, `my`, `nl`, `no`, `pa`, `pl`, `pt`, `ro`, `sk`, `sr`, `sv`, `ta`, `th`, `tl`, `tr`, `uk`, `uz`, `vi`.

## Pakete installieren

### Über CLI (empfohlen)

```bash
# Core-Pakete
dotnet add package StockSharp.Algo
dotnet add package StockSharp.Algo.Strategies

# Connector (Beispiel - Binance)
dotnet add package StockSharp.Binance

# Indikatoren
dotnet add package StockSharp.Algo.Indicators

# Rücktests
dotnet add package StockSharp.Algo.Testing

# Lokalisierung (Russisch)
dotnet add package StockSharp.Localization.ru
```

### Über Visual Studio

1. Rechtsklick auf das Projekt -> **NuGet-Pakete verwalten...**
2. Nach `StockSharp` suchen
3. Gewünschtes Paket auswählen -> **Installieren**

Alle Abhängigkeiten werden automatisch installiert.

### Über JetBrains Rider

1. Rechtsklick auf das Projekt -> **NuGet-Pakete verwalten**
2. Nach `StockSharp` suchen
3. Paket auswählen -> **Installieren**

### Über Paket-Manager-Konsole (Visual Studio)

```powershell
Install-Package StockSharp.Algo
Install-Package StockSharp.Binance
Install-Package StockSharp.Algo.Strategies
```

## Privater NuGet-Server {#private-nuget-server}

Einige Komponenten (Krypto-Connectors usw.) sind nur über den privaten NuGet-Server für registrierte Benutzer verfügbar.

### Methode 1: Authentifizierung per Token in der URL

1. Registrieren Sie sich auf der StockSharp-Website.
2. Kopieren Sie das Token aus Ihrem [persönlichen Konto](https://stocksharp.com/de/profile/).
3. Fügen Sie die Paketquelle hinzu:

**CLI:**

```bash
dotnet nuget add source "https://nuget.stocksharp.com/{IHR_TOKEN}/v3/index.json" --name StockSharpPrivate
```

**Visual Studio:** Öffnen Sie **Extras -> Optionen -> NuGet-Paket-Manager -> Paketquellen** und fügen Sie eine neue Quelle mit der URL `https://nuget.stocksharp.com/{IHR_TOKEN}/v3/index.json` hinzu.

**Rider:** Öffnen Sie **Einstellungen -> Build, Ausführung, Bereitstellung -> NuGet -> Quellen** und fügen Sie die Quelle hinzu.

### Methode 2: Authentifizierung per Benutzername und Passwort

1. Fügen Sie eine Paketquelle mit der URL `https://nuget.stocksharp.com/x/v3/index.json` hinzu.
2. Beim Versuch, diese Quelle zu verwenden, erscheint eine Anmeldeaufforderung.
3. Geben Sie die Zugangsdaten Ihres StockSharp-Kontos ein (oder `x` als Benutzernamen und Ihr Token als Passwort).

**CLI:**

```bash
dotnet nuget add source "https://nuget.stocksharp.com/x/v3/index.json" --name StockSharpPrivate --username IHR_LOGIN --password IHR_PASSWORT --store-password-in-clear-text
```

> [!TIP]
> Um gespeicherte Anmeldedaten unter Windows zurückzusetzen, öffnen Sie **Systemsteuerung -> Benutzerkonten -> Anmeldeinformationsverwaltung** und löschen Sie Einträge zu `nuget.stocksharp.com`.

## Pakete aktualisieren

### CLI

```bash
# Auf verfügbare Updates prüfen
dotnet list package --outdated

# Bestimmtes Paket aktualisieren
dotnet add package StockSharp.Algo
```

### Visual Studio

1. **NuGet-Pakete verwalten...** -> Registerkarte **Aktualisierungen**
2. Pakete auswählen -> **Aktualisieren**

### Rider

1. **NuGet-Pakete verwalten** -> Registerkarte **Aktualisierungen**
2. Pakete auswählen -> **Aktualisieren**

## Problembehandlung

### Versionskonflikt

Alle StockSharp-Pakete müssen dieselbe Version haben. Wenn Typ- oder Methodenfehler auftreten, stellen Sie sicher, dass alle `StockSharp.*`-Pakete auf dieselbe Version aktualisiert sind.

### Paket auf öffentlichem NuGet nicht gefunden

Einige Connectors sind nur über den [privaten Server](#private-nuget-server) verfügbar. Stellen Sie sicher, dass die richtige Quelle hinzugefügt wurde.

### GUI-Probleme außerhalb von Windows

GUI-Komponenten (`StockSharp.Xaml`, `StockSharp.Xaml.Charting`) funktionieren nur unter Windows. Unter Linux/macOS verwenden Sie Konsolenanwendungen ohne GUI-Pakete.

### Fehler bei der Paketwiederherstellung

```bash
# NuGet-Cache leeren
dotnet nuget locals all --clear

# Restore erneut versuchen
dotnet restore
```

## Projektdateistruktur (.csproj)

### Minimal (Konsolen-Handelsbot)

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="StockSharp.Algo" Version="*" />
    <PackageReference Include="StockSharp.Binance" Version="*" />
  </ItemGroup>
</Project>
```

### Erweitert (Strategie mit Indikatoren und Tests)

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <!-- Kern -->
    <PackageReference Include="StockSharp.Algo" Version="*" />
    <PackageReference Include="StockSharp.Configuration" Version="*" />

    <!-- Strategien und Indikatoren -->
    <PackageReference Include="StockSharp.Algo.Strategies" Version="*" />
    <PackageReference Include="StockSharp.Algo.Indicators" Version="*" />

    <!-- Konnektor -->
    <PackageReference Include="StockSharp.Binance" Version="*" />

    <!-- Rücktests -->
    <PackageReference Include="StockSharp.Algo.Testing" Version="*" />

    <!-- Lokalisierung -->
    <PackageReference Include="StockSharp.Localization.ru" Version="*" />
  </ItemGroup>
</Project>
```

### WPF-Anwendung mit Charts

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net10.0-windows</TargetFramework>
    <UseWPF>true</UseWPF>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="StockSharp.Algo" Version="*" />
    <PackageReference Include="StockSharp.Algo.Strategies" Version="*" />
    <PackageReference Include="StockSharp.Xaml.Charting" Version="*" />
    <PackageReference Include="StockSharp.Binance" Version="*" />
  </ItemGroup>
</Project>
```

## Installation überprüfen

Erstellen Sie eine minimale Anwendung:

```csharp
using StockSharp.Algo;
using StockSharp.BusinessEntities;

Console.WriteLine("StockSharp erfolgreich konfiguriert!");

var connector = new Connector();
Console.WriteLine($"Connector erstellt: {connector}");
```

```bash
dotnet run
```

## Beispiele

Fertige Beispiele für die Verwendung von StockSharp sind im [Beispielverzeichnis](https://github.com/stocksharp/stocksharp/tree/master/Samples) des Repositorys verfügbar. Sie decken die Verbindung zu Börsen, das Abonnieren von Daten, den Aufbau von Kerzen, Indikatoren, Strategien und Tests ab.
