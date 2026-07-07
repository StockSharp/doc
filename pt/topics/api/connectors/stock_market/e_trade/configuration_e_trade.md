# Configuração E\*TRADE

Para trabalhar com um conector, tem de especificar o **Login** e a **Password**. O **Login** e a **Password** são fornecidos pela corretora. Para obter acesso à API, recomenda-se contactar a corretora.

O mecanismo de interação é apresentado nesta figura:

![ETrade](../../../../../images/etrade.png)

O [E\*TRADE](../e_trade.md) utiliza o protocolo de autorização OAuth 1.0a, que requer login e password através do browser no site [E\*TRADE](https://etrade.com/). A sequência completa do procedimento de autorização é apresentada na figura seguinte:

![etrade authorization](../../../../../images/etrade_autoriazation.png)

Um procedimento de autorização completo deve ser executado apenas uma vez por dia (o servidor [E\*TRADE](../e_trade.md) repõe os AccessTokens emitidos anteriormente à meia-noite EST). Se o procedimento de autorização completo já tiver sido realizado no dia atual em EST, o [ETradeMessageAdapter](xref:StockSharp.ETrade.ETradeMessageAdapter) descarrega automaticamente o AccessToken armazenado num subdiretório do algoritmo [E\*TRADE](../e_trade.md).
