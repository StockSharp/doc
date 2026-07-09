# Strategie-Designer

Der Hauptprozess zum Entwerfen einer Strategie und ihrer Komponentenelemente findet im Panel **Schema** statt, indem Blöcke kombiniert und mit Linien verbunden werden. Das Panel Schema besteht aus den Panels **Palette**, **Designer** und **Eigenschaften**.

![Designer Designer schemes strategies and component elements 00](../../../../images/designer_designer_schemes_strategies_and_component_elements_00.png)

## Palette-Panel

Das Panel **Palette** enthält Blöcke, aus denen Strategien erstellt werden. Alle Elemente in der Palette sind in Kategorien unterteilt, die im Abschnitt [Description of blocks](elements.md) beschrieben werden. Um einen Block zum Panel **Designer** hinzuzufügen, klicken Sie mit der rechten Maustaste auf den gewünschten Block und ziehen ihn bei gedrückter Taste in das Panel **Designer**. Danach wird das Element automatisch ausgewählt, und seine Parameter werden im Fenster zur Bearbeitung der Blockeigenschaften angezeigt.

## Designer-Panel

Das Panel **Designer** ist der Bereich, in dem der gesamte Prozess zum Erstellen einer Strategie durch Kombinieren von Blöcken und Verbindungen (Linien) erfolgt. Es stellt das Strategieschema visuell dar. Ausführliche Informationen zum Erstellen einer Strategie finden Sie im Abschnitt [Creating an algorithm from blocks](first_strategy.md).

## Properties-Panel

Das Panel **Eigenschaften** zeigt die Parameter des auf dem Panel **Designer** ausgewählten Blocks an. Wenn ein Block im Panel **Designer** ausgewählt ist, wird sein Rahmen schwarz eingefärbt.

![Designer The Properties Panel 00](../../../../images/designer_properties_panel_00.png)

Das Panel **Eigenschaften** kann in zwei Modi angezeigt werden: *Basiseinstellungen* und *Erweiterte Einstellungen*.

Standardmäßig werden die Eigenschaften beim Erstellen eines Schemas zunächst im Modus *Basic settings* angezeigt. Um in den Modus *Advanced settings* zu wechseln, müssen Sie auf die entsprechende Überschrift klicken.

Im Modus *Basic settings* werden nur die wichtigsten Eigenschaften des Blocks angezeigt. Für den Block [Kerzen](elements/data_sources/candles.md) werden beispielsweise der Timeframe, das Flag zum Empfangen nur gebildeter Kerzen, das Flag für die Möglichkeit, Kerzen aus einem kleineren Timeframe zu bilden, und das Flag zum Abonnieren von Kerzen per Signal angezeigt.

Im Modus *Advanced settings* werden alle änderbaren und konfigurierbaren Eigenschaften des Blocks angezeigt.

![Designer The Properties Panel 00](../../../../images/designer_properties_panel_01.png)

Alle Blöcke enthalten einen Satz vordefinierter Eigenschaften, die im Modus *Advanced settings* sichtbar werden:

- **Name** – der Name des Elements, der im Designer angezeigt wird.
- **Logging level** – die Logging-Stufe für dieses Element.
- **Parameters** – Parameter des Elements in übergeordneten Elementen anzeigen.
- **Sockets** – Sockets des Elements in übergeordneten Elementen anzeigen.

Ausführliche Informationen zu den Eigenschaften jedes Blocks finden Sie im Abschnitt [Description of blocks](elements.md).

## Siehe auch

[Description of blocks](elements.md)

