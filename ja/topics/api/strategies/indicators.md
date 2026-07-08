# ストラテジー内のインジケーター

StockSharp では、[Strategy](xref:StockSharp.Algo.Strategies.Strategy) クラスがインジケーターを扱うための特別な仕組みを提供します。これにより、インジケーターの形成状態を制御し、ストラテジーがいつ動作可能になるかを判断できます。

## Indicators プロパティ

[Strategy.Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) プロパティは、ストラテジーで使用されるインジケーターのコレクションです。このコレクションは、インジケーター形成 (ウォームアップ) の状態を自動的に追跡するように設計されています。

```cs
// インジケーターコレクションへアクセス
INotifyList<IIndicator> indicators = strategy.Indicators;
```

## IsFormed プロパティ

デフォルトでは、[Strategy.IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) プロパティの実装は、[Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) コレクション内のすべてのインジケーターが形成済みかどうかを確認します。

```cs
// Strategy クラスの標準実装
public virtual bool IsFormed => _indicators.AllFormed;
```

コレクション内のすべてのインジケーターが形成済みになる (それぞれの [IIndicator.IsFormed](xref:StockSharp.Algo.Indicators.IIndicator.IsFormed) プロパティが `true` を返す) と、ストラテジーは「ウォームアップ済み」で動作可能な状態と見なされます。

## コレクションへのインジケーター追加

ストラテジーが準備完了かどうかを正しく判断するには、使用するインジケーターを [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) コレクションへ追加する必要があります。

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// インジケーターを作成
	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };
	
	// インジケーターをコレクションへ追加
	Indicators.Add(_shortSma);
	Indicators.Add(_longSma);
	
	// ...
}
```

## 追加すべきインジケーター

[Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) コレクションへ追加するのは、**独立したインジケーター**のみにしてください。これは不要な待機を避け、ストラテジーが準備完了になるタイミングを正しく判断するための重要なルールです。

### インジケーター追加のルール:

1. **独立したインジケーター** - マーケットデータ (ローソク足、ティックなど) を直接処理するインジケーターを追加します。

   ```cs
   // 独立したインジケーター
   var sma = new SimpleMovingAverage { Length = 20 };
   var rsi = new RelativeStrengthIndex { Length = 14 };
   
   Indicators.Add(sma);
   Indicators.Add(rsi);
   ```

2. **インジケーターチェーン** - インジケーターチェーン (あるインジケーターの出力が別のインジケーターの入力になる構成) を使用する場合は、チェーン内の**最初のインジケーターのみ**をコレクションへ追加します。

   ```cs
   // インジケーターチェーン
   var sma = new SimpleMovingAverage { Length = 20 };
   var stdev = new StandardDeviation { Length = 20 };
   var bollingerBands = new BollingerBands 
   { 
       SmaIndicator = sma,
       DeviationIndicator = stdev
   };
   
   // チェーン内の最初のインジケーターのみを追加
   Indicators.Add(sma);
   // 他のインジケーターに依存するインジケーターは追加しない
   // Indicators.Add(stdev); - 不正
   // Indicators.Add(bollingerBands); - 不正
   ```

3. **複合インジケーター** - 複数の独立したインジケーターを使用するインジケーター (例: MACD) の場合は、それらをすべて追加します。

   ```cs
   var fastEma = new ExponentialMovingAverage { Length = 12 };
   var slowEma = new ExponentialMovingAverage { Length = 26 };
   var signalEma = new ExponentialMovingAverage { Length = 9 };
   var macd = new MovingAverageConvergenceDivergence
   {
       FastEma = fastEma,
       SlowEma = slowEma,
       SignalEma = signalEma
   };
   
   // 基本インジケーターを追加
   Indicators.Add(fastEma);
   Indicators.Add(slowEma);
   ```

## 使用例

### 2 つの移動平均を使う基本例

```cs
public class SmaStrategy : Strategy
{
	private SimpleMovingAverage _longSma;
	private SimpleMovingAverage _shortSma;
	
	// ...
	
	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);
		
		_longSma = new SimpleMovingAverage { Length = LongSmaLength };
		_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
		
		// 状態を追跡するため、インジケーターをコレクションへ追加
		Indicators.Add(_longSma);
		Indicators.Add(_shortSma);
		
		// ...
	}
	
	private void ProcessCandle(ICandleMessage candle)
	{
		// インジケーターを処理
		var longValue = _longSma.Process(candle);
		var shortValue = _shortSma.Process(candle);
		
		// 売買ロジックを実行する前に、ストラテジーが準備完了か確認
		if (!IsFormed)
			return;
			
		// 売買ロジック
		// ...
	}
}
```

### IsFormedAndOnline の使用例

ストラテジーが取引可能な状態かどうかを確認するには、[IsFormedAndOnlineAndAllowTrading](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnlineAndAllowTrading(StockSharp.Algo.Strategies.StrategyTradingModes)) メソッドがよく使用されます。このメソッドは、インジケーター形成、オンライン状態、取引許可の確認を組み合わせます。

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// インジケーターを処理
	var longValue = _longSma.Process(candle);
	var shortValue = _shortSma.Process(candle);
	
	// ストラテジー準備状態の包括的な確認
	if (!IsFormedAndOnlineAndAllowTrading())
		return;
		
	// 売買ロジック
	// ...
}
```

## インジケーター使用の最適化

より複雑なストラテジーでは、インジケーターの扱いを適切に整理することが重要です。

```cs
public class ComplexStrategy : Strategy
{
	private SimpleMovingAverage _sma;
	private RelativeStrengthIndex _rsi;
	private BollingerBands _bollinger;
	private StandardDeviation _stdev;
	
	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);
		
		// インジケーターを作成
		_sma = new SimpleMovingAverage { Length = 20 };
		_rsi = new RelativeStrengthIndex { Length = 14 };
		
		_stdev = new StandardDeviation { Length = 20 };
		_bollinger = new BollingerBands 
		{ 
			SmaIndicator = _sma,
			DeviationIndicator = _stdev 
		};
		
		// 独立したインジケーターのみを追加
		Indicators.Add(_sma);
		Indicators.Add(_rsi);
		// _stdev と _bollinger は _sma に依存するため追加しない
		
		// ...
	}
	
	// ...
}
```

## 高度な機能

標準の動作では不十分な場合は、ストラテジー内で [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) プロパティをオーバーライドできます。

```cs
public override bool IsFormed
{
	get
	{
		// 標準のインジケーター確認
		if (!base.IsFormed)
			return false;
			
		// 追加のストラテジー準備条件
		return _customCondition && _additionalCheck;
	}
}
```

## 関連項目

- [インジケーター一覧](../indicators/list_of_indicators.md)
- [カスタムインジケーター](../indicators/custom_indicator.md)
- [プラットフォームとのストラテジー互換性](compatibility.md)
