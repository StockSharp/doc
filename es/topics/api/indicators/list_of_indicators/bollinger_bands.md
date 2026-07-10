# Bollinger Bands

**Bollinger Bands** son un indicador oscilante utilizado para medir la volatilidad del mercado. Permite evaluar si el precio es alto o bajo respecto a la media móvil. La banda central corresponde a la media móvil simple del precio. Las bandas superior e inferior son niveles en los que el precio puede considerarse alto o bajo en relación con la media móvil.

Para utilizar el indicador, se debe utilizar la clase [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands).
##### Cálculo

Los siguientes parámetros con la configuración correspondiente se utilizan para calcular Bollinger Bands:
- tipo de desviación estándar: normalmente el doble;
- período de media móvil, a discreción del operador.

Así, el indicador está formado por tres líneas: central, superior e inferior, cada una con su fórmula:

Línea media (ML) = Media móvil (SMA (Close, N))
Banda superior = ML + (D x desviación estándar)
Banda inferior = ML - (D x desviación estándar), donde

D - el ancho del canal establecido en la configuración, Desviación estándar (StdDev) - desviación estándar, calculada mediante la fórmula: SQRT(Sum(Close, n))^2, n)/n), donde
Sum - la suma de n períodos, n - período de cálculo, SQRT - raíz cuadrada, Close - precio de cierre.

![IndicatorBollingerBands](../../../../images/indicatorbollingerbands.png)

## Véase también

[CHV](chv.md)
