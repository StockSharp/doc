# Греки

В [S\#](../../api.md) реализована формула [Блэка — Шоулза](https://ru.wikipedia.org/wiki/Модель_Блэка_—_Шоулза) для расчета основных "греков": дельта, гамма, вега, тета и ро. На основе этой формулы реализованы стратегии [Котирование по волатильности](volatility_trading.md) и [Дельта\-хеджирование](delta_hedging.md). Также [S\#](../../api.md) позволяет рассчитать премию опциона и [IV](https://en.wikipedia.org/wiki/Implied_volatility). 

В следующем фрагменте кода показаны методы класса [BlackScholes](xref:StockSharp.Algo.Derivatives.BlackScholes) для расчета греков

```cs
var bs = new BlackScholes(option, _connector, _connector);
DateTimeOffset currentTime = DateTimeOffset.Now;
decimal delta = bs.Delta(currentTime);
decimal gamma = bs.Gamma(currentTime);
decimal vega = bs.Vega(currentTime);
decimal theta = bs.Theta(currentTime);
decimal rho = bs.Rho(currentTime);
decimal iv = bs.ImpliedVolatility(currentTime, premium);  // где premium - премия по опциону
```

Кроме того в дистрибутив входит пример OptionCalculator, в котором рассчитываются и визуализируются все "греки" при помощи графического компонента [OptionDesk](xref:StockSharp.Xaml.OptionDesk). См. [Графические компоненты для опционов](graphic_components.md). 
