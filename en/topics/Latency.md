# Latency

In order to estimate the speed of orders registration and to determine which broker or technology faster, the [S\#](StockSharpAbout.md) includes a mechanism of calculating the time difference between an order creation in the trading algorithm and its registration on the exchange (the same for cancelation).

For the delay calculation it is necessary to use the [ILatencyManager](../api/StockSharp.Algo.Latency.ILatencyManager.html) implementation, by way of [LatencyManager](../api/StockSharp.Algo.Latency.LatencyManager.html).

### Prerequisites

[Strategies](Strategy.md)

### The latency calculation adding to the SampleSMA

The latency calculation adding to the SampleSMA

1. You should add the text box for the total delay display in the information output window:

   ```cs
   <Label Grid.Column="0" Grid.Row="6" Content="Latency:" />
   <Label x:Name="Latency" Grid.Column="1" Grid.Row="6" />
   						
   ```
2. Next, you need to extend the event method\-handler of the strategy parameter change:

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

### Next Steps

[Commission](Commissions.md)
