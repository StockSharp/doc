# Level 1

Um Level-1-Daten zu importieren, wählen Sie im Hauptmenü der Anwendung **Importieren \=\> Level 1**

![Hydra Level1-Ansicht importieren](../../../images/hydra_import_level1.png)

## Importprozess.

1. **Importeinstellungen**.

   Siehe Import von [Kerzen](candles.md).
2. Importparameter für [S#](../../api.md)-Felder konfigurieren.

   Siehe Import von [Kerzen](candles.md).

   **Betrachten wir ein Beispiel für den Import von Level 1 aus einer CSV-Datei:**
   - Die Datei, aus der Sie Daten importieren möchten, hat die folgende Vorlage:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{Changes:{BestBidPrice};{BestBidVolume};{BestAskPrice};{BestAskVolume};{LastTradeTime};{LastTradePrice};{LastTradeVolume}}

     ```

     Hier entsprechen die Werte von {SecurityId.SecurityCode} und {SecurityId.BoardCode} den Werten **Instrument** bzw. **Handelsplatz**. Daher weisen wir im Feld **Feldreihenfolge** die Werte 0 bzw. 1 zu.
   - Für die Felder {ServerTime:default:yyyyMMdd} und {ServerTime:default:HH:mm:ss.ffffff} wählen Sie im Fenster **S#-Feld** die Felder **Datum** bzw. **Zeit**. Wir weisen die Werte 2 und 3 zu.
   - Für das Feld {BestBidPrice} wählen Sie im Fenster **S#-Feld** das Feld **Bester Kaufpreis**. Wir weisen ihm den Wert 4 zu.
   - Für das Feld {BestBidVolume} wählen Sie im Fenster **S#-Feld** das Feld **Bestes Kaufvolumen**. Wir weisen ihm den Wert 5 zu.
   - Für das Feld {BestAskPrice} wählen Sie im Fenster **S#-Feld** das Feld **Bester Verkaufspreis**. Wir weisen ihm den Wert 6 zu.
   - Für das Feld {BestAskVolume} wählen Sie im Fenster **S#-Feld** das Feld **Bestes Verkaufsvolumen**. Wir weisen ihm den Wert 7 zu.
   - Für das Feld {LastTradeTime} wählen Sie im Fenster **S#-Feld** das Feld **Zeit des letzten Trades**. Wir weisen ihm den Wert 8 zu.
   - Für das Feld {LastTradePrice} wählen Sie im Fenster **S#-Feld** das Feld **Preis des letzten Trades**. Wir weisen ihm den Wert 9 zu.
   - Für das Feld {LastTradeVolume} wählen Sie im Fenster **S#-Feld** das Feld **Volumen des letzten Trades**. Wir weisen ihm den Wert 10 zu.
   - Das Fenster zur Feldeinstellung sieht wie folgt aus:![Hydra Import Level-1-Eigenschaften](../../../images/hydra_import_prop_level1.png)

   Der Benutzer kann eine große Anzahl von Eigenschaften für die heruntergeladenen Daten konfigurieren. Auf Basis der Vorlage der importierten Datei müssen Sie die Eigenschaft angeben und ihr die erforderliche Nummer in der Reihenfolge zuweisen.
3. Um eine Vorschau der Daten anzuzeigen, klicken Sie auf die Schaltfläche **Vorschau**.![Hydra Import Level-1-Vorschau](../../../images/hydra_import_preview_level1.png)
4. Klicken Sie auf die Schaltfläche **Importieren**.
