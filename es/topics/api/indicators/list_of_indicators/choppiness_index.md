# CHOP

**Choppiness Index (CHOP)** es un indicador diseñado para determinar si el mercado se encuentra en un movimiento lateral (dentro de un rango) o en un estado de tendencia.

Para utilizar el indicador, debe utilizar la clase [ChoppinessIndex](xref:StockSharp.Algo.Indicators.ChoppinessIndex).

## Descripción

El Choppiness Index (CHOP) fue creado para evaluar cuantitativamente la volatilidad y determinar la naturaleza del movimiento del mercado. A diferencia de muchos otros indicadores, CHOP no pretende identificar la dirección de la tendencia ni generar señales de compra o venta. En cambio, ayuda a los operadores a determinar si el mercado está en consolidación (movimiento lateral) o en una tendencia direccional.

El indicador CHOP oscila entre 0 y 100:
- Los valores más cercanos a 100 indican una fuerte consolidación (alto "entrecortamiento")
- Los valores más cercanos a 0 indican una fuerte tendencia direccional (baja "entrecortamiento")

CHOP es particularmente útil para:
- Determinar una estrategia comercial adecuada basada en el carácter del mercado.
- Identificar transiciones de movimiento lateral a tendencia y viceversa
- Confirmar o refutar señales de otros indicadores.
- Evitar señales falsas durante la consolidación

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de cálculo (valor predeterminado: 14)

## Cálculo

El cálculo de Choppiness Index implica los siguientes pasos:

1. Calcule la suma de True Range durante el período seleccionado:
   ```
   Sum of TR = Sum(TR(i)) para i de 1 a Length
   ```

2. Calcule el High más alto y el Low más bajo durante el período seleccionado:
   ```
   Highest High = valor High máximo durante el periodo Length
   Lowest Low = valor Low mínimo durante el periodo Length
   ```

3. Calcule el índice CHOP:
   ```
   CHOP = 100 * LOG10(Sum TR / (Highest High - Lowest Low)) / LOG10(Length)
   ```

donde:
- TR: rango real para cada vela
- Length - período seleccionado
- LOG10 - logaritmo decimal

## Interpretación

- **Valores High CHOP (por encima de 60-70)** indican que el mercado se encuentra en un movimiento lateral (consolidación). Durante este período, es mejor evitar las estrategias de tendencia y considerar estrategias de negociación de rango.

- **Valores bajos de CHOP (por debajo de 30-40)** indica una fuerte tendencia direccional. Este es un buen momento para utilizar estrategias de tendencia y seguir el movimiento de precios.

- **Transiciones entre valores altos y bajos** puede indicar un cambio en el carácter del mercado. Una caída de CHOP desde valores altos puede indicar el comienzo de una nueva tendencia. Un aumento de CHOP desde valores bajos puede advertir sobre el agotamiento de la tendencia y la transición a la consolidación.

- **Setting threshold levels**: Normalmente, se utilizan los siguientes niveles de umbral:
  - Por encima de 60-70: High "entrecortamiento" (movimiento lateral)
  - 30-60: "entrecortamiento" moderado (estado de transición)
  - Por debajo de 30: Low "entrecortamiento" (tendencia fuerte)

![indicator_choppiness_index](../../../../images/indicator_choppiness_index.png)

## Véase también

[ATR](atr.md)
[ADX](adx.md)
[VHF](vhf.md)
[TrueRange](true_range.md)