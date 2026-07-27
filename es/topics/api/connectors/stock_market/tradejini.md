# Tradejini

El **conector de Tradejini** conecta StockSharp con un bróker o mercado electrónico de instrumentos financieros. Traduce los datos y las operaciones específicos del proveedor al modelo unificado de mensajes de StockSharp, permitiendo usar las mismas suscripciones y flujos de trabajo en distintos mercados.

## Funciones principales

- Cobertura habitual: acciones, futuros, opciones, Forex y materias primas.
- Búsqueda de instrumentos y datos de referencia del proveedor.
- Datos de mercado admitidos por el adaptador: velas.
- Solicitudes de datos históricos para gráficos, análisis y pruebas retrospectivas.
- Flujos de envío de órdenes y ejecuciones admitidos por el proveedor.
- Actualizaciones de carteras, saldos, posiciones y estado de ejecuciones.
- El transporte, las sesiones y los formatos del proveedor quedan ocultos tras la API estándar de StockSharp.

## Uso habitual

Úselo para estrategias en vivo, terminales, servicios de gestión de órdenes y herramientas de supervisión que necesiten acceso directo al proveedor.

Los instrumentos, la profundidad de datos, los permisos de negociación, los límites y la disponibilidad dependen de Tradejini, del plan de API y de la cuenta conectada.

## Véase también

[Configuración del conector](tradejini/configuration_tradejini.md)

[Configuración gráfica](tradejini/graphical_configuration_tradejini.md)

[Inicialización del adaptador](tradejini/adapter_initialization_tradejini.md)
