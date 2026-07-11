# その他のログソース

前のトピックでは、[S#](../../api.md) クラスに組み込まれたオブジェクトがログのソースでした。[S#](../../api.md) は、ログのソースが独自クラスである場合、またはログのソースを特定のクラスに関連付ける必要はなくアプリケーション全体に役立てる場合のための機能を提供します。前者の場合は、自分のクラスで [ILogSource](xref:Ecng.Logging.ILogSource) インターフェイスを実装するか、[BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver) を継承する必要があります。後者の場合は、.NET トレースシステムを使用する [TraceSource](xref:Ecng.Logging.TraceSource) を使用できます。これを行う方法は、*Samples\/08\_Misc\/01\_Logging* サンプルに示されています。

## ロギングサンプル

1. [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver) を継承するカスタムクラスを作成します。

   ```cs
   private class TestSource : BaseLogReceiver
   {
   }
   ```
2. [LogManager](xref:Ecng.Logging.LogManager) を作成し、ユーザークラスの変数を宣言します。

   ```cs
   private readonly LogManager _logManager = new LogManager();
   private readonly TestSource _testSource;
   				
   ```
3. ログソースを追加します。

   ```cs
   _logManager.Sources.Add(_testSource = new TestSource());
   _logManager.Sources.Add(new Ecng.Logging.TraceSource());
   				
   ```
4. ログリスナーを追加します。

   ```cs
   // ログメッセージは GUI コンポーネントに表示されます
   _logManager.Listeners.Add(new GuiLogListener(Monitor));
   // ファイルにも書き込みます
   _logManager.Listeners.Add(new FileLogListener
   {
   	FileName = "logs",
   });
   				
   ```
5. カスタムクラスのロギングメッセージを追加します。ロギングレベルはランダムに選択されます。

   ```cs
   var level = RandomGen.GetEnum<LogLevels>();
   switch (level)
   {
   	case LogLevels.Inherit:
   	case LogLevels.Debug:
   	case LogLevels.Info:
   	case LogLevels.Off:
   		_testSource.AddInfoLog("{0} (source)!!!".Put(level));
   		break;
   	case LogLevels.Warning:
   		_testSource.AddWarningLog("Warning (source)!!!");
   		break;
   	case LogLevels.Error:
		_testSource.AddErrorLog("エラー (ソース)!!!");
   		break;
   	default:
   		throw new ArgumentOutOfRangeException();
   }
   ```
6. トレースメッセージを追加します。

   ```cs
   var level = RandomGen.GetEnum<LogLevels>();
   switch (level)
   {
   	case LogLevels.Inherit:
   	case LogLevels.Debug:
   	case LogLevels.Info:
   	case LogLevels.Off:
   		Trace.TraceInformation("{0} (trace)!!!".Put(level));
   		break;
   	case LogLevels.Warning:
   		Trace.TraceWarning("Warning (trace)!!!");
   		break;
   	case LogLevels.Error:
   		Trace.TraceError("Error (trace)!!!");
   		break;
   	default:
   		throw new ArgumentOutOfRangeException();
   }
   ```
7. サンプル実行の結果。![ログ記録の例](../../../images/sample_logging.png)
