# Monitorização visual

Para simplificar a monitorização, pode utilizar o componente especial [Monitor](xref:StockSharp.Xaml.Monitor). Consulte também [Componentes visuais de registo](../graphical_user_interface/logging.md).

![painel de registo GUI](../../../images/gui_logcontrol.png)

Esta janela permite apresentar mensagens de todos os [ILogSource](xref:Ecng.Logging.ILogSource):

- estratégias ([Strategy](xref:StockSharp.Algo.Strategies.Strategy));
- conectores ([IConnector](xref:StockSharp.BusinessEntities.IConnector));
- implementações próprias de [ILogSource](xref:Ecng.Logging.ILogSource) (por exemplo, a janela principal no algoritmo).

O aninhamento das fontes é mostrado sob a forma de uma árvore. Cada nó pai contém mensagens de todas as fontes aninhadas e assim sucessivamente até ao nível mais baixo. Para conectores, isto também é útil ao utilizar [BasketTrader](../connectors.md). De modo semelhante, o mesmo aninhamento pode ser organizado para o seu próprio algoritmo implementando a propriedade [ILogSource.Parent](xref:Ecng.Logging.ILogSource.Parent).

## Utilizar Monitor

1. Primeiro, tem de criar uma janela e adicionar o componente.
2. Depois, a janela criada deve ser adicionada ao seu [LogManager](xref:Ecng.Logging.LogManager) através de [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener):

   ```cs
   _logManager.Listeners.Add(new GuiLogListener(monitor));
   ```
3. Depois disso, todas as fontes [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) (estratégias, conectores, etc.) enviarão mensagens para o [Monitor](xref:StockSharp.Xaml.Monitor).

## Conteúdo recomendado

[Componentes visuais de registo](../graphical_user_interface/logging.md)
