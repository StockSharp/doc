# StocksTrader

**StocksTrader** conecta StockSharp con la API REST oficial de StocksTrader.

El conector ofrece la detección de cuentas demo y reales, la consulta periódica del estado de la cuenta, la búsqueda de instrumentos y las últimas instantáneas del precio de compra, del precio de venta y del último precio. La negociación abarca órdenes de mercado, limitadas y stop, la modificación y cancelación de órdenes pendientes, la modificación del stop-loss y del take-profit de las posiciones abiertas, el cierre de posiciones y el historial de órdenes y operaciones.

StocksTrader no dispone de una API de datos de mercado en tiempo real, por lo que una solicitud de nivel 1 devuelve la última instantánea disponible y finaliza de inmediato, mientras que las órdenes, las operaciones y el estado de la cuenta se consultan periódicamente.

Antes de conectarse, cree un token de portador (bearer) en el terminal web de StocksTrader.

## Véase también

[Configuración del conector](stocks_trader/configuration_stocks_trader.md)

[Configuración gráfica](stocks_trader/graphical_configuration_stocks_trader.md)

[Inicialización del adaptador](stocks_trader/adapter_initialization_stocks_trader.md)

[Documentación oficial de la API de StocksTrader](https://api-doc.stockstrader.com/)
