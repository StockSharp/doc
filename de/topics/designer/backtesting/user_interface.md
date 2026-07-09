# Benutzeroberfläche

Um den Test auf historischen Daten auszuführen, wählen Sie eine Strategie aus, deren Schema auf der Historie getestet werden soll. Die Strategie wird im Panel [Schemata](../user_interface/schemas.md) im Strategieordner durch Doppelklick auf die gewünschte Strategie ausgewählt. Wenn Sie eine Strategie für den Arbeitsbereich auswählen, erscheint ein neuer Tab mit der Strategie. Beim Wechsel in diesen Tab wird im Menüband automatisch der Tab **Emulation** geöffnet.

![Designer Interface Backtesting 00](../../../images/designer_interface_backtesting_00.png)

Im Tab **Emulation** können Sie den Strategienamen ändern und eine kurze Beschreibung angeben.

Um den Test auf historischen Daten auszuführen, geben Sie im **Emulation tab** im Feld Market Data den Pfad zu den historischen Daten an und legen den Testzeitraum fest. Das Testing der Strategie wird durch Klicken auf die **Start button** ![Designer Interface Backtesting 01](../../../images/designer_interface_backtesting_01.png) gestartet. Nach dem Start der Strategie zum Testing werden die Schaltfläche **Pause** ![Designer Interface Backtesting 02](../../../images/designer_interface_backtesting_02.png), die das Testing anhält, und die Schaltfläche **Stop** ![Designer Interface Backtesting 03](../../../images/designer_interface_backtesting_03.png), die das Testing vollständig beendet, aktiv. Beim Bearbeiten der Strategie sind die Schaltflächen **Undo(Ctrl+Z)** ![Designer Interface Backtesting 04](../../../images/designer_interface_backtesting_04.png), zum Rückgängigmachen der letzten Aktion, **Redo(Ctrl+Y)** ![Designer Interface Backtesting 05](../../../images/designer_interface_backtesting_05.png), zum Wiederherstellen rückgängig gemachter Aktionen, und **Refresh(Ctrl+R)** ![Designer Interface Backtesting 06](../../../images/designer_interface_backtesting_06.png), zum vollständigen Aktualisieren des Schemas, hilfreich. Außerdem können Sie im **Emulation tab** den **Debugger** ([Debugging](debugging.md)) verwenden oder die **Optimization** der Strategie ausführen.

Der ausgewählte Strategietab enthält standardmäßig die folgenden Panels:

- Das Panel **Schema**, in dem der Hauptarbeitsprozess für die Gestaltung der Strategie und ihrer Komponenten durch Kombinieren von Würfeln und Verbindungslinien ausgeführt wird. Das Schema wird im Abschnitt [Diagramm-Panel](../strategies/using_visual_designer/diagram_panel.md) ausführlich beschrieben.
- Das Panel mit Informationselementen, das **Chart**, **Orders**, **Trades**, **Statistik** und weitere Komponenten enthält. Sie können die benötigte Komponente hinzufügen, indem Sie sie im Tab **Emulation** in der Gruppe **Komponenten** auswählen.
- Das Panel **Eigenschaften** ist standardmäßig rechts im Strategietab eingeklappt. Im Panel **Eigenschaften** können Sie die allgemeinen **Emulation**-Einstellungen festlegen. Beispielsweise kann das **Marktdatenspeicherformat** je nach Dateiformat des ausgewählten Speichers auf **BIN** oder **CSV** gesetzt werden. Der Datentyp kann Ticks oder Candles sein. Wenn Ticks ausgewählt ist, werden Kerzen aus den Ticks gebildet, die in den [Backtesting-Einstellungen](../user_interface/components/backtesting_settings.md) angegeben sind.

## Empfohlene Inhalte

[Backtesting-Einstellungen](../user_interface/components/backtesting_settings.md)

