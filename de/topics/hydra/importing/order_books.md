# Orderbücher

Um Orderbücher zu importieren, wählen Sie im Hauptmenü der Anwendung den Eintrag **Importieren \=\> Orderbücher**.

![hydra import depths](../../../images/hydra_import_depths.png)

## Importprozess.

1. **Importeinstellungen.**.

   Siehe Import von [Kerzen](candles.md).
2. Importparameter für [S#](../../api.md)-Felder konfigurieren.

   Siehe Import von [Kerzen](candles.md).

   **Betrachten wir ein Beispiel für den Import eines Orderbuchs aus einer CSV-Datei:**
   - Die Datei, aus der Sie Daten importieren möchten, hat die folgende Vorlage:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{Quote.Price};{Quote.Volume};{Side}

     ```

     Hier entsprechen die Werte von {SecurityId.SecurityCode} und {SecurityId.BoardCode} den Werten **Instrument** bzw. **Handelsplatz**. Daher weisen wir im Feld **Feldreihenfolge** die Werte 0 bzw. 1 zu.
   - Für die Felder {ServerTime:default:yyyyMMdd} und {ServerTime:default:HH:mm:ss.ffffff} wählen Sie im Fenster **S#-Feld** die Felder **Datum** bzw. **Zeit**. Wir weisen die Werte 2 und 3 zu.
   - Für das Feld {Quote.Price} wählen Sie im Fenster **S#-Feld** das Feld **Preis** - den Quote-Preis. Wir weisen ihm den Wert 4 zu.
   - Für das Feld {Quote.Volume} wählen Sie im Fenster **S#-Feld** das Feld **Volumen** - das Quote-Volumen. Wir weisen ihm den Wert 5 zu.
   - Für das Feld {Side} wählen Sie im Fenster **S#-Feld** das Feld **Richtung** - die Handelsrichtung (Kaufen oder Verkaufen). Wir weisen ihm den Wert 6 zu.
   - Das Fenster zur Feldeinstellung sieht wie folgt aus:![hydra import prop depth](../../../images/hydra_import_prop_depth.png)

   Der Benutzer kann eine große Anzahl von Eigenschaften für die heruntergeladenen Daten konfigurieren. Auf Basis der Vorlage der importierten Datei müssen Sie die Eigenschaft angeben und ihr die erforderliche Nummer in der Reihenfolge zuweisen.
3. Um eine Vorschau der Daten anzuzeigen, klicken Sie auf die Schaltfläche **Vorschau**.![hydra import preview depth](../../../images/hydra_import_preview_depth.png)
4. Klicken Sie auf die Schaltfläche **Importieren**.
