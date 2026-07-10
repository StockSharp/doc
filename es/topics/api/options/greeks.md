# Griegas

La fórmula del [modelo Black–Scholes](https://en.wikipedia.org/wiki/Black–Scholes_model) está implementada en [S#](../../api.md) para calcular las “griegas” básicas: delta, gamma, vega, theta y rho. Las estrategias de [Trading de volatilidad](volatility_trading.md) y [Cobertura delta](delta_hedging.md) se implementan sobre la base de esta fórmula. Además, [S#](../../api.md) permite calcular la prima de la opción y la [IV](https://en.wikipedia.org/wiki/Implied_volatility). 

El siguiente código muestra los métodos de la clase [BlackScholes](xref:StockSharp.Algo.Derivatives.BlackScholes) para calcular las “griegas”.

```cs
var bs = new BlackScholes(option, _connector, _connector);
DateTimeOffset currentTime = DateTimeOffset.Now;
decimal delta = bs.Delta(currentTime);
decimal gamma = bs.Gamma(currentTime);
decimal vega = bs.Vega(currentTime);
decimal theta = bs.Theta(currentTime);
decimal rho = bs.Rho(currentTime);
decimal iv = bs.ImpliedVolatility(currentTime, premium);  // premium es la prima del contrato de opción
```

Además, el paquete de instalación incluye el ejemplo OptionCalculator, en el que todas las “griegas” se calculan y visualizan mediante el componente gráfico [OptionDesk](xref:StockSharp.Xaml.OptionDesk). Véase [Componentes gráficos](graphic_components.md).
