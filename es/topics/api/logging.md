# Registro

Para la monitorización de algoritmos de trading escritos en [S#](../api.md), puede usar la clase especial [LogManager](xref:Ecng.Logging.LogManager). Esta clase recibe los mensajes [LogMessage](xref:Ecng.Logging.LogMessage) desde [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) mediante el evento [ILogSource.Log](xref:Ecng.Logging.ILogSource.Log) y los pasa a los receptores [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners). Por lo tanto, el código del algoritmo podrá pasar información de depuración (por ejemplo, sobre errores ocurridos durante la operación o información adicional sobre cálculos matemáticos) y [LogManager](xref:Ecng.Logging.LogManager) decidirá cómo mostrar esta información al operador.

Normalmente [S#](../api.md) contiene las siguientes implementaciones de [ILogListener](xref:Ecng.Logging.ILogListener), cuya elección afecta a dónde se pasarán los mensajes recibidos de las estrategias: 

1. [FileLogListener](xref:Ecng.Logging.FileLogListener) - escribe mensajes en un archivo de texto. Se recomienda usarlo para algoritmos ya creados y usar los registros en casos de fuerza mayor.
2. [ConsoleLogListener](xref:Ecng.Logging.ConsoleLogListener) - muestra mensajes en la ventana de consola (si el algoritmo no tiene ventana, se creará automáticamente). Se recomienda usarlo para depurar y probar el algoritmo 
3. [DebugLogListener](xref:Ecng.Logging.DebugLogListener) - muestra mensajes en la ventana de depuración. Esta ventana se puede ver mediante programas especiales como [DebugView](https://technet.microsoft.com/en-us/sysinternals/bb896647.aspx). Se recomienda usarlo para depurar y probar el algoritmo. 
4. [EmailLogListener](xref:Ecng.Logging.EmailLogListener) - envía mensajes a la dirección de correo electrónico especificada. Se recomienda usarlo si el algoritmo se encuentra en un equipo no controlado (en el servidor de un proveedor de hosting). 
5. [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) - muestra mensajes mediante la ventana especial [LogControl](xref:StockSharp.Xaml.LogControl). Puede trabajar en dos modos: cuando todos los mensajes se muestran en una única ventana y cuando se crea una ventana separada para cada [ILogSource](xref:Ecng.Logging.ILogSource). Se recomienda usarlo si el algoritmo tiene una interfaz gráfica. 

[LogListener](xref:Ecng.Logging.LogListener) se puede configurar para filtrar mensajes mediante la propiedad [LogListener.Filters](xref:Ecng.Logging.LogListener.Filters). Por ejemplo, mediante filtros puede especificar qué tipo de mensajes deben procesarse. Esto es especialmente útil cuando se usa [EmailLogListener](xref:Ecng.Logging.EmailLogListener), por ejemplo, para enviar correo solo en situaciones de emergencia (error del algoritmo de trading) y no con cada mensaje de depuración. 

## Siguientes pasos

[Registro de estrategias](logging/strategy_logging.md)

[Registro de IConnector](logging/iconnector_logging.md)

[Otras fuentes de registro](logging/other_logs_sources.md)

[Monitorización visual](logging/visual_monitoring.md)

[Creación de ILogListener](logging/custom_iloglistener.md)
