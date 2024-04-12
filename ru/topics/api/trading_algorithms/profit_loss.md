# Прибыль\-убыток

Для учета общей прибыли\-убытка (P&L) в торговом роботе необходимо использовать реализацию интерфейса [IPnLManager](xref:StockSharp.Algo.PnL.IPnLManager), в виде [PnLManager](xref:StockSharp.Algo.PnL.PnLManager).

## Предварительные условия

[Стратегии](../strategies.md)

## Добавление в SampleSMA учета прибыли\-убытка

1. В окно вывода информации необходимо добавить текстовое поле для P&L:

   ```cs
   <Label Grid.Column="0" Grid.Row="3" Content="P&amp;L:" />
   <Label x:Name="PnL" Grid.Column="1" Grid.Row="3" />
   						
   ```
2. Далее, необходимо расширить метод\-обработчик события изменения параметров стратегии:

   ```cs
   this.GuiAsync(() =>
   {
   	Status.Content = _strategy.ProcessState;
   	PnL.Content = _strategy.PnL;
   	Slippage.Content = _strategy.Slippage;
   });
   						
   ```

## Следующие шаги

[Позиция](position.md)
