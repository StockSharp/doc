# Indicator

![Designer Indicator 00](../../../../../../images/designer_indicator_00.png)

Dieser Block wird zur Berechnung von Indikatorwerten verwendet.

## Eingehende Sockets

- **Any Data** – ein bestimmter Datentyp, auf dessen Grundlage der ausgewählte Indikator berechnet werden soll (je nach Indikator kann dies ein numerischer Wert, eine Kerze usw. sein).

## Ausgehende Sockets

- **Indicator** – der berechnete Indikatorwert, der zur Anzeige im Chart-Panel oder für weitere Berechnungen verwendet werden kann.

## Parameter

- **Indicator Type** - ein Parameter zur Auswahl des gewünschten Indikators sowie mehrere zusätzliche Parameter, die dem ausgewählten Indikatortyp entsprechen. Der Satz dieser Parameter ändert sich, wenn sich der ausgewählte Indikatortyp ändert.
- **Final** - nur [finale Werte](../../../../../api/indicators.md) des Indikators weitergeben.
- **Formed** - nur Werte weitergeben, wenn der Indikator vollständig [gebildet](../../../../../api/indicators.md) ist.

![Designer Indicator 01](../../../../../../images/designer_indicator_01.png)

## Siehe auch

[List of Indicators](../../../../../api/indicators/list_of_indicators.md)
[Logical Condition](logical_condition.md)

