# Options quoting

![Designer Quoting 00](../../../../../../images/designer_quoting_00.png)

Der Würfel wird verwendet, um Optionen gemäß den angegebenen Parametern zu quotieren.

### Eingehende Sockets

Eingehende Sockets

- **Model** - das Berechnungsmodell (zum Beispiel Black-Scholes).
- **Volume** - das Quoting-Volumen.

### Ausgehende Sockets

Ausgehende Sockets

- **Order** - die registrierte Order, die verwendet werden kann, um über das Element **Trades** by Order Trades dafür zu erhalten und sie mit dem Würfel **Chart panel** im Chart anzuzeigen.

### Parameter

Parameter

- **Quoting** - der Parameter, nach dem das Quoting durchgeführt wird. Mögliche Werte sind **Volatility** (das Quoting-Volumen folgt den angegebenen Volatilitätsgrenzen) oder **Theoretical price** (das Quoting-Volumen folgt den angegebenen Grenzen des theoretischen Preises).
- **Direction** - die Quoting-Richtung kann die Werte Purchase und Sell annehmen.
- **Minimum** - der Mindestwert für Volatilität oder theoretischen Preis.
- **Maximum** - der Höchstwert für Volatilität oder theoretischen Preis.

## Empfohlene Inhalte

[Derivatives](strikes.md)

