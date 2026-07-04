# Ticks

Um Trades zu importieren, wahlen Sie die Registerkarte **Import \=\> Ticks**.

![hydra import trades](../../../images/hydra_import_trades.png)

## Importprozess.

1. **Import settings.**.

   Siehe Import von [Candles](candles.md).
2. Importparameter fur [S#](../../api.md)-Felder konfigurieren.

   Siehe Import von [Candles](candles.md).

   **Betrachten wir ein Beispiel fur den Import von Trades (Ticks) aus einer CSV-Datei:**
   - Die Datei, aus der Sie Daten importieren mochten, hat die folgende Vorlage:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{TradeId};{TradePrice};{TradeVolume};{OriginSide}
     	  				
     ```

     Hier entsprechen die Werte von {SecurityId.SecurityCode} und {SecurityId.BoardCode} den Werten **Security** bzw. **Board**. Daher weisen wir im Feld **Field order** die Werte 0 bzw. 1 zu.
   - Fur die Felder {ServerTime:default:yyyyMMdd} und {ServerTime:default:HH:mm:ss.ffffff} wahlen Sie im Fenster **S# field** die Felder **Date** bzw. **Time**. Wir weisen die Werte 2 und 3 zu.
   - Fur das Feld {TradeId} wahlen Sie im Fenster **S# field** das Feld **Identifier** - die Trade-Kennung oder Trade-Nummer. Wir weisen ihm den Wert 4 zu.
   - Fur das Feld {TradePrice} wahlen Sie das Feld **Price** - den Trade-Preis aus dem Fenster **S# field**. Wir weisen ihm den Wert 5 zu.
   - Fur das Feld {TradeVolume} wahlen Sie im Fenster **S# field** das Feld **Volume** - das Trade-Volumen. Wir weisen ihm den Wert 6 zu.
   - Fur das Feld {OriginSide} wahlen Sie im Fenster **S# field** das Feld **Initiator** - den Trade-Initiator (Seller oder Buyer). Wir weisen ihm den Wert 7 zu.
   - Das Fenster zur Feldeinstellung sieht wie folgt aus:![hydra import prop trade](../../../images/hydra_import_prop_trade.png)

   Der Benutzer kann eine grosse Anzahl von Eigenschaften fur die heruntergeladenen Daten konfigurieren. Auf Basis der Vorlage der importierten Datei mussen Sie die Eigenschaft angeben und ihr die erforderliche Nummer in der Reihenfolge zuweisen. 
3. Um eine Vorschau der Daten anzuzeigen, klicken Sie auf die Schaltflache **Preview**.![hydra import preview trade](../../../images/hydra_import_preview_trade.png)
4. Klicken Sie auf die Schaltflache **Import**.

