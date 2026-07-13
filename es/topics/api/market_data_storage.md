# Almacenamiento de datos de mercado

[S#](../api.md) proporciona funcionalidad para almacenar y cargar datos. El almacenamiento de datos puede ser necesario en varios casos:

1. Durante el trabajo de una estrategia de negociación, cuando es necesario guardar el estado (órdenes, posiciones, ajustes, etc.);
2. Guardado de datos de mercado desde el terminal de negociación para probar algoritmos en tiempo real;
3. Acumulación de datos para análisis y minería de datos.

[S#](../api.md) hace claro el mecanismo de guardado gracias al acceso de alto nivel y a la ocultación de detalles técnicos (para más detalles, véase [API](market_data_storage/api.md)). Además es versátil, con posibilidad de ampliar los tipos de almacenamientos admitidos.

## Contenido recomendado

[Trabajo con la API](market_data_storage/api.md)

[Trabajo con almacenamiento remoto](market_data_storage/remote.md)
