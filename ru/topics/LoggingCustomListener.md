# Создание ILogListener

Если необходимо создать свою реализацию [ILogListener](xref:StockSharp.Logging.ILogListener) (например, когда требуется сохранять сообщения в базе данных), то можно или унаследоваться от класса [LogListener](xref:StockSharp.Logging.LogListener) или напрямую реализовать интерфейс [ILogListener](xref:StockSharp.Logging.ILogListener). Через метод [ILogListener.WriteMessages](xref:StockSharp.Logging.ILogListener.WriteMessages) передается объект класса [LogMessage](xref:StockSharp.Logging.LogMessage). Данный класс содержит информацию об источнике сообщения [LogMessage.Source](xref:StockSharp.Logging.LogMessage.Source) (например, стратегия, которая сгенерировала сообщение), тип сообщения [LogMessage.Level](xref:StockSharp.Logging.LogMessage.Level) (информация, предупреждение или ошибка), а также сам текст [LogMessage.Message](xref:StockSharp.Logging.LogMessage.Message). Ниже в качестве примера приведен код класс [EmailLogListener](xref:StockSharp.Logging.EmailLogListener): 

```cs
/// <summary>
/// Логгер, отсылающий данные на email. 
/// </summary>
public class EmailLogListener : LogListener
{
	/// <summary>
	/// Создать <see cref="EmailLogListener"/>.
	/// </summary>
	/// <param name="from">Адрес, от имени которого будет отправлено сообщение.</param>
	/// <param name="to">Адрес, куда будет отправлено сообщение.</param>
	public EmailLogListener(string from, string to)
	{
		if (from.IsEmpty())
			throw new ArgumentNullException("from");
		if (to.IsEmpty())
			throw new ArgumentNullException("to");
		From = from;
		To = to;
	}
	/// <summary>
	/// Адрес, от имени которого будет отправлено сообщение.
	/// </summary>
	public string From { get; private set; }
	/// <summary>
	/// Адрес, куда будет отправлено сообщение.
	/// </summary>
	public string To { get; private set; }
	/// <summary>
	/// Записать сообщение.
	/// </summary>
	/// <param name="message">Отладочное сообщение.</param>
	protected override void OnWriteMessage(LogMessage message)
	{
		var email = new SmtpClient();
		email.Send(new MailMessage(From, To, message.Source.Name + " " + message.Level, message.Message));
	}
}
```
