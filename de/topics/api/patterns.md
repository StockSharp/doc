# Muster

**Pattern** (aus dem Englischen: pattern - Modell, Vorlage) bezeichnet in der technischen Analyse stabile, wiederkehrende Kombinationen aus Preisdaten, Volumen oder Indikatoren. Die Musteranalyse basiert auf einem der Axiome der technischen Analyse: "Geschichte wiederholt sich" - es wird angenommen, dass wiederkehrende Datenkombinationen zu ähnlichen Ergebnissen führen.

Patterns werden auch als "**Vorlagen**" oder "**Formationen**" der technischen Analyse bezeichnet.

Patterns werden üblicherweise unterteilt in:

- Unbestimmte Muster (können sowohl zur Fortsetzung als auch zur Änderung des aktuellen Trends führen).
- Fortsetzungsmuster des aktuellen Trends.
- Umkehrmuster eines bestehenden Trends.

## Verwenden von Patterns

### Im Designer

[Designer](../designer.md) enthält integrierte vordefinierte Candlestick-Muster, die in Ihrer Handelsstrategie verwendet werden können. Patterns werden über den [Indikator](../designer/strategies/using_visual_designer/elements/common/indicator.md)-Würfel aufgerufen, anschließend wird der entsprechende Wert ausgewählt. Das Pattern selbst wird aus der Dropdown-Liste im rechten Fenster ausgewählt.

![Musterübersicht](../../images/indicatorpatterncommon00.png)

Es ist auch möglich, vorhandene Patterns zu bearbeiten und eigene benutzerdefinierte Patterns hinzuzufügen. Klicken Sie dazu auf die Schaltfläche ![Designer Schaltfläche Bearbeiten](../../images/designer_creating_repository_of_historical_data_01.png); danach wird das Fenster zur Pattern-Bearbeitung angezeigt.

![Muster Bildschirmfoto](../../images/indicatorpatterncommon01.png)

Um ein eigenes Pattern zu erstellen, klicken Sie oben im Fenster auf die Schaltfläche ![Designer Plus-Schaltfläche](../../images/designer_panel_circuits_01_button.png). Ein Klick auf die Schaltfläche ![Designer Löschschaltfläche](../../images/designer_delete_button.png) löscht das Pattern.

### Im Terminal

In [Terminal](../terminal.md) werden Patterns wie jeder andere Indikator zum Chart hinzugefügt. Klicken Sie dazu einfach mit der rechten Maustaste auf den Chart und wählen Sie den passenden Indikator aus der Liste der verfügbaren Indikatoren aus.

### In der StockSharp API

Bei Verwendung von [S#](../api.md) (oder beim Erstellen von [Strategien aus Code](../designer/strategies/using_code.md) im Designer) erfolgt die Arbeit mit Patterns wie bei jedem anderen Indikator. Verwendungsbeispiel:

```cs
// Erstellen eines Pattern-Indikators
var patternIndicator = new CandlePatternIndicator
{
	// Festlegen des gewünschten Patterns
	Pattern = new ExpressionCandlePattern("Mein Muster", new[]
	{
		new CandleExpressionCondition(Paths.FileSystem, "C > O"), // Aktuelle Kerze steigt
		new CandleExpressionCondition(Paths.FileSystem, "pC < pO") // Vorherige Kerze fällt
	})
};

// Indikator zur Sammlung hinzufügen
Indicators.Add(patternIndicator);

// Eine Kerze verarbeiten
var result = patternIndicator.Process(candle);

// Ergebnis prüfen
if (result.GetValue<bool>())
{
	// Muster erkannt, erforderliche Aktionen ausführen
}
```

## Format der Pattern-Beschreibung

Beim Bearbeiten eines Patterns stellt jede Zeile eine separate Kerze dar. Die oberste Zeile ist die aktuelle Kerze; entsprechend ist die zweite Zeile eine Kerze zurück, die dritte und die folgenden Zeilen sind minus 2 und weitere Kerzen.

Der Editor verwendet die folgenden Parameter:
- O - Eröffnungskurs,
- H - Hoch,
- L - Tief,
- C - Schlusskurs,
- V - Volumen,
- OI - Open Interest,
- B - Kerzenkörper,
- LEN - Länge der Kerze (vom Hoch bis zum Tief),
- BS - unterer Schatten der Kerze,
- TS - oberer Schatten der Kerze.

Mit Parametern können die folgenden Indizes (Referenzen) auf die gewünschten Werte verwendet werden. Beispiel für den Schlusskurs:
- C: Schlusskurs der aktuellen Kerze,
- C1: Schlusskurs der 1. Kerze nach der aktuellen Kerze,
- C2: Schlusskurs der 2. Kerze nach der aktuellen Kerze,
- pC: Schlusskurs der vorherigen Kerze,
- pC1: Schlusskurs der Kerze vor der vorherigen Kerze,
Alle Referenzen müssen innerhalb des Bereichs des aktuellen Patterns liegen. Der Bereich des Patterns 3 Black Crows besteht beispielsweise aus der aktuellen und zwei vorherigen Kerzen; daher ist ein Verweis auf die dritte vorherige Kerze nicht zulässig.

Für zusätzliche Prüfungen von Parametern in Korrelation wird der Ausdruck && verwendet, der ein logisches UND darstellt.

Beim Beschreiben eines Patterns können außerdem die folgenden Funktionen verwendet werden: abs, acos, asin, atan, ceiling, cos, exp, floor, log, log10, max, min, pow, round, sign, sin, sqrt, tan, truncate. Mehr zur Verwendung von Funktionen wird in der Beschreibung des [Formel](../designer/strategies/using_visual_designer/elements/common/formula.md)-Würfels erläutert.

Bei Verwendung von [ExpressionCandlePattern](xref:StockSharp.Algo.Candles.Patterns.ExpressionCandlePattern) im Code werden Formeln nach denselben Regeln wie oben beschrieben erstellt und verwenden dieselben Variablen.

## Standard-Patterns

Für die schnelle Erstellung von Patterns auf Basis vorhandener Patterns können Sie den Bereich am unteren Rand des Pattern-Editor-Fensters verwenden. Ein Klick auf die Schaltfläche ![Designer Plus-Schaltfläche](../../images/designer_panel_circuits_01_button.png) am unteren Fensterrand fügt die Logik des in der gegenüberliegenden Dropdown-Liste ausgewählten Patterns in das Bearbeitungsfenster ein. Die Schaltfläche ![Designer Löschschaltfläche](../../images/designer_delete_button.png) am unteren Fensterrand löscht die ausgewählte Zeile im Bearbeitungsfenster.

## Erweiterte Funktionen

- [ComplexCandlePattern](xref:StockSharp.Algo.Candles.Patterns.ComplexCandlePattern) - ermöglicht das Kombinieren mehrerer Patterns zu einem einzelnen zusammengesetzten Pattern für komplexere Analysen.
- [ICandlePatternProvider](xref:StockSharp.Algo.Candles.Patterns.ICandlePatternProvider) - Interface des Pattern-Providers, das das Laden und Speichern benutzerdefinierter Patterns ermöglicht.
