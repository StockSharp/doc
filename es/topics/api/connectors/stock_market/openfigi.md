# OpenFIGI

**OpenFIGI** conecta StockSharp con OpenFIGI API v3 para buscar identificadores globales de instrumentos.

## Funciones principales

- Funciones verificadas en el código fuente: búsqueda y filtrado de valores por bolsa, MIC, moneda, sector de mercado y tipo de valor. El adaptador devuelve metadatos, pero no cotizaciones ni operaciones.
- El transporte, las sesiones y los formatos del proveedor quedan ocultos tras la API estándar de StockSharp.

## Uso habitual

Utilice este conector para relacionar identificadores con registros FIGI y crear un catálogo normalizado de instrumentos.

La cobertura, los límites y el acceso anónimo o con clave dependen de OpenFIGI.

## Véase también

[Configuración del conector](openfigi/configuration_openfigi.md)

[Configuración gráfica](openfigi/graphical_configuration_openfigi.md)

[Inicialización del adaptador](openfigi/adapter_initialization_openfigi.md)
