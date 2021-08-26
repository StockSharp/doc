# Custom indicator

To create your own indicator, you must implement the [IIndicator](../api/StockSharp.Algo.Indicators.IIndicator.html) interface. As an example you can take the source code of other indicators that are in the [GitHub\/StockSharp](https://github.com/StockSharp/StockSharp) repository. Here is the code for the simple moving average indicator implementation [SimpleMovingAverage](../api/StockSharp.Algo.Indicators.SimpleMovingAverage.html): 

```cs
\/\/\/ \<summary\>
\/\/\/ \<summary\> 
\/\/\/ Simple moving average. 
\/\/\/ \<\/summary\> 
\[DisplayName("SMA")\] 
\[DescriptionLoc(LocalizedStrings.Str818Key)\] 
public class SimpleMovingAverage : LengthIndicator\<decimal\> 
{ 
	\/\/\/ \<summary\> 
	\/\/\/ Initializes a new instance of the \<see cref\="SimpleMovingAverage"\/\>. 
	\/\/\/ \<\/summary\> 
	public SimpleMovingAverage()
	{ 
		Length \= 32; 
	} 
	\/\/\/ \<summary\> 
	\/\/\/ To handle the input value. 
	\/\/\/ \<\/summary\> 
	\/\/\/ \<param name\="input"\>The input value.\<\/param\> 
	\/\/\/ \<returns\>The resulting value.\<\/returns\> 
	protected override IIndicatorValue OnProcess(IIndicatorValue input)
	{ 
		var newValue \= input.GetValue\<decimal\>(); 
        
		if (input.IsFinal) 
		{ 
			Buffer.Add(newValue); 
            
			if (Buffer.Count \> Length) 
				Buffer.RemoveAt(0); 
		} 
        
		if (input.IsFinal) 
			return new DecimalIndicatorValue(this, Buffer.Sum() \/ Length); 
        
		return new DecimalIndicatorValue(this, (Buffer.Skip(1).Sum() + newValue) \/ Length); 
	} 
} 
```

[SimpleMovingAverage](../api/StockSharp.Algo.Indicators.SimpleMovingAverage.html) inherits from the [LengthIndicator\`1](../api/StockSharp.Algo.Indicators.LengthIndicator`1.html) class, from which it is necessary to inherit all of the indicators, having as parameter the length of the period. 

Some indicators are composite and use other indicators in their calculations. Therefore, you can reuse the indicators from each other, as shown as an example of realization of the Chaikin Volatility indicator [ChaikinVolatility](../api/StockSharp.Algo.Indicators.ChaikinVolatility.html): 

```cs
\/\/\/ \<summary\>
\/\/\/ Chaikin volatility.
\/\/\/ \<\/summary\>
\/\/\/ \<remarks\>
\/\/\/ http:\/\/www2.wealth\-lab.com\/WL5Wiki\/Volatility.ashx http:\/\/www.incrediblecharts.com\/indicators\/chaikin\_volatility.php.
\/\/\/ \<\/remarks\>
\[DisplayName("Chaikin's Volatility")\]
\[DescriptionLoc(LocalizedStrings.Str730Key)\]
public class ChaikinVolatility : BaseIndicator
{
	\/\/\/ \<summary\>
	\/\/\/ Initializes a new instance of the \<see cref\="ChaikinVolatility"\/\>.
	\/\/\/ \<\/summary\>
	public ChaikinVolatility()
	{
		Ema \= new ExponentialMovingAverage();
		Roc \= new RateOfChange();
	}
	\/\/\/ \<summary\>
	\/\/\/ Moving Average.
	\/\/\/ \<\/summary\>
	\[TypeConverter(typeof(ExpandableObjectConverter))\]
	\[DisplayName("MA")\]
	\[DescriptionLoc(LocalizedStrings.Str731Key)\]
	\[CategoryLoc(LocalizedStrings.GeneralKey)\]
	public ExponentialMovingAverage Ema { get; }
	\/\/\/ \<summary\>
	\/\/\/ Rate of change.
	\/\/\/ \<\/summary\>
	\[TypeConverter(typeof(ExpandableObjectConverter))\]
	\[DisplayName("ROC")\]
	\[DescriptionLoc(LocalizedStrings.Str732Key)\]
	\[CategoryLoc(LocalizedStrings.GeneralKey)\]
	public RateOfChange Roc { get; }
	\/\/\/ \<summary\>
	\/\/\/ Whether the indicator is set.
	\/\/\/ \<\/summary\>
	public override bool IsFormed \=\> Roc.IsFormed;
	\/\/\/ \<summary\>
	\/\/\/ To handle the input value.
	\/\/\/ \<\/summary\>
	\/\/\/ \<param name\="input"\>The input value.\<\/param\>
	\/\/\/ \<returns\>The resulting value.\<\/returns\>
	protected override IIndicatorValue OnProcess(IIndicatorValue input)
	{
		var candle \= input.GetValue\<Candle\>();
		var emaValue \= Ema.Process(input.SetValue(this, candle.HighPrice \- candle.LowPrice));
		if (Ema.IsFormed)
		{
			return Roc.Process(emaValue);
		}
		return input;				
	}
	\/\/\/ \<summary\>
	\/\/\/ Load settings.
	\/\/\/ \<\/summary\>
	\/\/\/ \<param name\="settings"\>Settings storage.\<\/param\>
	public override void Load(SettingsStorage settings)
	{
		base.Load(settings);
		Ema.LoadNotNull(settings, "Ema");
		Roc.LoadNotNull(settings, "Roc");
	}
	\/\/\/ \<summary\>
	\/\/\/ Save settings.
	\/\/\/ \<\/summary\>
	\/\/\/ \<param name\="settings"\>Settings storage.\<\/param\>
	public override void Save(SettingsStorage settings)
	{
		base.Save(settings);
		settings.SetValue("Ema", Ema.Save());
		settings.SetValue("Roc", Roc.Save());
	}
}
```

The last types of indicators are those that do not just consist of other indicators, but also graphically displayed in several states at the same time (with a few lines). For example, [AverageDirectionalIndex](../api/StockSharp.Algo.Indicators.AverageDirectionalIndex.html): 

```cs
\/\/\/ \<summary\>
\/\/\/ Welles Wilder Average Directional Index.
\/\/\/ \<\/summary\>
\[DisplayName("ADX")\] 
\[DescriptionLoc(LocalizedStrings.Str757Key)\] 
public class AverageDirectionalIndex : BaseComplexIndicator 
{ 
	\/\/\/ \<summary\> 
	\/\/\/ Initializes a new instance of the \<see cref\="AverageDirectionalIndex"\/\>. 
	\/\/\/ \<\/summary\> 
	public AverageDirectionalIndex()
		: this(new DirectionalIndex { Length \= 14 }, new WilderMovingAverage { Length \= 14 }) 
	{ 	} 
	\/\/\/ \<summary\> 
	\/\/\/ Initializes a new instance of the \<see cref\="AverageDirectionalIndex"\/\>. 
	\/\/\/ \<\/summary\> 
	\/\/\/ \<param name\="dx"\>Welles Wilder Directional Movement Index.\<\/param\> 
	\/\/\/ \<param name\="movingAverage"\>Moving Average.\<\/param\> 
	public AverageDirectionalIndex(DirectionalIndex dx, LengthIndicator\<decimal\> movingAverage)
	{ 
		if (dx \=\= null) throw new ArgumentNullException(nameof(dx)); 
        
		if (movingAverage \=\= null) 	throw new ArgumentNullException(nameof(movingAverage)); 
        
		InnerIndicators.Add(Dx \= dx); 
		InnerIndicators.Add(MovingAverage \= movingAverage); 
		Mode \= ComplexIndicatorModes.Sequence; 
	} 
    
	\/\/\/ \<summary\> 
	\/\/\/ Welles Wilder Directional Movement Index. 
	\/\/\/ \<\/summary\> 
	\[Browsable(false)\] 
	public DirectionalIndex Dx { get; } 
    
	\/\/\/ \<summary\> 
	\/\/\/ Moving Average. 
	\/\/\/ \<\/summary\> 
	\[Browsable(false)\] 
	public LengthIndicator\<decimal\> MovingAverage { get; } 
    
	\/\/\/ \<summary\> 
	\/\/\/ Period length. 
	\/\/\/ \<\/summary\> 
	\[DisplayNameLoc(LocalizedStrings.Str736Key)\] 
	\[DescriptionLoc(LocalizedStrings.Str737Key)\] 
	\[CategoryLoc(LocalizedStrings.GeneralKey)\] 
	public virtual int Length
	{ 
		get { return MovingAverage.Length; } 
		set 
		{ 
			MovingAverage.Length \= Dx.Length \= value; 
			Reset(); 
		} 
	} 
	\/\/\/ \<summary\> 
	\/\/\/ Load settings. 
	\/\/\/ \<\/summary\> 
	\/\/\/ \<param name\="settings"\>Settings storage.\<\/param\> 
	public override void Load(SettingsStorage settings)
	{ 
		base.Load(settings); 
		Length \= settings.GetValue\<int\>(nameof(Length)); 
	} 
	\/\/\/ \<summary\> 
	\/\/\/ Save settings. 
	\/\/\/ \<\/summary\> 
	\/\/\/ \<param name\="settings"\>Settings storage.\<\/param\> 
	public override void Save(SettingsStorage settings)
	{ 
		base.Save(settings); 
		settings.SetValue(nameof(Length), Length); 
	} 
}
```

Such indicators should be inherited from the [BaseComplexIndicator](../api/StockSharp.Algo.Indicators.BaseComplexIndicator.html) class, and pass indicator components to the [BaseComplexIndicator.InnerIndicators](../api/StockSharp.Algo.Indicators.BaseComplexIndicator.InnerIndicators.html)[BaseComplexIndicator](../api/StockSharp.Algo.Indicators.BaseComplexIndicator.html) will process these parts, one after another. If the [BaseComplexIndicator.Mode](../api/StockSharp.Algo.Indicators.BaseComplexIndicator.Mode.html) set as the [ComplexIndicatorModes.Sequence](../api/StockSharp.Algo.Indicators.ComplexIndicatorModes.Sequence.html), the resulting value of the first indicator will be passed as input value to the second one, and so on until the end. If the [ComplexIndicatorModes.Parallel](../api/StockSharp.Algo.Indicators.ComplexIndicatorModes.Parallel.html), value set, the results of embedded indicators are ignored. 

## Recommended content
