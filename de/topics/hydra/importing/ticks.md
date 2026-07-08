# Ticks

Um Trades zu importieren, wählen Sie die Registerkarte **Import \=\> Ticks**.

![hydra import trades](../../../images/hydra_import_trades.png)

## Importprozess.

1. **Import settings.**.

   Siehe Import von [Kerzen](candles.md).
2. Importparameter für [S#](../../api.md)-Felder konfigurieren.

   Siehe Import von [Kerzen](candles.md).

   **Betrachten wir ein Beispiel für den Import von Trades (Ticks) aus einer CSV-Datei:**
   - Die Datei, aus der Sie Daten importieren möchten, hat die folgende Vorlage:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{TradeId};{TradePrice};{TradeVolume};{OriginSide}

     ```

     Hier entsprechen die Werte von {SecurityId.SecurityCode} und {SecurityId.BoardCode} den Werten **Security** bzw. **Board**. Daher weisen wir im Feld **Field order** die Werte 0 bzw. 1 zu.
   - Für die Felder {ServerTime:default:yyyyMMdd} und {ServerTime:default:HH:mm:ss.ffffff} wählen Sie im Fenster **S# field** die Felder **Date** bzw. **Time**. Wir weisen die Werte 2 und 3 zu.
   - Für das Feld {TradeId} wählen Sie im Fenster **S# field** das Feld **Identifier** - die Trade-Kennung oder Trade-Nummer. Wir weisen ihm den Wert 4 zu.
   - Für das Feld {TradePrice} wählen Sie das Feld **Price** - den Trade-Preis aus dem Fenster **S# field**. Wir weisen ihm den Wert 5 zu.
   - Für das Feld {TradeVolume} wählen Sie im Fenster **S# field** das Feld **Volume** - das Trade-Volumen. Wir weisen ihm den Wert 6 zu.
   - Für das Feld {OriginSide} wählen Sie im Fenster **S# field** das Feld **Initiator** - den Trade-Initiator (Seller oder Buyer). Wir weisen ihm den Wert 7 zu.
   - Das Fenster zur Feldeinstellung sieht wie folgt aus:![hydra import prop trade](../../../images/hydra_import_prop_trade.png)

   Der Benutzer kann eine große Anzahl von Eigenschaften für die heruntergeladenen Daten konfigurieren. Auf Basis der Vorlage der importierten Datei müssen Sie die Eigenschaft angeben und ihr die erforderliche Nummer in der Reihenfolge zuweisen.
3. Um eine Vorschau der Daten anzuzeigen, klicken Sie auf die Schaltfläche **Preview**.![hydra import preview trade](../../../images/hydra_import_preview_trade.png)
4. Klicken Sie auf die Schaltfläche **Import**.

