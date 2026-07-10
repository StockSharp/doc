# Oberfläche

Nachdem eine Strategie dem Ordner **Live-Handel** hinzugefügt wurde, öffnet ein Doppelklick auf die hinzugefügte Strategie einen Tab mit dem Titel "Live [Strategiename]". Beim Wechsel zu diesem Tab wird im **Menüband** automatisch der Tab **Live-Handel** geöffnet. Im Tab **Live-Handel** können Sie das Instrument und das Portfolio angeben, mit denen die Strategie arbeiten soll. Durch Drücken der Schaltfläche **Starten** starten Sie den Live-Handel für die Strategie; durch Drücken der Schaltfläche **Stoppen** halten Sie ihn an.

![Designer Live-Handel-Oberfläche 00](../../../images/designer_interface_live_trade_00.png)

Der Strategietab enthält den Strategie-Designer für Schemata und Komponentenelemente, ähnlich wie in [Strategie-Designer](../strategies/using_visual_designer/diagram_panel.md) beschrieben. Zusätzlich enthält der Tab das Panel [Live-Einstellungen](../user_interface/components/live_settings.md), das standardmäßig eingeklappt ist und rechts am Tab angedockt wird.

Das Hinzufügen einer Strategie zu **Live-Handel** kopiert sie aus dem ursprünglichen Code, wenn [Schemata](../strategies/using_visual_designer.md) oder [Code](../strategies/using_code.md) verwendet werden. Änderungen am Algorithmus innerhalb der **Live-Handel**-Kopie wirken sich daher nicht auf das Original aus. Wenn beim Starten der Strategie eine Abweichung zwischen **Live-Handel** und dem Original besteht, wird eine Warnung angezeigt:

![Designer Live-Handel-Oberfläche 01](../../../images/designer_interface_live_trade_01.png)

- **Ja** bedeutet, Änderungen aus dem Original auf die **Live-Handel**-Kopie anzuwenden.
- **Nein** bedeutet, den Unterschied zu ignorieren und die **Live-Handel**-Kopie ohne Anwendung von Änderungen zu starten.
- **Abbrechen** bedeutet, nichts zu starten.

Änderungen in der **Live-Handel**-Kopie sollten minimal sein und auf Testing sowie die anschließende Übertragung in das Original abzielen. Andernfalls besteht das Risiko, Änderungen zu verlieren, wenn die **Live-Handel**-Kopie auf die Version des Originals aktualisiert wird.

## Siehe auch

[Verbindungseinstellungen](../connections_settings.md)
