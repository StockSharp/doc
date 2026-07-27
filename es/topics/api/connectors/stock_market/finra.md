# FINRA

El **conector de FINRA** conecta StockSharp con un servicio de datos financieros e información de referencia. Traduce los datos específicos del proveedor al modelo unificado de mensajes de StockSharp, permitiendo usar las mismas suscripciones y flujos de trabajo con distintas fuentes de datos.

## Funciones principales

- Cobertura habitual: acciones y datos de referencia de emisores.
- Búsqueda de instrumentos y datos de referencia del proveedor.
- Datos de mercado, empresas, presentaciones, divulgaciones y referencia admitidos por el proveedor.
- Datos de mercado admitidos por el adaptador: cotizaciones de nivel 1.
- Solicitudes de datos históricos para gráficos, análisis y pruebas retrospectivas.
- Este adaptador está destinado al acceso a datos y no enruta órdenes.
- El transporte, las sesiones y los formatos del proveedor quedan ocultos tras la API estándar de StockSharp.

## Uso habitual

Úselo para datos maestros de valores, supervisión de divulgaciones, investigación de emisores, procesos de cumplimiento y análisis histórico.

Los instrumentos, la profundidad de datos, los permisos de negociación, los límites y la disponibilidad dependen de FINRA, del plan de API y de la cuenta conectada.

## Véase también

[Configuración del conector](finra/configuration_finra.md)

[Configuración gráfica](finra/graphical_configuration_finra.md)

[Inicialización del adaptador](finra/adapter_initialization_finra.md)
