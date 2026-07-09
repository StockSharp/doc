# Optionsquotierung

![Designer Quoting 00](../../../../../../images/designer_quoting_00.png)

Der Würfel wird verwendet, um Optionen gemäß den angegebenen Parametern zu quotieren.

### Eingehende Sockets

Eingehende Sockets

- **Modell** - das Berechnungsmodell (zum Beispiel Black-Scholes).
- **Volumen** - das Quoting-Volumen.

### Ausgehende Sockets

Ausgehende Sockets

- **Auftrag** - die registrierte Order, die verwendet werden kann, um über das Element **Trades nach Order** Trades dafür zu erhalten und sie mit dem Würfel **Diagrammbereich** im Chart anzuzeigen.

### Parameter

Parameter

- **Quoting** - der Parameter, nach dem das Quoting durchgeführt wird. Mögliche Werte sind **Volatilität** (das Quoting-Volumen folgt den angegebenen Volatilitätsgrenzen) oder **Theoretischer Preis** (das Quoting-Volumen folgt den angegebenen Grenzen des theoretischen Preises).
- **Richtung** - die Quoting-Richtung kann die Werte Kauf und Verkauf annehmen.
- **Minimum** - der Mindestwert für Volatilität oder theoretischen Preis.
- **Maximum** - der Höchstwert für Volatilität oder theoretischen Preis.

## Empfohlene Inhalte

[Ausübungspreise](strikes.md)

