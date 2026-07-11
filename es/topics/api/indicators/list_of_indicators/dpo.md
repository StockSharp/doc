# DPO

**oscilador de precio sin tendencia (DPO)** es un oscilador que elimina las tendencias de precios en un intento de estimar la duración de los ciclos de precios de pico a pico o de valle a valle. A diferencia de otros osciladores, como la convergencia estocástica o la divergencia de convergencia de media móvil (MACD), DPO no es un indicador de impulso. Destaca los picos y valles del precio, que se utilizan para estimar los puntos de entrada y salida.

Para utilizar el indicador, se debe utilizar la clase [DetrendedPriceOscillator](xref:StockSharp.Algo.Indicators.DetrendedPriceOscillator).

El oscilador de precio sin tendencia se calcula restando una media móvil simple (SMA) del valor del precio actual. La longitud de la media móvil la determina el usuario.

![IndicatorDetrendedPriceOscillator](../../../../images/indicatordetrendedpriceoscillator.png)

## Véase también

[DMI](dmi.md)
