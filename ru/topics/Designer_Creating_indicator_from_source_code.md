# Создание индикатора на C\#

Создание собственного индикатора в [API](StockSharpAbout.md) описано в пункте [Собственный индикатор](IndicatorsCustom.md).

Ниже, на примере индикатора SMA, будет показано, как созданный индикатор интегрировать на схему в виде кубика. В качестве примера можно взять исходные коды других индикаторов, которые находятся в репозитории [GitHub\/StockSharp](https://github.com/StockSharp/StockSharp).

Для начала необходимо создать кубик как описано в пункте [Интерфейс](Designer_Creation_element_containing_source_code.md).

Опишем входные и выходные параметры кубика:

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

Опишем индикатор как отдельный класс:

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

Добавим индикатор в код кубика и инициализируем его в конструкторе класса кубика:

```cs
		private SimpleMovingAverage _NewSMA;
		public NewSMA ()
		{
			_NewSMA = new SimpleMovingAverage();
		}
```

Индикатор SMA имеет единственное свойство, длину периода. В коде кубика опишем это свойство:

```cs
		public int Length
		{
			get { return _NewSMA.Length; }
			set { _NewSMA.Length = value;}
		}
```

Допишем метод **ProcessCandle** так чтобы он рассчитывал значение индикатора и генерировал событие выходного параметра:

```cs
		[DiagramExternal]
		public void ProcessCandle(Candle candle)
		{
			NewIndicator?.Invoke(_NewSMA.Process(candle));
		}
```

Полный код кубика:

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
