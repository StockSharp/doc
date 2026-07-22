# SBE の設定

SBE サーバー管理者から提供されたパラメーターを使用してクライアント接続を設定します。

- `Address` - SBE サーバーの TCP エンドポイント。
- `SenderCompId` - ログオン時に使用するクライアント識別子。
- `TargetCompId` - 接続先サーバーの識別子。
- `Password` - 認証情報。
- `IsSupportNativeCandles` - サーバーから提供されるローソク足を有効にします。無効の場合、クライアントが市場データからローソク足を構築します。

サーバーは [SbeServerSettings](xref:StockSharp.Server.Sbe.SbeServerSettings) を使用します。

- `IsEnabled` - SBE エンドポイントを有効にします。
- `Address` - 待ち受けエンドポイント。既定値は `127.0.0.1:5002` です。
- `HeartBeat` - セッションのハートビート間隔。既定値は 60 秒です。
- `TargetCompId` - サーバー識別子。既定値は `StockSharp` です。

クライアントとサーバーでは同じ SBE スキーマバージョンを使用してください。
