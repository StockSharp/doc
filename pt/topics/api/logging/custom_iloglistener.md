# ILogListener Personalizado

Se quiser criar a sua própria implementação de [ILogListener](xref:Ecng.Logging.ILogListener) (por exemplo, quando quiser guardar mensagens na base de dados), a classe [LogListener](xref:Ecng.Logging.LogListener) pode ser herdada ou a interface [ILogListener](xref:Ecng.Logging.ILogListener) pode ser implementada directamente. O objecto da classe [LogMessage](xref:Ecng.Logging.LogMessage) é passado através do método [ILogListener.WriteMessages](xref:Ecng.Logging.ILogListener.WriteMessages(System.Collections.Generic.IEnumerable{Ecng.Logging.LogMessage}))**(**[System.Collections.Generic.IEnumerable\<Ecng.Logging.LogMessage\>](xref:System.Collections.Generic.IEnumerable`1) messages **)**. Esta classe contém a informação sobre a fonte da mensagem [LogMessage.Source](xref:Ecng.Logging.LogMessage.Source) (por exemplo, uma estratégia que gerou a mensagem), o tipo da mensagem [LogMessage.Level](xref:Ecng.Logging.LogMessage.Level) (informação, aviso ou erro), bem como o próprio texto [LogMessage.Message](xref:Ecng.Logging.LogMessage.Message). O exemplo seguinte mostra o código-fonte de [EmailLogListener](xref:Ecng.Logging.EmailLogListener):

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
