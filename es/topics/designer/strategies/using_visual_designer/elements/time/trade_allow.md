# Negociación permitida

![Captura de Negociación permitida](../../../../../../images/designer_tradealloweddiagramelement_00.png)

Este bloque se usa para comprobar si actualmente se permite operar. Se comprueban las siguientes condiciones:

- Todas las suscripciones de la estrategia a datos de mercado deben estar en estado [Online](../../../../../api/market_data/subscriptions.md) (recepción de datos en tiempo real).
- Todos los indicadores deben estar [formados](../../../../../api/indicators.md).
- En caso de [negociación en vivo](../../../../live_execution/getting_started.md), el valor del disparador entrante debe tener una marca de tiempo mayor que la hora de inicio de la estrategia.

### Conectores de entrada


- **Activador** - señal que determina el momento en que debe realizarse la comprobación.

### Conectores de salida


- **Indicador** - bandera que determina si la sesión de negociación está activa.

## Véase también

[Hora actual](current_time.md)
