# API de negociación de Finam

El **conector de la API de negociación de Finam** enlaza las aplicaciones StockSharp con las cuentas de corretaje y los datos de mercado proporcionados por Finam. Convierte instrumentos, cotizaciones, órdenes, operaciones y el estado de la cartera al modelo unificado de mensajes de StockSharp.

## Funciones principales

- Búsqueda de acciones, bonos, divisas, fondos, futuros y opciones disponibles en Finam.
- Cotizaciones de nivel 1, libros de órdenes, operaciones públicas y velas temporales.
- Solicitudes de velas históricas y suscripciones de mercado en tiempo real.
- Envío de órdenes de mercado, límite, stop y stop-limit, además de cancelación.
- Actualizaciones del estado de órdenes, operaciones propias, efectivo y posiciones.
- Canje automático del secreto de API por un token de sesión de corta duración.
- Direcciones REST y WebSocket configurables para pasarelas compatibles y entornos de prueba.

## Uso habitual

El conector resulta útil para robots de negociación, terminales, monitores de cartera y servicios de gestión de órdenes que necesiten una interfaz StockSharp única para los datos de mercado y la negociación de Finam.

Se requiere un secreto de la API de negociación de Finam. Puede elegirse una cuenta concreta o dejar vacío el identificador para utilizar automáticamente la primera cuenta disponible para el token. Los instrumentos se identifican con el formato de Finam `ticker@MIC`. Los mercados, la profundidad histórica, los datos en tiempo real, los permisos de negociación y los límites dependen de la cuenta y de las condiciones del servicio Finam.

## Véase también

[Configuración del conector](finam_trade/configuration_finam_trade.md)

[Configuración gráfica](finam_trade/graphical_configuration_finam_trade.md)

[Inicialización del adaptador](finam_trade/adapter_initialization_finam_trade.md)
