# Strategien aus dem Designer ausführen

[Shell](../shell.md) kann Strategien ausführen, die im [Designer](../designer.md) erstellt und über den [Export](../designer/export_import/export.md) ausgegeben wurden. Code muss dafür nicht geschrieben werden: Das Schema wird von derselben Engine ausgeführt wie im Designer.

## Import

Öffnen Sie den Bereich [Echtzeit](user_interface/real_time.md) oder [Emulation](user_interface/emulation.md), klicken Sie auf **Hinzufügen** und im geöffneten Fenster auf **Importieren**.

![Auswahl einer Strategie in Shell](../../images/shell_add_strategy_00.png)

Wählen Sie die Datei eines aus dem Designer exportierten Schemas aus. Es erscheint in der Liste neben den mitgelieferten Strategien unter dem Namen, den es im Designer hatte. Neben der Liste wird angezeigt, wie der Import ausgegangen ist: Das Schema ist importiert, der Import ist abgebrochen, oder der Import ist mit Angabe des Grundes fehlgeschlagen.

Importierte Schemata werden im Ordner `Data` neben der Anwendung gespeichert und bleiben nach einem Neustart in der Liste.

## Start

Danach arbeitet man mit dem Schema genauso wie mit einer mitgelieferten Strategie: Wählen Sie es aus, klicken Sie auf **OK**, füllen Sie im Panel **Strategie** Instrument, Portfolio, Volumen und die Parameter des Schemas aus und klicken Sie auf **Start**.

Im Bereich [Emulation](user_interface/emulation.md) wird das Schema auf historischen Daten durchlaufen und liefert dieselbe Statistik, dasselbe Chart und dieselben Listen von Orders und Trades wie jede andere Strategie.

## Empfohlene Inhalte

[Strategie erstellen](create_strategy.md)

[Echtzeit](user_interface/real_time.md)
