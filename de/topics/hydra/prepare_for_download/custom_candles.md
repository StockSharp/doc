# Benutzerdefinierte Kerzen

Der Benutzer kann einen **benutzerdefinierten Typ** von Kerzen auswählen und selbst festlegen, welche Kerzen erstellt werden. Dabei werden die Kerzen direkt, also unmittelbar, erstellt.

![Hydra Kerzentyp 00 00](../../../images/hydra_type_candle_00_00.png)

Betrachten wir ein Beispiel für eine solche Erstellung. Die Börse **Bitmex** bietet keine Möglichkeit, Kerzen mit einem Zeitrahmen von 10 Minuten zu empfangen.

![Hydra Kerzentyp 00 01](../../../images/hydra_type_candle_00_01.png)

Die Reihenfolge zum Erhalten solcher Kerzen:

1. Wählen Sie **benutzerdefinierte** Kerzen.
2. In den Einstellungen geben wir **TF** candles und einen Zeitraum von 10 Minuten an.
3. In der Quelle geben wir an, woraus die Kerzen erstellt werden sollen - **Orderprotokoll**. ![Hydra Kerzentyp 00 02](../../../images/hydra_type_candle_00_02.png)
4. Wir legen den Zeitraum fest. Wie Sie sehen, ist neben dem Kerzennamen der Hinweis **Generiert** erschienen.![Hydra Kerzentyp 00 03](../../../images/hydra_type_candle_00_03.png)
5. Klicken Sie auf Start, und der Datendownload beginnt.![Hydra Kerzentyp 00 04](../../../images/hydra_type_candle_00_04.png)
6. Wechseln Sie zum Kerzenabschnitt und [sehen Sie sich die heruntergeladenen Daten an](../working_with_data/view_and_export.md).![Hydra Kerzentyp 00 06](../../../images/hydra_type_candle_00_06.png)

Wie Sie sehen, wurden die Daten erfolgreich empfangen.

Betrachten wir ein Beispiel, in dem wir eine [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage) erhalten müssen:

1. Wählen Sie **benutzerdefinierte** Kerzen.
2. Geben Sie in den Einstellungen Range-Kerzen und das Volumen 10 an.
3. In der Quelle geben wir an, woraus die Kerzen erstellt werden sollen - **Ticks**.![Hydra Kerzentyp 00 07](../../../images/hydra_type_candle_00_07.png)
4. Wir legen den Zeitraum fest.
5. Klicken Sie auf Start, und der Datendownload beginnt.![Hydra Kerzentyp 00 08](../../../images/hydra_type_candle_00_08.png)
6. Wechseln Sie zum Kerzenabschnitt und [sehen Sie sich die heruntergeladenen Daten an](../working_with_data/view_and_export.md).![Hydra Kerzentyp 00 09](../../../images/hydra_type_candle_00_09.png)
