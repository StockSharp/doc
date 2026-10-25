# Erweiterte Instrumentinformationen

Quellen für erweiterte Informationen sind **CSV**-Dateien im Ordner `c:\\Users\\Users\\Documents\\StockSharp\\Hydra\\Extended info\\`. Sie werden beim Start von [Hydra](../../hydra.md) automatisch geladen.

Erweiterte Informationen können beliebige zusätzliche Angaben zu einem Instrument enthalten, zum Beispiel Land, Stadt, Board usw.

Jede Quelle erweiterter Informationen (CSV-Datei) enthält eine Liste von Instrumenten und die verfügbaren Eigenschaften. Für jede Quelle sind die erweiterten Informationen eindeutig.

Wenn die Quelle keine erweiterten Informationen für ein Instrument enthält, bleiben die entsprechenden Spalten in der Instrumentenliste leer.

Um die erforderlichen erweiterten Informationen auszuwählen, gehen Sie wie folgt vor:

1. Klicken Sie im Panel **Instrumente** auf die Schaltfläche **Erweiterte Informationen**.![Erweiterte Instrumentinformationen Bildschirmfoto 1](../../../images/hydra_extensioninfo_securities.png)
2. Es erscheint ein Fenster, in dem Sie den Pfad zur erforderlichen CSV-Datei auswählen müssen.![Erweiterte Instrumentinformationen Bildschirmfoto 2](../../../images/hydra_extensioninfo_window.png)

Unten sehen Sie ein Beispiel einer **CSV**-Datei mit erweiterten Informationen. Die Werte werden durch Semikolons getrennt: Die erste Zeile enthält die Namen der Eigenschaften, die zweite ihre Typen und jede weitere Zeile die Kennung eines Instruments und die Werte seiner Eigenschaften:

```
SecurityId;Country;City;Site
string;string;string;string
SBER@TQBR;Russland;Moskau;www.sberbank.ru
GAZP@TQBR;Russland;Sankt Petersburg;www.gazprom.ru
```
