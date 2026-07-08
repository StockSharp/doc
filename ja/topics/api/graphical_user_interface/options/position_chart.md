# ポジションチャート

[OptionPositionChart](xref:StockSharp.Xaml.Charting.OptionPositionChart) グラフィカルコンポーネントは、原資産に関連するポジションとオプションの「Greeks」を表示するチャートです。

以下は、このチャートを使用する SampleOptionQuoting の例です。この例のソースコードは *Samples\/06\_Strategies\/09\_LiveOptionsQuoting* フォルダーにあります。

![option volsmile](../../../../images/option_volsmile.png)

## SampleOptionQuoting の例

1. XAML コードで、[OptionPositionChart](xref:StockSharp.Xaml.Charting.OptionPositionChart) 要素を追加し、**PosChart** という名前を割り当てます。

   ```xaml
   <Window x:Class="OptionCalculator.MainWindow"
           xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
           xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
           xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
           xmlns:xaml="http://schemas.stocksharp.com/xaml"
           Title="{x:Static loc:LocalizedStrings.XamlStr396}" Height="400" Width="1030">
       <Grid Margin="5,5,5,5">
       
   	    .........................................................
   	    
   	    <xaml:OptionPositionChart x:Name="PosChart" Grid.Row="7" Grid.Column="0" Grid.ColumnSpan="6" />
   	</Grid>
   </Window>
   				
   ```

2. C# コードで、接続を作成し、必要なイベントを購読します。

   ```cs
   ...                 
   public readonly Connector Connector = new Connector();
   ...                 
   // 接続成功イベントを購読
   Connector.Connected += () =>
   {
   	// GUI ラベルを更新
   	this.GuiAsync(() => ChangeConnectStatus(true));
   };
   // 切断イベントを購読
   Connector.Disconnected += () =>
   {
   	// GUI ラベルを更新
   	this.GuiAsync(() => ChangeConnectStatus(false));
   };
   // 接続エラーイベントを購読
   Connector.ConnectionError += error => this.GuiAsync(() =>
   {
   	// GUI ラベルを更新
   	ChangeConnectStatus(false);
   	MessageBox.Show(this, error.ToString(), LocalizedStrings.ErrorConnection);
   });
   // 原資産のリストを埋める
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
   // ティック価格を購読し、資産価格を更新する
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

3. 接続時に、コントロールの初期設定を行います。

   1. [OptionPositionChart.Model](xref:StockSharp.Xaml.Charting.OptionPositionChart.Model) コントロールのモデルをリセットします。 
   2. 初期値でチャートを再描画します: [OptionPositionChart.Refresh](xref:StockSharp.Xaml.Charting.OptionPositionChart.Refresh(System.Nullable{System.Decimal},System.Nullable{System.DateTimeOffset},System.Nullable{System.DateTimeOffset}))**(**[System.Nullable\<System.Decimal\>](xref:System.Nullable`1) assetPrice, [System.Nullable\<System.DateTimeOffset\>](xref:System.Nullable`1) currentTime, [System.Nullable\<System.DateTimeOffset\>](xref:System.Nullable`1) expiryDate **)**。 
   3. マーケットデータと銘柄のメッセージプロバイダーを指定します。

   ```cs
   private void ConnectClick(object sender, RoutedEventArgs e)
   {
   	if (!_isConnected)
   	{
   		ConnectBtn.IsEnabled = false;
   ...
   		PosChart.Model = null;
   ...
   		PosChart.MarketDataProvider = Connector;
   		PosChart.SecurityProvider = Connector;
   		PosChart.PositionProvider = Connector;
   		Connector.Connect();
   	}
   	else
   		Connector.Disconnect();
   }
   ```

4. 銘柄を受信したとき、原資産をリストに追加します。

   ```cs
   Connector.SecurityReceived += (sub, security) =>
   {
   	if (security.Type == SecurityTypes.Future)
   		_assets.Add(security);
   };
   ```

5. 原資産またはオプションの Level1 が変更された場合、および新しい約定を取得した場合に、\_isDirty フラグを設定します。これにより、タイマーイベント内で RefreshChart メソッド（下記参照）を呼び出してチャートを再描画できます（コードは省略されています）。このようにして再描画の頻度を制御します。

   ```cs
   Connector.Level1Received += (sub, security) =>
   {
   	if (_model.UnderlyingAsset == security || _model.UnderlyingAsset.Id == security.UnderlyingSecurityId)
   		_isDirty = true;
   };
   Connector.TickTradeReceived += (sub, trade) =>
   {
   	if (_model.UnderlyingAsset == trade.Security || _model.UnderlyingAsset.Id == trade.Security.UnderlyingSecurityId)
   		_isDirty = true;
   };
   ```

6. 新しいポジションの発生イベントハンドラーで、チャートを再描画するために `RefreshChart` を呼び出します。

   ```cs
   Connector.NewPosition += position => this.GuiAsync(() =>
   {
   	var asset = SelectedAsset;
   	if (asset == null)
   		return;
   	var assetPos = position.Security == asset;
   	var newPos = position.Security.UnderlyingSecurityId == asset.Id;
   	if (!assetPos && !newPos)
   		return;
   	RefreshChart();
   });
   Connector.PositionChanged += position => this.GuiAsync(() =>
   {
   	if ((PosChart.AssetPosition != null && PosChart.AssetPosition == position) || PosChart.Positions.Cache.Contains(position))
   		RefreshChart();
   });
   ```

7. このメソッドはチャートを再描画します。

   ```cs
   private void RefreshChart()
   {
   	var asset = SelectedAsset;
   	var trade = asset.LastTrade;
   	if (trade != null)
   		PosChart.Refresh(trade.Price);
   }
   ```

## 推奨コンテンツ

[ボラティリティ取引](../../options/volatility_trading.md)
