# Zeichenkettenformatierung

![Zeichenkettenformatierung Bildschirmfoto](../../../../../../images/designer_string_format_00.png)

Der Würfel wandelt einen eingehenden Wert beliebigen Typs in eine Textzeichenfolge um. Die Umwandlung erfolgt anhand einer Vorlage mit Platzhaltern in geschweiften Klammern. Jeder Platzhalter verweist auf den gesamten Wert (`{0}`) oder auf eine seiner Eigenschaften (`{Price}`, `{Trade.Price}` usw.). Nach einem Doppelpunkt kann ein Format angegeben werden, um zu steuern, wie Zahlen, Datumswerte oder andere Objekte im Text erscheinen.

### Eingehende Sockets

Eingehende Sockets

- **Eingabe** - der zu formatierende Wert. Der Socket akzeptiert Daten beliebigen Typs.

### Ausgehende Sockets

Ausgehende Sockets

- **Text** - das Ergebnis der Anwendung der Vorlage auf den eingehenden Wert.

### Parameter

Parameter

- **Vorlage** - Vorlage für die Zeichenfolgenformatierung, die auf den eingehenden Wert angewendet wird. Die Standardvorlage ist `{0}`; das bedeutet, dass der Wert ohne zusätzliche Formatierung eingefügt wird. Platzhalter können Eigenschaftsnamen und Formatzeichenfolgen enthalten, zum Beispiel `Price: {0:0.00}` oder `{Price:0.00}`.

### Beispiele

- Die Vorlage `Price: {0:0.00}` mit Eingabe `10.5` erzeugt `Price: 10.50`.
- Die Vorlage `{Price} - {Volume}` mit dem eingehenden Trade-Objekt `{ Price = 100, Volume = 2 }` erzeugt `100 - 2`.
- Die Vorlage `{Trade.Price} - {Trade.Volume}` mit dem eingehenden Objekt `{ Trade = { Price = 100, Volume = 2 } }` erzeugt `100 - 2`.
- Die Vorlage `Time: {Time:HH:mm:ss}` mit einem eingehenden Objekt mit `Time = 2024-05-01T09:15:00` erzeugt `Time: 09:15:00`.

## Empfohlene Inhalte

[Zeichenkettenverkettung](string_concat.md)
[Benachrichtigung](notification.md)

