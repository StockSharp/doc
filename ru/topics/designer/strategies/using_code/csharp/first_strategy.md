# Пример стратегии на C\#

Создание стратегии из исходного кода будет рассмотрено на примере стратегии SMA \- аналогичный пример\-стратегии SMA, собранной из кубиков в пункте [Создание алгоритма из кубиков](../../using_visual_designer/first_strategy.md).

Данный раздел не будет описывать конструкции C# языка (как и [Strategy](../../../../api/strategies.md), на базе чего создаются стратегии), но будут упомянуты особенности для работы кода в **Дизайнере**.

> [!TIP]
> Стратегии, создающиеся в **Дизайнере** совместимы со стратегиями, создающиеся в [API](../../../../api.md) за счет использования общего базового класса [Strategy](../../../../api/strategies.md). Это делает возможность запуска таких стратегий вне **Дизайнера** значительно проще, чем [схем](../../../live_execution/running_strategies_outside_of_designer.md).

1. Параметры стратегии создаются через специальный подход:

```cs
_candleTypeParam = this.Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
_long = this.Param(nameof(Long), 80);
_short = this.Param(nameof(Short), 20);


private readonly StrategyParam<DataType> _candleTypeParam;

public DataType CandleType
{
	get => _candleTypeParam.Value;
	set => _candleTypeParam.Value = value;
}

private readonly StrategyParam<int> _long;

public int Long
{
	get => _long.Value;
	set => _long.Value = value;
}

private readonly StrategyParam<int> _short;

public int Short
{
	get => _short.Value;
	set => _short.Value = value;
}
```

При использовании класса [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) автоматически используется подход сохранения и восстановления настроек.

2. При создании индикаторов и подписки на маркет-данные необходимо связать их, чтобы поступающие данные из подписки могли обновлять значения индикаторов:

```cs
// ---------- create indicators -----------

var longSma = new SMA { Length = Long };
var shortSma = new SMA { Length = Short };

// ----------------------------------------

// --- bind candles set and indicators ----

var subscription = SubscribeCandles(CandleType);

subscription
	// bind indicators to the candles
	.Bind(longSma, shortSma, OnProcess)
	// start processing
	.Start();
```

3. При работе с графиком необходимо учитывать, что в случае запуска стратегии [вне Дизайнера](../../../live_execution/running_strategies_outside_of_designer.md) объект графика может быть отсутствовать.

```cs
var area = CreateChartArea();

// area can be null in case of no GUI (strategy hosted in Runner or in own console app)
if (area != null)
{
	DrawCandles(area, subscription);

	DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
	DrawIndicator(area, longSma);

	DrawOwnTrades(area);
}
```

4. Запустить защиту позиций через [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)), если такое требует логика стратегии:

```cs
// start protection by take profit and-or stop loss
StartProtection(TakeValue, StopValue);
```

5. Сама логика стратегии, реализованная в методе OnProcess. Метод вызывается подпиской, созданной на этапе 1:

```cs
private void OnProcess(ICandleMessage candle, decimal? longValue, decimal? shortValue)
{
	LogInfo(LocalizedStrings.SmaNewCandleLog, candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId);

	// in case we subscribed on non finished only candles
	if (candle.State != CandleStates.Finished)
		return;

	// calc new values for short and long
	var isShortLessThenLong = shortValue < longValue;

	if (_isShortLessThenLong == null)
	{
		_isShortLessThenLong = isShortLessThenLong;
	}
	else if (_isShortLessThenLong != isShortLessThenLong)
	{
		// crossing happened

		// if short less than long, the sale, otherwise buy
		var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;

		// calc size for open position or revert
		var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;

		var priceStep = GetSecurity().PriceStep ?? 1;

		// calc order price as a close price + offset
		var price = candle.ClosePrice + (direction == Sides.Buy ? priceStep : -priceStep);

		if (direction == Sides.Buy)
			BuyLimit(price, volume);
		else
			SellLimit(price, volume);

		// store current values for short and long
		_isShortLessThenLong = isShortLessThenLong;
	}
}
```
