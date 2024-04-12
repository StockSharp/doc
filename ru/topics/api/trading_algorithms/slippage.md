# Проскальзывание

В [S\#](../../api.md) входит механизм подсчета проскальзывания, который позволяет оценить быстроту алгоритма (и его реализацию),путем мониторинга цены первоначальной заявки и последующих сделок.

Учет проскальзывания ведется через специальный менеджер. Базовый интерфейс менеджера проскальзывания называется [ISlippageManager](xref:StockSharp.Algo.Slippage.ISlippageManager). Данный интерфейс имеет реализацию в виде [SlippageManager](xref:StockSharp.Algo.Slippage.SlippageManager). В классе подключений к торговым системам [Connector](xref:StockSharp.Algo.Connector) имеется свойство [Connector.SlippageManager](xref:StockSharp.Algo.Connector.SlippageManager), которое можно использовать для расчета проскальзывания. 

В стратегиях [Strategy](xref:StockSharp.Algo.Strategies.Strategy) используется собственный механизм расчета проскальзывания. В этом случае величину проскальзывания можно получить через свойство [Strategy.Slippage](xref:StockSharp.Algo.Strategies.Strategy.Slippage).

## Предварительные условия

[Стратегии](../strategies.md)

[Котирование](../strategies/quoting.md)

## Добавление в SampleSMA учета проскальзывания

1. Так как SampleSMA использует механизм котирования, то в этом алгоритме необходимо учитывать проскальзывание.

   В окно вывода информации необходимо добавить текстовое поле для проскальзывания:

   ```cs
   <Label Grid.Column="0" Grid.Row="4" Content="Проскаль.:" />
   <Label x:Name="Slippage" Grid.Column="1" Grid.Row="4" />
   						
   ```
2. Далее, необходимо расширить метод\-обработчик события изменения параметров стратегии:

   ```cs
   private void OnStrategyPropertyChanged(object sender, PropertyChangedEventArgs e)
   {
      this.GuiAsync(() =>
      {
         	Status.Content = _strategy.ProcessState;
       	Slippage.Content = _strategy.Slippage;
      });
   }
   						
   ```

## Следующие шаги

[Прибыль\-убыток](profit_loss.md)
