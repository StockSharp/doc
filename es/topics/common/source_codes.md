# Códigos fuente

El código abierto de [S#](../api.md) se distribuye entre varios repositorios. El [repositorio del núcleo de StockSharp](https://github.com/StockSharp/StockSharp) contiene el modelo de mensajes, las entidades de negocio, las abstracciones comunes de los conectores, los algoritmos, las herramientas de prueba y otras bases de la plataforma. Las implementaciones de conectores específicas de cada proveedor no se almacenan en el repositorio del núcleo.

Todos los conectores abiertos específicos de cada proveedor se mantienen en [StockSharp\/Connectors](https://github.com/StockSharp/Connectors). Cada conector es un proyecto .NET independiente y el repositorio incluye `Connectors.slnx` para compilarlos juntos.

El motor independiente de gráficos para navegador y el conjunto de gráficos para terminales web se mantienen en [StockSharp\/Charts](https://github.com/StockSharp/Charts). Consulte [Gráficos JavaScript](../api/graphical_user_interface/charts/javascript_charts.md).

[Instrucciones para usar GitHub](https://docs.github.com/es/get-started/start-your-journey/hello-world)

Lista de componentes disponibles con código fuente:

- Clases comunes para crear conexiones propias.
- Formato del almacenamiento de datos de mercado.
- Simulador de negociación.
- Simulador histórico.
- Indicadores (más de 140) de análisis técnico.
- Algoritmos para calcular ganancias/pérdidas, deslizamiento y retraso.
- Algoritmos para construir velas de cualquier marco temporal, así como velas no basadas en tiempo (tick, range, etc.).
- Registro.
- Importación y exportación.

Los códigos fuente de todos los componentes cerrados, así como los programas listos para usar, están disponibles tras la compra. Para más información sobre el coste de los códigos fuente, consulte [Coste del código fuente](https://stocksharp.com/es/store/?groups=22).

## Contenido recomendado

[Instrucciones de instalación](../api/setup.md)
