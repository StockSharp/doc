# Rank Correlation Index

El **Rank Correlation Index (RCI)** es un oscilador basado en el coeficiente de correlación de rango de Spearman. Compara los rangos de precios.
con el tiempo se ubica dentro de la ventana móvil y muestra qué tan cerca está el movimiento reciente de una secuencia perfectamente ascendente o descendente.

Utilice la clase [RankCorrelationIndex](xref:StockSharp.Algo.Indicators.RankCorrelationIndex) para acceder al indicador.

## Cálculo

1. Asigne a cada punto de datos dentro de la ventana **Length** un rango de tiempo (1 para el valor más antiguo, `Length` para el más reciente).
2. Clasifique los precios por valor (1 para el precio más bajo, `Length` para el más alto).
3. Calcule la diferencia `d = RankTime − RankPrice` para cada barra.
4. Aplicar la fórmula de Spearman:
   `RCI = 1 − (6 × Σ d²) / (Length × (Length² − 1))`.

Cuando se multiplica por 100, el indicador oscila entre −100 y +100.

## Parámetros

- **Length** — tamaño de ventana para el procedimiento de clasificación.

## Interpretación

- **RCI ≈ +100** — secuencia perfectamente ascendente (fuerte tendencia alcista).
- **RCI ≈ −100** — secuencia de caída perfecta (fuerte tendencia bajista).
- **RCI around 0**: mercado aleatorio o lateral.
- Divergencias entre precio y el RCI advierten de posibles reversiones.

El indicador es útil para evaluar tendencias a corto plazo y detectar puntos de inflexión, especialmente cuando se combina con herramientas de impulso.

![indicator_rank_correlation_index](../../../../images/indicator_rank_correlation_index.png)

## Véase también

[Momentum](momentum.md)
[RoC](roc.md)
[RSI](rsi.md)
