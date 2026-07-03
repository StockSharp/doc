# Indicador

![Designer Indicator 00](../../../../../../images/designer_indicator_00.png)

Este bloque se usa para calcular valores de indicadores.

## Sockets de entrada

- **Any Data** – tipo específico de datos con base en el cual debe calcularse el indicador seleccionado (según el indicador, puede ser un valor numérico, una vela, etc.).

## Sockets de salida

- **Indicator** – valor calculado del indicador, que puede usarse para mostrarlo en el panel de gráfico o para cálculos posteriores.

## Parámetros

- **Indicator Type** - parámetro usado para seleccionar el indicador deseado, y varios parámetros adicionales que corresponden al tipo de indicador seleccionado. El conjunto de estos parámetros cambia cuando cambia el tipo de indicador seleccionado.
- **Final** - pasar solo [valores finales](../../../../../api/indicators.md) del indicador.
- **Formed** - pasar solo valores cuando el indicador está completamente [formado](../../../../../api/indicators.md).

![Designer Indicator 01](../../../../../../images/designer_indicator_01.png)

## Véase también

[Lista de indicadores](../../../../../api/indicators/list_of_indicators.md)
[Condición lógica](logical_condition.md)
