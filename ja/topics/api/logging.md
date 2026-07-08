# ロギング

[S#](../api.md) で書かれた取引アルゴリズムを監視するには、特別な [LogManager](xref:Ecng.Logging.LogManager) クラスを使用できます。このクラスは、[LogMessage](xref:Ecng.Logging.LogMessage) メッセージを [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) から [ILogSource.Log](xref:Ecng.Logging.ILogSource.Log) イベントを通じて受け取り、それらを [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners) リスナーへ渡します。したがって、アルゴリズムコードはデバッグ情報（たとえば、動作中に発生したエラーや、数学的計算に関する追加情報）を渡すことができ、[LogManager](xref:Ecng.Logging.LogManager) がその情報をオペレーターにどのように表示するかを決定します。 

通常、[S#](../api.md) には [ILogListener](xref:Ecng.Logging.ILogListener) の次の実装が含まれており、どれを選ぶかによってストラテジーから受け取ったメッセージの渡し先が変わります。 

1. [FileLogListener](xref:Ecng.Logging.FileLogListener) - メッセージをテキストファイルに書き込みます。すでに作成済みのアルゴリズムで使用し、不可抗力の状況でログを利用する場合に推奨されます。 
2. [ConsoleLogListener](xref:Ecng.Logging.ConsoleLogListener) - メッセージをコンソールウィンドウへ出力します（アルゴリズムにウィンドウがない場合は自動的に作成されます）。アルゴリズムのデバッグおよびテストで使用することが推奨されます。 
3. [DebugLogListener](xref:Ecng.Logging.DebugLogListener) - メッセージをデバッグウィンドウへ出力します。このウィンドウは [DebugView](https://technet.microsoft.com/en-us/sysinternals/bb896647.aspx) などの特別なプログラムで確認できます。アルゴリズムのデバッグおよびテストで使用することが推奨されます。 
4. [EmailLogListener](xref:Ecng.Logging.EmailLogListener) - 指定されたメールアドレスにメッセージを送信します。アルゴリズムが管理外のコンピューター（ホスティング業者のサーバー）に配置されている場合に使用することが推奨されます。 
5. [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) - 特別な [LogControl](xref:StockSharp.Xaml.LogControl) ウィンドウを通じてメッセージを表示します。すべてのメッセージを単一のウィンドウに出力するモードと、各 [ILogSource](xref:Ecng.Logging.ILogSource) に対して個別のウィンドウを作成するモードの 2 つで動作できます。アルゴリズムにグラフィカルインターフェイスがある場合に使用することが推奨されます。 

[LogListener](xref:Ecng.Logging.LogListener) は、[LogListener.Filters](xref:Ecng.Logging.LogListener.Filters) プロパティを通じてメッセージをフィルタリングするように構成できます。たとえば、フィルターを通じて、どの種類のメッセージを処理するかを指定できます。これは、[EmailLogListener](xref:Ecng.Logging.EmailLogListener) を使用する場合に特に有用です。たとえば、各デバッグメッセージではなく、緊急状況（取引アルゴリズムのエラー）の場合にのみメールを送信するようにできます。 

## 次のステップ

[ストラテジーのロギング](logging/strategy_logging.md)

[IConnector のロギング](logging/iconnector_logging.md)

[その他のログソース](logging/other_logs_sources.md)

[視覚的な監視](logging/visual_monitoring.md)

[ILogListener の作成](logging/custom_iloglistener.md)
