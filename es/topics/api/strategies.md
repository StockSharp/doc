# Estrategias en StockSharp

## Introducción

StockSharp proporciona una infraestructura potente para crear, probar y ejecutar estrategias de trading. La base para desarrollar estrategias de trading algorítmico es la clase base [Strategy](xref:StockSharp.Algo.Strategies.Strategy), que proporciona un conjunto de funciones y abstracciones estándar para trabajar con datos de mercado, ejecutar operaciones de trading y analizar resultados.

## Navegación

### Fundamentos de las estrategias

- [Suscripciones a datos de mercado en estrategias](strategies/subscriptions.md) - Guía detallada sobre el uso de suscripciones a datos de mercado en estrategias. Explica cómo crear y configurar suscripciones, gestionar su ciclo de vida y supervisar su estado.

- [Indicadores en estrategias](strategies/indicators.md) - Información sobre el trabajo con indicadores de análisis técnico en estrategias. Cubre la adición de indicadores a una estrategia, el control de su formación y su uso en la lógica de trading.

- [Operaciones de trading en estrategias](strategies/trading_operations.md) - Guía para realizar operaciones de trading en estrategias. Describe métodos para crear y enviar órdenes, cerrar posiciones y supervisar su estado.

- [Protección de posiciones](strategies/take_profit_and_stop_loss.md) - Descripción de los mecanismos para proteger posiciones abiertas mediante Take Profit y Stop Loss. Examina los enfoques local y de servidor para la protección de posiciones.

- [Parámetros de estrategia](strategies/parameters.md) - Guía para trabajar con parámetros de estrategia mediante [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1). Describe cómo crear parámetros configurables, configurar su visualización en la GUI y usarlos en la optimización.

- [Logging en estrategias](strategies/logging.md) - Guía para usar el mecanismo de logging en estrategias para hacer seguimiento y depurar el rendimiento del algoritmo.

### Funciones avanzadas

- [Compatibilidad de estrategias con plataformas](strategies/compatibility.md) - Recomendaciones para crear estrategias compatibles con distintas plataformas StockSharp: [Designer](../designer.md), [Shell](../shell.md), [Runner](../runner.md) y pruebas en la nube.

- [APIs de alto nivel en estrategias](strategies/high_level_api.md) - Descripción de métodos de alto nivel para simplificar el trabajo con suscripciones, indicadores, gráficos y protección de posiciones. Explica cómo escribir código más limpio centrándose en la lógica de trading.

- [Trabajo con gráficos en estrategias](strategies/chart.md) - Guía para visualizar datos de estrategia en un gráfico. Explica cómo acceder al gráfico, crear áreas, agregar elementos y renderizar datos.

- [Guardado y carga de ajustes](strategies/settings_saving_and_loading.md) - Descripción del mecanismo para guardar y cargar ajustes de estrategia mediante los métodos [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) y [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)).

- [Carga de estado](strategies/orders_and_trades_loading.md) - Guía para cargar órdenes y operaciones ejecutadas previamente en una estrategia, por ejemplo, al reiniciar una estrategia durante una sesión de trading.

- [Redondeo de precios](strategies/shrink_price.md) - Guía para redondear correctamente precios en estrategias mediante el método [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)).

- [Tipo Unit](strategies/unit_type.md) - Descripción del tipo de datos [Unit](xref:StockSharp.Messages.Unit) para simplificar operaciones aritméticas sobre cantidades como porcentajes, puntos o pips.

- [Modelo de eventos](strategies/event_model.md) - Explicación del modelo de eventos de estrategias basado en [IMarketRule](xref:StockSharp.Algo.IMarketRule). Cubre la creación de reglas para reaccionar a eventos de mercado, la combinación de condiciones y la gestión de ciclos de vida de reglas.

## Primeros pasos con el desarrollo de estrategias

Para empezar a desarrollar su propia estrategia, se recomienda:

1. Familiarizarse con los fundamentos del trabajo con estrategias para comprender los principios generales de las estrategias en StockSharp.

2. Estudiar la sección [Suscripciones a datos de mercado en estrategias](strategies/subscriptions.md) para comprender el mecanismo de recepción y procesamiento de datos de mercado.

3. Revisar la sección [Indicadores en estrategias](strategies/indicators.md) para comprender el trabajo con indicadores de análisis técnico.

4. Explorar la sección [Operaciones de trading en estrategias](strategies/trading_operations.md) para comprender los mecanismos de operaciones de trading.

5. Examinar la sección [Parámetros de estrategia](strategies/parameters.md) para aprender sobre los mecanismos de configuración de estrategias.

6. Conocer la sección [APIs de alto nivel en estrategias](strategies/high_level_api.md) para simplificar el código de estrategia mediante funciones integradas de alto nivel.

## Pruebas de estrategias

StockSharp proporciona varios métodos para probar estrategias:

- **Pruebas con datos históricos** - Permite evaluar la eficacia de la estrategia con datos históricos.
- **Optimización de parámetros** - Ayuda a encontrar valores óptimos de los parámetros de estrategia.
- **Pruebas con cuenta virtual** - Permite comprobar el rendimiento de la estrategia en tiempo real sin arriesgar fondos reales.

Puede encontrar descripciones detalladas de los métodos de prueba y evaluación del rendimiento de estrategias en la sección [Pruebas](../api/testing.md).
