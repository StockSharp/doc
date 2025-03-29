# Использование правил

## Создание правил

- **Создание правила на условие регистрации заявки:**

  ```cs
  private void btnBuy_Click(object sender, RoutedEventArgs e)
  {
     var order = new Order
     { 
         Portfolio = Portfolio.SelectedPortfolio,
         Price = _instr1.BestAsk.Price,
         Security = _instr1,
         Volume = 1,
         Direction = Sides.Buy,
     };
     order
         .WhenRegistered(Connector)
         .Do(() => Connector.AddInfoLog("Заявка успешно зарегистрирована"))
         .Once()
         .Apply(this);
      
  	// регистрация заявки
     Connector.RegisterOrder(order);
  }
  ```

  Теперь, когда сработает событие (заявка будет зарегистрирована на бирже), указанное через метод [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** действие будет вызвано. 

  В конце формирования правила вызывается метод [MarketRuleHelper.Apply](xref:StockSharp.Algo.MarketRuleHelper.Apply(StockSharp.Algo.IMarketRule))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule **)**. До тех пор, пока метод не будет вызван для правила - оно неактивно (обработчик в [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** не будет вызываться). 
  
- **Создание правил внутри стратегии:**

  ```cs
  class FirstStrategy : Strategy
  {
      protected override void OnStarted(DateTimeOffset time)
      {
          // Подписка на свечи
          var candleSubscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
          this
              .WhenCandlesStarted(candleSubscription)
              .Do(ProcessCandle)
              .Apply(this);
              
          // Подписка на тиковые сделки
          var tickSubscription = new Subscription(DataType.Ticks, Security);
          tickSubscription
              .WhenTickTradeReceived(this)
              .Do(ProcessTick)
              .Apply(this);
              
          // Отправляем запросы на подписку
          Subscribe(candleSubscription);
          Subscribe(tickSubscription);
              
          base.OnStarted(time);
      }
      
      // Методы для обработки событий
      private void ProcessCandle(ICandleMessage candle) { /* ... */ }
      private void ProcessTick(ITickTradeMessage tick) { /* ... */ }
  }    
  ```
  
- **Удаление ненужных правил.**

  У [IMarketRule](xref:StockSharp.Algo.IMarketRule) есть [IMarketRule.Token](xref:StockSharp.Algo.IMarketRule.Token) - токен правила, с которым он ассоциирован. Например, для правила [MarketRuleHelper.WhenCanceled](xref:StockSharp.Algo.MarketRuleHelper.WhenCanceled(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.ITransactionProvider))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order, [StockSharp.BusinessEntities.ITransactionProvider](xref:StockSharp.BusinessEntities.ITransactionProvider) provider **)** токеном будет являться заявка.

  Когда сработало правило успешной отмены заявки, то лучше удалить все остальные правила, связанные с этой заявкой:

  ```cs
  var order = this.CreateOrder(direction, (decimal)Security.GetCurrentPrice(direction), Volume);
  var ruleCanceled = order.WhenCanceled(Connector);
  ruleCanceled
      .Do(() =>
      {
          this.AddInfoLog("Заявка успешно отменена");
          // удаление всех правил связанных с order
          Rules.RemoveRulesByToken(ruleCanceled, (IMarketRule)ruleCanceled.Token);
      })
      .Once()
      .Apply(this);
  order
      .WhenRegistered(Connector)
      .Do(() => this.AddInfoLog("Заявка успешно зарегистрирована"))
      .Once()
      .Apply(this);
  order
      .WhenRegisterFailed(Connector)
      .Do(() => this.AddInfoLog("Заявка не принята биржей"))
      .Once()
      .Apply(this);
  order
      .WhenMatched(Connector)
      .Do(() => this.AddInfoLog("Заявка полностью исполнена"))
      .Once()
      .Apply(this);
  // регистрирация заявки
  RegisterOrder(order);
  ```
  
- **Объединение правил по условию [MarketRuleHelper.Or](xref:StockSharp.Algo.MarketRuleHelper.Or(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)** / [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)**.**

  Когда выйдет время **ИЛИ** закроется свеча:

  ```cs
  // Создаем подписку на свечи
  var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
  var timeInterval = TimeSpan.FromMilliseconds(5000);
  
  Connector
      .WhenIntervalElapsed(timeInterval)
      .Or(this.WhenCandlesStarted(subscription))
      .Do(() => this.AddInfoLog("Свеча закрыта или вышло время"))
      .Once()
      .Apply(this);
      
  // Отправляем запрос на подписку
  Subscribe(subscription);
  ```

  Или такой формат записи:

  ```cs
  // Создаем подписку на свечи
  var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
  var timeInterval = TimeSpan.FromMilliseconds(5000);
  
  MarketRuleHelper
      .Or(new IMarketRule[] {
          Connector.WhenIntervalElapsed(timeInterval), 
          this.WhenCandlesStarted(subscription)
      })
      .Do(() => this.AddInfoLog("Свеча закрыта или вышло время"))
      .Once()
      .Apply(this);
      
  // Отправляем запрос на подписку
  Subscribe(subscription);
  ```

  Когда цена последней сделки будет выше 135000 **И** ниже 140000:

  ```cs
  // Создаем подписку на тиковые сделки
  var subscription = new Subscription(DataType.Ticks, Security);
  var priceMore = new Unit(135000m, UnitTypes.Limit);
  var priceLess = new Unit(140000m, UnitTypes.Limit);
  				
  MarketRuleHelper
      .And(new IMarketRule[] {
          subscription.WhenLastTradePriceMore(this, 135000m), 
          subscription.WhenLastTradePriceLess(this, 140000m)
      })
      .Do(() => this.AddInfoLog($"Цена последней сделки находится в диапазоне от {priceMore} до {priceLess}"))
      .Apply(this);
      
  // Отправляем запрос на подписку
  Subscribe(subscription);
  ```

  > [!TIP]
  > Обработчик в [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** вызовется после того, как сработает последнее правило добавленное через [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)**.
  
- **Периодичность работы правила - [IMarketRule.Until](xref:StockSharp.Algo.IMarketRule.Until(System.Func{System.Boolean}))**(**[System.Func\<System.Boolean\>](xref:System.Func`1) canFinish **)**:**

  ```cs
  bool flag = false;
  
  // Создаем подписку на тиковые сделки
  var subscription = new Subscription(DataType.Ticks, Security);
  				
  subscription
      .WhenTickTradeReceived(this)
      .Do((tick) =>
      {
          if(условие) flag = true;
      })
      .Until(() => flag)			
      .Apply(this);
      
  // Отправляем запрос на подписку
  Subscribe(subscription);
  ```

## Примеры использования правил

### Правила на свечи

```cs
// Создаем подписку на 5-минутные свечи
var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);

// Переменная для подсчета свечей
var i = 0;
var diff = "10%".ToUnit();

// Правило, которое активируется при начале новой свечи
this.WhenCandlesStarted(subscription)
    .Do((candle) =>
    {
        i++;

        // Вложенное правило: проверка, когда общий объем превышает порог
        this
            .WhenTotalVolumeMore(candle, diff)
            .Do((candle1) =>
            {
                LogInfo($"Правило WhenCandlesStarted и WhenTotalVolumeMore candle={candle1}");
                LogInfo($"Правило WhenCandlesStarted и WhenTotalVolumeMore i={i}");
            })
            .Once().Apply(this);

    }).Apply(this);
    
// Отправляем запрос на подписку
Subscribe(subscription);
```

### Правила на стаканы (глубину рынка)

```cs
// Подписка на данные стакана
var mdSub = new Subscription(DataType.MarketDepth, Security);

// Метод 1: Создание правила в цепочке
mdSub.WhenOrderBookReceived(this).Do((depth) =>
{
    LogInfo($"Правило WhenOrderBookReceived №1 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
}).Once().Apply(this);

// Метод 2: Сначала создаем переменную правила
var whenMarketDepthChanged = mdSub.WhenOrderBookReceived(this);

whenMarketDepthChanged.Do((depth) =>
{
    LogInfo($"Правило WhenOrderBookReceived №2 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
}).Once().Apply(this);

// Правило внутри правила
mdSub.WhenOrderBookReceived(this).Do((depth) =>
{
    LogInfo($"Правило WhenOrderBookReceived №3 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");

    // Правило без указания Once()
    mdSub.WhenOrderBookReceived(this).Do((depth1) =>
    {
        LogInfo($"Правило WhenOrderBookReceived №4 BestBid={depth1.GetBestBid()}, BestAsk={depth1.GetBestAsk()}");
    }).Apply(this);
}).Once().Apply(this);

// Отправляем запрос на подписку
Subscribe(mdSub);
```

### Правила с условием завершения

```cs
// Подписка на данные стакана
var mdSub = new Subscription(DataType.MarketDepth, Security);

// Счетчик
var i = 0;

// Создаем правило, которое обрабатывает стаканы до тех пор, пока i не достигнет 10
mdSub.WhenOrderBookReceived(this).Do(depth =>
{
    i++;
    LogInfo($"Правило WhenOrderBookReceived BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
    LogInfo($"Правило WhenOrderBookReceived i={i}");
})
.Until(() => i >= 10)
.Apply(this);

// Отправляем запрос на подписку
Subscribe(mdSub);
```

### Правила на заявки

```cs
// Подписка на тиковые сделки
var sub = new Subscription(DataType.Ticks, Security);

// Когда получим первый тик, создадим заявку
sub.WhenTickTradeReceived(this).Do(() =>
{
    var order = CreateOrder(Sides.Buy, default, 1);

    var ruleReg = order.WhenRegistered(this);
    var ruleRegFailed = order.WhenRegisterFailed(this);

    ruleReg
        .Do(() => LogInfo("Заявка №1 зарегистрирована"))
        .Once()
        .Apply(this)
        .Exclusive(ruleRegFailed);  // Правила взаимоисключающие

    ruleRegFailed
        .Do(() => LogInfo("Заявка №1 не зарегистрирована"))
        .Once()
        .Apply(this)
        .Exclusive(ruleReg);  // Правила взаимоисключающие

    RegisterOrder(order);
}).Once().Apply(this);

// Отправляем запрос на подписку
Subscribe(sub);
```

### Правила на изменение цены

```cs
// Подписка на тиковые сделки
var sub = new Subscription(DataType.Ticks, Security);

// Правило активируется при первом тике и создает другое правило
sub.WhenTickTradeReceived(this).Do(t =>
{
    // Создаем правило, которое активируется при движении цены на 2 пункта в любом направлении
    sub
        .WhenLastTradePriceMore(this, t.Price + 2)
        .Or(sub.WhenLastTradePriceLess(this, t.Price - 2))
        .Do(t =>
        {
            LogInfo($"Сработало правило WhenLastTradePriceMore или WhenLastTradePriceLess: tick={t}");
        })
        .Apply(this);
})
.Once() // вызвать это правило только один раз
.Apply(this);

// Отправляем запрос на подписку
Subscribe(sub);
```