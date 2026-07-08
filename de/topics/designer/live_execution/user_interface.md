# Oberfläche

Nachdem eine Strategie dem Ordner **Live** hinzugefügt wurde, öffnet ein Doppelklick auf die hinzugefügte Strategie einen Tab mit dem Titel "Live [Strategy Name]". Beim Wechsel zu diesem Tab wird im **Ribbon** automatisch der Tab **Live** geöffnet. Im Tab **Live** können Sie das Instrument und das Portfolio angeben, mit denen die Strategie arbeiten soll. Durch Drücken der Schaltfläche **Start** starten Sie den Live-Handel für die Strategie; durch Drücken der Schaltfläche **Stop** halten Sie ihn an.

![Designer Interface Live trade 00](../../../images/designer_interface_live_trade_00.png)

Der Strategietab enthält den Strategy Designer für Schemas und Komponentenelemente, ähnlich wie in [Strategie-Designer](../strategies/using_visual_designer/diagram_panel.md) beschrieben. Zusätzlich enthält der Tab das Panel [Live Trading Properties](../user_interface/components/live_settings.md), das standardmäßig eingeklappt ist und rechts am Tab angedockt wird.

Das Hinzufügen einer Strategie zu **Live** kopiert sie aus dem ursprünglichen Code, wenn [Schemas](../strategies/using_visual_designer.md) oder [Code](../strategies/using_code.md) verwendet werden. Änderungen am Algorithmus innerhalb der **Live**-Kopie wirken sich daher nicht auf das Original aus. Wenn beim Starten der Strategie eine Abweichung zwischen **Live** und dem Original besteht, wird eine Warnung angezeigt:

![Designer Interface Live trade 01](../../../images/designer_interface_live_trade_01.png)

- **Yes** bedeutet, Änderungen aus dem Original auf die **live**-Kopie anzuwenden.
- **No** bedeutet, den Unterschied zu ignorieren und die **live**-Kopie ohne Anwendung von Änderungen zu starten.
- **Cancel** bedeutet, nichts zu starten.

Änderungen in der **Live**-Kopie sollten minimal sein und auf Testing sowie die anschließende Übertragung in das Original abzielen. Andernfalls besteht das Risiko, Änderungen zu verlieren, wenn die **Live**-Kopie auf die Version des Originals aktualisiert wird.

## Siehe auch

[Verbindungseinstellungen](../connections_settings.md)
