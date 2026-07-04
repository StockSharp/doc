# Level 1

Um Level-1-Daten zu importieren, wahlen Sie im Hauptmenu der Anwendung **Import \=\> Level 1**

![hydra import level1](../../../images/hydra_import_level1.png)

## Importprozess.

1. **Import settings.**.

   Siehe Import von [Candles](candles.md).
2. Importparameter fur [S#](../../api.md)-Felder konfigurieren.

   Siehe Import von [Candles](candles.md).

   **Betrachten wir ein Beispiel fur den Import von Level 1 aus einer CSV-Datei:**
   - Die Datei, aus der Sie Daten importieren mochten, hat die folgende Vorlage:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{Changes:{BestBidPrice};{BestBidVolume};{BestAskPrice};{BestAskVolume};{LastTradeTime};{LastTradePrice};{LastTradeVolume}}
     	  				
     ```

     Hier entsprechen die Werte von {SecurityId.SecurityCode} und {SecurityId.BoardCode} den Werten **Security** bzw. **Board**. Daher weisen wir im Feld **Field order** die Werte 0 bzw. 1 zu.
   - Fur die Felder {ServerTime:default:yyyyMMdd} und {ServerTime:default:HH:mm:ss.ffffff} wahlen Sie im Fenster **S# field** die Felder Date bzw. **Time**. Wir weisen die Werte 2 und 3 zu.
   - Fur das Feld {BestBidPrice} wahlen Sie im Fenster **S# field** das Feld **Best buy price**. Wir weisen ihm den Wert 4 zu.
   - Fur das Feld {BestBidVolume} wahlen Sie im Fenster **S# field** das Feld **Best buy volume**. Wir weisen ihm den Wert 5 zu.
   - Fur das Feld {BestAskPrice} wahlen Sie im Fenster **S# field** das Feld **Best sale price**. Wir weisen ihm den Wert 6 zu.
   - Fur das Feld {BestAskVolume} wahlen Sie im Fenster **S# field** das Feld **Best sale volume**. Wir weisen ihm den Wert 7 zu.
   - Fur das Feld {LastTradeTime} wahlen Sie im Fenster **S# field** das Feld **Last trade time**. Wir weisen ihm den Wert 8 zu.
   - Fur das Feld {LastTradePrice} wahlen Sie im Fenster **S# field** das Feld **Last trade price**. Wir weisen ihm den Wert 9 zu.
   - Fur das Feld {LastTradeVolume} wahlen Sie im Fenster **S# field** das Feld **Last trade volume**. Wir weisen ihm den Wert 10 zu.
   - Das Fenster zur Feldeinstellung sieht wie folgt aus:![hydra import prop level 1](../../../images/hydra_import_prop_level1.png)

   Der Benutzer kann eine grosse Anzahl von Eigenschaften fur die heruntergeladenen Daten konfigurieren. Auf Basis der Vorlage der importierten Datei mussen Sie die Eigenschaft angeben und ihr die erforderliche Nummer in der Reihenfolge zuweisen. 
3. Um eine Vorschau der Daten anzuzeigen, klicken Sie auf die Schaltflache **Preview**.![hydra import preview level 1](../../../images/hydra_import_preview_level1.png)
4. Klicken Sie auf die Schaltflache **Import**.

