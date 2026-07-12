# Registo

Para monitorizar algoritmos de negociação escritos em [S#](../api.md), pode utilizar a classe especial [LogManager](xref:Ecng.Logging.LogManager). Esta classe recebe as mensagens [LogMessage](xref:Ecng.Logging.LogMessage) de [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) através do evento [ILogSource.Log](xref:Ecng.Logging.ILogSource.Log) e passa-as para os ouvintes [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners). Assim, o código do algoritmo poderá passar informação de depuração (por exemplo, sobre erros ocorridos durante a operação ou informação adicional sobre cálculos matemáticos) e o [LogManager](xref:Ecng.Logging.LogManager) decidirá como apresentar esta informação ao operador.

Normalmente, o [S#](../api.md) contém as seguintes implementações de [ILogListener](xref:Ecng.Logging.ILogListener), cuja escolha afecta o destino das mensagens recebidas das estratégias:

1. [FileLogListener](xref:Ecng.Logging.FileLogListener) - escreve mensagens num ficheiro de texto. Recomenda-se a sua utilização em algoritmos já criados e para usar os registos em casos de força maior.
2. [ConsoleLogListener](xref:Ecng.Logging.ConsoleLogListener) - escreve mensagens na janela da consola (se o algoritmo não tiver uma janela, esta será criada automaticamente). Recomenda-se a sua utilização para depuração e teste do algoritmo.
3. [DebugLogListener](xref:Ecng.Logging.DebugLogListener) - escreve mensagens na janela de depuração. Esta janela pode ser vista através de programas especiais como [DebugView](https://technet.microsoft.com/en-us/sysinternals/bb896647.aspx). Recomenda-se a sua utilização para depuração e teste do algoritmo.
4. [EmailLogListener](xref:Ecng.Logging.EmailLogListener) - envia mensagens para o endereço de email especificado. Recomenda-se a sua utilização se o algoritmo estiver localizado num computador não controlado (no servidor de um alojador).
5. [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) - apresenta mensagens através da janela especial [LogControl](xref:StockSharp.Xaml.LogControl). Pode trabalhar em dois modos: quando todas as mensagens são apresentadas numa única janela e quando é criada uma janela separada para cada [ILogSource](xref:Ecng.Logging.ILogSource). Recomenda-se a sua utilização se o algoritmo tiver uma interface gráfica.

[LogListener](xref:Ecng.Logging.LogListener) pode ser configurado para filtrar mensagens através da propriedade [LogListener.Filters](xref:Ecng.Logging.LogListener.Filters). Por exemplo, através dos filtros pode especificar que tipo de mensagens deve ser processado. Isto é particularmente útil quando é utilizado [EmailLogListener](xref:Ecng.Logging.EmailLogListener), para, por exemplo, enviar email apenas em situações de emergência (erro do algoritmo de negociação), em vez de o fazer para cada mensagem de depuração.

## Próximos Passos

[Registo de estratégia](logging/strategy_logging.md)

[Registo de IConnector](logging/iconnector_logging.md)

[Outras fontes de registo](logging/other_logs_sources.md)

[Monitorização visual](logging/visual_monitoring.md)

[Criação de ILogListener](logging/custom_iloglistener.md)
