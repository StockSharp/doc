# ローソク足の結合: 履歴 + リアルタイム

履歴ローソク足とリアルタイムデータを結合するには、適切なストレージを初期化する必要があります。取引オブジェクト用ストレージ [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry)、市場データ用ストレージ [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry)、およびスナップショットストレージレジストリ [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) です。

`Samples/Candles/CombineHistoryRealtime` プロジェクトでは、このセットアップを実際に示しています。

## ストレージとコネクタのセットアップ

```cs
public partial class MainWindow
{
	private readonly Connector _connector;
	private const string _connectorFile = "ConnectorFile.json";
	
	// 履歴データへのパス
	private readonly string _pathHistory = Paths.HistoryDataPath;

	private readonly IFileSystem _fileSystem = Paths.FileSystem;
	
	private Subscription _subscription;
	private ChartCandleElement _candleElement;

	private readonly ChannelExecutor _executor;
	
	public MainWindow()
	{
		InitializeComponent();

		_executor = TimeSpan.FromSeconds(1).CreateExecutorAndRun(ex => ex.LogError());
		
		// ストレージを初期化
		var entityRegistry = new CsvEntityRegistry(_fileSystem, _pathHistory, _executor);
		var storageRegistry = new StorageRegistry
		{
			DefaultDrive = new LocalMarketDataDrive(_fileSystem, _pathHistory)
		};
		
		// 設定済みストレージを使用してコネクタを作成
		_connector = new Connector(
			entityRegistry.Securities, 
			entityRegistry.PositionStorage, 
			new InMemoryExchangeInfoProvider(), 
			storageRegistry, 
			new SnapshotRegistry(_fileSystem, "SnapshotRegistry"));
		
		// メッセージアダプタープロバイダーを登録
		ConfigManager.RegisterService<IMessageAdapterProvider>(
			new InMemoryMessageAdapterProvider(_connector.Adapter.InnerAdapters));
		
		// ファイルが存在する場合はコネクタ設定を読み込み
		if (_fileSystem.FileExists(_connectorFile))
		{
			_connector.Load(_connectorFile.Deserialize<SettingsStorage>(_fileSystem));
		}
		
		// デフォルトのローソク足データタイプ（5 分）を設定
		CandleDataTypeEdit.DataType = TimeSpan.FromMinutes(5).TimeFrame();
	}
}
```

## 接続のセットアップ

```cs
// 接続パラメーターを設定するメソッド
private void Setting_Click(object sender, RoutedEventArgs e)
{
	// コネクタ設定ウィンドウを呼び出し
	if (_connector.Configure(this))
	{
		// 設定をファイルへ保存
		_connector.Save().Serialize(_fileSystem, _connectorFile);
	}
}

// 取引システムへ接続するメソッド
private void Connect_Click(object sender, RoutedEventArgs e)
{
	// インストゥルメント選択用のデータソースとしてコネクタを設定
	SecurityPicker.SecurityProvider = _connector;
	
	// ローソク足受信イベントを購読
	_connector.CandleReceived += Connector_CandleReceived;
	
	// 接続
	_connector.Connect();
}
```

## ローソク足の処理とチャート表示

```cs
// ローソク足受信イベントのハンドラ
private void Connector_CandleReceived(Subscription subscription, ICandleMessage candle)
{
	// チャートにローソク足を描画
	Chart.Draw(_candleElement, candle);
}
```

## ローソク足サブスクリプションの作成

```cs
// インストゥルメントが選択されたときに呼び出されるメソッド
private void SecurityPicker_SecuritySelected(Security security)
{
	// インストゥルメントが選択されているか確認
	if (security == null) 
		return;
	
	// 以前のサブスクリプションが存在する場合は購読解除
	if (_subscription != null) 
		_connector.UnSubscribe(_subscription);
	
	// 選択されたインストゥルメント用の新しいサブスクリプションを作成
	_subscription = new(CandleDataTypeEdit.DataType, security)
	{
		MarketData =
		{
			// 過去 720 日分の履歴データをリクエスト
			From = DateTime.Today.AddDays(-720),
			
			// モード: 履歴データを読み込み、リアルタイムで構築
			BuildMode = MarketDataBuildModes.LoadAndBuild,
		}
	};
	
	// チャートを設定
	Chart.ClearAreas();
	
	// ローソク足を表示するチャートエリアと要素を作成
	var area = new ChartArea();
	_candleElement = new ChartCandleElement();
	
	// エリアと要素をチャートに追加
	Chart.AddArea(area);
	
	// 自動描画のためにチャート要素をサブスクリプションにリンク
	Chart.AddElement(area, _candleElement, _subscription);
	
	// サブスクリプションを開始
	_connector.Subscribe(_subscription);
}
```

## 完全な MainWindow クラスの例

```cs
namespace StockSharp.Samples.Candles.CombineHistoryRealtime;

using System;
using System.Windows;

using Ecng.Common;
using Ecng.Serialization;
using Ecng.Configuration;
using Ecng.ComponentModel;
using Ecng.Logging;
using Ecng.IO;

using StockSharp.Configuration;
using StockSharp.Algo;
using StockSharp.Algo.Storages;
using StockSharp.Algo.Storages.Csv;
using StockSharp.BusinessEntities;
using StockSharp.Xaml;
using StockSharp.Messages;
using StockSharp.Xaml.Charting;
using StockSharp.Charting;

/// <summary>
/// MainWindow.xaml の相互作用ロジック
/// </summary>
public partial class MainWindow
{
	private readonly Connector _connector;
	private const string _connectorFile = "ConnectorFile.json";

	private readonly string _pathHistory = Paths.HistoryDataPath;
	private readonly IFileSystem _fileSystem = Paths.FileSystem;

	private Subscription _subscription;
	private ChartCandleElement _candleElement;

	private readonly ChannelExecutor _executor;
	
	public MainWindow()
	{
		InitializeComponent();

		_executor = TimeSpan.FromSeconds(1).CreateExecutorAndRun(ex => ex.LogError());

		var entityRegistry = new CsvEntityRegistry(_fileSystem, _pathHistory, _executor);
		var storageRegistry = new StorageRegistry
		{
			DefaultDrive = new LocalMarketDataDrive(_fileSystem, _pathHistory)
		};
		_connector = new Connector(
			entityRegistry.Securities, 
			entityRegistry.PositionStorage, 
			new InMemoryExchangeInfoProvider(), 
			storageRegistry, 
			new SnapshotRegistry(_fileSystem, "SnapshotRegistry"));

		// すべてのコネクタを登録
		ConfigManager.RegisterService<IMessageAdapterProvider>(
			new InMemoryMessageAdapterProvider(_connector.Adapter.InnerAdapters));

		if (_fileSystem.FileExists(_connectorFile))
		{
			_connector.Load(_connectorFile.Deserialize<SettingsStorage>(_fileSystem));
		}

		CandleDataTypeEdit.DataType = TimeSpan.FromMinutes(5).TimeFrame();
	}

	protected override void OnClosed(EventArgs e)
	{
		AsyncHelper.Run(_executor.DisposeAsync);

		base.OnClosed(e);
	}

	private void Setting_Click(object sender, RoutedEventArgs e)
	{
		if (_connector.Configure(this))
		{
			_connector.Save().Serialize(_fileSystem, _connectorFile);
		}
	}

	private void Connect_Click(object sender, RoutedEventArgs e)
	{
		SecurityPicker.SecurityProvider = _connector;
		_connector.CandleReceived += Connector_CandleReceived;
		_connector.Connect();
	}

	private void Connector_CandleReceived(Subscription subscription, ICandleMessage candle)
	{
		Chart.Draw(_candleElement, candle);
	}

	private void SecurityPicker_SecuritySelected(Security security)
	{
		if (security == null) return;
		if (_subscription != null) _connector.UnSubscribe(_subscription);

		_subscription = new(CandleDataTypeEdit.DataType, security)
		{
			MarketData =
			{
				From = DateTime.Today.AddDays(-720),
				BuildMode = MarketDataBuildModes.LoadAndBuild,
			}
		};

		// -----------------チャート--------------------------------
		Chart.ClearAreas();

		var area = new ChartArea();
		_candleElement = new ChartCandleElement();

		Chart.AddArea(area);
		Chart.AddElement(area, _candleElement, _subscription);

		_connector.Subscribe(_subscription);
	}
}
```

## サンプルの機能

> [!IMPORTANT]
> ファイルシステムを扱うすべてのクラス（[CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry)、[LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive)、[SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry)）は、コンストラクターで `IFileSystem` インスタンスを必要とします。標準実装には `Paths.FileSystem` を使用してください。`CsvEntityRegistry` は、ディスクアクセスを同期するために `ChannelExecutor` も必要とします。シリアライズメソッド（`Serialize`、`Deserialize`）も、パラメーターとして `IFileSystem` を受け取ります。

1. **ストレージの作成**:
   - [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry) はエンティティの保存に使用され、`IFileSystem` と `ChannelExecutor` を必要とします
   - [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) はストレージへのパスを指定して設定されます
   - [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) はスナップショットを扱うために作成され、`IFileSystem` を必要とします

2. **サブスクリプションの作成**:
   - [Subscription](xref:StockSharp.BusinessEntities.Subscription) クラスが使用されます
   - MarketData の From パラメーターは、履歴を読み込む開始日を指定します
   - 履歴とリアルタイムの自動結合には BuildMode = MarketDataBuildModes.LoadAndBuild が設定されます

3. **チャート表示**:
   - Chart.AddElement メソッドを使用して、チャート要素をサブスクリプションにリンクします
   - 新しいローソク足を受信すると、チャートは自動的に更新されます

4. **イベント処理**:
   - 受信したローソク足を処理するために CandleReceived イベントを購読します
   - 選択されたインストゥルメントが変更されたときに、以前のサブスクリプションを購読解除します

## 拡張機能

このサンプルは、次の機能で拡張できます。

### リアルタイムモードへの遷移の追跡

```cs
// リアルタイムモードへの遷移イベントを購読
_connector.SubscriptionOnline += OnSubscriptionOnline;

// イベントハンドラ
private void OnSubscriptionOnline(Subscription subscription)
{
	if (subscription == _subscription)
	{
		this.GuiAsync(() => StatusLabel.Content = "オンラインモード");
	}
}
```

### 履歴読み込み期間の設定

```cs
// 履歴読み込み期間を設定
private void SetHistoryPeriod(int days)
{
	if (_subscription != null)
	{
		_connector.UnSubscribe(_subscription);
		
		_subscription.MarketData.From = DateTime.Today.AddDays(-days);
		
		_connector.Subscribe(_subscription);
	}
}
```

### 追加のローソク足処理

```cs
// 情報出力を伴う拡張ローソク足処理
private void ExtendedCandleProcessing(Subscription subscription, ICandleMessage candle)
{
	// チャートにローソク足を描画
	Chart.Draw(_candleElement, candle);
	
	// ログへローソク足に関する情報を出力
	this.GuiAsync(() => 
	{
		var status = subscription.State == SubscriptionStates.Online ? "Real-time" : "History";
		LogControl.LogMessage($"{status}: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice}");
	});
}
```
