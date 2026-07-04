# Benutzerdefinierter ILogListener

Wenn Sie eine eigene Implementierung von [ILogListener](xref:Ecng.Logging.ILogListener) erstellen möchten (zum Beispiel, um Nachrichten in einer Datenbank zu speichern), kann die Klasse [LogListener](xref:Ecng.Logging.LogListener) geerbt oder das Interface [ILogListener](xref:Ecng.Logging.ILogListener) direkt implementiert werden. Das Objekt der Klasse [LogMessage](xref:Ecng.Logging.LogMessage) wird über die Methode [ILogListener.WriteMessages](xref:Ecng.Logging.ILogListener.WriteMessages(System.Collections.Generic.IEnumerable{Ecng.Logging.LogMessage}))**(**[System.Collections.Generic.IEnumerable\<Ecng.Logging.LogMessage\>](xref:System.Collections.Generic.IEnumerable`1) messages **)** übergeben. Diese Klasse enthält Informationen über die Nachrichtenquelle [LogMessage.Source](xref:Ecng.Logging.LogMessage.Source) (zum Beispiel eine Strategie, die die Nachricht erzeugt hat), den Nachrichtentyp [LogMessage.Level](xref:Ecng.Logging.LogMessage.Level) (Information, Warnung oder Fehler) sowie den Text [LogMessage.Message](xref:Ecng.Logging.LogMessage.Message) selbst. Das folgende Beispiel zeigt den Quellcode von [EmailLogListener](xref:Ecng.Logging.EmailLogListener):

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

