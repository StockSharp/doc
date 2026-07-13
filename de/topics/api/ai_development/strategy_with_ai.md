# Eine Strategie mit KI schreiben

Eine Schritt-für-Schritt-Anleitung zur Erstellung einer StockSharp-Handelsstrategie mithilfe von KI-Tools.

## Vorbereitung

### 1. Ein KI-Tool installieren

Wählen Sie eines der verfügbaren Tools:
- **Claude Code** — `npm install -g @anthropic-ai/claude-code` (erfordert Node.js)
- **Cursor** — von [cursor.com](https://cursor.com) herunterladen
- **GitHub Copilot** — installieren Sie das Plugin für Ihre IDE

### 2. Ein Projekt erstellen

```bash
dotnet new console -n MyStrategy --framework net10.0
cd MyStrategy
dotnet add package StockSharp.Algo
dotnet add package StockSharp.Algo.Strategies
dotnet add package StockSharp.Algo.Indicators
dotnet add package StockSharp.Algo.Testing
dotnet add package StockSharp.Binance
```

### 3. Kontext vorbereiten

Erstellen Sie eine Datei `CLAUDE.md` (oder `.cursorrules`) im Projekt-Root:

```markdown
# Projektregeln

- Technologiestapel: StockSharp 5.x, .NET 10
- Strategien erben von der Klasse Strategy
- Kerzen über Connector.Subscribe(subscription) abonnieren
- Aufträge über RegisterOrder(order) registrieren
- Protokollierung: this.AddInfoLog(), this.AddWarningLog(), this.AddErrorLog()
- Indikatoren: mit new erstellen und indicator.Process(candle) aufrufen
- connector.Error und Strategiefehler immer behandeln
```

## Schritt-für-Schritt-Beispiel: SMA-Strategie

### Schritt 1: Die Aufgabe der KI beschreiben

Beispiel-Prompt:

```
Erstelle eine Handelsstrategie mit StockSharp, die:
- von Strategy erbt
- zwei einfache gleitende Durchschnitte (SMA) verwendet: schnelle (Periode 10) und langsame (Periode 30)
- kauft, wenn die schnelle SMA die langsame SMA nach oben kreuzt
- verkauft, wenn die schnelle SMA die langsame SMA nach unten kreuzt
- Positionsgröße: 1 Lot
- 5-Minuten-Kerzen verwendet
- Kerzen in OnStarted() abonniert
- Kerzen über Abonnementregeln verarbeitet
```

### Schritt 2: Den generierten Code überprüfen

Die KI generiert etwa Folgendes:

```csharp
using StockSharp.Algo;
using StockSharp.Algo.Indicators;
using StockSharp.Algo.Strategies;
using StockSharp.BusinessEntities;
using StockSharp.Messages;

public class SmaCrossStrategy : Strategy
{
    private readonly StrategyParam<int> _fastPeriod;
    private readonly StrategyParam<int> _slowPeriod;
    private readonly StrategyParam<DataType> _candleType;

    private SimpleMovingAverage _fastSma;
    private SimpleMovingAverage _slowSma;

    private bool _prevFastAbove;
    private bool _isFirstCandle = true;

    public SmaCrossStrategy()
    {
        _fastPeriod = Param(nameof(FastPeriod), 10);
        _slowPeriod = Param(nameof(SlowPeriod), 30);
        _candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(5)));
    }

    public int FastPeriod
    {
        get => _fastPeriod.Value;
        set => _fastPeriod.Value = value;
    }

    public int SlowPeriod
    {
        get => _slowPeriod.Value;
        set => _slowPeriod.Value = value;
    }

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    protected override void OnStarted(DateTimeOffset time)
    {
        base.OnStarted(time);

        _fastSma = new SimpleMovingAverage { Length = FastPeriod };
        _slowSma = new SimpleMovingAverage { Length = SlowPeriod };
        _isFirstCandle = true;

        var subscription = new Subscription(CandleType, Security);

        this
            .WhenCandlesFinished(subscription)
            .Do(ProcessCandle)
            .Apply(this);

        Subscribe(subscription);
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        var fastValue = _fastSma.Process(candle);
        var slowValue = _slowSma.Process(candle);

        if (!_fastSma.IsFormed || !_slowSma.IsFormed)
            return;

        var fastAbove = fastValue.GetValue<decimal>() > slowValue.GetValue<decimal>();

        if (!_isFirstCandle)
        {
            if (fastAbove && !_prevFastAbove)
            {
                // Schnelle SMA kreuzt die langsame von unten nach oben — kaufen
                if (Position <= 0)
                    RegisterOrder(this.BuyAtMarket(Volume));
            }
            else if (!fastAbove && _prevFastAbove)
            {
                // Schnelle SMA kreuzt die langsame von oben nach unten — verkaufen
                if (Position >= 0)
                    RegisterOrder(this.SellAtMarket(Volume));
            }
        }

        _prevFastAbove = fastAbove;
        _isFirstCandle = false;
    }
}
```

### Schritt 3: Was zu prüfen ist

Gehen Sie diese Checkliste durch:

- **Vererbung**: Klasse erbt von `Strategy` ✓
- **Parameter**: verwendet `StrategyParam<T>` für die Optimierung ✓
- **Kerzenabonnement**: über `Subscribe(new Subscription(...))` ✓
- **Kerzenverarbeitung**: über die Regel `WhenCandlesFinished` ✓
- **IsFormed-Prüfung**: Indikatoren werden auf Bereitschaft geprüft ✓
- **Aufträge**: über `RegisterOrder()` mit `BuyAtMarket` / `SellAtMarket` ✓
- **Position**: `Position` wird vor dem Platzieren von Aufträgen geprüft ✓

### Schritt 4: Die KI bitten, Rücktests hinzuzufügen

```
Füge Rücktest-Code für diese Strategie mit historischen Daten hinzu.
Verwende HistoryEmulationConnector, lade Daten aus dem lokalen Speicher
und gib Zusammenfassungsstatistiken aus (PnL, Anzahl der Trades, maximaler Drawdown).
```

## Beispiel-Prompts

### Bollinger-Bands-Strategie

```
Erstelle eine StockSharp-Strategie, die mit Bollinger-Bändern handelt:
- kaufen, wenn der Preis das untere Band berührt
- verkaufen, wenn der Preis das obere Band berührt
- Periode 20, Multiplikator 2,0
- Stop-Loss: 1 % vom Einstiegspreis
- Take-Profit: 2 % vom Einstiegspreis
- StrategyParam für alle Parameter verwenden
```

### Arbitrage-Strategie

```
Erstelle eine Paararbitrage-Strategie mit StockSharp:
- zwei Instrumente (über Parameter angegeben)
- den Spread zwischen den Preisen berechnen
- einsteigen, wenn der Spread um 2 Standardabweichungen abweicht
- aussteigen, wenn der Spread zum Mittelwert zurückkehrt
- Volumen-Neutralisierung (gleich große Positionen in Geldwerten)
```

### Orderbuch-Scalping

```
Erstelle eine Scalping-Strategie mit StockSharp:
- Orderbuch (MarketDepth) über Subscribe abonnieren
- Bid/Ask-Ungleichgewicht analysieren
- bei starkem Ungleichgewicht (> 3:1) einsteigen
- schneller Ausstieg per Take-Profit (5 Ticks)
- Stop-Loss: 3 Ticks
- maximal 1 Position gleichzeitig
```

## Häufige Fehler der KI

### 1. Veraltete Ereignisse

**Falsch** (alte API):
```csharp
connector.NewSecurities += securities => { ... };
connector.CandleSeriesProcessing += (series, candle) => { ... };
```

**Korrekt** (aktuelle API):
```csharp
// Abonnements verwenden
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security);
connector.Subscribe(subscription);
```

### 2. Erstellung von Aufträgen ohne Hilfsmethoden

**Falsch**:
```csharp
var order = new Order
{
    Security = Security,
    Portfolio = Portfolio,
    Side = Sides.Buy,
    Type = OrderTypes.Market,
    Volume = 1,
};
```

**Korrekt** (unter Verwendung von Strategie-Hilfsmethoden):
```csharp
RegisterOrder(this.BuyAtMarket(Volume));
// or
RegisterOrder(this.SellAtLimit(price, Volume));
```

### 3. Fehlende IsFormed-Prüfung

**Falsch**:
```csharp
var value = _sma.Process(candle);
// Wert sofort verwenden — möglicherweise noch nicht bereit
```

**Korrekt**:
```csharp
var value = _sma.Process(candle);
if (!_sma.IsFormed)
    return;
```

## Tipps

1. **Der KI Dokumentation geben** — verweisen Sie sie auf [doc.stocksharp.com](https://doc.stocksharp.com) oder kopieren Sie Codebeispiele aus `Samples/`
2. **CLAUDE.md verwenden** — eine Projektregel-Datei reduziert die Anzahl der Fehler erheblich
3. **Einfach beginnen** — erstellen Sie zuerst eine einfache Strategie und fügen Sie dann Filter und Risikomanagement hinzu
4. **Auf historischen Daten testen** — führen Sie vor dem Live-Handel immer einen Rücktest durch
5. **Das Repository klonen** — wenn die KI Zugriff auf die StockSharp-Quellen hat, wird sie die API genauer verwenden
