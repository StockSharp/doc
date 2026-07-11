# FDI

**índice de dimensión fractal (FDI)** cuantifica la rugosidad de una serie de precios.

Para utilizar el indicador, debe utilizar la clase [FractalDimension](xref:StockSharp.Algo.Indicators.FractalDimension).

## Descripción

El FDI varía de 1 a 2 y refleja el comportamiento del mercado:
- Los valores cercanos a 1 indican una tendencia persistente (camino más suave).
- Los valores alrededor de 1,5 corresponden a un paseo aleatorio.
- Los valores cercanos a 2 denotan un mercado oscilante o ruidoso.

El indicador se basa en la geometría fractal y mide la complejidad de la trayectoria del precio.

## Parámetros

El indicador tiene el siguiente parámetro:
- **Length** – período de cálculo (valor predeterminado: 30)

## Cálculo

FDI se calcula comparando la longitud total de la trayectoria del precio con el rango alto-bajo general:

1. Sum diferencias absolutas entre precios consecutivos durante el período para obtener la longitud de la trayectoria de precios.
2. Encuentre la diferencia entre el máximo máximo y el mínimo mínimo para el período.
3. Calcule FDI usando:
   ```
   FDI = 1 + (log(PathLength) - log(Range)) / log(2 * (Length - 1))
   ```
4. Sujete el resultado entre 1 y 2.

## Interpretación

- **FDI near 1** – fuerte comportamiento de tendencia.
- **FDI around 1.5** – paseo aleatorio; La fuerza de la tendencia es neutral.
- **FDI closer to 2** – mercado entrecortado o lateral.

![Gráfico del indicador FDI](../../../../images/indicator_fractal_dimension.png)

## Véase también

[Exponente de Hurst](hurst_exponent.md)

[Media móvil adaptativa fractal](fractal_adaptive_moving_average.md)
