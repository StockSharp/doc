# Configuración del conector: StocksTrader

Genere un token en el terminal web de StocksTrader e indique los parámetros de conexión.

- `Token` - token de portador emitido por el terminal web.
- `AccountId` - identificador de la cuenta. Opcional cuando solo una cuenta coincide con el modo seleccionado.
- `IsDemo` - selecciona la cuenta de demostración. Valor predeterminado: `true`.
- `Address` - punto de conexión REST. Valor predeterminado: `https://api.stockstrader.com/`.
- `PollingInterval` - frecuencia con la que se solicitan las órdenes, las operaciones y el estado de la cuenta. Valor predeterminado: 5 segundos; los valores menores se elevan a 2 segundos.

Como el proveedor no ofrece datos en tiempo real, `PollingInterval` determina la rapidez con la que los cambios de las órdenes y de las posiciones llegan a la estrategia. Redúzcalo para una negociación activa y auméntelo para no superar los límites de solicitudes del proveedor.

Los precios de protección se transmiten a través de [StocksTraderOrderCondition](xref:StockSharp.StocksTrader.StocksTraderOrderCondition): los precios de stop-loss y take-profit de la orden o de la posición abierta.

## Véase también

[Documentación oficial de la API de StocksTrader](https://api-doc.stockstrader.com/)
