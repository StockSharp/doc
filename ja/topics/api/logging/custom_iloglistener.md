# カスタム ILogListener

[ILogListener](xref:Ecng.Logging.ILogListener) の独自実装を作成したい場合（たとえば、メッセージをデータベースに保存したい場合）、[LogListener](xref:Ecng.Logging.LogListener) クラスを継承するか、[ILogListener](xref:Ecng.Logging.ILogListener) インターフェイスを直接実装できます。[LogMessage](xref:Ecng.Logging.LogMessage) クラスのオブジェクトは、[ILogListener.WriteMessages](xref:Ecng.Logging.ILogListener.WriteMessages(System.Collections.Generic.IEnumerable{Ecng.Logging.LogMessage}))**(**[System.Collections.Generic.IEnumerable\<Ecng.Logging.LogMessage\>](xref:System.Collections.Generic.IEnumerable`1) messages **)** メソッドを通じて渡されます。このクラスには、メッセージソース [LogMessage.Source](xref:Ecng.Logging.LogMessage.Source)（たとえば、メッセージを生成したストラテジー）、メッセージタイプ [LogMessage.Level](xref:Ecng.Logging.LogMessage.Level)（情報、警告、またはエラー）、および [LogMessage.Message](xref:Ecng.Logging.LogMessage.Message) のテキスト自体に関する情報が含まれます。次の例は [EmailLogListener](xref:Ecng.Logging.EmailLogListener) のソースコードを示しています。 

```cs
/// <summary>
public class EmailLogListener : LogListener
{
	public EmailLogListener(string from, string to)
	{
		if (from.IsEmpty())
			throw new ArgumentNullException("from");
		if (to.IsEmpty())
			throw new ArgumentNullException("to");
		From = from;
		To = to;
	}
	public string From { get; private set; }
	public string To { get; private set; }
	protected override void OnWriteMessage(LogMessage message)
	{
		var email = new SmtpClient();
		email.Send(new MailMessage(From, To, message.Source.Name + " " + message.Level, message.Message));
	}
}
```
