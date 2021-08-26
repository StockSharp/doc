# Пример стратегии на C\#

Для создания стратегий из исходного кода необходимы навыки программирования на языке C\#, а также знания библиотеки для профессиональной разработки торговых роботов на языке C\# [S\#.API](StockSharpAbout.md).

Создание стратегии из исходного кода будет рассмотрено на примере стратегии SMA \- аналогичный пример\-стратегии SMA, собранной из кубиков в пункте [Создание алгоритма из кубиков](Designer_Algorithm_creation_of_elements.md).

Первой строкой идет наименование namespace. Может быть любым сочетанием латинских букв, по умолчанию StockSharp.Designer.Strategies

Далее идет перечисление Dll библиотек, необходимых для работы кода кубика:

```cs
sing System.Windows.Media;
using System.Runtime.InteropServices;
using Ecng.Common;
using Ecng.ComponentModel;
using Ecng.Collections;
using StockSharp.Messages;
using StockSharp.Algo;
```

Dll библиотеки добавляются нажатием кнопки **Ссылки** вкладки **Исходный код**.

Далее объявляется класс. Название класса может быть любым сочетанием латинских букв. Класс кубика должен быть унаследован от класса [Strategy](../api/StockSharp.Algo.Strategies.Strategy.html):

```cs
	public class NewStrategy : Strategy
```

Далее в примере идет объявление переменных. Для работы стратегии SMA необходимы две скользящие средние средние с разными периодами расчёта, длинная SMA и короткая SMA:

```cs
 private readonly SimpleMovingAverage \_long;
 private readonly SimpleMovingAverage \_short;
```

Объявляются параметры стратегии, в них будут храниться параметры индикаторов SMA:

```cs
 private readonly StrategyParam\<int\> \_longParam;
 private readonly StrategyParam\<int\> \_shortParam;
```

Так же объявляются переменные, необходимые для отображения графических элементов. Они не несут полезной нагрузки, и показаны в примере как демонстрация возможностей. Для отображения графических элементов лучше использовать стандартные кубики [S\#.Designer](Designer.md). Как это сделать показано в пункте [Комбинирование C\# и стандартных кубиков](Designer_Combine_Source_code_and_standard_elements.md):

```cs
 private readonly List\<MyTrade\> \_myTrades \= new List\<MyTrade\>();
 private readonly ChartCandleElement \_candlesElem;\/\/ Свечи
 private readonly ChartTradeElement \_tradesElem;\/\/ Сделки
 private readonly ChartIndicatorElement \_shortElem;\/\/ Индикатор
 private readonly ChartIndicatorElement \_longElem; \/\/ Индикатор
 private readonly ChartArea \_area \= new ChartArea();\/\/ Панель графиков
```

Атрибут [DiagramExternalAttribute](../api/StockSharp.Xaml.Diagram.Elements.DiagramExternalAttribute.html) необходим для обозначения входных и выходных параметров кубика. Если атрибутом обозначено событие, то значит это будет выходной параметр, если метод — значит это входной параметр:

```cs
 \[DiagramExternal\]
 public event Action\<Order\> NewMyOrder;
```
```cs
 \[DiagramExternal\]
 public void ProcessCandle(Candle candle)
```

После объявления переменных необходимо задать свойства кубика. Эти свойства будут отображаться в панели **Свойства** на общей схеме:

![Designer Creating a strategy from the source code 00](~/images/Designer_Creating_strategy_from_source_code_00.png)

```cs
public int Long
{
    get { return \_longParam.Value; }
    set
    {
        \_longParam.Value \= value;
        \_long.Length \= value;
    }
}
public int Short
{
    get { return \_shortParam.Value; }
    set
    {
        \_shortParam.Value \= value;
        \_short.Length \= value;
    }
}
```

Инициализация переменных происходит в конструкторе класса:

```cs
public NewStrategy()
{
    \_longParam \= new StrategyParam\<int\>(this, nameof(Long), 80);
    \_shortParam \= new StrategyParam\<int\>(this, nameof(Short), 20);
    \_long \= new SimpleMovingAverage { Length \= Long };
    \_short \= new SimpleMovingAverage { Length \= Short };
    \/\/Инициализация графических элементов
    \_candlesElem \= new ChartCandleElement { ShowAxisMarker \= false };
    \_tradesElem \= new ChartTradeElement { FullTitle \= LocalizedStrings.Str985 };
    \_shortElem \= new ChartIndicatorElement
    {
        Color \= Colors.Coral,
        ShowAxisMarker \= false,
        FullTitle \= \_short.ToString()
    };
    \_longElem \= new ChartIndicatorElement
    {
        ShowAxisMarker \= false,
        FullTitle \= \_long.ToString()
    };
}
```

У класса [Strategy](../api/StockSharp.Algo.Strategies.Strategy.html) есть методы, которые можно переопределить. В примере переопределяется метод [OnReseted](../api/StockSharp.Algo.Strategies.Strategy.OnReseted.html) для того, чтобы при переинициализации торгового алгоритма переинициализировались скользящие средние и графические элементы:

```cs
protected override void OnReseted()
{
    this.AddInfoLog("OnReseted");
    \_long.Reset();
    \_short.Reset();
    var chart \= this.GetChart();
    if (chart \!\= null)
    {
        foreach (var element in \_area.Elements.ToArray())
        {
            if (\_area.Elements.Contains(element))
                chart.RemoveElement(\_area, element);
        }
        if (chart.Areas.Contains(\_area))
            chart.RemoveArea(\_area);
    }
    base.OnReseted();
}
```

Переопределяется метод [OnStarted](../api/StockSharp.Algo.Strategies.Strategy.OnStarted.html) для того, чтобы: 1) при старте торгового алгоритма переинициализировались скользящие средние, 2) на график добавились все графические элементы, 3) подписаться на появление новых сделок, 4) подписаться на появление новых заявок или изменение старых, начать получать новую информацию по инструменту.

```cs
protected override void OnStarted()
{
    this.AddInfoLog("OnStarted");
    \/\/ переинициализация скользящих средних
    \_long.Reset();
    \_short.Reset();
    \/\/ добавление на график графических элементов
    var chart \= this.GetChart();
    if (\!chart.Areas.Contains(\_area))
    {
        chart.AddArea(\_area);
        chart.AddElement(\_area, \_candlesElem);
        chart.AddElement(\_area, \_tradesElem);
        chart.AddElement(\_area, \_shortElem);
        chart.AddElement(\_area, \_longElem);
    }
    \/\/ подписка на появление новых сделок, необходимо для отображения сделок
    this
    .WhenNewMyTrades()
    .Do(trades \=\> \_myTrades.AddRange(trades))
    .Apply(this);
    \/\/ подписка на изменения заявок
    this
    .WhenOrderRegistered()
    .Or(this.WhenOrderChanged())
    .Do(ord \=\> NewMyOrder?.Invoke(ord))
    .Apply(this);
    \/\/ начать получать новую информацию
    Connector.SubscribeLevel1(Security);
    base.OnStarted();
}
```

Переопределяется метод [OnStopped](../api/StockSharp.Algo.Strategies.Strategy.OnStopped.html), чтобы перестать получать новую информацию по инструменту:

```cs
protected override void OnStopped()
{
    this.AddInfoLog("OnStopped");
    Connector.UnSubscribeLevel1(Security);
    base.OnStopped();
}
```

Строка **this.AddInfoLog("OnStopped");** выводит сообщение "OnStopped" в окно **Логи**.

В методе ProcessCandle(Candle candle) идет основной расчёт стратегии. Так как он обозначен атрибутом [DiagramExternalAttribute](../api/StockSharp.Xaml.Diagram.Elements.DiagramExternalAttribute.html), значит это входной параметр кубика, принимающий **Свечи**.

```cs
\[DiagramExternal\]
public void ProcessCandle(Candle candle)
{
    \/\/ Запущена или остановлена стратегия
    if (ProcessState \=\= ProcessStates.Stopping)
    {
        CancelActiveOrders();
        return;
    }
    this.AddInfoLog(LocalizedStrings.Str3634Params.Put(candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.Security));
    \/\/ Расчет скользящих средних
    var longValue \= \_long.Process(candle);
    var shortValue \= \_short.Process(candle);
    var isShortLessThenLong \= \_short.GetCurrentValue() \< \_long.GetCurrentValue();
    \/\/ пересекла ли короткая SMA длинную SMA
    if (\_isShortLessThenLong \!\= isShortLessThenLong)
    {
        \/\/ определение направление для заявки
        var direction \= isShortLessThenLong ? Sides.Sell : Sides.Buy;
        \/\/ объёма для заявки
        var volume \= Position \=\= 0 ? Volume : Position.Abs().Min(Volume) \* 2;
        \/\/ расчет цены для заявки
        var price \= candle.ClosePrice + ((direction \=\= Sides.Buy ? Security.PriceStep : \-Security.PriceStep) ?? 1);
        \/\/выставление заявки
        RegisterOrder(this.CreateOrder(direction, price, volume));
        \_isShortLessThenLong \= isShortLessThenLong;
    }
    \/\/отрисовка графических элементов
    var trade \= \_myTrades.FirstOrDefault();
    \_myTrades.Clear();
    var data \= new ChartDrawData();
    data
      .Group(candle.OpenTime)
        .Add(\_candlesElem, candle)
        .Add(\_shortElem, shortValue)
        .Add(\_longElem, longValue)
        .Add(\_tradesElem, trade);
    this.GetChart().Draw(data);
}
```

## См. также

[Интеграция C\# кода на общую схему](Designer_Integration_Source_code_in_scheme.md)
