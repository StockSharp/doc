# ストラテジーパラメーター

ストラテジーの設定と最適化のために、StockSharp は特別なクラス [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) を提供します。ストラテジーパラメーターを使用すると、コードを変更せずに取引アルゴリズムの設定を変更できます。これは、テストモードとライブ取引モードを切り替える場合に特に便利です。さらに、これらのパラメーターは最適化中に値を自動的に反復し、最適なストラテジー設定を見つけるために使用されます。

通常の C# プロパティとは異なり、このクラスで作成されたパラメーターはビジュアル設定 (たとえば Designer) に自動的に表示され、ストラテジー最適化に使用できます。

## ストラテジーパラメーターの作成

パラメーターは、ストラテジーコンストラクター内で [Strategy.Param](xref:StockSharp.Algo.Strategies.Strategy.Param``1(System.String,``0)) メソッドを使用して作成します。

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<int> _longSmaLength;

	public int LongSmaLength
	{
		get => _longSmaLength.Value;
		set => _longSmaLength.Value = value;
	}

	public SmaStrategy()
	{
		_longSmaLength = Param(nameof(LongSmaLength), 80)
							.SetGreaterThanZero()
							.SetDisplay("Long SMA length", string.Empty, "基本設定");
	}
}
```

この例では、初期値 80 の `LongSmaLength` パラメーターを作成し、値が 0 より大きいことを保証するバリデーターを設定し、ユーザーインターフェイス用の表示設定を構成しています。

## パラメーター設定メソッド

[StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) クラスは、パラメーター設定のためにいくつかのメソッドを提供します。

### SetDisplay

[StrategyParam\<T\>.SetDisplay](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetDisplay(System.String,System.String,System.String)) メソッドは、パラメーターの表示名、説明、カテゴリーを設定します。

```cs
_longSmaLength = Param(nameof(LongSmaLength), 80)
					.SetDisplay("Long SMA length", "Period of the long moving average", "基本設定");
```

### SetValidator

[StrategyParam\<T\>.SetValidator](xref:Ecng.ComponentModel.Extensions.SetValidator``1(``0,System.ComponentModel.DataAnnotations.ValidationAttribute)) メソッドは、パラメーター値を確認するバリデーターを設定します。StockSharp は、一般的なタスクに使用できる定義済みバリデーターを幅広く提供しています。

```cs
// 数値が 0 より大きいことを確認
_longSmaLength = Param(nameof(LongSmaLength), 80)
					.SetValidator(new IntGreaterThanZeroAttribute());

// 数値が負でないことを確認
_volume = Param(nameof(Volume), 1)
			.SetValidator(new DecimalNotNegativeAttribute());

// 値の範囲を確認
_percentage = Param(nameof(Percentage), 50)
				.SetValidator(new RangeAttribute(0, 100));

// 必須値を確認
_security = Param<Security>(nameof(Security))
				.SetValidator(new RequiredAttribute());
```

利便性のため、[StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) には最も一般的なバリデーター用の組み込みメソッドがあります。

```cs
// 数値が 0 より大きいことを確認
_longSmaLength = Param(nameof(LongSmaLength), 80).SetGreaterThanZero();

// 数値が負でないことを確認
_volume = Param(nameof(Volume), 1).SetNotNegative();

// 値が NULL または負でないことを確認
_interval = Param<TimeSpan?>(nameof(Interval)).SetNullOrNotNegative();

// 値の範囲を設定
_percentage = Param(nameof(Percentage), 50).SetRange(0, 100);
```

組み込みバリデーターでは不十分な場合は、[ValidationAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validationattribute) を継承して独自のバリデーターを作成できます。

```cs
public class EvenNumberAttribute : ValidationAttribute
{
	public EvenNumberAttribute()
		: base("Value must be an even number.")
	{
	}

	public override bool IsValid(object value)
	{
		if (value is int intValue)
			return intValue % 2 == 0;
		
		return false;
	}
}

// カスタムバリデーターを使用
_barCount = Param(nameof(BarCount), 10)
				.SetValidator(new EvenNumberAttribute());
```

### SetHidden

[StrategyParam\<T\>.SetHidden](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetHidden(System.Boolean)) メソッドは、プロパティエディターでパラメーターを非表示にします。

```cs
_systemParam = Param(nameof(SystemParam), "value")
				.SetHidden(true);
```

### SetBasic

[StrategyParam\<T\>.SetBasic](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetBasic(System.Boolean)) メソッドは、パラメーターを基本パラメーターとしてマークします。これはユーザーインターフェイスでの表示に影響します。基本パラメーターは簡易プロパティエディターモードで表示されます。

```cs
_longSmaLength = Param(nameof(LongSmaLength), 80)
					.SetBasic(true);
```

![strategy parameters basic advanced](../../../images/strategy_parameters_basic_advanced.png)

### SetReadOnly

[StrategyParam\<T\>.SetReadOnly](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetReadOnly(System.Boolean)) メソッドは、パラメーターを読み取り専用にします。

```cs
_calculatedParam = Param(nameof(CalculatedParam), 0)
					.SetReadOnly(true);
```

### SetCanOptimize と SetOptimize

[StrategyParam\<T\>.SetCanOptimize](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetCanOptimize(System.Boolean)) と [StrategyParam\<T\>.SetOptimize](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetOptimize(`0,`0,`0)) メソッドは、パラメーターを最適化に使用できるかどうかを指定し、最適化用の値範囲を設定します。

```cs
_longSmaLength = Param(nameof(LongSmaLength), 80)
					.SetCanOptimize(true)
					.SetOptimize(10, 200, 10);
```

上の例では、パラメーターは 10 から 200 まで、ステップ 10 の範囲で最適化されます。

## ストラテジーでのパラメーター使用

ストラテジーパラメーターは通常のプロパティと同じように使用します。

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };
	
	// ...
}
```

## パラメーターの保存と読み込み

パラメーター値は、基底 [Strategy](xref:StockSharp.Algo.Strategies.Strategy) クラスで自動的に保存および読み込みされます。[Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) と [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)) メソッドをオーバーライドする場合は、基底クラスのメソッドを呼び出す必要があります。

```cs
public override void Save(SettingsStorage settings)
{
	base.Save(settings);
	
	// 追加の保存ロジック...
}

public override void Load(SettingsStorage settings)
{
	base.Load(settings);
	
	// 追加の読み込みロジック...
}
```

## 例: 複数のパラメーターを持つストラテジー

以下は、複数のパラメーターを持つストラテジーの例です。

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _series;
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;

	public DataType Series
	{
		get => _series.Value;
		set => _series.Value = value;
	}

	public int LongSmaLength
	{
		get => _longSmaLength.Value;
		set => _longSmaLength.Value = value;
	}

	public int ShortSmaLength
	{
		get => _shortSmaLength.Value;
		set => _shortSmaLength.Value = value;
	}

	public SmaStrategy()
	{
		base.Name = "SMA strategy";

		Param("TypeId", GetType().GetTypeName(false)).SetHidden();
		_longSmaLength = Param(nameof(LongSmaLength), 80)
							.SetGreaterThanZero()
							.SetDisplay("Long SMA length", string.Empty, "基本設定")
							.SetCanOptimize(true)
							.SetOptimize(20, 200, 10);
		
		_shortSmaLength = Param(nameof(ShortSmaLength), 30)
							.SetGreaterThanZero()
							.SetDisplay("Short SMA length", string.Empty, "基本設定")
							.SetCanOptimize(true)
							.SetOptimize(5, 50, 5);
		
		_series = Param(nameof(Series), DataType.TimeFrame(TimeSpan.FromMinutes(15)))
					.SetDisplay("Series", string.Empty, "基本設定");
	}

	// ...
}
```

この例では、2 つの移動平均のクロスに基づくストラテジーを、3 つの設定可能なパラメーターで作成しました。
- `Series` - データ型とタイムフレーム
- `LongSmaLength` - 長期移動平均の期間
- `ShortSmaLength` - 短期移動平均の期間

2 つの数値パラメーターについては、指定範囲で最適化できるように構成しました。

## 関連項目

[設定の保存と読み込み](settings_saving_and_loading.md)
