# Otras fuentes de logs

En los temas anteriores, los objetos integrados en las clases de [S#](../../api.md) eran fuentes de logs. [S#](../../api.md) ofrece posibilidades para los casos en que la fuente de logs es su propia clase, o cuando la fuente no tiene que estar asociada a una clase concreta sino que sirve a toda la aplicación. Para el primer caso debe implementar en su clase la interfaz [ILogSource](xref:Ecng.Logging.ILogSource) o heredar de [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver). En la segunda situación, puede usar [TraceSource](xref:Ecng.Logging.TraceSource), que usa el sistema de tracing de .NET. Cómo hacerlo se muestra en el ejemplo *Samples\/08\_Misc\/01\_Logging*.

## Ejemplo de logging

1. Cree una clase personalizada que herede de [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver).

   ```cs
   private class TestSource : BaseLogReceiver
   {
   }
   ```
2. Cree [LogManager](xref:Ecng.Logging.LogManager) y declare una variable de la clase de usuario.

   ```cs
   private readonly LogManager _logManager = new LogManager();
   private readonly TestSource _testSource;
   				
   ```
3. Agregue fuentes de logs.

   ```cs
   _logManager.Sources.Add(_testSource = new TestSource());
   _logManager.Sources.Add(new Ecng.Logging.TraceSource());
   				
   ```
4. Agregue listeners de logs.

   ```cs
   // los mensajes de log se mostrarán en el componente GUI
   _logManager.Listeners.Add(new GuiLogListener(Monitor));
   // también se escriben en archivos
   _logManager.Listeners.Add(new FileLogListener
   {
   	FileName = "logs",
   });
   				
   ```
5. Agregue mensajes de logging de la clase personalizada. El nivel de logging se elige aleatoriamente.

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
   		_testSource.AddErrorLog("Error (source)!!!");
   		break;
   	default:
   		throw new ArgumentOutOfRangeException();
   }
   ```
6. Agregue mensajes de tracing.

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
7. Resultado del funcionamiento del ejemplo.![Ejemplo de registro](../../../images/sample_logging.png)
