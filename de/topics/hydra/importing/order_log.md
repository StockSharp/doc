# Order-Log

Um das Order-Log zu importieren, wählen Sie im Hauptmenü der Anwendung den Eintrag **Import \=\> Order-Log**.

![hydra import orderlog](../../../images/hydra_import_orderlog.png)

## Importprozess.

1. **Importeinstellungen.**.

   Siehe Import von [Candles](candles.md).
2. Importparameter für [S#](../../api.md)-Felder konfigurieren.

   Siehe Import von [Candles](candles.md).

   **Betrachten wir ein Beispiel für den Import eines Order-Logs aus einer CSV-Datei:**
   - Die Datei, aus der Sie Daten importieren möchten, hat die folgende Vorlage:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{OrderId};{OrderPrice};{OrderVolume};{Side};{OrderState};{TimeInForce};{TradeId};{TradePrice}

     ```

     Hier entsprechen die Werte von {SecurityId.SecurityCode} und {SecurityId.BoardCode} den Werten **Security** bzw. **Board**. Daher weisen wir im Feld **Field order** die Werte 0 bzw. 1 zu.
   - Für die Felder {ServerTime:default:yyyyMMdd} und {ServerTime:default:HH:mm:ss.ffffff} wählen Sie im Fenster **S# field** die Felder **Date** bzw. **Time**. Wir weisen die Werte 2 und 3 zu.
   - Für das Feld {OrderId} wählen Sie im Fenster **S# field** das Feld **ID** - die Order-ID. Wir weisen ihm den Wert 4 zu.
   - Für das Feld {OrderPrice} wählen Sie im Fenster **S# field** das Feld **Price** - den Orderpreis. Wir weisen ihm den Wert 5 zu.
   - Für das Feld {OrderVolume} wählen Sie im Fenster **S# field** das Feld **Volume** - das Ordervolumen. Wir weisen ihm den Wert 6 zu.
   - Für das Feld {Side} wählen Sie im Fenster **S# field** das Feld **Direction** - die Orderrichtung (Buy oder Sell). Wir weisen ihm den Wert 7 zu.
   - Für das Feld {OrderState} wählen Sie im Fenster **S# field** das Feld **Action** - den Orderstatus (active, inactive oder error). Wir weisen ihm den Wert 8 zu.
   - Für das Feld {TimeInForce} wählen Sie im Fenster **S# field** die **Time** in force - eine Ausführungsbedingung der Limit-Order. Wir weisen ihr den Wert 9 zu.
   - Für das Feld {TradeId} wählen Sie im Fenster **S# field** das Feld **ID (trade)** - die Trade-Kennung. Wir weisen ihm den Wert 10 zu.
   - Für das Feld {TradePrice} wählen Sie im Fenster **S# field** das Feld **Price (trade)** - den Trade-Preis. Wir weisen ihm den Wert 11 zu.
   - Das Fenster zur Feldeinstellung sieht wie folgt aus:![hydra import prop orderlog](../../../images/hydra_import_prop_orderlog.png)

   Der Benutzer kann eine große Anzahl von Eigenschaften für die heruntergeladenen Daten konfigurieren. Auf Basis der Vorlage der importierten Datei müssen Sie die Eigenschaft angeben und ihr die erforderliche Nummer in der Reihenfolge zuweisen.
3. Um eine Vorschau der Daten anzuzeigen, klicken Sie auf die Schaltfläche **Preview**.![hydra import preview orderlog](../../../images/hydra_import_preview_orderlog.png)
4. Klicken Sie auf die Schaltfläche **Import**.

