# コネクタ設定: TraderMade

TraderMade に接続する前に、次のアダプタープロパティを設定します。この一覧は [TraderMadeMessageAdapter](xref:StockSharp.TraderMade.TraderMadeMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `RestKey` (`SecureString`)

## 詳細設定

これらのプロパティは、REST と WebSocket のアクセス、接続先、市場データオプション、シンボルフィルター、結果上限を制御します。

- `StreamingKey` (`SecureString`)
- `RestEndpoint` (`Uri`)
- `StreamingEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `EnableLadder` (`bool`)
- `Weekend` (`bool`)
- `QuoteCurrencies` (`string`)
- `Symbols` (`string`)
- `MaximumSecurities` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_tradermade.md)

[アダプターの初期化](adapter_initialization_tradermade.md)
