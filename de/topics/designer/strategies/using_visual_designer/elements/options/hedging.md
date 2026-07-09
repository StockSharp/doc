# Absicherung

![Designer Hedging 00](../../../../../../images/designer_hedging_00.png)

Der Würfel wird zur Absicherung von Optionspositionen verwendet.

### Eingehende Sockets

Eingehende Sockets

- **Model** - das Berechnungsmodell (zum Beispiel Black-Scholes).
- **Instrument** - das Instrument, also der Basiswert.
- **Volume** - der numerische Wert des Volumens.
- **Position by underlying asset** - die Position im Basiswert.
- **Flag** - das Signal (Flag), das den Absicherungsprozess startet.

### Ausgehende Sockets

Ausgehende Sockets

- **Order** - die registrierte Order, die verwendet werden kann, um über das Element Trades by Order Trades dafür zu erhalten und sie mit dem Würfel Chart panel im Chart anzuzeigen.

### Parameter

Parameter

- **Hedging type** - der Absicherungstyp; mögliche Werte sind Delta, Gamma, Vega, Theta oder Rho.

## Empfohlene Inhalte

[Optionsquotierung](options_quoting.md)

