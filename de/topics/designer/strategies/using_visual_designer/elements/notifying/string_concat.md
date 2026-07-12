# Zeichenkettenverkettung

![Zeichenkettenverkettung Bildschirmfoto](../../../../../../images/designer_string_concat_00.png)

Der Würfel verkettet mehrere eingehende Werte anhand einer Vorlage mit Platzhaltern in geschweiften Klammern zu einer einzelnen Textzeichenfolge. Jeder Platzhaltername fügt einen Eingabeanschluss mit demselben Namen hinzu. Verschachtelte Eigenschaften können über Punkte referenziert werden; ein Format kann nach einem Doppelpunkt angegeben werden.

### Eingehende Anschlüsse

Eingehende Anschlüsse

- Werden dynamisch aus den Platzhalternamen erstellt. Jeder Anschluss akzeptiert Daten beliebigen Typs.

### Ausgehende Anschlüsse

Ausgehende Anschlüsse

- **Text** - die verkettete und formatierte Zeichenfolge.

### Parameter

Parameter

- **Vorlage** - Vorlage für die Zeichenfolgenverkettung und -formatierung. Beim Bearbeiten der Vorlage wird die Liste der Eingabeanschlüsse aktualisiert.

### Beispiele

- Die Vorlage `Price: {price:0.00}, Qty: {qty}` mit `price = 10.5` und `qty = 2` erzeugt `Price: 10.50, Qty: 2`.
- Die Vorlage `{time:HH:mm:ss} - {trade.Price}` mit den Anschlüssen `time` und `trade` (`trade.Price = 100`) erzeugt `09:15:00 - 100`.
- Die Vorlage `{side} {volume} @ {trade.Price}` mit den Anschlüssen `side = Buy`, `volume = 1`, `trade.Price = 100` erzeugt `Buy 1 @ 100`.

## Empfohlene Inhalte

[Zeichenkettenformatierung](string_format.md)
[Benachrichtigung](notification.md)

