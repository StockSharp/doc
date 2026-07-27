# Bavest

El **conector de Bavest** conecta StockSharp con un servicio profesional de datos y análisis de mercado. Traduce los datos y las operaciones específicos del proveedor al modelo unificado de mensajes de StockSharp, permitiendo usar las mismas suscripciones y flujos de trabajo en distintos mercados.

## Funciones principales

- Cobertura habitual: acciones.
- Búsqueda de instrumentos y datos de referencia del proveedor.
- Datos de mercado, empresas, presentaciones, divulgaciones y referencia admitidos por el proveedor.
- Datos de mercado admitidos por el adaptador: cotizaciones de nivel 1, velas, noticias financieras y divulgaciones financieras.
- Solicitudes de datos históricos para gráficos, análisis y pruebas retrospectivas.
- Suscripciones en tiempo real mediante el flujo de datos del proveedor.
- Este adaptador está destinado al acceso a datos y no enruta órdenes.
- El transporte, las sesiones y los formatos del proveedor quedan ocultos tras la API estándar de StockSharp.

## Uso habitual

Úselo para alimentar gráficos, almacenamiento de mercado, análisis, investigación y pruebas de estrategias con datos del proveedor.

Los instrumentos, la profundidad de datos, los permisos de negociación, los límites y la disponibilidad dependen de Bavest, del plan de API y de la cuenta conectada.

## Véase también

[Configuración del conector](bavest/configuration_bavest.md)

[Configuración gráfica](bavest/graphical_configuration_bavest.md)

[Inicialización del adaptador](bavest/adapter_initialization_bavest.md)
