# Choice FinX

El **conector de Choice FinX** conecta StockSharp con un bróker o mercado electrónico de instrumentos financieros. Traduce los datos y las operaciones específicos del proveedor al modelo unificado de mensajes de StockSharp, permitiendo usar las mismas suscripciones y flujos de trabajo en distintos mercados.

## Funciones principales

- Cobertura habitual: acciones, futuros, opciones, Forex y materias primas.
- Búsqueda de instrumentos y datos de referencia del proveedor.
- Datos de mercado admitidos por el adaptador: cotizaciones de nivel 1, operaciones tick a tick, libros de órdenes y velas.
- Solicitudes de datos históricos para gráficos, análisis y pruebas retrospectivas.
- Flujos de envío de órdenes y ejecuciones admitidos por el proveedor.
- Actualizaciones de carteras, saldos, posiciones y estado de ejecuciones.
- Suscripciones en tiempo real mediante el flujo de datos del proveedor.
- El transporte, las sesiones y los formatos del proveedor quedan ocultos tras la API estándar de StockSharp.

## Uso habitual

Úselo para estrategias en vivo, terminales, servicios de gestión de órdenes y herramientas de supervisión que necesiten acceso directo al proveedor.

Los instrumentos, la profundidad de datos, los permisos de negociación, los límites y la disponibilidad dependen de Choice FinX, del plan de API y de la cuenta conectada.

## Véase también

[Configuración del conector](choice_finx/configuration_choice_finx.md)

[Configuración gráfica](choice_finx/graphical_configuration_choice_finx.md)

[Inicialización del adaptador](choice_finx/adapter_initialization_choice_finx.md)
