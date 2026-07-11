# Absicherung

![Absicherung Bildschirmfoto](../../../../../../images/designer_hedging_00.png)

Der Würfel wird zur Absicherung von Optionspositionen verwendet.

### Eingehende Sockets

Eingehende Sockets

- **Modell** - das Berechnungsmodell (zum Beispiel Black-Scholes).
- **Handelsinstrument** - das Instrument, also der Basiswert.
- **Volumen** - der numerische Wert des Volumens.
- **Position nach Basiswert** - die Position im Basiswert.
- **Markierung** - das Signal (Flag), das den Absicherungsprozess startet.

### Ausgehende Sockets

Ausgehende Sockets

- **Auftrag** - die registrierte Order, die verwendet werden kann, um über das Element **Trades nach Order** Trades dafür zu erhalten und sie mit dem Würfel **Diagrammbereich** im Chart anzuzeigen.

### Parameter

Parameter

- **Hedging-Typ** - der Absicherungstyp; mögliche Werte sind Delta, Gamma, Vega, Theta oder Rho.

## Empfohlene Inhalte

[Optionsquotierung](options_quoting.md)

