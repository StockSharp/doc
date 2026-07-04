# Order books

Um Order Books zu importieren, wahlen Sie im Hauptmenu der Anwendung den Eintrag **Import \=\> Order books**.

![hydra import depths](../../../images/hydra_import_depths.png)

## Importprozess.

1. **Import settings.**.

   Siehe Import von [Candles](candles.md).
2. Importparameter fur [S#](../../api.md)-Felder konfigurieren.

   Siehe Import von [Candles](candles.md).

   **Betrachten wir ein Beispiel fur den Import eines Order Books aus einer CSV-Datei:**
   - Die Datei, aus der Sie Daten importieren mochten, hat die folgende Vorlage:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{Quote.Price};{Quote.Volume};{Side}
     	  				
     ```

     Hier entsprechen die Werte von {SecurityId.SecurityCode} und {SecurityId.BoardCode} den Werten **Security** bzw. **Board**. Daher weisen wir im Feld **Field order** die Werte 0 bzw. 1 zu.
   - Fur die Felder {ServerTime:default:yyyyMMdd} und {ServerTime:default:HH:mm:ss.ffffff} wahlen Sie im Fenster **S# field** die Felder **Date** bzw. **Time**. Wir weisen die Werte 2 und 3 zu.
   - Fur das Feld {Quote.Price} wahlen Sie im Fenster **S# field** das Feld **Price** - den Quote-Preis. Wir weisen ihm den Wert 4 zu.
   - Fur das Feld {Quote.Volume} wahlen Sie im Fenster **S# field** das Feld **Volume** - das Quote-Volumen. Wir weisen ihm den Wert 5 zu.
   - Fur das Feld {Side} wahlen Sie im Fenster **S# field** das Feld **Direction** - die Handelsrichtung (Buy oder Sell). Wir weisen ihm den Wert 6 zu.
   - Das Fenster zur Feldeinstellung sieht wie folgt aus:![hydra import prop depth](../../../images/hydra_import_prop_depth.png)

   Der Benutzer kann eine grosse Anzahl von Eigenschaften fur die heruntergeladenen Daten konfigurieren. Auf Basis der Vorlage der importierten Datei mussen Sie die Eigenschaft angeben und ihr die erforderliche Nummer in der Reihenfolge zuweisen. 
3. Um eine Vorschau der Daten anzuzeigen, klicken Sie auf die Schaltflache **Preview**.![hydra import preview depth](../../../images/hydra_import_preview_depth.png)
4. Klicken Sie auf die Schaltflache **Import**.

