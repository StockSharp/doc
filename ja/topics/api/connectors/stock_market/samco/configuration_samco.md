# コネクタ設定: Samco

Samco に接続する前に、次のアダプタープロパティを設定します。この一覧は [SamcoMessageAdapter](xref:StockSharp.Samco.SamcoMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)

## 詳細設定

これらのプロパティは、認証とセッション状態、接続先、ストリーミング、定期照会を制御します。

- `Secret` (`SecureString`)
- `SessionToken` (`SecureString`)
- `RestEndpoint` (`string`)
- `InstrumentEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_samco.md)

[アダプターの初期化](adapter_initialization_samco.md)
