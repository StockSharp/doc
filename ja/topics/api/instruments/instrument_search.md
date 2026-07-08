# 金融商品の検索

米国株式取引所へのほとんどのコネクター（たとえば [Interactive Brokers](../connectors/stock_market/interactive_brokers.md)、[PolygonIO](../connectors/stock_market/polygonio.md) など）は、[IConnector.Connect](xref:StockSharp.BusinessEntities.IConnector.Connect) メソッドで接続を確立した後、利用可能なすべての金融商品をクライアントへ転送しません。これは、米国の取引所で取引される金融商品の数が非常に多いためであり、ブローカーサーバーやデータソースの負荷を軽減するために行われます。

## 金融商品検索の基本

S# で金融商品を検索するには、マーケットデータの受信と同様に、サブスクリプション機構を使用します。このアプローチにより、金融商品を含むすべての種類のデータに対して統一されたコードを使用できます。

### 金融商品検索用サブスクリプションの作成

金融商品を検索するには、[Subscription](xref:StockSharp.BusinessEntities.Subscription) クラスのインスタンスを、フィルタリングパラメーターを含む [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) メッセージに基づいて作成する必要があります。

```csharp
// 検索用のフィルターオブジェクトを作成します
var lookupMessage = new SecurityLookupMessage
{
	// 検索条件を設定します
	SecurityId = new SecurityId
	{
		// 金融商品コードで検索します（"AAPL*" のようなマスクを使用できます）
		SecurityCode = "AAPL",
		// 任意で、ボードコードを指定できます
		BoardCode = "NASDAQ"
	},
	// 金融商品の種類を指定できます
	SecurityType = SecurityTypes.Stock,
	// トランザクション ID を設定します
	TransactionId = Connector.TransactionIdGenerator.GetNextId()
};

// 金融商品検索用のサブスクリプションを作成します
var subscription = new Subscription(lookupMessage);
```

### 使用可能なフィルタリングパラメーター

[SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) メッセージでは、次の検索条件を設定できます。

- **SecurityId** — 金融商品識別子。次を含みます。
  - **SecurityCode** — 金融商品コード、または金融商品コードのマスク（たとえば "AAPL" または "MS*"）
  - **BoardCode** — 取引所ボードコード（たとえば [ExchangeBoard.Nasdaq](xref:StockSharp.BusinessEntities.ExchangeBoard.Nasdaq)）
- **SecurityType** — 金融商品の種類（[SecurityTypes.Stock](xref:StockSharp.Messages.SecurityTypes.Stock)、[SecurityTypes.Future](xref:StockSharp.Messages.SecurityTypes.Future) など）
- **SecurityTypes** — 高度な検索用の金融商品種類の配列
- **Currency** — 金融商品の取引通貨
- **ExpiryDate** — 有効期限（デリバティブ用）
- **Strike** — 権利行使価格（オプション用）
- **OptionType** — オプションの種類（オプション用）
- **Name** — 金融商品名、またはその一部
- **Class** — 金融商品クラス

### 検索結果の処理

サブスクリプションを作成した後、金融商品を受信するためのイベントを購読し、リクエストを送信する必要があります。

```csharp
// 金融商品受信イベントのハンドラー
private void OnSecurityReceived(Subscription subscription, Security security)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;
		
	Console.WriteLine($"Found instrument: {security.Id} - {security.Name}, Type: {security.Type}");
	
	// ここで金融商品をコレクションに追加するか、その他の処理を実行できます
	Securities.Add(security);
}

// 検索完了イベントのハンドラー
private void OnSubscriptionFinished(Subscription subscription)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;
		
	Console.WriteLine($"Search completed. Instruments found: {Securities.Count}");
}

// サブスクリプションエラーのハンドラー
private void OnSubscriptionFailed(Subscription subscription, Exception error, bool isSubscribe)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;
		
	Console.WriteLine($"Instrument search error: {error.Message}");
}

// イベントを購読します
Connector.SecurityReceived += OnSecurityReceived;
Connector.SubscriptionFinished += OnSubscriptionFinished;
Connector.SubscriptionFailed += OnSubscriptionFailed;

// 金融商品検索リクエストを送信します
Connector.Subscribe(subscription);
```

### 金融商品検索の完全な例

以下は、金融商品を検索するメソッドの完全な例です。

```csharp
public void FindSecurities(string searchCode, SecurityTypes? securityType = null)
{
	// 金融商品検索用のオブジェクトを作成します
	var lookupMessage = new SecurityLookupMessage
	{
		SecurityId = new SecurityId
		{
			SecurityCode = searchCode,
			// 特定のボードで検索する必要がある場合
			// BoardCode = ExchangeBoard.Nyse.Code,
		},
		SecurityType = securityType,
		TransactionId = Connector.TransactionIdGenerator.GetNextId()
	};
	
	// サブスクリプションを作成します
	var subscription = new Subscription(lookupMessage);
	
	// 検索結果用のコレクションをクリアします
	_searchResults.Clear();
	
	// 結果を蓄積するための一時コレクション
	var foundSecurities = new List<Security>();
	
	// 金融商品受信用のサブスクリプション
	void OnSecurityReceived(Subscription sub, Security security)
	{
		if (sub != subscription)
			return;
			
		// 見つかった金融商品をコレクションに追加します
		foundSecurities.Add(security);
		Console.WriteLine($"Found: {security.Id}, {security.Name}");
	}
	
	// 検索完了用のサブスクリプション
	void OnSubscriptionFinished(Subscription sub)
	{
		if (sub != subscription)
			return;
			
		// 結果をメインコレクションにコピーします
		_searchResults.AddRange(foundSecurities);
		
		Console.WriteLine($"Search completed. Instruments found: {foundSecurities.Count}");
		
		// イベントの購読を解除します
		Connector.SecurityReceived -= OnSecurityReceived;
		Connector.SubscriptionFinished -= OnSubscriptionFinished;
		Connector.SubscriptionFailed -= OnSubscriptionFailed;
	}
	
	// サブスクリプションエラーの処理
	void OnSubscriptionFailed(Subscription sub, Exception error, bool isSubscribe)
	{
		if (sub != subscription)
			return;
			
		Console.WriteLine($"Instrument search error: {error.Message}");
		
		// イベントの購読を解除します
		Connector.SecurityReceived -= OnSecurityReceived;
		Connector.SubscriptionFinished -= OnSubscriptionFinished;
		Connector.SubscriptionFailed -= OnSubscriptionFailed;
	}
	
	// イベントを購読します
	Connector.SecurityReceived += OnSecurityReceived;
	Connector.SubscriptionFinished += OnSubscriptionFinished;
	Connector.SubscriptionFailed += OnSubscriptionFailed;
	
	// 検索リクエストを送信します
	Connector.Subscribe(subscription);
}
```

### WPF アプリケーションでの使用例

グラフィカルアプリケーションでは、金融商品の検索はボタンクリックハンドラーから呼び出されることがよくあります。

```csharp
private void FindButton_Click(object sender, RoutedEventArgs e)
{
	// テキストフィールドから検索条件を取得します
	var searchText = SearchTextBox.Text;
	
	if (string.IsNullOrWhiteSpace(searchText))
	{
		MessageBox.Show("Enter a search criterion");
		return;
	}
	
	// 検索サブスクリプションを作成して送信します
	var lookupMessage = new SecurityLookupMessage
	{
		SecurityId = new SecurityId { SecurityCode = searchText },
		// インターフェイスで種類が選択されている場合
		SecurityType = SecurityTypeComboBox.SelectedItem as SecurityTypes?
	};
	
	var subscription = new Subscription(lookupMessage);
	
	// ここで読み込みインジケーターを表示できます
	LoadingIndicator.Visibility = Visibility.Visible;
	
	// リクエストを送信します
	Connector.Subscribe(subscription);
}
```

## SecurityLookupWindow の使用

StockSharp は、金融商品検索用の既製ダイアログ [SecurityLookupWindow](xref:StockSharp.Xaml.SecurityLookupWindow) も提供します。

```csharp
private void ShowSecurityLookupWindow_Click(object sender, RoutedEventArgs e)
{
	var lookupWindow = new SecurityLookupWindow
	{
		// すべての金融商品を検索できるようにするか指定します
		// （コネクターがこの機能をサポートしている場合）
		ShowAllOption = Connector.Adapter.IsSupportSecuritiesLookupAll(),
		
		// 初期検索条件を設定します
		CriteriaMessage = new SecurityLookupMessage
		{
			SecurityId = new SecurityId { SecurityCode = "AAPL" },
			SecurityType = SecurityTypes.Stock
		}
	};
	
	// モーダルダイアログとしてウィンドウを表示します
	if (lookupWindow.ShowModal(this))
	{
		// ユーザーが選択を確定した場合、リクエストを送信します
		Connector.Subscribe(new Subscription(lookupWindow.CriteriaMessage));
	}
}
```

## まとめ

StockSharp のサブスクリプション機構は、金融商品検索を含め、データを取得するための統一された方法を提供します。これにより、異なるコネクターやデータ型を扱う場合にも同じアプローチを使用でき、取引アプリケーションの開発が大幅に簡素化されます。
