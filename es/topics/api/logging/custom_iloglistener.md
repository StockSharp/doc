# ILogListener personalizado

Si desea crear su propia implementación de [ILogListener](xref:Ecng.Logging.ILogListener) (por ejemplo, cuando quiera guardar mensajes en la base de datos), se puede heredar la clase [LogListener](xref:Ecng.Logging.LogListener) o implementar directamente la interfaz [ILogListener](xref:Ecng.Logging.ILogListener). El objeto de la clase [LogMessage](xref:Ecng.Logging.LogMessage) se pasa mediante el método [ILogListener.WriteMessages](xref:Ecng.Logging.ILogListener.WriteMessages(System.Collections.Generic.IEnumerable{Ecng.Logging.LogMessage}))**(**[System.Collections.Generic.IEnumerable\<Ecng.Logging.LogMessage\>](xref:System.Collections.Generic.IEnumerable`1) messages **)**. Esta clase contiene información sobre la fuente del mensaje [LogMessage.Source](xref:Ecng.Logging.LogMessage.Source) (por ejemplo, una estrategia que generó el mensaje), el tipo de mensaje [LogMessage.Level](xref:Ecng.Logging.LogMessage.Level) (información, advertencia o error), así como el propio texto [LogMessage.Message](xref:Ecng.Logging.LogMessage.Message). El siguiente ejemplo muestra el código fuente de [EmailLogListener](xref:Ecng.Logging.EmailLogListener): 

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
