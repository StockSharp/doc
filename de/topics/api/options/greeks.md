# Optionsgriechen

Die Formel des [Black-Scholes-Modells](https://en.wikipedia.org/wiki/Black–Scholes_model) ist in [S#](../../api.md) implementiert, um die grundlegenden Optionsgriechen zu berechnen: Delta, Gamma, Vega, Theta und Rho. Die Strategien [Volatilitätshandel](volatility_trading.md) und [Delta-Hedging](delta_hedging.md) basieren auf dieser Formel. Außerdem ermöglicht [S#](../../api.md) die Berechnung der Optionsprämie und der [IV](https://en.wikipedia.org/wiki/Implied_volatility).

Der folgende Code zeigt Methoden der Klasse [BlackScholes](xref:StockSharp.Algo.Derivatives.BlackScholes) zur Berechnung der Optionsgriechen.

```cs
var bs = new BlackScholes(option, _connector, _connector);
DateTimeOffset currentTime = DateTimeOffset.Now;
decimal delta = bs.Delta(currentTime);
decimal gamma = bs.Gamma(currentTime);
decimal vega = bs.Vega(currentTime);
decimal theta = bs.Theta(currentTime);
decimal rho = bs.Rho(currentTime);
decimal iv = bs.ImpliedVolatility(currentTime, premium);  // premium ist die Prämie des Optionskontrakts
```

Zusätzlich enthält das Installationspaket das Beispiel OptionCalculator, in dem alle Optionsgriechen berechnet und mit der grafischen Komponente [OptionDesk](xref:StockSharp.Xaml.OptionDesk) visualisiert werden. Siehe [Grafische Komponenten](graphic_components.md).
