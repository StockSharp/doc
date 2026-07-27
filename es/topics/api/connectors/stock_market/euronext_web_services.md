# Euronext Web Services

El **conector de Euronext Web Services** conecta StockSharp con un servicio profesional de datos y análisis de mercado. Traduce los datos y las operaciones específicos del proveedor al modelo unificado de mensajes de StockSharp, permitiendo usar las mismas suscripciones y flujos de trabajo en distintos mercados.

## Funciones principales

- Cobertura habitual: acciones.
- Búsqueda de instrumentos y datos de referencia del proveedor.
- Datos de mercado, empresas, presentaciones, divulgaciones y referencia admitidos por el proveedor.
- Datos de mercado admitidos por el adaptador: cotizaciones de nivel 1, operaciones tick a tick, libros de órdenes y velas.
- Solicitudes de datos históricos para gráficos, análisis y pruebas retrospectivas.
- Este adaptador está destinado al acceso a datos y no enruta órdenes.
- El transporte, las sesiones y los formatos del proveedor quedan ocultos tras la API estándar de StockSharp.

## Uso habitual

Úselo para alimentar gráficos, almacenamiento de mercado, análisis, investigación y pruebas de estrategias con datos del proveedor.

Los instrumentos, la profundidad de datos, los permisos de negociación, los límites y la disponibilidad dependen de Euronext Web Services, del plan de API y de la cuenta conectada.

## Véase también

[Configuración del conector](euronext_web_services/configuration_euronext_web_services.md)

[Configuración gráfica](euronext_web_services/graphical_configuration_euronext_web_services.md)

[Inicialización del adaptador](euronext_web_services/adapter_initialization_euronext_web_services.md)
