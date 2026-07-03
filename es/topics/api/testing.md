# Backtesting\/Emulación

Las estrategias escritas con [Strategy](xref:StockSharp.Algo.Strategies.Strategy) pueden probarse en tres modos: 

1. Pruebas con [datos históricos](testing/historical_data.md). Con este tipo de datos se puede realizar tanto análisis de mercado para buscar patrones como optimización de parámetros de la estrategia. 
2. Pruebas con [datos aleatorios](testing/random_data.md). Herramienta cómoda para pruebas iniciales de la estrategia con el fin de identificar errores en los algoritmos, o para pruebas automatizadas que se ejecutan según programación. 
3. Pruebas con [simulador](testing/simulator.md) basadas en datos recibidos desde una conexión real al sistema de trading (por ejemplo, desde [OpenECry](connectors/stock_market/openecry.md)), pero sin registro real de órdenes (la ejecución se emula en función de los libros de órdenes entrantes). 

Al usar los tres modos, se pone el máximo énfasis en que el código de la estrategia escrito con [Strategy](xref:StockSharp.Algo.Strategies.Strategy) no cambie al pasar de trading real a pruebas y viceversa. Esto se logra mediante la implementación de la interfaz principal [IConnector](xref:StockSharp.BusinessEntities.IConnector), que actúa como gateway al sistema de trading. Cómo se usa la interfaz ya se mostró en la sección [API](../api.md). En modo de pruebas, no actúa un sistema de trading real, sino la emulación (según el modo seleccionado). Por lo tanto, el código de la estrategia nunca sabrá si está operando con un exchange real o con una emulación. 
