# カスタムタイプのローソク足

[S#](../../api.md) では、カスタムローソク足タイプを扱えるようにすることで、ローソク足構築機能を拡張できます。これは、現在 [S#](../../api.md) でサポートされていないローソク足を扱う必要がある場合に便利です。以下では、デルタローソク足（買い出来高と売り出来高の差に基づいて形成されるローソク足）の例を使って、独自のローソク足タイプを作成する手順を示します。

## デルタローソク足の実装

1. まず、独自のローソク足メッセージタイプを作成する必要があります。このタイプは [CandleMessage](xref:StockSharp.Messages.CandleMessage) クラスを継承する必要があります。

   ```cs
   /// <summary>
   /// 買い出来高と売り出来高のデルタに基づいて形成されるローソク足。
   /// </summary>
   public class DeltaCandleMessage : CandleMessage
   {
       // ヘルパーからメッセージタイプ識別子を取得し、
       // RegisterCandleType で同じ値を使用します
       
       /// <summary>
       /// <see cref="DeltaCandleMessage"/> の新しいインスタンスを初期化します。
       /// </summary>
       public DeltaCandleMessage()
           : base(DeltaCandleHelper.DeltaCandleType)
       {
       }
       
       /// <summary>
       /// ローソク足形成用のデルタしきい値。
       /// </summary>
       public decimal DeltaThreshold { get; set; }
       
       /// <summary>
       /// 現在のデルタ値。
       /// </summary>
       public decimal CurrentDelta { get; set; }
       
       /// <summary>
       /// <see cref="DeltaCandleMessage"/> のコピーを作成します。
       /// </summary>
       /// <returns>コピー。</returns>
       public override Message Clone()
       {
           return CopyTo(new DeltaCandleMessage
           {
               DeltaThreshold = DeltaThreshold,
               CurrentDelta = CurrentDelta
           });
       }
       
       /// <summary>
       /// ローソク足パラメーター。
       /// </summary>
       public override object Arg
       {
           get => DeltaThreshold;
           set => DeltaThreshold = (decimal)value;
       }
       
       /// <summary>
       /// ローソク足引数の型。
       /// </summary>
       public override Type ArgType => typeof(decimal);
   }
   ```

2. 次に、[DataType](xref:StockSharp.Messages.DataType) クラスで独自のデータタイプを作成する必要があります。

   ```cs
   public static class DeltaCandleHelper
   {
       /// <summary>
       /// デルタローソク足用の一意な MessageType を定義します。
       /// </summary>
       public const MessageTypes DeltaCandleType = (MessageTypes)10001;
       
       /// <summary>
       /// <see cref="DeltaCandleMessage"/> データタイプ。
       /// </summary>
       public static readonly DataType CandleDelta = 
           DataType.Create(typeof(DeltaCandleMessage)).Immutable();
       
       /// <summary>
       /// デルタローソク足用のデータタイプを作成します。
       /// </summary>
       /// <param name="threshold">デルタしきい値。</param>
       /// <returns>データタイプ。</returns>
       public static DataType Delta(this decimal threshold)
       {
           return DataType.Create(typeof(DeltaCandleMessage), threshold);
       }
       
       /// <summary>
       /// デルタローソク足タイプをシステムに登録します。
       /// </summary>
       public static void RegisterDeltaCandleType()
       {
           // StockSharp に新しいローソク足タイプを登録
           Extensions.RegisterCandleType<decimal>(
               typeof(DeltaCandleMessage),      // ローソク足メッセージタイプ
               DeltaCandleType,                // メッセージタイプ
               "delta",                        // ストレージ用のファイル名
               str => str.To<decimal>(),       // 文字列からパラメーターへのコンバーター
               arg => arg.ToString(),          // パラメーターから文字列へのコンバーター
               a => a > 0,                     // パラメーターバリデーター
               false                           // このようなローソク足をソースから取得できるか（構築だけでないか）
           );
       }
   }
   ```

3. 次に、新しいタイプ用のローソク足ビルダーを作成する必要があります。そのために、[CandleBuilder\<TCandleMessage\>](xref:StockSharp.Algo.Candles.Compression.CandleBuilder`1) の実装を作成します。

   ```cs
   /// <summary>
   /// <see cref="DeltaCandleMessage"/> タイプ用のローソク足ビルダー。
   /// </summary>
   public class DeltaCandleBuilder : CandleBuilder<DeltaCandleMessage>
   {
       /// <summary>
       /// <see cref="DeltaCandleBuilder"/> の新しいインスタンスを初期化します。
       /// </summary>
       /// <param name="exchangeInfoProvider">取引所情報プロバイダー。</param>
       public DeltaCandleBuilder(IExchangeInfoProvider exchangeInfoProvider)
           : base(exchangeInfoProvider)
       {
       }
       
       /// <inheritdoc />
       protected override DeltaCandleMessage CreateCandle(ICandleBuilderSubscription subscription, ICandleBuilderValueTransform transform)
       {
           var time = transform.Time;
           
           return FirstInitCandle(subscription, new DeltaCandleMessage
           {
               DeltaThreshold = subscription.Message.GetArg<decimal>(),
               OpenTime = time,
               CloseTime = time,
               HighTime = time,
               LowTime = time,
               CurrentDelta = 0
           }, transform);
       }
       
       /// <inheritdoc />
       protected override bool IsCandleFinishedBeforeChange(ICandleBuilderSubscription subscription, DeltaCandleMessage candle, ICandleBuilderValueTransform transform)
       {
           // デルタの絶対値がしきい値を超えたとき、ローソク足はクローズします
           return Math.Abs(candle.CurrentDelta) >= candle.DeltaThreshold;
       }
       
       /// <inheritdoc />
       protected override void UpdateCandle(ICandleBuilderSubscription subscription, DeltaCandleMessage candle, ICandleBuilderValueTransform transform)
       {
           base.UpdateCandle(subscription, candle, transform);
           
           // 取引方向に基づいてデルタを更新
           if (transform.Side == Sides.Buy)
               candle.CurrentDelta += transform.Volume ?? 0;
           else if (transform.Side == Sides.Sell)
               candle.CurrentDelta -= transform.Volume ?? 0;
       }
   }
   ```

4. 次に、[CandleBuilderProvider](xref:StockSharp.Algo.Candles.Compression.CandleBuilderProvider) にローソク足ビルダーを登録する必要があります。

   ```cs
   private Connector _connector;
   ...
   // デルタローソク足タイプをシステムに登録
   DeltaCandleHelper.RegisterDeltaCandleType();
   
   // デルタローソク足ビルダーを登録
   _connector.Adapter.CandleBuilderProvider.Register(new DeltaCandleBuilder(_connector.ExchangeInfoProvider));
   ```

5. `DeltaCandleMessage` タイプのローソク足用サブスクリプションを作成し、そこからデータをリクエストします。

   ```cs
   // デルタしきい値
   decimal deltaThreshold = 1000m;
   
   // デルタローソク足用のサブスクリプションを作成
   var subscription = new Subscription(
       // 独自の拡張メソッドを使用してデータタイプを作成
       deltaThreshold.Delta(), 
       security)
   {
       MarketData =
       {
           // ローソク足をティックから構築することを指定
           BuildMode = MarketDataBuildModes.Build,
           BuildFrom = DataType.Ticks
       }
   };
   
   // ローソク足受信イベントを購読
   _connector.CandleReceived += (sub, candle) =>
   {
       if (sub != subscription)
           return;
       
       var deltaCandle = (DeltaCandleMessage)candle;
       
       // デルタローソク足を処理
       Console.WriteLine($"デルタローソク足 {candle.OpenTime}: O:{candle.OpenPrice} H:{candle.HighPrice} " +
                        $"L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume} Delta:{deltaCandle.CurrentDelta}");
   };
   
   // オンラインモード遷移を購読
   _connector.SubscriptionOnline += sub => 
   {
       if (sub == subscription)
           Console.WriteLine("デルタローソク足の購読がオンラインモードに移行しました");
   };
   
   // サブスクリプションを開始
   _connector.Subscribe(subscription);
   ```

## 取引戦略でのデルタローソク足の使用

デルタローソク足を使用するシンプルな戦略の例です。

```cs
public class DeltaCandleStrategy : Strategy
{
	private readonly StrategyParam<decimal> _deltaThreshold;
	private readonly StrategyParam<decimal> _volume;
	private readonly StrategyParam<decimal> _signalDelta;

	private IChart _chart;
	private IChartCandleElement _chartCandleElement;
	private IChartIndicatorElement _deltaIndicatorElement;

	public decimal DeltaThreshold
	{
		get => _deltaThreshold.Value;
		set => _deltaThreshold.Value = value;
	}

	public decimal Volume
	{
		get => _volume.Value;
		set => _volume.Value = value;
	}

	public decimal SignalDelta
	{
		get => _signalDelta.Value;
		set => _signalDelta.Value = value;
	}

	public DeltaCandleStrategy()
	{
		// 戦略パラメーター
		_deltaThreshold = Param(nameof(DeltaThreshold), 1000m)
			.SetDisplay("デルタしきい値", "ローソク足形成用の出来高デルタ値", "メイン設定")
			.SetGreaterThanZero()
			.SetCanOptimize(true)
			.SetOptimize(500m, 2000m, 100m);

		_volume = Param(nameof(Volume), 1m)
			.SetDisplay("注文数量", "取引操作の数量", "メイン設定")
			.SetGreaterThanZero();

		_signalDelta = Param(nameof(SignalDelta), 500m)
			.SetDisplay("シグナル用の最小デルタ", "シグナル生成用の最小デルタ値", "メイン設定")
			.SetGreaterThanZero()
			.SetCanOptimize(true);

		Name = "DeltaCandleStrategy";
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// 利用可能な場合はチャートを初期化
		_chart = GetChart();
		if (_chart != null)
		{
			var area = _chart.AddArea();
			_chartCandleElement = area.AddCandles();
			_deltaIndicatorElement = area.AddIndicator();
			_deltaIndicatorElement.DrawStyle = DrawStyles.Histogram;
			_deltaIndicatorElement.Color = System.Drawing.Color.Purple;
		}

		// デルタローソク足用のサブスクリプションを作成
		var subscription = new Subscription(DeltaThreshold.Delta(), Security)
		{
			MarketData =
			{
				BuildMode = MarketDataBuildModes.Build,
				BuildFrom = DataType.Ticks
			}
		};

		// デルタローソク足を処理するルールを作成
		this
			.WhenCandleReceived(subscription)
			.Do(ProcessDeltaCandle)
			.Apply(this);

		// サブスクリプションを開始
		Subscribe(subscription);
	}

	private void ProcessDeltaCandle(ICandleMessage candle)
	{
		// 利用可能な場合はチャートに描画
		if (_chart != null)
		{
			var deltaCandle = (DeltaCandleMessage)candle;
			
			var data = _chart.CreateData();
			data.Group(candle.OpenTime)
				.Add(_chartCandleElement, candle)
				.Add(_deltaIndicatorElement, deltaCandle.CurrentDelta);
				
			_chart.Draw(data);
		}

		// 完了済みのローソク足だけを処理
		if (candle.State != CandleStates.Finished)
			return;
			
		var deltaCandle = (DeltaCandleMessage)candle;
		
		// デルタがシグナルに十分か確認
		if (Math.Abs(deltaCandle.CurrentDelta) < SignalDelta)
		{
			this.AddInfoLog($"Delta {deltaCandle.CurrentDelta} はしきい値 {SignalDelta} 未満です。シグナルは生成されません。");
			return;
		}

		// 操作方向はデルタの符号に依存
		var direction = deltaCandle.CurrentDelta > 0 ? Sides.Buy : Sides.Sell;
		
		this.AddInfoLog($"デルタローソク足が完了しました。Delta: {deltaCandle.CurrentDelta}。方向: {direction}");
		
		// 価格の決定にはローソク足の終値を使用
		var price = deltaCandle.ClosePrice;
		var volume = Volume;
		
		// 反対方向のポジションをすでに保有している場合は、
		// 既存ポジションをクローズするために数量を増やします
		if ((Position < 0 && direction == Sides.Buy) || 
			(Position > 0 && direction == Sides.Sell))
		{
			volume = Math.Max(volume, Math.Abs(Position) + volume);
		}
		
		// 注文を登録
		RegisterOrder(this.CreateOrder(direction, price, volume));
	}
}
```

## カスタムローソク足タイプ作成時の重要なポイント

1. **MessageTypes の一意性** - 選択する `MessageTypes` 識別子が、StockSharp の既存タイプと競合しないことを確認してください。カスタムタイプには 10000 より大きい値を使用することを推奨します。

2. **ローソク足タイプの登録** - StockSharp のグラフィカルコントロールおよびデータストレージと適切に統合するには、`Extensions.RegisterCandleType` による登録が必要です。登録しない場合、ローソク足タイプはコード内でのみ動作し、ユーザーインターフェイスでは利用できません。

3. **ローソク足パラメーター** - ローソク足引数の型を返す `ArgType` プロパティを実装してください。これは、グラフィカルインターフェイスでパラメーターを正しく表示するために使用されます。

4. **ファイルシステム** - `RegisterCandleType` メソッドの `fileName` パラメーターは、StockSharp データストレージを使用する際にローソク足をファイルシステムへ保存するために使用されます。

5. **パラメーター検証** - パラメーター検証メソッドは、サブスクリプションを作成する前に値の正確性を確認するために StockSharp で使用されます。

これで、StockSharp エコシステム全体（ユーザーインターフェイスとデータストレージを含む）に適切に統合され、出来高デルタ分析に基づく取引戦略の構築に使用できる、完全なカスタムローソク足タイプを作成できました。
