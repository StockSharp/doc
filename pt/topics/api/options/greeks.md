# Gregas

A fórmula do [modelo Black-Scholes](https://en.wikipedia.org/wiki/Black–Scholes_model) é implementada no [S#](../../api.md) para calcular os “Greeks” básicos: delta, gamma, vega, theta e rho. As estratégias de [Negociação de volatilidade](volatility_trading.md) e [Cobertura delta](delta_hedging.md) são implementadas com base nesta fórmula. Além disso, o [S#](../../api.md) permite calcular o prémio da opção e a [IV](https://en.wikipedia.org/wiki/Implied_volatility).

O código seguinte mostra os métodos da classe [BlackScholes](xref:StockSharp.Algo.Derivatives.BlackScholes) para calcular os “Greeks”.

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

Além disso, o pacote de instalação inclui o exemplo OptionCalculator, no qual todos os “Greeks” são calculados e visualizados usando o componente gráfico [OptionDesk](xref:StockSharp.Xaml.OptionDesk). Consulte [Componentes gráficos](graphic_components.md).
