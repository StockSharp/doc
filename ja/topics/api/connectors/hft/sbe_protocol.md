# SBE プロトコル

**SBE (Simple Binary Encoding)** のサポートは、低遅延の市場データと取引メッセージ向けにコンパクトなバイナリ転送を提供します。

StockSharp には、レコードをエンコードおよびデコードする [SbeRecordSerializer](xref:StockSharp.Server.Sbe.SbeRecordSerializer)、クライアント接続を受け付ける [SbeServer](xref:StockSharp.Server.Sbe.SbeServer)、クライアント接続用の [StockSharpSBEMessageAdapter](xref:StockSharp.SBE.StockSharpSBEMessageAdapter) が含まれています。

この実装は、銘柄検索、Level1、板情報、ティック、任意のネイティブローソク足、ポートフォリオとポジションのデータ、および注文操作をサポートします。クライアントとサーバーのスキーマ識別子およびバージョンは一致している必要があります。

## 関連項目

[SBE の設定](sbe_protocol/configuration_sbe.md)

[SBE アダプターの初期化](sbe_protocol/adapter_initialization_sbe.md)

[FIX プロトコル](../common/fix_protocol.md)

[FAST プロトコル](../common/fast_protocol.md)
