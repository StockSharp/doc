# Order log

Um das Order Log zu importieren, wahlen Sie im Hauptmenu der Anwendung den Eintrag **Import \=\> Order log**.

![hydra import orderlog](../../../images/hydra_import_orderlog.png)

## Importprozess.

1. **Import settings.**.

   Siehe Import von [Candles](candles.md).
2. Importparameter fur [S#](../../api.md)-Felder konfigurieren.

   Siehe Import von [Candles](candles.md).

   **Betrachten wir ein Beispiel fur den Import eines Order Logs aus einer CSV-Datei:**
   - Die Datei, aus der Sie Daten importieren mochten, hat die folgende Vorlage:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{OrderId};{OrderPrice};{OrderVolume};{Side};{OrderState};{TimeInForce};{TradeId};{TradePrice}
     	  				
     ```

     Hier entsprechen die Werte von {SecurityId.SecurityCode} und {SecurityId.BoardCode} den Werten **Security** bzw. **Board**. Daher weisen wir im Feld **Field order** die Werte 0 bzw. 1 zu.
   - Fur die Felder {ServerTime:default:yyyyMMdd} und {ServerTime:default:HH:mm:ss.ffffff} wahlen Sie im Fenster **S# field** die Felder **Date** bzw. **Time**. Wir weisen die Werte 2 und 3 zu.
   - Fur das Feld {OrderId} wahlen Sie im Fenster **S# field** das Feld **ID** - die Order-ID. Wir weisen ihm den Wert 4 zu.
   - Fur das Feld {OrderPrice} wahlen Sie im Fenster **S# field** das Feld **Price** - den Orderpreis. Wir weisen ihm den Wert 5 zu.
   - Fur das Feld {OrderVolume} wahlen Sie im Fenster **S# field** das Feld **Volume** - das Ordervolumen. Wir weisen ihm den Wert 6 zu.
   - Fur das Feld {Side} wahlen Sie im Fenster **S# field** das Feld **Direction** - die Orderrichtung (Buy oder Sell). Wir weisen ihm den Wert 7 zu.
   - Fur das Feld {OrderState} wahlen Sie im Fenster **S# field** das Feld **Action** - den Orderstatus (active, inactive oder error). Wir weisen ihm den Wert 8 zu.
   - Fur das Feld {TimeInForce} wahlen Sie im Fenster **S# field** die **Time** in force - eine Ausfuhrungsbedingung der Limit-Order. Wir weisen ihr den Wert 9 zu.
   - Fur das Feld {TradeId} wahlen Sie im Fenster **S# field** das Feld **ID (trade)** - die Trade-Kennung. Wir weisen ihm den Wert 10 zu.
   - Fur das Feld {TradePrice} wahlen Sie im Fenster **S# field** das Feld **Price (trade)** - den Trade-Preis. Wir weisen ihm den Wert 11 zu.
   - Das Fenster zur Feldeinstellung sieht wie folgt aus:![hydra import prop orderlog](../../../images/hydra_import_prop_orderlog.png)

   Der Benutzer kann eine grosse Anzahl von Eigenschaften fur die heruntergeladenen Daten konfigurieren. Auf Basis der Vorlage der importierten Datei mussen Sie die Eigenschaft angeben und ihr die erforderliche Nummer in der Reihenfolge zuweisen. 
3. Um eine Vorschau der Daten anzuzeigen, klicken Sie auf die Schaltflache **Preview**.![hydra import preview orderlog](../../../images/hydra_import_preview_orderlog.png)
4. Klicken Sie auf die Schaltflache **Import**.

