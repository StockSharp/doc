# コネクターの設定: Bloomberg BLPAPI and EMSX

公式の開発者向けパッケージをインストールし、そのパス、認証情報、その他の接続パラメーターを指定します。

- `ServerAddress` - サービスのアドレスです。 既定値: `new DnsEndPoint("localhost", 8194)`.
- `SdkPath` - ローカルファイルまたはディレクトリへのパスです。
- `IsEmsxEnabled` - コネクターの動作を制御する切り替え項目です。
- `EmsxService` - 接続パラメーターです。 既定値: `//blp/emapisvc`.
- `Broker` - 接続パラメーターです。
