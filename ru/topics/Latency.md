# Задержка

Для того, чтобы оценить скорость регистрации заявок, а также определить, какой брокер или технология быстрее, в [S\#](StockSharpAbout.md) входит механизм расчета разницы времени между созданием заявки в торговом роботе и регистрацией на бирже.

Для учета задержки необходимо использовать реализацию интерфейса [ILatencyManager](xref:StockSharp.Algo.Latency.ILatencyManager), в виде [LatencyManager](xref:StockSharp.Algo.Latency.LatencyManager).

### Предварительные условия

[Стратегии](Strategy.md)

### Добавление в SampleSMA учет задержки

1. В окно вывода информации необходимо добавить текстовое поле для вывода общей задержки:

   ```cs
   <Label Grid.Column="0" Grid.Row="6" Content="Задержка:" />
   <Label x:Name="Latency" Grid.Column="1" Grid.Row="6" />
   						
   ```
2. Далее, необходимо расширить метод\-обработчик события изменения параметров стратегии:

   ```cs
   this.GuiAsync(() =>
   {
   	Status.Content = _strategy.ProcessState;
   	PnL.Content = _strategy.PnL;
   	Slippage.Content = _strategy.Slippage;
   	Position.Content = _strategy.Position;
   	Latency.Content = _strategy.Latency;
   });
   						
   ```

### Следующие шаги

[Комиссия](Commissions.md)
