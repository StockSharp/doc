# Outras fontes de registo

Nos tópicos anteriores, os objectos incorporados nas classes [S#](../../api.md) eram fontes de registo. O [S#](../../api.md) fornece possibilidades para os casos em que a fonte de registo é a sua própria classe, ou quando a fonte não tem de estar associada a uma classe específica mas serve toda a aplicação. No primeiro caso, tem de implementar na sua classe a interface [ILogSource](xref:Ecng.Logging.ILogSource) ou herdar de [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver). Na segunda situação, pode utilizar [TraceSource](xref:Ecng.Logging.TraceSource), que usa o sistema de tracing do .NET. A forma de o fazer é mostrada no exemplo *Samples\/08\_Misc\/01\_Logging*.

## Exemplo de registo

1. Crie uma classe personalizada que herde de [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver).

   ```cs
   private class TestSource : BaseLogReceiver
   {
   }
   ```
2. Crie o [LogManager](xref:Ecng.Logging.LogManager) e declare uma variável da classe do utilizador.

   ```cs
   private readonly LogManager _logManager = new LogManager();
   private readonly TestSource _testSource;
   				
   ```
3. Adicione fontes de log.

   ```cs
   _logManager.Sources.Add(_testSource = new TestSource());
   _logManager.Sources.Add(new Ecng.Logging.TraceSource());
   				
   ```
4. Adicione ouvintes de registo.

   ```cs
   // as mensagens de log serão apresentadas no componente GUI
   _logManager.Listeners.Add(new GuiLogListener(Monitor));
   // também escrever em ficheiros
   _logManager.Listeners.Add(new FileLogListener
   {
   	FileName = "logs",
   });
   				
   ```
5. Adicione mensagens de registo da classe personalizada. O nível de registo é escolhido aleatoriamente.

   ```cs
   var level = RandomGen.GetEnum<LogLevels>();
   switch (level)
   {
   	case LogLevels.Inherit:
   	case LogLevels.Debug:
   	case LogLevels.Info:
   	case LogLevels.Off:
		_testSource.AddInfoLog("{0} (fonte)!!!".Put(level));
   		break;
   	case LogLevels.Warning:
		_testSource.AddWarningLog("Aviso (fonte)!!!");
   		break;
   	case LogLevels.Error:
		_testSource.AddErrorLog("Erro (fonte)!!!");
   		break;
   	default:
   		throw new ArgumentOutOfRangeException();
   }
   ```
6. Adicione mensagens de tracing.

   ```cs
   var level = RandomGen.GetEnum<LogLevels>();
   switch (level)
   {
   	case LogLevels.Inherit:
   	case LogLevels.Debug:
   	case LogLevels.Info:
   	case LogLevels.Off:
		Trace.TraceInformation("{0} (rastreio)!!!".Put(level));
   		break;
   	case LogLevels.Warning:
		Trace.TraceWarning("Aviso (rastreio)!!!");
   		break;
   	case LogLevels.Error:
		Trace.TraceError("Erro (rastreio)!!!");
   		break;
   	default:
   		throw new ArgumentOutOfRangeException();
   }
   ```
7. O resultado do funcionamento do exemplo.![Exemplo de registo](../../../images/sample_logging.png)
