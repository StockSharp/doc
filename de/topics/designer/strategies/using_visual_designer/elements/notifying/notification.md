# Benachrichtigung

![Benachrichtigung Bildschirmfoto](../../../../../../images/designer_notice_00.png)

Der Würfel sendet eine Benachrichtigung, wenn Daten an seinem Eingabe-Socket eintreffen. Der eingehende Wert wird über `ToString` in Text umgewandelt. Sie können eine [Variable](../data_sources/variable.md) verbinden, um festen Text zu senden, Trades- oder Kerzen-Streams anschließen, um deren Details anzuzeigen, oder die Würfel [Zeichenkettenformatierung](string_format.md) und [Zeichenkettenverkettung](string_concat.md) verwenden, um eine eigene Nachricht vorzubereiten.

### Eingehende Sockets

Eingehende Sockets

- **Nachricht** - zu sendende Daten. Jeder Wert wird akzeptiert und in eine Zeichenfolge umgewandelt.

### Parameter

Parameter

- **Typ** - Nachrichtentyp (Popup-Fenster, E-Mail, SMS usw.). Die Benachrichtigungstypen sind im Abschnitt [Benachrichtigungen](../../../../../terminal/notifications.md) beschrieben.
- **Telegram** - Kanal für Telegram-Benachrichtigungen.
- **Kopfzeile** - die Überschrift der Nachricht.

## Empfohlene Inhalte

[Zeichenkettenformatierung](string_format.md)
[Zeichenkettenverkettung](string_concat.md)
[Benachrichtigungen](../../../../../terminal/notifications.md)

