# Bigul

El **conector de Bigul** conecta StockSharp con un bróker o mercado electrónico de instrumentos financieros. Traduce los datos y las operaciones específicos del proveedor al modelo unificado de mensajes de StockSharp, permitiendo usar las mismas suscripciones y flujos de trabajo en distintos mercados.

## Funciones principales

- Cobertura habitual: acciones, futuros y opciones.
- Búsqueda de instrumentos y datos de referencia del proveedor.
- Datos de mercado admitidos por el adaptador: cotizaciones de nivel 1, operaciones tick a tick y libros de órdenes.
- Flujos de envío de órdenes y ejecuciones admitidos por el proveedor.
- Actualizaciones de carteras, saldos, posiciones y estado de ejecuciones.
- Suscripciones en tiempo real mediante el flujo de datos del proveedor.
- El transporte, las sesiones y los formatos del proveedor quedan ocultos tras la API estándar de StockSharp.

## Uso habitual

Úselo para estrategias en vivo, terminales, servicios de gestión de órdenes y herramientas de supervisión que necesiten acceso directo al proveedor.

Los instrumentos, la profundidad de datos, los permisos de negociación, los límites y la disponibilidad dependen de Bigul, del plan de API y de la cuenta conectada.

## Véase también

[Configuración del conector](bigul/configuration_bigul.md)

[Configuración gráfica](bigul/graphical_configuration_bigul.md)

[Inicialización del adaptador](bigul/adapter_initialization_bigul.md)
