# Slippage

The [S\#](StockSharpAbout.md) includes the mechanism of the slippage calculation that allows to estimate the speed of the algorithm reaction (and its implementation) by monitoring the price of the initial order and following trades.

The slippage tracking is carried out through a special manager. The basic interface of the slippage manager is the [ISlippageManager](../api/StockSharp.Algo.Slippage.ISlippageManager.html). This interface has the implementation in form of [SlippageManager](../api/StockSharp.Algo.Slippage.SlippageManager.html). The connections to the trading systems class [Connector](../api/StockSharp.Algo.Connector.html) has the [Connector.SlippageManager](../api/StockSharp.Algo.Connector.SlippageManager.html) property, which can be used to calculate the slippage. 

[Strategy](../api/StockSharp.Algo.Strategies.Strategy.html) strategies use their own mechanism for slippage calculation. In this case, the value of slippage can be obtained through the [Strategy.Slippage](../api/StockSharp.Algo.Strategies.Strategy.Slippage.html) property.

### Prerequisites

[Strategies](Strategy.md)

[Quoting](StrategyQuoting.md)

### The slippage calculation adding to the SampleSMA

The slippage calculation adding to the SampleSMA

1. Since the SampleSMA uses the quoting mechanism, then the slippage must take into account in this algorithm.

   The text box for the slippage should be added to the information output window:

   ```cs
   <Label Grid.Column="0" Grid.Row="4" Content="Slippage:" />
   <Label x:Name="Slippage" Grid.Column="1" Grid.Row="4" />
   						
   ```
2. Next, you need to extend the event method\-handler of the strategy parameter change:

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

### Next Steps

[Profit\-loss](PnL.md)
