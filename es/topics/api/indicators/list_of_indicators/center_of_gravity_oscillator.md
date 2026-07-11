# CGO

**Oscilador de centro de gravedad (CGO)** es un indicador técnico desarrollado por John Ehlers, basado en el concepto de centro de gravedad en física y aplicado al análisis del movimiento de precios en el mercado.

Para utilizar el indicador, debe utilizar la clase [CenterOfGravityOscillator](xref:StockSharp.Algo.Indicators.CenterOfGravityOscillator).

## Descripción

El Oscilador de centro de gravedad (CGO) es un indicador adelantado que intenta identificar puntos de reversión del mercado tratando la serie de precios como un sistema físico y determinando su "centro de gravedad". El indicador calcula dónde se encuentra el "equilibrio" en los movimientos actuales de precios y utiliza esta información para pronosticar futuros cambios de dirección de tendencia.

CGO es particularmente útil para:
- Identificar posibles puntos de reversión antes de que aparezcan en el gráfico de precios.
- Revelando la fuerza y la debilidad de la tendencia actual
- Detectar divergencias ocultas entre el precio y el indicador.
- Creación de sistemas de trading basados en señales líderes.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de cálculo (valor predeterminado: 10)

## Cálculo

El cálculo de Oscilador de centro de gravedad (CGO) se basa en la fórmula:

```
CGO = - Sum(Price(i) * (i + 1)) / Sum(Price(i))
```

donde:
- i - índice de valor de precio en el período de 0 a (Length-1)
- Price(i) - precio (normalmente precio de cierre) para el índice i correspondiente
- Sum: suma de todos los valores en el período Length

En esta fórmula, cada precio se pondera por su posición en la serie temporal y luego se normaliza por la suma total de precios. Se agrega el signo menos antes de la fórmula para hacer que el indicador aumente cuando el precio sube, haciéndolo más intuitivo.

## Interpretación

- **Cruce de línea cero**: Cuando CGO cruza la línea cero de abajo hacia arriba, esto puede verse como una señal alcista. Cruzar de arriba a abajo puede indicar una señal bajista.

- **Extremos del indicador**: Cuando CGO alcanza extremos (máximos o mínimos), esto puede indicar un posible cambio de tendencia.

- **Divergencias**: 
  - Divergencia alcista: cuando el precio forma un nuevo mínimo, pero CGO no lo confirma, formando un mínimo más alto.
  - Divergencia bajista: cuando el precio alcanza un nuevo máximo, pero CGO forma un máximo más bajo.

- **Movimiento del indicador**: El movimiento rápido CGO en una dirección puede indicar el comienzo de una nueva tendencia. Si el indicador se mueve lentamente u oscila alrededor de la línea cero, esto puede indicar una consolidación del mercado.

Dado que CGO es un indicador adelantado, sus señales suelen aparecer antes de los cambios correspondientes en el gráfico de precios, lo que brinda a los operadores una ventaja a la hora de tomar decisiones de trading.

![Gráfico del indicador CGO](../../../../images/indicator_center_of_gravity_oscillator.png)

## Véase también

[SineWave](sine_wave.md)
[HarmonicOscillator](harmonic_oscillator.md)
[FisherTransform](ehlers_fisher_transform.md)
[RVI](rvi.md)