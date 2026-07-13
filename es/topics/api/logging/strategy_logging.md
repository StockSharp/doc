# Registro de estrategias

La clase [Strategy](xref:StockSharp.Algo.Strategies.Strategy) implementa la interfaz [ILogSource](xref:Ecng.Logging.ILogSource). Por lo tanto, las estrategias se pueden pasar a [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources), y todos sus mensajes llegarán automáticamente a [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners).

## Requisitos previos

[Estrategias de negociación](../strategies.md)

## Registro en un archivo de prueba

1. Primero, necesita crear el administrador especial:

   ```cs
   var logManager = new LogManager();
   ```
2. Después necesita crear un registrador de archivo, pasándole el nombre del archivo, y agregarlo a [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners):

   ```cs
   var fileListener = new FileLogListener("{0}_{1:00}_{2:00}.txt".Put(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
   logManager.Listeners.Add(fileListener);
   ```
3. Para registrar mensajes, debe agregar una estrategia a [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources):

   ```cs
   logManager.Sources.Add(lkohSmaStrategy);
   ```
4. Después de agregar la estrategia al administrador de registro, todos sus mensajes se registrarán en el archivo.

## Reproducción de sonido

1. Cree un registrador y pásele el nombre del archivo de sonido:

   ```cs
   var soundListener = new SoundLogListener("error.mp3");

   logManager.Listeners.Add(soundListener);
   logManager.Sources.Add(lkohSmaStrategy);
   ```
2. Configure el filtro para que el sonido se reproduzca solo cuando el tipo de mensaje sea [LogLevels.Error](xref:Ecng.Logging.LogLevels.Error):

   ```cs
   soundListener.Filters.Add(msg => msg.Level == LogLevels.Error);
   ```

## Envío de correo electrónico

1. Cree el registrador y pásele los parámetros de los mensajes enviados:

   ```cs
   var emailListener = new EmailLogListener("from@stocksharp.com", "to@stocksharp.com");
   logManager.Listeners.Add(emailListener);
   logManager.Sources.Add(lkohSmaStrategy);
   ```
2. Configure el filtro para el envío de mensajes de los tipos [LogLevels.Error](xref:Ecng.Logging.LogLevels.Error) y [LogLevels.Warning](xref:Ecng.Logging.LogLevels.Warning):

   ```cs
   emailListener.Filters.Add(msg => msg.Level == LogLevels.Error);
   emailListener.Filters.Add(msg => msg.Level == LogLevels.Warning);
   ```

## Registro en LogWindow

1. Cree el registrador [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener):

   ```cs
    // cada estrategia tendrá sus propias ventanas
   var guiListener = new GuiLogListener();
   logManager.Listeners.Add(guiListener);
   logManager.Sources.Add(lkohSmaStrategy);
   ```
2. Esta es la ventana de registro cuando la estrategia está funcionando: ![Captura de registro de estrategias](../../../images/strategy_logging.png)

## Contenido recomendado

[Componentes visuales de registro](../graphical_user_interface/logging.md)
