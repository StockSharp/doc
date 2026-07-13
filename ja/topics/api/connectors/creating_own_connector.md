# 独自コネクターの作成

メッセージングメカニズムは、[StockSharp](https://github.com/StockSharp/StockSharp) アーキテクチャの内部論理レイヤーであり、標準プロトコルを使用してプラットフォームのさまざまな要素間の相互作用を提供します。

主なクラスは 2 つあります。

- [Message](xref:StockSharp.Messages.Message) - 情報を運ぶメッセージ。
- [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter) - メッセージアダプター（=コンバーター）。

**メッセージ** は情報を送信するエージェントとして機能します。メッセージには独自の型 [MessageTypes](xref:StockSharp.Messages.MessageTypes) があります。各メッセージ型は特定のクラスに対応します。さらに、すべてのメッセージクラスは抽象クラス [Message](xref:StockSharp.Messages.Message) から継承されます。この抽象クラスは、メッセージ型 [Message.Type](xref:StockSharp.Messages.Message.Type) や [Message.LocalTime](xref:StockSharp.Messages.Message.LocalTime)（メッセージの作成または受信のローカル時刻）などのプロパティを派生クラスに付与します。

メッセージには *入力* と *出力* があります。

- *入力* メッセージ - 外部システムへ送信されるメッセージ。通常、これらはプログラムによって生成されるコマンドです。たとえば、[ConnectMessage](xref:StockSharp.Messages.ConnectMessage) メッセージは、サーバーへの接続を要求するコマンドです。
- *出力* メッセージ - 外部システムから届くメッセージ。これらは、マーケットデータ、トランザクション、ポートフォリオ、接続イベントなどに関する情報を伝達するメッセージです。たとえば、[QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) メッセージは板情報の変化に関する情報を伝達します。

**メッセージアダプター** は、取引システムとプログラムの間の仲介役を果たします。コネクターの種類ごとに、抽象クラス [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter) から継承された個別のアダプタークラスがあります。

アダプターは主に 2 つの機能を実行します。

1. 受信メッセージを特定の取引システムのコマンドへ変換します。
2. 取引システムから受け取った情報（接続、マーケットデータ、トランザクションなど）を送信メッセージへ変換します。

以下では、[Coinbase](https://github.com/StockSharp/StockSharp/tree/master/Connectors/Coinbase) 用の独自アダプターを作成するプロセスについて説明します（ソースコード付きのすべてのコネクターは [StockSharp リポジトリ](https://github.com/StockSharp/StockSharp/tree/master/Connectors) で利用でき、チュートリアルとして提供されています）。

## Coinbase メッセージアダプター作成例

### 1. アダプタークラスの作成

まず、抽象クラス [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter) から継承した **CoinbaseMessageAdapter** メッセージアダプタークラスを作成します。

```cs
public partial class CoinbaseMessageAdapter : AsyncMessageAdapter
{
	private Authenticator _authenticator;
	private HttpClient _restClient;
	private SocketClient _socketClient;

	// その他のアダプターフィールドとプロパティ
}
```

### 2. アダプターコンストラクター

アダプターコンストラクターでは、次の操作を行う必要があります。

1. メッセージ ID の作成に使用されるトランザクション ID ジェネレーターを渡します。

2. 次のメソッドを使用して、サポートされるメッセージ型を指定します。
 - [AddMarketDataSupport](xref:StockSharp.Messages.Extensions.AddMarketDataSupport(StockSharp.Messages.MessageAdapter)) - マーケットデータを購読するためのメッセージのサポート。
 - [AddTransactionalSupport](xref:StockSharp.Messages.Extensions.AddTransactionalSupport(StockSharp.Messages.MessageAdapter)) - トランザクションメッセージのサポート。

3. [AddSupportedMarketDataType](xref:StockSharp.Messages.Extensions.AddSupportedMarketDataType(StockSharp.Messages.MessageAdapter,StockSharp.Messages.DataType)) メソッドを使用して、アダプターがサポートする具体的なマーケットデータ型を指定します。

```cs
public CoinbaseMessageAdapter(IdGenerator transactionIdGenerator)
	: base(transactionIdGenerator)
{
	HeartbeatInterval = TimeSpan.FromSeconds(5);

	// マーケットデータとトランザクションのサポートを追加
	this.AddMarketDataSupport();
	this.AddTransactionalSupport();

	// サポートされていないメッセージ型を削除
	this.RemoveSupportedMessage(MessageTypes.Portfolio);
	this.RemoveSupportedMessage(MessageTypes.OrderGroupCancel);

	// サポートされるマーケットデータ型を追加
	this.AddSupportedMarketDataType(DataType.Ticks);
	this.AddSupportedMarketDataType(DataType.MarketDepth);
	this.AddSupportedMarketDataType(DataType.Level1);
	this.AddSupportedMarketDataType(DataType.CandleTimeFrame);
}
```

### 3. アダプターの接続と切断

アダプターを取引システムに接続するには、[AsyncMessageAdapter.ConnectAsync](xref:StockSharp.Messages.AsyncMessageAdapter.ConnectAsync(StockSharp.Messages.ConnectMessage,System.Threading.CancellationToken)) メソッドが呼び出されます。このメソッドには、受信 [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) メッセージが渡されます。接続に成功すると、アダプターは送信 [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) メッセージを送信します。

```cs
public override async ValueTask ConnectAsync(ConnectMessage connectMsg, CancellationToken cancellationToken)
{
	// トランザクションモード用のキーの存在を確認
	if (this.IsTransactional())
	{
		if (Key.IsEmpty())
			throw new InvalidOperationException(LocalizedStrings.KeyNotSpecified);

		if (Secret.IsEmpty())
			throw new InvalidOperationException(LocalizedStrings.SecretNotSpecified);
	}

	// 認証器を初期化
	_authenticator = new(this.IsTransactional(), Key, Secret, Passphrase);

	// クライアントがまだ作成されていないことを確認
	if (_restClient != null)
		throw new InvalidOperationException(LocalizedStrings.NotDisconnectPrevTime);

	if (_socketClient != null)
		throw new InvalidOperationException(LocalizedStrings.NotDisconnectPrevTime);

	// REST クライアントを作成
	_restClient = new(_authenticator) { Parent = this };

	// WebSocket クライアントを作成して設定
	_socketClient = new(_authenticator, ReConnectionSettings.ReAttemptCount) { Parent = this };
	SubscribePusherClient();

	// WebSocket クライアントに接続
	await _socketClient.Connect(cancellationToken);

	// 接続成功メッセージを送信
	SendOutMessage(new ConnectMessage());
}
```

アダプターを取引システムから切断するには、[AsyncMessageAdapter.DisconnectAsync](xref:StockSharp.Messages.AsyncMessageAdapter.DisconnectAsync(StockSharp.Messages.DisconnectMessage,System.Threading.CancellationToken)) メソッドが呼び出されます。切断に成功すると、アダプターは送信 [DisconnectMessage](xref:StockSharp.Messages.DisconnectMessage) メッセージを送信します。

```cs
public override ValueTask DisconnectAsync(DisconnectMessage disconnectMsg, CancellationToken cancellationToken)
{
	// クライアントが作成されていることを確認
	if (_restClient == null)
		throw new InvalidOperationException(LocalizedStrings.ConnectionNotOk);

	if (_socketClient == null)
		throw new InvalidOperationException(LocalizedStrings.ConnectionNotOk);

	// REST クライアントのリソースを解放
	_restClient.Dispose();
	_restClient = null;

	// WebSocket クライアントを切断
	_socketClient.Disconnect();

	// 切断メッセージを送信
	SendOutDisconnectMessage(true);
	return default;
}
```

さらに、アダプターは状態をリセットするための [AsyncMessageAdapter.ResetAsync](xref:StockSharp.Messages.AsyncMessageAdapter.ResetAsync(StockSharp.Messages.ResetMessage,System.Threading.CancellationToken)) メソッドを提供します。このメソッドは接続を閉じ、アダプターを初期状態に戻します。

```cs
public override ValueTask ResetAsync(ResetMessage resetMsg, CancellationToken cancellationToken)
{
	// REST クライアントのリソースを解放
	if (_restClient != null)
	{
		try
		{
			_restClient.Dispose();
		}
		catch (Exception ex)
		{
			SendOutError(ex);
		}

		_restClient = null;
	}

	// WebSocket クライアントを切断してクリア
	if (_socketClient != null)
	{
		try
		{
			UnsubscribePusherClient();
			_socketClient.Disconnect();
		}
		catch (Exception ex)
		{
			SendOutError(ex);
		}

		_socketClient = null;
	}

	// 認証器のリソースを解放
	if (_authenticator != null)
	{
		try
		{
			_authenticator.Dispose();
		}
		catch (Exception ex)
		{
			SendOutError(ex);
		}

		_authenticator = null;
	}

	// 追加データをクリア
	_candlesTransIds.Clear();

	// リセットメッセージを送信
	SendOutMessage(new ResetMessage());
	return default;
}
```

### 開発完了後

コネクターを実装したら、使用方法には 2 つの選択肢があります。

1. 有料または無料の製品として [StockSharp Store](https://stocksharp.com/store) に公開します。この場合、ユーザーは [インストーラー](../../installer/setup.md) を通じてコネクターを自動的にインストールします。
2. 個人利用の場合は、ビルド済みのコネクター *.dll* ファイルをアプリケーション（または任意の StockSharp 製品）のフォルダーにコピーします。起動時に、アプリケーションは次の基準で現在のディレクトリをスキャンしてアダプターを探します。

   - 名前が `StockSharp.` で始まる **.dll** 拡張子のファイルのみが対象になります。
   - 残りの各ファイルが、有効な .NET アセンブリであることを確認されます。
   - アセンブリが読み込まれ、`IMessageAdapter` を実装するすべての型が収集されます。
   - スキャンまたは読み込み中に発生したエラーはログに書き込まれ、検索は停止されません。読み込みに失敗した場合は、アプリケーションのログウィンドウまたはログファイルを開いてエラー詳細を確認してください。

このドキュメントでは、アダプターの動作、その作成、および取引システムとの接続管理に関する一般原則について説明しました。以下のドキュメントでは、アダプター機能の実装について扱います。

- [銘柄検索](creating_own_connector/instrument_lookup.md)
- [マーケットデータの操作](creating_own_connector/market_data.md)
- [ポートフォリオと注文の現在状態の要求](creating_own_connector/portfolio_and_orders_state.md)
- [取引操作の処理](creating_own_connector/trading_operations.md)
- [設定の保存](creating_own_connector/settings.md)
- [拡張注文条件](creating_own_connector/order_extended.md)
