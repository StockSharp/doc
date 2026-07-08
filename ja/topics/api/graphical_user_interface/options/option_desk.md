# オプションデスク

[OptionDesk](xref:StockSharp.Xaml.OptionDesk) グラフィカルコンポーネントは、オプションデスクを表示するためのテーブルです。Put オプションと Call オプションについて、“Greeks”、インプライドボラティリティ、理論価格、最良売気配と最良買気配を表示します。 

以下は、このコンポーネントを使用する **OptionCalculator** の例です。この例のソースコードは、*Samples\/06\_Strategies\/09\_LiveOptionsQuoting* フォルダーにあります。

![option desk](../../../../images/option_desk.png)

## OptionCalculator の例

1. XAML コードで、[OptionDesk](xref:StockSharp.Xaml.OptionDesk) 要素を追加し、**Desk** という名前を割り当てます。 

   ```xaml
   <Window x:Class="OptionCalculator.MainWindow"
           xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
           xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
           xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
           xmlns:xaml="http://schemas.stocksharp.com/xaml"
           Title="{x:Static loc:LocalizedStrings.XamlStr396}" Height="400" Width="1030">
       <Grid Margin="5,5,5,5">
       
   	    .........................................................
   	    
   	    <xaml:OptionDesk x:Name="Desk" Grid.Row="6" Grid.ColumnSpan="3" Grid.Column="0" />
       
   	</Grid>
   </Window>
   	  				
   ```
2. C# コードで、接続を作成し、必要なイベントを購読します。 

   ```cs
   ...                 
   public readonly Connector Connector = new Connector();
   ...                 
   // 接続成功イベントを購読します
   Connector.Connected += () =>
   {
   	// GUI ラベルを更新します
   	this.GuiAsync(() => ChangeConnectStatus(true));
   };
   // 切断イベントを購読します
   Connector.Disconnected += () =>
   {
   	// GUI ラベルを更新します
   	this.GuiAsync(() => ChangeConnectStatus(false));
   };
   // 接続エラーイベントを購読します
   Connector.ConnectionError += error => this.GuiAsync(() =>
   {
   	// GUI ラベルを更新します
   	ChangeConnectStatus(false);
   	MessageBox.Show(this, error.ToString(), LocalizedStrings.ErrorConnection);
   });
   // 原資産のリストを埋めます
   Connector.SecurityReceived += (sub, security) =>
   {
   	if (security.Type == SecurityTypes.Future)
   		_assets.Add(security);
   };
   Connector.Level1Received += (sub, security) =>
   {
   	if (_model.UnderlyingAsset == security || _model.UnderlyingAsset.Id == security.UnderlyingSecurityId)
   		_isDirty = true;
   };
   // ティック価格を購読し、資産価格を更新します
   Connector.TickTradeReceived += (sub, trade) =>
   {
   	if (_model.UnderlyingAsset == trade.Security || _model.UnderlyingAsset.Id == trade.Security.UnderlyingSecurityId)
   		_isDirty = true;
   };
   Connector.NewPosition += position => this.GuiAsync(() =>
   {
   	var asset = SelectedAsset;
   	if (asset == null)
   		return;
   	var assetPos = position.Security == asset;
   	var newPos = position.Security.UnderlyingSecurityId == asset.Id;
   	if (!assetPos && !newPos)
   		return;
   	if (assetPos)
   		PosChart.AssetPosition = position;
   	if (newPos)
   		PosChart.Positions.Add(position);
   	RefreshChart();
   });
   Connector.PositionChanged += position => this.GuiAsync(() =>
   {
   	if ((PosChart.AssetPosition != null && PosChart.AssetPosition == position) || PosChart.Positions.Cache.Contains(position))
   		RefreshChart();
   });
   try
   {
   	if (File.Exists(_settingsFile))
   		Connector.Load(new JsonSerializer<SettingsStorage>().Deserialize(_settingsFile));
   }
   ...
   ```
3. 接続時に、マーケットデータ用のメッセージプロバイダーを設定します。

   ```cs
   private void ConnectClick(object sender, RoutedEventArgs e)
   {
   	if (!_isConnected)
   	{
   		ConnectBtn.IsEnabled = false;
   		_model.Clear();
   		_model.MarketDataProvider = Connector;
   ...
   		Connector.Connect();
   	}
   	else
   		Connector.Disconnect();
   }
   ```
4. 銘柄を受信したら、原資産をリストに追加します。

   ```cs
   // 原資産のリストを埋めます
   Connector.SecurityReceived += (sub, security) =>
   {
   	if (security.Type == SecurityTypes.Future)
   		_assets.Add(security);
   };
   ```
5. 銘柄を選択する場合:
   - 選択された銘柄を原資産として扱うオプションチェーンの配列を埋めます。
   - この配列を [OptionDeskModel.Options](xref:StockSharp.Xaml.OptionDeskModel.Options) プロパティに割り当てます。
   - [OptionDeskModel.Clear](xref:StockSharp.Xaml.OptionDeskModel.Clear) メソッドを使用してオプションボードの値をクリアします。
   ```cs
   private void Assets_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
   {
   	var asset = SelectedAsset;
   	_model.UnderlyingAsset = asset;
   	_model.Clear();
   	_options.Clear();
   	var options = asset.GetDerivatives(Connector);
   	foreach (var security in options)
   	{
   		_model.Add(security);
   		_options.Add(security);
   	}
   	ProcessPositions();
   }
   ```

## 推奨コンテンツ

[グリークス](../../options/greeks.md)
