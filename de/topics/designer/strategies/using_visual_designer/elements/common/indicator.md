# Indikator

![Designer Indikator 00](../../../../../../images/designer_indicator_00.png)

Dieser Block wird zur Berechnung von Indikatorwerten verwendet.

## Eingehende Anschlüsse

- **Beliebige Daten** – ein bestimmter Datentyp, auf dessen Grundlage der ausgewählte Indikator berechnet werden soll (je nach Indikator kann dies ein numerischer Wert, eine Kerze usw. sein).

## Ausgehende Anschlüsse

- **Indikator** – der berechnete Indikatorwert, der zur Anzeige im Chart-Panel oder für weitere Berechnungen verwendet werden kann.

## Parameter

- **Indikatortyp** - ein Parameter zur Auswahl des gewünschten Indikators sowie mehrere zusätzliche Parameter, die dem ausgewählten Indikatortyp entsprechen. Der Satz dieser Parameter ändert sich, wenn sich der ausgewählte Indikatortyp ändert.
- **Endwert** - nur [finale Werte](../../../../../api/indicators.md) des Indikators weitergeben.
- **Abgeschlossen** - nur Werte weitergeben, wenn der Indikator vollständig [gebildet](../../../../../api/indicators.md) ist.

![Designer Indikator 01](../../../../../../images/designer_indicator_01.png)

## Siehe auch

[Liste der Indikatoren](../../../../../api/indicators/list_of_indicators.md)
[Logische Bedingung](logical_condition.md)

