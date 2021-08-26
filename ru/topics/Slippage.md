# Проскальзывание

В [S\#](StockSharpAbout.md) входит механизм подсчета проскальзывания, который позволяет оценить быстроту алгоритма (и его реализацию),путем мониторинга цены первоначальной заявки и последующих сделок.

Учет проскальзывания ведется через специальный менеджер. Базовый интерфейс менеджера проскальзывания называется [ISlippageManager](../api/StockSharp.Algo.Slippage.ISlippageManager.html). Данный интерфейс имеет реализацию в виде [SlippageManager](../api/StockSharp.Algo.Slippage.SlippageManager.html). В классе подключений к торговым системам [Connector](../api/StockSharp.Algo.Connector.html) имеется свойство [Connector.SlippageManager](../api/StockSharp.Algo.Connector.SlippageManager.html), которое можно использовать для расчета проскальзывания. 

В стратегиях [Strategy](../api/StockSharp.Algo.Strategies.Strategy.html) используется собственный механизм расчета проскальзывания. В этом случае величину проскальзывания можно получить через свойство [Strategy.Slippage](../api/StockSharp.Algo.Strategies.Strategy.Slippage.html).

### Предварительные условия

[Стратегии](Strategy.md)

[Котирование](StrategyQuoting.md)

### Добавление в SampleSMA учета проскальзывания

Добавление в SampleSMA учета проскальзывания

1. Так как SampleSMA использует механизм котирования, то в этом алгоритме необходимо учитывать проскальзывание.

   В окно вывода информации необходимо добавить текстовое поле для проскальзывания:

   ```cs
   \<Label Grid.Column\="0" Grid.Row\="4" Content\="Проскаль.:" \/\>
   \<Label x:Name\="Slippage" Grid.Column\="1" Grid.Row\="4" \/\>
   						
   ```
2. Далее, необходимо расширить метод\-обработчик события изменения параметров стратегии:

   ```cs
   private void OnStrategyPropertyChanged(object sender, PropertyChangedEventArgs e)
   {
      this.GuiAsync(() \=\>
      {
         	Status.Content \= \_strategy.ProcessState;
       	Slippage.Content \= \_strategy.Slippage;
      });
   }
   						
   ```

### Следующие шаги

[Прибыль\-убыток](PnL.md)
