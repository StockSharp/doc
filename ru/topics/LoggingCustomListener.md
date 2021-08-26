# Создание ILogListener

Если необходимо создать свою реализацию [ILogListener](../api/StockSharp.Logging.ILogListener.html) (например, когда требуется сохранять сообщения в базе данных), то можно или унаследоваться от класса [LogListener](../api/StockSharp.Logging.LogListener.html) или напрямую реализовать интерфейс [ILogListener](../api/StockSharp.Logging.ILogListener.html). Через метод [ILogListener.WriteMessages](../api/StockSharp.Logging.ILogListener.WriteMessages.html) передается объект класса [LogMessage](../api/StockSharp.Logging.LogMessage.html). Данный класс содержит информацию об источнике сообщения [LogMessage.Source](../api/StockSharp.Logging.LogMessage.Source.html) (например, стратегия, которая сгенерировала сообщение), тип сообщения [LogMessage.Level](../api/StockSharp.Logging.LogMessage.Level.html) (информация, предупреждение или ошибка), а также сам текст [LogMessage.Message](../api/StockSharp.Logging.LogMessage.Message.html). Ниже в качестве примера приведен код класс [EmailLogListener](../api/StockSharp.Logging.EmailLogListener.html): 

```cs
\/\/\/ \<summary\>
\/\/\/ Логгер, отсылающий данные на email. 
\/\/\/ \<\/summary\>
public class EmailLogListener : LogListener
{
	\/\/\/ \<summary\>
	\/\/\/ Создать \<see cref\="EmailLogListener"\/\>.
	\/\/\/ \<\/summary\>
	\/\/\/ \<param name\="from"\>Адрес, от имени которого будет отправлено сообщение.\<\/param\>
	\/\/\/ \<param name\="to"\>Адрес, куда будет отправлено сообщение.\<\/param\>
	public EmailLogListener(string from, string to)
	{
		if (from.IsEmpty())
			throw new ArgumentNullException("from");
		if (to.IsEmpty())
			throw new ArgumentNullException("to");
		From \= from;
		To \= to;
	}
	\/\/\/ \<summary\>
	\/\/\/ Адрес, от имени которого будет отправлено сообщение.
	\/\/\/ \<\/summary\>
	public string From { get; private set; }
	\/\/\/ \<summary\>
	\/\/\/ Адрес, куда будет отправлено сообщение.
	\/\/\/ \<\/summary\>
	public string To { get; private set; }
	\/\/\/ \<summary\>
	\/\/\/ Записать сообщение.
	\/\/\/ \<\/summary\>
	\/\/\/ \<param name\="message"\>Отладочное сообщение.\<\/param\>
	protected override void OnWriteMessage(LogMessage message)
	{
		var email \= new SmtpClient();
		email.Send(new MailMessage(From, To, message.Source.Name + " " + message.Level, message.Message));
	}
}
```

## См. также
