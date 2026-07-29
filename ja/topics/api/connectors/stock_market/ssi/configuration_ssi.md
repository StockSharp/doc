# コネクタ設定: SSI

SSI に接続する前に、次のアダプタープロパティを設定します。この一覧は [SSIMessageAdapter](xref:StockSharp.SSI.SSIMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ClientId` (`string`)
- `Account` (`string`)

## 詳細設定

これらのプロパティは、認証とセッション状態、接続先、ストリーミング、定期照会を制御します。

- `PrivateKey` (`SecureString`)
- `Otp` (`SecureString`)
- `RestEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `PollingInterval` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_ssi.md)

[アダプターの初期化](adapter_initialization_ssi.md)
