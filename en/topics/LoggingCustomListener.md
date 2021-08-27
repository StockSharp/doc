# Custom ILogListener

If you want to create your own implementation of the [ILogListener](xref:StockSharp.Logging.ILogListener) (for example, when you want to save messages in the database), the class [LogListener](xref:StockSharp.Logging.LogListener) can be inherited or the [ILogListener](xref:StockSharp.Logging.ILogListener) interface can be implemented directly. The object of the [LogMessage](xref:StockSharp.Logging.LogMessage) class is passed through the [ILogListener.WriteMessages](xref:StockSharp.Logging.ILogListener.WriteMessages) method. This class contains the information about the message source [LogMessage.Source](xref:StockSharp.Logging.LogMessage.Source) ( for example, a strategy that has generated the message), the message type [LogMessage.Level](xref:StockSharp.Logging.LogMessage.Level) (information, warning or error), as well as the [LogMessage.Message](xref:StockSharp.Logging.LogMessage.Message). text itself. The following example shows the source code of [EmailLogListener](xref:StockSharp.Logging.EmailLogListener): 

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

## Recommended content
