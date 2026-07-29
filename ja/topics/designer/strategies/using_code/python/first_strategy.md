# Python ストラテジーの例

ソースコードからのストラテジー作成は、[キューブからアルゴリズムを作成](../../using_visual_designer/first_strategy.md)セクションでキューブから組み立てた SMA ストラテジーの例と同様の、SMA ストラテジーの例を使って示します。

このセクションでは、Python 言語の構文（またはストラテジー作成の基底として使用される [Strategy](../../../../api/strategies.md)）については説明せず、**Designer** でコードを扱う際の固有の機能に触れます。

> [!TIP]
> **Designer** で作成されたストラテジーは、[API](../../../../api.md) で作成されたストラテジーと互換性があります。これは共通の基底クラス [Strategy](../../../../api/strategies.md) を使用しているためです。これにより、**Designer** 外でこのようなストラテジーを実行することは、[スキーム](../../../live_execution/running_strategies_outside_of_designer.md)を実行する場合よりも大幅に簡単になります。

1. Python 開発を始める前に、IronPython が .NET との連携のために提供している clr モジュールを通じて .NET 依存関係をインポートする必要があります。

```python
import clr

clr.AddReference("System.Drawing")
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Algo")
```

接続する必要がある主要なライブラリ:

- `clr` モジュール - Python コードと .NET Framework の連携を可能にするためにインポートされます
- System.Drawing - 色やグラフィックを扱うために必要なライブラリ
- StockSharp.Messages - 基本的な S# メッセージとデータ型を含みます
- StockSharp.Algo - Strategy 基底クラスを含む、基本的な S# アルゴリズムコンポーネントを含みます

> [!NOTE]
> これらのライブラリの型を使用する前に、ファイルの先頭でライブラリを接続することが重要です。ライブラリを接続した後は、通常の Python import によって特定の型をインポートできます。

2. ストラテジーパラメーターは、特別な方法で作成します。

```python
def __init__(self):
	super(sma_strategy, self).__init__()
	self._isShortLessThenLong = None

	# ストラテジーパラメーターを初期化
	self._candleTypeParam = self.Param("CandleType", DataType.TimeFrame(TimeSpan.FromMinutes(1))) \
		.SetDisplay("ローソク足タイプ", "戦略計算用のローソク足タイプ。", "一般")

	self._long = self.Param("Long", 80)
	self._short = self.Param("Short", 30)

	self._takeValue = self.Param("TakeValue", Unit(0, UnitTypes.Absolute))
	self._stopValue = self.Param("StopValue", Unit(2, UnitTypes.Percent))

@property
def CandleType(self):
	return self._candleTypeParam.Value

@CandleType.setter
def CandleType(self, value):
	self._candleTypeParam.Value = value

@property
def Long(self):
	return self._long.Value

@Long.setter
def Long(self, value):
	self._long.Value = value

@property
def Short(self):
	return self._short.Value

@Short.setter
def Short(self, value):
	self._short.Value = value

@property
def TakeValue(self):
	return self._takeValue.Value

@TakeValue.setter
def TakeValue(self, value):
	self._takeValue.Value = value

@property
def StopValue(self):
	return self._stopValue.Value

@StopValue.setter
def StopValue(self, value):
	self._stopValue.Value = value
```

[StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) クラスを使用すると、設定の保存と復元の仕組みが自動的に使用されます。

3. インジケーターを作成し、市場データを購読する場合は、購読から入ってくるデータでインジケーター値を更新できるように、それらをバインドする必要があります。

```python
# インジケーターを作成
longSma = SMA()
longSma.Length = self.Long
shortSma = SMA()
shortSma.Length = self.Short

# ローソク足セットとインジケーターをバインド
subscription = self.SubscribeCandles(self.CandleType)
# インジケーターをローソク足にバインドし、処理を開始
subscription.Bind(longSma, shortSma, self.OnProcess).Start()
```

4. チャートを扱う場合、ストラテジーを [Designer 外で](../../../live_execution/running_strategies_outside_of_designer.md)実行すると、チャートオブジェクトが存在しない可能性があることを考慮することが重要です。

```python
# GUI が利用可能な場合はチャートを設定
area = self.CreateChartArea()
if area is not None:
	self.DrawCandles(area, subscription)
	self.DrawIndicator(area, shortSma, Color.Coral)
	self.DrawIndicator(area, longSma)
	self.DrawOwnTrades(area)
```

5. ストラテジーロジックで必要な場合は、[StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)) によってポジション保護を開始します。

```python
self.StartProtection(self.TakeValue, self.StopValue)
```

6. ストラテジーロジック本体は、OnProcess メソッドに実装します。このメソッドは、ステップ 1 で作成した購読によって呼び出されます。

```python
def OnProcess(self, candle, longValue, shortValue):
	"""
	確定した各ローソク足を処理し、情報をログに記録し、SMA クロス時に取引ロジックを実行します。
	
	:param candle: 処理されたローソク足メッセージ。
		:param longValue: 長期 SMA の現在値。
		:param shortValue: 短期 SMA の現在値。
	"""
	self.LogInfo("新しいローソク足 {0}: {6} {1};{2};{3};{4}; 出来高 {5}", candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId)

	# ローソク足が確定していなければ何もしない
	if candle.State != CandleStates.Finished:
		return

	# 短期 SMA が長期 SMA より小さいかを判定
	isShortLessThenLong = shortValue < longValue

	if self._isShortLessThenLong is None:
		self._isShortLessThenLong = isShortLessThenLong
	elif self._isShortLessThenLong != isShortLessThenLong:
		# クロスが発生
		direction = Sides.Sell if isShortLessThenLong else Sides.Buy

		# ポジションを開く、または反転するための数量を計算
		volume = self.Volume if self.Position == 0 else Math.Min(Math.Abs(self.Position), self.Volume) * 2

		# 価格ステップを取得（設定されていない場合は既定で 1）
		priceStep = self.GetSecurity().PriceStep or 1

		# オフセットを付けて注文価格を計算
		price = candle.ClosePrice + (priceStep if direction == Sides.Buy else -priceStep)

		if direction == Sides.Buy:
			self.BuyLimit(price, volume)
		else:
			self.SellLimit(price, volume)

		# 状態を更新
		self._isShortLessThenLong = isShortLessThenLong
```

7. Python ストラテジーでは、仮想メソッド [CreateClone](xref:StockSharp.Algo.Strategies.Strategy.CreateClone) をオーバーライドすることが必須要件です。このメソッドは、ストラテジーパラメーターの最適化時やテスト時に [Designer](../../../../designer.md) が必要とする新しいストラテジーインスタンスを作成するために必要です。これをオーバーライドしないと、これらの操作は不可能になります。

```python
def CreateClone(self):
	"""
	!! 必須 !! ストラテジーの新しいインスタンスを作成します。
	"""
	return sma_strategy()
```

> [!IMPORTANT]
> CreateClone メソッドをオーバーライドしないと、履歴データでストラテジーをテストしたり、パラメーター最適化を行ったりすることができなくなります。Python でストラテジーを作成する場合は、必ずこのメソッドを追加してください。
