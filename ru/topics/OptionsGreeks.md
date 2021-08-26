# Греки

В [S\#](StockSharpAbout.md) реализована формула [Блэка — Шоулза](https://ru.wikipedia.org/wiki/Модель_Блэка_—_Шоулза) для расчета основных "греков": дельта, гамма, вега, тета и ро. На основе этой формулы реализованы стратегии [Котирование по волатильности](OptionsQuoting.md) и [Дельта\-хеджирование](OptionsHedge.md). Также [S\#](StockSharpAbout.md) позволяет рассчитать премию опциона и [IV](https://en.wikipedia.org/wiki/Implied_volatility). 

В следующем фрагменте кода показаны методы класса [BlackScholes](../api/StockSharp.Algo.Derivatives.BlackScholes.html) для расчета греков

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

Кроме того в дистрибутив входит пример OptionCalculator, в котором рассчитываются и визуализируются все "греки" при помощи графического компонента [OptionDesk](../api/StockSharp.Xaml.OptionDesk.html). См. [Графические компоненты для опционов](OptionsGUI.md). 
