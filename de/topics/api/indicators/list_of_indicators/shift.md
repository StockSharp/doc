# Shift

**Shift** ist ein Hilfsindikator, der den eingehenden Wertstrom um eine angegebene Anzahl von Perioden versetzt. Er verändert die
Daten nicht, sondern verzögert sie nur oder richtet sie für zusammengesetzte Berechnungen aus.

Verwenden Sie die Klasse [Shift](xref:StockSharp.Algo.Indicators.Shift), um auf den Indikator zuzugreifen.

## Beschreibung

Der Indikator speichert einen Puffer der neuesten Werte und gibt denjenigen aus, der vor **Length** Balken eingegangen ist. Wenn der Datenverlauf
kürzer als der erforderliche Offset ist, gilt der Wert als undefiniert.

## Parameter

- **Length** – Anzahl der Perioden, um die die Daten verschoben werden.

## Nutzung

- Signale verschiedener Indikatoren rechtzeitig aufeinander abstimmen.
- Erstellen Sie benutzerdefinierte Indikatoren und Strategien, die verzögerte Eingaben erfordern.
- Erstellen Sie synthetische Reihen, um beispielsweise die Differenz zwischen aktuellen Preisen und vergangenen Werten zu berechnen.

![indicator_shift](../../../../images/indicator_shift.png)

## Siehe auch

[PassThrough](pass_through.md)
[Sum](sum_n.md)
