# コネクタ設定: IIFL

IIFL に接続する前に、次のアダプタープロパティを設定します。この一覧は [IIFLMessageAdapter](xref:StockSharp.IIFL.IIFLMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ClientId` (`string`)

## 詳細設定

これらのプロパティは、認証とセッション状態、接続先、ストリーミング、定期照会を制御します。

- `AuthorizationCode` (`string`)
- `SessionToken` (`SecureString`)
- `PortfolioName` (`string`)
- `RestEndpoint` (`string`)
- `BridgeHost` (`string`)
- `BridgePort` (`int`)
- `TokenValidationEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_iifl.md)

[アダプターの初期化](adapter_initialization_iifl.md)
