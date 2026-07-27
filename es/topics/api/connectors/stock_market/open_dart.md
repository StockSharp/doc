# Open DART

El **conector de Open DART** conecta StockSharp con un servicio de datos financieros e información de referencia. Traduce los datos específicos del proveedor al modelo unificado de mensajes de StockSharp, permitiendo usar las mismas suscripciones y flujos de trabajo con distintas fuentes de datos.

## Funciones principales

- Cobertura habitual: acciones y datos de referencia de emisores.
- Búsqueda de instrumentos y datos de referencia del proveedor.
- Datos de mercado, empresas, presentaciones, divulgaciones y referencia admitidos por el proveedor.
- Datos de mercado admitidos por el adaptador: cotizaciones de nivel 1, noticias financieras y divulgaciones financieras.
- Solicitudes de datos históricos para gráficos, análisis y pruebas retrospectivas.
- Este adaptador está destinado al acceso a datos y no enruta órdenes.
- El transporte, las sesiones y los formatos del proveedor quedan ocultos tras la API estándar de StockSharp.

## Uso habitual

Úselo para datos maestros de valores, supervisión de divulgaciones, investigación de emisores, procesos de cumplimiento y análisis histórico.

Los instrumentos, la profundidad de datos, los permisos de negociación, los límites y la disponibilidad dependen de Open DART, del plan de API y de la cuenta conectada.

## Véase también

[Configuración del conector](open_dart/configuration_open_dart.md)

[Configuración gráfica](open_dart/graphical_configuration_open_dart.md)

[Inicialización del adaptador](open_dart/adapter_initialization_open_dart.md)
