# インストゥルメントバスケットによるローソク足の作成

[ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity)、[WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity)、または [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) のローソク足を作成するには、通常の [Security](xref:StockSharp.BusinessEntities.Security) インストゥルメントと同じサブスクリプション機構を使用します。

以下は、AAPL - MSFT スプレッドの 1 分足を作成する例です。

```cs
private Connector _connector;
private Security _instr1;
private Security _instr2;
private WeightedIndexSecurity _indexInstr;
private Subscription _indexSubscription;
private const string _secCode1 = "AAPL";
private const string _secCode2 = "MSFT";
readonly TimeSpan _timeFrame = TimeSpan.FromMinutes(1);
private ChartArea _area;
private ChartCandleElement _candleElement;

// 接続のセットアップとコネクタの設定
private void ConfigureConnector()
{
	if (_connector.Configure(this))
	{
		_connector.Save().Serialize(_connectorFile);
	}
}

// チャートのセットアップ
private void SetupChart()
{
	_area = new ChartArea();
	_chart.Areas.Add(_area);
	_candleElement = new ChartCandleElement();
	_area.Elements.Add(_candleElement);
	
	// ローソク足受信イベントを購読
	_connector.CandleReceived += OnCandleReceived;
}

// サービス登録
private void RegisterServices()
{
	ConfigManager.RegisterService<ISecurityProvider>(_connector);
	ConfigManager.RegisterService<ICompilerService>(new RoslynCompilerService());
}

// インデックスインストゥルメントを作成し、ローソク足を購読
private void CreateIndexAndSubscribe()
{
	// インデックスインストゥルメント（スプレッド）を作成
	_indexInstr = new WeightedIndexSecurity() 
	{ 
		Board = ExchangeBoard.Nyse, 
		Id = "IndexInstr" 
	};
	
	// ウェイト付きでインストゥルメントを追加（スプレッド用に 1 と -1）
	_indexInstr.Weights.Add(_instr1, 1);
	_indexInstr.Weights.Add(_instr2, -1);
	
	// インデックスインストゥルメントのローソク足へのサブスクリプションを作成
	_indexSubscription = new Subscription(
		DataType.TimeFrame(_timeFrame),  // 1 分足
		_indexInstr)  // このインデックスインストゥルメント
	{
		MarketData = 
		{
			// ティックからローソク足を構築するようにサブスクリプションを設定
			BuildMode = MarketDataBuildModes.Build,
			BuildFrom = DataType.Ticks,
			
			// 30 日分の履歴データをリクエスト
			From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
			To = DateTime.Now
		}
	};
	
	// 要素をチャートに追加し、サブスクリプションにバインド
	_chart.AddElement(_area, _candleElement, _indexSubscription);
	
	// サブスクリプションを開始
	_connector.Subscribe(_indexSubscription);
}

// ローソク足受信イベントのハンドラ
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// ローソク足がこのサブスクリプションに属するか確認
	if (subscription != _indexSubscription)
		return;
	
	// 必要な場合は、完了済みのローソク足だけに処理を限定
	if (candle.State != CandleStates.Finished)
		return;
	
	// チャートにローソク足を描画
	var chartData = new ChartDrawData();
	chartData.Group(candle.OpenTime).Add(_candleElement, candle);
	
	this.GuiAsync(() => _chart.Draw(chartData));
}

// アプリケーション終了時に購読解除
private void Unsubscribe()
{
	if (_indexSubscription != null)
	{
		_connector.CandleReceived -= OnCandleReceived;
		_connector.UnSubscribe(_indexSubscription);
		_indexSubscription = null;
	}
}
```

## インデックスサブスクリプションのその他の使用例

### 構成要素のローソク足からインデックスローソク足へのサブスクリプションを作成

```cs
// 構成要素のローソク足からインデックスローソク足を構築するサブスクリプションを作成
var indexFromCandlesSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	_indexInstr)
{
	MarketData = 
	{
		// 構成要素のローソク足から構築するようにサブスクリプションを設定
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};

// サブスクリプションを開始
_connector.Subscribe(indexFromCandlesSubscription);
```

### オーダーブックからインデックスローソク足へのサブスクリプションを作成

```cs
// オーダーブックからインデックスローソク足を構築するサブスクリプションを作成
var indexFromDepthSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(1)),
	_indexInstr)
{
	MarketData = 
	{
		// オーダーブックから構築するようにサブスクリプションを設定
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.MarketDepth,
		BuildField = Level1Fields.SpreadMiddle,  // スプレッドの中央値を使用
		From = DateTime.Today.Subtract(TimeSpan.FromDays(7)),
		To = DateTime.Now
	}
};

// サブスクリプションを開始
_connector.Subscribe(indexFromDepthSubscription);
```

### ボラティリティインデックスの操作

```cs
// 式に基づくボラティリティインデックスを作成
var volatilityIndex = new ExpressionIndexSecurity
{
	Board = ExchangeBoard.Nyse,
	Id = "VOLX",
	Expression = "StdDev({0}, 20) / SMA({0}, 20) * 100",  // ボラティリティ計算用の式
};

// メインインストゥルメントをインデックスに追加
volatilityIndex.InnerSecurityIds.Add(_instr1.ToSecurityId());

// ボラティリティインデックスのローソク足へのサブスクリプションを作成
var volatilitySubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	volatilityIndex)
{
	MarketData = 
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks,
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};

// サブスクリプションを開始
_connector.Subscribe(volatilitySubscription);
```

## 関連項目

[Continuous Futures](../instruments/continuous_futures.md)

[Index](../instruments/index.md)
