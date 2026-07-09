# Order-Log

Um das Order-Log zu importieren, wählen Sie im Hauptmenü der Anwendung den Eintrag **Importieren \=\> Order-Log**.

![hydra import orderlog](../../../images/hydra_import_orderlog.png)

## Importprozess.

1. **Importeinstellungen.**.

   Siehe Import von [Kerzen](candles.md).
2. Importparameter für [S#](../../api.md)-Felder konfigurieren.

   Siehe Import von [Kerzen](candles.md).

   **Betrachten wir ein Beispiel für den Import eines Order-Logs aus einer CSV-Datei:**
   - Die Datei, aus der Sie Daten importieren möchten, hat die folgende Vorlage:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{OrderId};{OrderPrice};{OrderVolume};{Side};{OrderState};{TimeInForce};{TradeId};{TradePrice}

     ```

     Hier entsprechen die Werte von {SecurityId.SecurityCode} und {SecurityId.BoardCode} den Werten **Instrument** bzw. **Handelsplatz**. Daher weisen wir im Feld **Feldreihenfolge** die Werte 0 bzw. 1 zu.
   - Für die Felder {ServerTime:default:yyyyMMdd} und {ServerTime:default:HH:mm:ss.ffffff} wählen Sie im Fenster **S#-Feld** die Felder **Datum** bzw. **Zeit**. Wir weisen die Werte 2 und 3 zu.
   - Für das Feld {OrderId} wählen Sie im Fenster **S#-Feld** das Feld **ID** - die Order-ID. Wir weisen ihm den Wert 4 zu.
   - Für das Feld {OrderPrice} wählen Sie im Fenster **S#-Feld** das Feld **Preis** - den Orderpreis. Wir weisen ihm den Wert 5 zu.
   - Für das Feld {OrderVolume} wählen Sie im Fenster **S#-Feld** das Feld **Volumen** - das Ordervolumen. Wir weisen ihm den Wert 6 zu.
   - Für das Feld {Side} wählen Sie im Fenster **S#-Feld** das Feld **Richtung** - die Orderrichtung (Kaufen oder Verkaufen). Wir weisen ihm den Wert 7 zu.
   - Für das Feld {OrderState} wählen Sie im Fenster **S#-Feld** das Feld **Aktion** - den Orderstatus (aktiv, inaktiv oder fehlerhaft). Wir weisen ihm den Wert 8 zu.
   - Für das Feld {TimeInForce} wählen Sie im Fenster **S#-Feld** die **Gültigkeitsdauer** - eine Ausführungsbedingung der Limit-Order. Wir weisen ihr den Wert 9 zu.
   - Für das Feld {TradeId} wählen Sie im Fenster **S#-Feld** das Feld **Kennung (Trade)** - die Trade-Kennung. Wir weisen ihm den Wert 10 zu.
   - Für das Feld {TradePrice} wählen Sie im Fenster **S#-Feld** das Feld **Preis (Trade)** - den Trade-Preis. Wir weisen ihm den Wert 11 zu.
   - Das Fenster zur Feldeinstellung sieht wie folgt aus:![hydra import prop orderlog](../../../images/hydra_import_prop_orderlog.png)

   Der Benutzer kann eine große Anzahl von Eigenschaften für die heruntergeladenen Daten konfigurieren. Auf Basis der Vorlage der importierten Datei müssen Sie die Eigenschaft angeben und ihr die erforderliche Nummer in der Reihenfolge zuweisen.
3. Um eine Vorschau der Daten anzuzeigen, klicken Sie auf die Schaltfläche **Vorschau**.![hydra import preview orderlog](../../../images/hydra_import_preview_orderlog.png)
4. Klicken Sie auf die Schaltfläche **Importieren**.
