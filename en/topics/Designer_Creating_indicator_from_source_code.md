# Create own indicator using C\#

Creating your own indicator in [S\#.API](StockSharpAbout.md) is described in [Custom indicator](IndicatorsCustom.md) section.

Below, using the SMA indicator as an example, you will see how to integrate the created indicator into a schema in the form of a cube. As an example, you can take the source codes of other indicators that are in the [GitHub\/StockSharp](https://github.com/StockSharp/StockSharp) repository.

First, you need to create a cube as described in [Getting started](Designer_Creation_element_containing_source_code.md) section.

We describe the input and output parameters of the cube:

```cs
		[DiagramExternal]
		public void ProcessCandle(Candle candle)
		{
            //...
		}
```
```cs
		[DiagramExternal]
		public event Action<IIndicatorValue> NewIndicator;
```

Let's describe the indicator as a separate class:

```cs
    /// <summary>
	/// Simple moving average.
	/// </summary>
	[DisplayName("SMA")]
	public class NewSimpleMovingAverage : LengthIndicator<decimal>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleMovingAverage"/>.
		/// </summary>
		public NewSimpleMovingAverage()
		{
			Length = 32;
		}
		/// <summary>
		/// To handle the input value.
		/// </summary>
		/// <param name="input">The input value.</param>
		/// <returns>The resulting value.</returns>
		protected override IIndicatorValue OnProcess(IIndicatorValue input)
		{
			var newValue = input.GetValue<decimal>();
			if (input.IsFinal)
			{
				Buffer.Add(newValue);
				if (Buffer.Count > Length) Buffer.RemoveAt(0);
			}
			if (input.IsFinal) return new DecimalIndicatorValue(this, Buffer.Sum() / Length);
			return new DecimalIndicatorValue(this, (Buffer.Skip(1).Sum() + newValue) / Length);
		}
	}
```

We add the indicator to the cube code and initialize it in the cube class constructor:

```cs
		private SimpleMovingAverage _NewSMA;
		public NewSMA ()
		{
			_NewSMA = new SimpleMovingAverage();
		}
```

The SMA indicator has a single property, it is the period length. We describe this property in the cube code:

```cs
		public int Length
		{
			get { return _NewSMA.Length; }
			set { _NewSMA.Length = value;}
		}
```

We add the **ProcessCandle** method so that it calculates the indicator value and generates an event of the output parameter:

```cs
		[DiagramExternal]
		public void ProcessCandle(Candle candle)
		{
			NewIndicator?.Invoke(_NewSMA.Process(candle));
		}
```

The complete cube code:

```cs
namespace StockSharp.Designer.Strategies
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Windows.Media;
	using System.Runtime.InteropServices;
	using Ecng.Common;
	using Ecng.ComponentModel;
	using Ecng.Collections;
	using MoreLinq;
	using StockSharp.Messages;
	using StockSharp.Algo;
	using StockSharp.Algo.Candles;
	using StockSharp.Algo.Strategies;
	using StockSharp.Algo.Indicators;
	using StockSharp.Logging;
	using StockSharp.BusinessEntities;
	using StockSharp.Localization;
	using StockSharp.Xaml;
	using StockSharp.Xaml.Charting;
	using StockSharp.Xaml.Diagram.Elements;
	[Guid("eea2da25-de12-4b6c-b43a-6a98e2fdb01c")]
	public class NewSMA : Strategy
	{
		private SimpleMovingAverage _NewSMA;
		public NewSMA ()
		{
			_NewSMA = new SimpleMovingAverage();
		}
		[DiagramExternal]
		public event Action<IIndicatorValue> NewIndicator;
		public int Length
		{
			get { return _NewSMA.Length; }
			set { _NewSMA.Length = value;}
		}
		[DiagramExternal]
		public void ProcessCandle(Candle candle)
		{
			NewIndicator?.Invoke(_NewSMA.Process(candle));
		}
	}
	
	/// <summary>
	/// Simple moving average.
	/// </summary>
	[DisplayName("SMA")]
	public class NewSimpleMovingAverage : LengthIndicator<decimal>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleMovingAverage"/>.
		/// </summary>
		public NewSimpleMovingAverage()
		{
			Length = 32;
		}
		/// <summary>
		/// To handle the input value.
		/// </summary>
		/// <param name="input">The input value.</param>
		/// <returns>The resulting value.</returns>
		protected override IIndicatorValue OnProcess(IIndicatorValue input)
		{
			var newValue = input.GetValue<decimal>();
			if (input.IsFinal)
			{
				Buffer.Add(newValue);
				if (Buffer.Count > Length) Buffer.RemoveAt(0);
			}
			if (input.IsFinal) return new DecimalIndicatorValue(this, Buffer.Sum() / Length);
			return new DecimalIndicatorValue(this, (Buffer.Skip(1).Sum() + newValue) / Length);
		}
	}
}
```
