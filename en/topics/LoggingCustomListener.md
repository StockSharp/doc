# Custom ILogListener

If you want to create your own implementation of the [ILogListener](../api/StockSharp.Logging.ILogListener.html) (for example, when you want to save messages in the database), the class [LogListener](../api/StockSharp.Logging.LogListener.html) can be inherited or the [ILogListener](../api/StockSharp.Logging.ILogListener.html) interface can be implemented directly. The object of the [LogMessage](../api/StockSharp.Logging.LogMessage.html) class is passed through the [ILogListener.WriteMessages](../api/StockSharp.Logging.ILogListener.WriteMessages.html) method. This class contains the information about the message source [LogMessage.Source](../api/StockSharp.Logging.LogMessage.Source.html) ( for example, a strategy that has generated the message), the message type [LogMessage.Level](../api/StockSharp.Logging.LogMessage.Level.html) (information, warning or error), as well as the [LogMessage.Message](../api/StockSharp.Logging.LogMessage.Message.html). text itself. The following example shows the source code of [EmailLogListener](../api/StockSharp.Logging.EmailLogListener.html): 

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
