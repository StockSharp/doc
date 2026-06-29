# 自定义 ILogListener

如果你想创建自己的 [ILogListener](xref:Ecng.Logging.ILogListener) 实现（例如，当你想将消息保存到数据库中时），可以继承 [LogListener](xref:Ecng.Logging.LogListener) 类或直接实现 [ILogListener](xref:Ecng.Logging.ILogListener) 接口。[LogMessage](xref:Ecng.Logging.LogMessage) 类的对象通过 [ILogListener.WriteMessages](xref:Ecng.Logging.ILogListener.WriteMessages(System.Collections.Generic.IEnumerable{Ecng.Logging.LogMessage}))**(**[System.Collections.Generic.IEnumerable\<Ecng.Logging.LogMessage\>](xref:System.Collections.Generic.IEnumerable`1) messages **)** 方法传递。该类包含有关消息源 [LogMessage.Source](xref:Ecng.Logging.LogMessage.Source)（例如，生成消息的策略）、消息类型 [LogMessage.Level](xref:Ecng.Logging.LogMessage.Level)（信息、警告或错误）以及 [LogMessage.Message](xref:Ecng.Logging.LogMessage.Message) 文本本身的信息。以下示例显示了 [EmailLogListener](xref:Ecng.Logging.EmailLogListener) 的源代码：

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
