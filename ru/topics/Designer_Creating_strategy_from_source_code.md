# Пример стратегии на C\#

Создание стратегии из исходного кода будет рассмотрено на примере стратегии SMA \- аналогичный пример\-стратегии SMA, собранной из кубиков в пункте [Создание алгоритма из кубиков](Designer_Algorithm_creation_of_elements.md).

Данный раздел не будет описывать конструкции C# языка (как и [Strategy](Strategy.md), на базе чего создаются стратегии), но будут упомянуты особенности для работы кода в **Дизайнере**.

> [!TIP]
> Стратегии, создающиеся в **Дизайнере** совместимы со стратегиями, создающиеся в [API](StockSharpAbout.md) за счет использования общего базового класса [Strategy](Strategy.md). Это делает возможность запуска таких стратегий вне **Дизайнера** значительно проще, чем [схем](Designer_run_strategy_on_server.md).

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

2. При создании индикаторов необходимо добавлять их во внутренний список стратегии ([Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators)):

```cs
_longSma = new SimpleMovingAverage { Length = Long };
_shortSma = new SimpleMovingAverage { Length = Short };

// !!! DO NOT FORGET add it in case use IsFormed property (see code below)
Indicators.Add(_longSma);
Indicators.Add(_shortSma);
```

Это делается для того, чтобы стратегия могла отслеживать все необходимые для ее логики работы индикаторы при переходе их в состояние [сформирован](Indicators.md).

3. При работе с графиком необходимо учитывать, что в случае запуска стратегии [вне Дизайнера](Designer_run_strategy_on_server.md) объект графика может быть отсутствовать.

```cs
_chart = this.GetChart();

// chart can be NULL in case hosting strategy in custom app like Runner or Shell
if (_chart != null)
{
	var area = _chart.AddArea();

	_chartCandlesElem = area.AddCandles();
	_chartTradesElem = area.AddTrades();
	_chartShortElem = area.AddIndicator(_shortSma);
	_chartLongElem = area.AddIndicator(_longSma);

	// you can apply custom color palette here
}
```

И далее в коде отрисовки данных на графике делать специальные проверки вида:

```cs
if (_chart == null)
	return;

var data = _chart.CreateData();
```

4. Чтобы удостоверится что стратегия полностью готова к работе, ее индикаторы сформированы историческими данными и все подписки перешли в [Онлайн](API_ConnectorsSubscriptions.md), можно использовать специальный метод:

```cs
// some of indicators added in OnStarted not yet fully formed
// or user turned off allow trading
if (this.IsFormedAndOnlineAndAllowTrading())
{
	// TODO
}
```
