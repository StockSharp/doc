# Export

Der Designer ermöglicht den Export jeder Art von Daten: Strategien, Blöcke und Indikatoren. Es gibt mehrere Möglichkeiten zum Export:

- Klicken Sie im Panel **Schemata** mit der rechten Maustaste auf die Strategie, den Block oder den Indikator. Wählen Sie im erscheinenden Menü **Exportieren**.
- Drücken Sie im Tab **Allgemein** die Schaltfläche **Exportieren**:

![Designer Export strategies 00](../../../images/designer_export_strategies_00.png)

Nach dem Drücken von **Exportieren** erscheint je nach Inhaltstyp ein Fenster:

- für ein [Schema](../strategies/using_visual_designer.md):

  ![Designer Export strategies 01](../../../images/designer_export_strategies_01.png)

  - Schema - exportiert das Schema unverändert. Der Modus **Eigenständig** ist für Schemas erforderlich, die eigene Elemente oder Indikatoren verwenden. In diesem Fall werden alle inneren Elemente innerhalb des Strategiediagramms exportiert.
  - Code - konvertiert das Schema in C#-Code.
  - DLL - kompiliert das Schema in eine DLL. Geeignet, wenn Sie den Code vertraulich halten müssen.

- für [Code](../strategies/using_code.md):

  ![Designer Export strategies 02](../../../images/designer_export_strategies_02.png)

  - Schema - exportiert den Code als JSON-Datei, die sowohl den Code selbst als auch die zum Kompilieren dieses Codes benötigten Referenzen enthält.
  - Code - exportiert den Code unverändert.
  - DLL - kompiliert den Code in eine DLL. Geeignet, wenn Sie den Code vertraulich halten müssen.

- für eine [dll](../strategies/using_dll.md) erscheint ein Dateiauswahlfenster.

## Siehe auch

[Strategien außerhalb von Designer ausführen](../live_execution/running_strategies_outside_of_designer.md)
