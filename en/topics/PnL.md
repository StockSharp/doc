# Profit\-loss

For the total profit\-loss calculation (P&L) in a trading algorithm it is necessary to use the [IPnLManager](xref:StockSharp.Algo.PnL.IPnLManager) interface implementation, by way of [PnLManager](xref:StockSharp.Algo.PnL.PnLManager).

## Prerequisites

[Strategies](Strategy.md)

## The profit\-loss calculation adding to the SampleSMA

1. You should add the text box for the P&L in the information output window:

   ```cs
   <Label Grid.Column="0" Grid.Row="3" Content="P&amp;L:" />
   <Label x:Name="PnL" Grid.Column="1" Grid.Row="3" />
   						
   ```
2. Next, you need to extend the event method\-handler of the strategy parameter change:

   ```cs
   this.GuiAsync(() =>
   {
   	Status.Content = _strategy.ProcessState;
   	PnL.Content = _strategy.PnL;
   	Slippage.Content = _strategy.Slippage;
   });
   						
   ```

## Next Steps

[Position](Position.md)
