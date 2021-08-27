# Позиция

Для учета позиции необходимо использовать реализацию интерфейса [IPositionManager](xref:StockSharp.Algo.Positions.IPositionManager), в виде [PositionManager](xref:StockSharp.Algo.Positions.PositionManager).

### Предварительные условия

[Стратегии](Strategy.md)

### Добавление в SampleSMA учет позиции

Добавление в SampleSMA учет позиции

1. В окно вывода информации необходимо добавить текстовое поле для вывода текущей позиции:

   ```cs
   <Label Grid.Column="0" Grid.Row="5" Content="Поза:" />
   <Label x:Name="Position" Grid.Column="1" Grid.Row="5" />
   						
   ```
2. Далее, необходимо расширить метод\-обработчик события изменения параметров стратегии:

   ```cs
   this.GuiAsync(() =>
   {
   	Status.Content = _strategy.ProcessState;
   	PnL.Content = _strategy.PnL;
   	Slippage.Content = _strategy.Slippage;
   	Position.Content = _strategy.Position;
   });
   						
   ```

### Следующие шаги

[Задержка](Latency.md)
