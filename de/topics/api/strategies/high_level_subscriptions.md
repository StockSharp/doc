# High-Level-Abonnements

## Überblick

Die Klasse `Strategy` stellt eine Reihe von High-Level-Methoden für Marktdatenabonnements bereit: `SubscribeCandles`, `SubscribeTicks`, `SubscribeLevel1` und `SubscribeOrderBook`. Diese Methoden geben ein Objekt `ISubscriptionHandler<T>` zurück, mit dem Datenhandler und Indikatoren bequem im Fluent-Stil gebunden werden können.

Im Unterschied zum manuellen Erstellen eines `Subscription`-Objekts und zum Aufruf von `Subscribe()` erledigen die High-Level-Methoden Folgendes:

- Sie erstellen automatisch ein Abonnement mit den richtigen Parametern.
- Sie stellen einen typisierten `ISubscriptionHandler<T>` zum Binden von Handlern bereit.
- Sie integrieren sich über `Bind`-Methoden in das Indikatorsystem.
- Sie unterstützen automatische Chart-Darstellung.
- Sie verwalten den Abonnement-Lebenszyklus korrekt.

## Abonnementmethoden

### SubscribeCandles

Abonniert Candles. Akzeptiert einen Timeframe oder `DataType`:

```csharp
// Nach Timeframe abonnieren
ISubscriptionHandler<ICandleMessage> SubscribeCandles(
    TimeSpan tf,
    bool isFinishedOnly = true,
    Security security = default);

// Nach DataType abonnieren (unterstützt alle Candle-Typen)
ISubscriptionHandler<ICandleMessage> SubscribeCandles(
    DataType dt,
    bool isFinishedOnly = true,
    Security security = default);

// Mit einem vorbereiteten Subscription-Objekt abonnieren
ISubscriptionHandler<ICandleMessage> SubscribeCandles(Subscription subscription);
```

Der Parameter `isFinishedOnly` ist standardmäßig `true` - der Handler empfängt nur abgeschlossene Candles.

### SubscribeTicks

Abonniert Tick-Trades:

```csharp
ISubscriptionHandler<ITickTradeMessage> SubscribeTicks(Security security = null);
ISubscriptionHandler<ITickTradeMessage> SubscribeTicks(Subscription subscription);
```

### SubscribeLevel1

Abonniert Level1-Daten (beste Bid-/Ask-Preise, letzter Trade und weitere Felder):

```csharp
ISubscriptionHandler<Level1ChangeMessage> SubscribeLevel1(Security security = null);
ISubscriptionHandler<Level1ChangeMessage> SubscribeLevel1(Subscription subscription);
```

### SubscribeOrderBook

Abonniert das Orderbuch:

```csharp
ISubscriptionHandler<IOrderBookMessage> SubscribeOrderBook(Security security = null);
ISubscriptionHandler<IOrderBookMessage> SubscribeOrderBook(Subscription subscription);
```

Wenn der Parameter `security` nicht angegeben ist, wird `Security` der Strategie verwendet.

## ISubscriptionHandler-Interface

Das Objekt `ISubscriptionHandler<T>` stellt die folgenden Methoden bereit:

### Start / Stop

Starten und Stoppen des Abonnements:

```csharp
handler.Start();   // ruft Subscribe auf
handler.Stop();    // ruft UnSubscribe auf
```

### Bind (ohne Indikatoren)

Binden eines einfachen Datenhandlers:

```csharp
handler.Bind(Action<T> callback);
```

### Bind (mit Indikatoren)

Binden eines Handlers mit einem oder mehreren Indikatoren. Der Indikator verarbeitet eingehende Daten automatisch, und der Handler erhält den bereits berechneten Wert:

```csharp
// Ein Indikator - decimal-Wert
handler.Bind(IIndicator indicator, Action<T, decimal> callback);

// Zwei Indikatoren
handler.Bind(IIndicator ind1, IIndicator ind2, Action<T, decimal, decimal> callback);

// Bis zu acht Indikatoren
handler.Bind(ind1, ind2, ind3, ..., callback);

// Array von Indikatoren
handler.Bind(IIndicator[] indicators, Action<T, decimal[]> callback);
```

Der Handler mit `Bind` wird nur aufgerufen, wenn alle Indikatoren einen nicht leeren Wert zurückgegeben haben.

### BindWithEmpty

Ähnlich wie `Bind`, aber der Handler wird auch aufgerufen, wenn der Indikator einen leeren Wert zurückgegeben hat. Werte werden als `decimal?` dargestellt:

```csharp
handler.BindWithEmpty(IIndicator indicator, Action<T, decimal?> callback);
```

### BindEx

Stellt Zugriff auf das vollständige Objekt `IIndicatorValue` bereit, anstatt den extrahierten `decimal`-Wert zu liefern:

```csharp
handler.BindEx(IIndicator indicator, Action<T, IIndicatorValue> callback, bool allowEmpty = false);
```

## Beispiel: Strategie mit Indikatoren

```csharp
public class SmaStrategy : Strategy
{
    private readonly StrategyParam<int> _shortPeriod;
    private readonly StrategyParam<int> _longPeriod;
    private readonly StrategyParam<DataType> _candleType;

    public int ShortPeriod
    {
        get => _shortPeriod.Value;
        set => _shortPeriod.Value = value;
    }

    public int LongPeriod
    {
        get => _longPeriod.Value;
        set => _longPeriod.Value = value;
    }

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public SmaStrategy()
    {
        _shortPeriod = Param(nameof(ShortPeriod), 10);
        _longPeriod = Param(nameof(LongPeriod), 20);
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var shortSma = new SimpleMovingAverage { Length = ShortPeriod };
        var longSma = new SimpleMovingAverage { Length = LongPeriod };

        var subscription = SubscribeCandles(CandleType);

        // Zwei Indikatoren binden - der Handler wird aufgerufen,
        // wenn beide Indikatoren gebildet sind
        subscription
            .Bind(shortSma, longSma, (candle, shortValue, longValue) =>
            {
                if (!IsFormedAndOnlineAndAllowTrading())
                    return;

                if (shortValue > longValue && Position <= 0)
                    BuyMarket(Volume + Math.Abs(Position));
                else if (shortValue < longValue && Position >= 0)
                    SellMarket(Volume + Math.Abs(Position));
            })
            .Start();

        // Chart einrichten
        var area = CreateChartArea();
        if (area != null)
        {
            DrawCandles(area, subscription);
            DrawIndicator(area, shortSma);
            DrawIndicator(area, longSma);
            DrawOwnTrades(area);
        }
    }
}
```

## Beispiel: Tick-Abonnement

```csharp
protected override void OnStarted2(DateTime time)
{
    base.OnStarted2(time);

    SubscribeTicks()
        .Bind(tick =>
        {
            if (!IsFormedAndOnlineAndAllowTrading())
                return;

            this.AddInfoLog("Tick: price={0}, volume={1}", tick.Price, tick.Volume);
        })
        .Start();
}
```

## Beispiel: Orderbuch-Abonnement

```csharp
protected override void OnStarted2(DateTime time)
{
    base.OnStarted2(time);

    SubscribeOrderBook()
        .Bind(book =>
        {
            var bestBid = book.GetBestBid();
            var bestAsk = book.GetBestAsk();

            if (bestBid != null && bestAsk != null)
            {
                var spread = bestAsk.Price - bestBid.Price;
                this.AddInfoLog("Spread: {0}", spread);
            }
        })
        .Start();
}
```

## Unterschiede zur manuellen Abonnementerstellung

| Aspekt | Manuelles Abonnement | High-Level-Methode |
|--------|----------------------|--------------------|
| Erstellung | `new Subscription(DataType, Security)` | `SubscribeCandles(tf)` |
| Datenverarbeitung | Abonnieren von Connector-Ereignissen | `Bind(callback)` |
| Indikatoren | Manueller Aufruf von `indicator.Process()` | Automatisch über `Bind(indicator, callback)` |
| Indikatorregistrierung | Manuelles Hinzufügen zu `Indicators` | Automatisch bei `Bind` |
| Chart-Darstellung | Manuelle Integration mit `IChart` | `DrawCandles`, `DrawIndicator` |

High-Level-Methoden werden für die meisten Strategien empfohlen, da sie den Codeumfang deutlich reduzieren und die Fehlerwahrscheinlichkeit senken.
