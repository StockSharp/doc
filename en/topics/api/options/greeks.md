# Greeks

The formula of [Black–Scholes model](https://en.wikipedia.org/wiki/Black–Scholes_model) is realized in the [S\#](../../api.md) to calculate the basic “Greeks”: delta, gamma, vega, theta and rho. The [volatility trading](volatility_trading.md) and [Delta\-hedging](delta_hedging.md) strategies are realized on the basis of this formula. Also [S\#](../../api.md) allows you to calculate the option premium and [IV](https://en.wikipedia.org/wiki/Implied_volatility). 

The following code shows the [BlackScholes](xref:StockSharp.Algo.Derivatives.BlackScholes) class methods to calculate the “Greeks”.

```cs
var bs = new BlackScholes(option, _connector, _connector);
DateTimeOffset currentTime = DateTimeOffset.Now;
decimal delta = bs.Delta(currentTime);
decimal gamma = bs.Gamma(currentTime);
decimal vega = bs.Vega(currentTime);
decimal theta = bs.Theta(currentTime);
decimal rho = bs.Rho(currentTime);
decimal iv = bs.ImpliedVolatility(currentTime, premium);  // premium is premium of the option contract
```

In addition the installation package includes the OptionCalculator example, in which all the “Greeks” are calculated and visualized using the [OptionDesk](xref:StockSharp.Xaml.OptionDesk) graphical component. See [Graphic components](graphic_components.md). 
