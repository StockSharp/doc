# Komplexe Candle-Muster

## Überblick

Die Klasse `ComplexCandlePattern` ermöglicht das Erstellen komplexer Candle-Muster, indem mehrere einfache Muster (`ICandlePattern`) zu einem Muster kombiniert werden. Beim Erkennen eines komplexen Musters wird jedes innere Muster nacheinander auf seinem Candle-Segment geprüft. Das Muster gilt nur dann als erkannt, wenn alle inneren Muster übereinstimmen.

## ICandlePattern

Das Basisinterface für alle Candle-Muster:

```csharp
public interface ICandlePattern : IPersistable
{
    // Mustername
    string Name { get; }

    // Anzahl der Candles, die für die Erkennung erforderlich sind
    int CandlesCount { get; }

    // Prüft, ob das Muster auf den angegebenen Candles erkannt wird
    bool Recognize(ReadOnlySpan<ICandleMessage> candles);
}
```

Die Registry `CandlePatternRegistry` enthält eine Reihe integrierter Muster: `Flat`, `White`, `Black`, `Hammer`, `BullishEngulfing`, `MorningStar`, `ThreeWhiteSoldiers` und weitere.

## ComplexCandlePattern

Die Klasse `ComplexCandlePattern` implementiert `ICandlePattern` und kombiniert mehrere innere Muster:

```csharp
public class ComplexCandlePattern : ICandlePattern
{
    // Leeres Muster erstellen
    public ComplexCandlePattern() { }

    // Muster mit Namen und Satz innerer Muster erstellen
    public ComplexCandlePattern(string name, IEnumerable<ICandlePattern> inner);

    // Name des komplexen Musters
    public string Name { get; }

    // Innere Muster
    public IEnumerable<ICandlePattern> Inner { get; }

    // Gesamtzahl der Candles (Summe von CandlesCount für alle inneren Muster)
    public int CandlesCount { get; }
}
```

Beim Aufruf von `Recognize` wird das Candle-Array entsprechend dem jeweiligen `CandlesCount` der inneren Muster in aufeinanderfolgende Segmente aufgeteilt. Wenn mindestens ein inneres Muster nicht übereinstimmt, gibt die Methode `false` zurück.

## Beispiel: Erstellen eines komplexen Musters

```csharp
using StockSharp.Algo.Candles.Patterns;

// Komplexes Muster erstellen: zuerst eine bärische Candle, dann Bullish Engulfing
var complex = new ComplexCandlePattern(
    "Reversal Up",
    new ICandlePattern[]
    {
        CandlePatternRegistry.Black,            // 1 Candle: bärisch
        CandlePatternRegistry.BullishEngulfing,  // 2 Candles: Bullish Engulfing
    }
);

// Für die Erkennung sind 3 Candles erforderlich (1 + 2)
Console.WriteLine($"Candles required: {complex.CandlesCount}"); // 3
```

## ICandlePatternProvider

Das Interface `ICandlePatternProvider` verwaltet Speicherung und Suche von Mustern:

```csharp
public interface ICandlePatternProvider
{
    // Ereignisse für Erstellung, Ersetzung und Löschung von Mustern
    event Action<ICandlePattern> PatternCreated;
    event Action<ICandlePattern, ICandlePattern> PatternReplaced;
    event Action<ICandlePattern> PatternDeleted;

    // Speicher initialisieren
    ValueTask InitAsync(CancellationToken cancellationToken);

    // Alle verfügbaren Muster
    IEnumerable<ICandlePattern> Patterns { get; }

    // Muster nach Namen suchen
    bool TryFind(string name, out ICandlePattern pattern);

    // Muster entfernen
    bool Remove(ICandlePattern pattern);

    // Muster speichern (erstellen oder ersetzen)
    void Save(ICandlePattern pattern);
}
```

### Implementierungen

- `InMemoryCandlePatternProvider` - speichert Muster im Arbeitsspeicher. Bei der Initialisierung lädt er alle integrierten Muster aus `CandlePatternRegistry.All`.
- `CandlePatternFileStorage` - speichert benutzerdefinierte Muster in einer Datei (JSON). Integrierte Muster aus `InMemoryCandlePatternProvider` sind auch über diesen Provider verfügbar.

## Beispiel: Arbeiten mit ICandlePatternProvider

```csharp
using StockSharp.Algo.Candles.Patterns;
using StockSharp.Configuration;

// Dateibasierten Musterspeicher erstellen
var executor = new ChannelExecutor();
var provider = new CandlePatternFileStorage(
    Paths.FileSystem,
    Paths.CandlePatternsFile,
    executor
);

// Initialisieren (lädt integrierte + benutzerdefinierte Muster aus der Datei)
await provider.InitAsync(CancellationToken.None);

// Ereignis für die Erstellung neuer Muster abonnieren
provider.PatternCreated += pattern =>
{
    Console.WriteLine($"Muster erstellt: {pattern.Name}");
};

// Komplexes Muster erstellen und speichern
var myPattern = new ComplexCandlePattern(
    "My Pattern",
    new ICandlePattern[]
    {
        CandlePatternRegistry.Hammer,
        CandlePatternRegistry.White,
    }
);

provider.Save(myPattern);

// Muster nach Namen suchen
if (provider.TryFind("My Pattern", out var found))
{
    Console.WriteLine($"Gefunden: {found.Name}, Kerzen: {found.CandlesCount}");
}
```

## ExpressionCandlePattern

Für die Erstellung formelbasierter Muster wird `ExpressionCandlePattern` verwendet. Jede Candle im Muster wird durch einen Ausdruck `CandleExpressionCondition` beschrieben; dabei stehen die folgenden Variablen zur Verfügung:

| Variable | Beschreibung |
|----------|--------------|
| `O` | Eröffnungskurs |
| `H` | Höchstkurs |
| `L` | Tiefstkurs |
| `C` | Schlusskurs |
| `V` | Volumen |
| `B` | Candle-Körper |
| `LEN` | Candle-Länge |
| `BS` | Unterer Schatten |
| `TS` | Oberer Schatten |

Das Präfix `p` verweist auf die vorherige Candle (`pO`, `pC`), `pp` auf die Candle zwei Perioden zurück usw.

Alle integrierten Muster in `CandlePatternRegistry` werden mit `ExpressionCandlePattern` aufgebaut.
