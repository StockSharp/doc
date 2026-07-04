# Instruments

Um Instrumente zu importieren, wahlen Sie die Registerkarte **Import \=\> Instruments**.

![hydra import securities](../../../images/hydra_import_securities.png)

## Importprozess.

1. **Import settings.**.

   Siehe Import von [Candles](candles.md).
2. Importparameter fur [S#](../../api.md)-Felder konfigurieren.

   Siehe Import von [Candles](candles.md).

   **Betrachten wir ein Beispiel fur den Import eines Instruments aus einer CSV-Datei:**
   - Die Datei, aus der Sie Daten importieren mochten, hat die folgende Vorlage:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{PriceStep};{SecurityType};{VolumeStep}
     	  				
     ```

     Hier entsprechen die Werte von {SecurityId.SecurityCode} und {SecurityId.BoardCode} den Werten **Security** bzw. **Board**. Daher weisen wir im Feld **Field order** die Werte 0 bzw. 1 zu.
   - Fur das Feld {PriceStep} wahlen Sie im Fenster **S# field** das Feld **Nominal** und weisen ihm den Wert 2 zu.
   - Fur das Feld {SecurityType} wahlen Sie im Fenster **S# field** das Feld **Type** - den Instrumenttyp (Aktie, Wahrung, Futures usw.). Wir weisen ihm den Wert 3 zu.
   - Fur das Feld {VolumeStep} wahlen Sie im Fenster **S# field** das Feld **Min volume (base)** - das Basis- oder Mindestvolumen des Instruments. Wir weisen ihm den Wert 4 zu.
   - Das Fenster zur Feldeinstellung sieht wie folgt aus:![hydra import prop securitiy](../../../images/hydra_import_prop_securitiy.png)

   Der Benutzer kann eine grosse Anzahl von Eigenschaften fur die heruntergeladenen Daten konfigurieren. Auf Basis der Vorlage der importierten Datei mussen Sie die Eigenschaft angeben und ihr die erforderliche Nummer in der Reihenfolge zuweisen.
3. Um eine Vorschau der Daten anzuzeigen, klicken Sie auf die Schaltflache **Preview**.![hydra import preview securitiy](../../../images/hydra_import_preview_securitiy.png)
4. Klicken Sie auf die Schaltflache **Import**.

