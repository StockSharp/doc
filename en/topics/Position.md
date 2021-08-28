# Position

For the position calculation it is necessary to use the [IPositionManager](xref:StockSharp.Algo.Positions.IPositionManager) interface implementation, by way of [PositionManager](xref:StockSharp.Algo.Positions.PositionManager).

### Prerequisites

[Strategies](Strategy.md)

### The position calculation adding to the SampleSMA

1. You should add the text box for the current position display in the information output window:

   ```cs
   <Label Grid.Column="0" Grid.Row="5" Content="Pos:" />
   <Label x:Name="Position" Grid.Column="1" Grid.Row="5" />
   						
   ```
2. Next, you need to extend the event method\-handler of the strategy parameter change:

   ```cs
   this.GuiAsync(() =>
   {
   	Status.Content = _strategy.ProcessState;
   	PnL.Content = _strategy.PnL;
   	Slippage.Content = _strategy.Slippage;
   	Position.Content = _strategy.Position;
   });
   						
   ```

### Next Steps

[Latency](Latency.md)
