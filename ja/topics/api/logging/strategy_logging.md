# ストラテジーのログ記録

[Strategy](xref:StockSharp.Algo.Strategies.Strategy) クラスは [ILogSource](xref:Ecng.Logging.ILogSource) インターフェイスを実装しています。そのため、ストラテジーを [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) に渡すことができ、そのすべてのメッセージは自動的に [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners) に送られます。

## 前提条件

[取引ストラテジー](../strategies.md)

## テストファイルへのログ記録

1. まず、専用のマネージャーを作成する必要があります。

   ```cs
   var logManager = new LogManager();
   ```
2. 次に、ファイル名を渡してファイルロガーを作成し、それを [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners) に追加する必要があります。

   ```cs
   var fileListener = new FileLogListener("{0}_{1:00}_{2:00}.txt".Put(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
   logManager.Listeners.Add(fileListener);
   ```
3. メッセージをログに記録するには、ストラテジーを [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) に追加する必要があります。

   ```cs
   logManager.Sources.Add(lkohSmaStrategy);
   ```
4. ストラテジーをログマネージャーに追加すると、そのすべてのメッセージがファイルに記録されます。

## サウンド再生

1. ロガーを作成し、サウンドファイルの名前を渡します。

   ```cs
   var soundListener = new SoundLogListener("error.mp3");
   						
   logManager.Listeners.Add(soundListener);
   logManager.Sources.Add(lkohSmaStrategy);
   ```
2. メッセージタイプが [LogLevels.Error](xref:Ecng.Logging.LogLevels.Error) の場合にのみサウンドが再生されるようにフィルターを設定します。

   ```cs
   soundListener.Filters.Add(msg => msg.Level == LogLevels.Error);
   ```

## メール送信

1. ロガーを作成し、送信するメールのパラメーターを渡します。

   ```cs
   var emailListener = new EmailLogListener("from@stocksharp.com", "to@stocksharp.com");
   logManager.Listeners.Add(emailListener);
   logManager.Sources.Add(lkohSmaStrategy);
   ```
2. [LogLevels.Error](xref:Ecng.Logging.LogLevels.Error) および [LogLevels.Warning](xref:Ecng.Logging.LogLevels.Warning) タイプのメッセージ送信に対してフィルターを設定します。

   ```cs
   emailListener.Filters.Add(msg => msg.Level == LogLevels.Error);
   emailListener.Filters.Add(msg => msg.Level == LogLevels.Warning);
   ```

## LogWindow へのログ記録

1. [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) ロガーを作成します。

   ```cs
    // 各ストラテジーはそれぞれ独自のウィンドウを持ちます
   var guiListener = new GuiLogListener();
   logManager.Listeners.Add(guiListener);
   logManager.Sources.Add(lkohSmaStrategy);
   ```
2. ストラテジー動作中のログウィンドウは次のとおりです: ![ストラテジーのログ記録 のスクリーンショット](../../../images/strategy_logging.png)

## 推奨コンテンツ

[ビジュアルログコンポーネント](../graphical_user_interface/logging.md)

