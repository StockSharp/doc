# Aroon

**Indicador Aroon** es un indicador técnico desarrollado por Tushar Chande en 1995 para identificar cambios de tendencia y su fuerza. El nombre "Aroon" proviene de una palabra sánscrita que significa "amanecer de una nueva era".

Para utilizar el indicador, debe utilizar la clase [Aroon](xref:StockSharp.Algo.Indicators.Aroon).

## Descripción

El indicador Aroon consta de dos líneas:
- **Aroon alcista** - mide la fuerza de una tendencia alcista
- **Aroon bajista** - mide la fuerza de una tendencia a la baja

Aroon ayuda a determinar:
- El comienzo de una nueva tendencia.
- La fuerza de la tendencia actual
- consolidación y movimiento lateral
- Posibles cambios de tendencia

El indicador es particularmente útil para identificar las primeras etapas de la formación de una nueva tendencia y para determinar los períodos de consolidación.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de cálculo (normalmente se utilizan entre 14 y 25 períodos)

## Cálculo

El cálculo del indicador Aroon se basa en determinar el tiempo (número de periodos) transcurrido desde que se alcanzaron los precios máximo y mínimo dentro del periodo especificado:

1. Aroon alcista se calcula mediante la fórmula:
   ```
   Aroon alcista = ((Length - Periods since high) / Length) * 100
   ```

2. Aroon bajista se calcula mediante la fórmula:
   ```
   Aroon bajista = ((Length - Periods since low) / Length) * 100
   ```

donde:
- Length - período seleccionado
- "Períodos desde máximo": número de períodos desde que se alcanzó el precio más alto dentro del período Length
- "Períodos desde el nivel más bajo": número de períodos desde que se alcanzó el precio más bajo dentro del período Length

Ambas líneas Aroon oscilan entre 0 y 100:
- Un valor de 100 significa que se alcanzó máximo/mínimo en el período más reciente
- Un valor de 0 significa que máximo/mínimo se alcanzó hace Length períodos

## Interpretación

- **Fuerte tendencia alcista**: Aroon alcista está cerca de 100 y Aroon bajista está cerca de 0
- **Fuerte tendencia a la baja**: Aroon bajista está cerca de 100 y Aroon alcista está cerca de 0
- **Movimiento lateral**: ambas líneas se mueven paralelas entre sí en niveles bajos
- **Posible reversión de tendencia**: cruce de líneas Aroon alcista y Aroon bajista
- **Consolidación**: ambas líneas oscilan alrededor de 50

![Aroon](../../../../images/indicator_aroon.png)

## Véase también

[ADX](adx.md)
[DMI](dmi.md)