# SimFin

**SimFin** conecta StockSharp con SimFin Web API v3 para obtener datos empresariales, precios y fundamentales.

## Funciones principales

- Funciones verificadas en el código fuente: búsqueda de empresas, precios diarios de nivel 1, velas diarias y fundamentales estructurados mediante `SimFinDataTypes.Fundamentals`. El adaptador es histórico y no admite negociación.
- El transporte, las sesiones y los formatos del proveedor quedan ocultos tras la API estándar de StockSharp.

## Uso habitual

Utilice este conector para descargar precios diarios, estados financieros, valores derivados y ratios opcionales de las empresas compatibles.

La cobertura, el historial, los campos, los límites y la disponibilidad dependen del plan de SimFin.

## Véase también

[Configuración del conector](simfin/configuration_simfin.md)

[Configuración gráfica](simfin/graphical_configuration_simfin.md)

[Inicialización del adaptador](simfin/adapter_initialization_simfin.md)
