# Andere Logquellen

In den vorherigen Themen waren in die Klassen von [S#](../../api.md) eingebettete Objekte die Logquellen. [S#](../../api.md) bietet Möglichkeiten für Fälle, in denen die Logquelle Ihre eigene Klasse ist oder die Quelle nicht an eine bestimmte Klasse gebunden sein muss, sondern der gesamten Anwendung dient. Im ersten Fall müssen Sie in Ihrer Klasse das Interface [ILogSource](xref:Ecng.Logging.ILogSource) implementieren oder von [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver) erben. Im zweiten Fall können Sie [TraceSource](xref:Ecng.Logging.TraceSource) verwenden, das das .NET-Tracing-System nutzt. Wie das funktioniert, zeigt das Beispiel *Samples\/08\_Misc\/01\_Logging*.

## Logging-Beispiel

1. Erstellen Sie eine benutzerdefinierte Klasse, die von [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver) erbt.

   ```cs
   private class TestSource : BaseLogReceiver
   {
   }
   ```
2. Erstellen Sie den [LogManager](xref:Ecng.Logging.LogManager) und deklarieren Sie eine Variable der Benutzerklasse.

   ```cs
   private readonly LogManager _logManager = new LogManager();
   private readonly TestSource _testSource;

   ```
3. Fügen Sie Logquellen hinzu.

   ```cs
   _logManager.Sources.Add(_testSource = new TestSource());
   _logManager.Sources.Add(new Ecng.Logging.TraceSource());

   ```
4. Fügen Sie Loglistener hinzu.

   ```cs
   // Lognachrichten werden in der GUI-Komponente angezeigt
   _logManager.Listeners.Add(new GuiLogListener(Monitor));
   // zusätzliches Schreiben in Dateien
   _logManager.Listeners.Add(new FileLogListener
   {
   	FileName = "logs",
   });

   ```
5. Fügen Sie Logging-Nachrichten der benutzerdefinierten Klasse hinzu. Die Logging-Stufe wird zufällig ausgewählt.

   ```cs
   var level = RandomGen.GetEnum<LogLevels>();
   switch (level)
   {
   	case LogLevels.Inherit:
   	case LogLevels.Debug:
   	case LogLevels.Info:
   	case LogLevels.Off:
   		_testSource.AddInfoLog("{0} (source)!!!".Put(level));
   		break;
   	case LogLevels.Warning:
   		_testSource.AddWarningLog("Warning (source)!!!");
   		break;
   	case LogLevels.Error:
		_testSource.AddErrorLog("Fehler (Quelle)!!!");
   		break;
   	default:
   		throw new ArgumentOutOfRangeException();
   }
   ```
6. Fügen Sie Tracing-Nachrichten hinzu.

   ```cs
   var level = RandomGen.GetEnum<LogLevels>();
   switch (level)
   {
   	case LogLevels.Inherit:
   	case LogLevels.Debug:
   	case LogLevels.Info:
   	case LogLevels.Off:
   		Trace.TraceInformation("{0} (trace)!!!".Put(level));
   		break;
   	case LogLevels.Warning:
   		Trace.TraceWarning("Warning (trace)!!!");
   		break;
   	case LogLevels.Error:
   		Trace.TraceError("Error (trace)!!!");
   		break;
   	default:
   		throw new ArgumentOutOfRangeException();
   }
   ```
7. Ergebnis der Beispielausführung.![Beispiel Protokollierung](../../../images/sample_logging.png)

