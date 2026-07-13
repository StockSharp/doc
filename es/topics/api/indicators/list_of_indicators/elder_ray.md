# Rayos de Elder

El **índice Elder-Ray** es un indicador compuesto de Alexander Elder que combina una media móvil exponencial con osciladores de fuerza alcista
y fuerza bajista. Visualiza el equilibrio entre compradores y vendedores y ayuda a identificar cuándo una de las partes pierde el control.

Utilice la clase [ElderRay](xref:StockSharp.Algo.Indicators.ElderRay) para acceder al indicador.

## Componentes

El indicador devuelve una estructura [ElderRayValue](xref:StockSharp.Algo.Indicators.ElderRayValue) que contiene:

- **EMA** — la media móvil exponencial base de los precios de cierre;
- **fuerza alcista** — la distancia entre la barra alta y el EMA;
- **fuerza bajista** — la distancia entre la barra baja y el EMA.

## Parámetros

Rayos de Elder hereda la configuración de [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage):

- **Length** — período EMA;
- **Alpha** — coeficiente de suavizado, cuando se configura directamente.

## Interpretación

- **fuerza alcista > 0** junto con un EMA en aumento confirma una tendencia alcista.
- **fuerza bajista < 0** con un EMA a la baja confirma una tendencia bajista.
- La reducción de fuerza alcista ante el aumento de precios o la subida de fuerza bajista ante la caída de precios forman divergencias y advierten sobre reversiones.
- Los cruces de la línea cero de fuerza alcista o fuerza bajista marcan el cambio en el control del mercado.

Las decisiones de negociación se toman analizando el EMA y ambos osciladores simultáneamente. Por ejemplo, una oportunidad de compra aparece cuando
el EMA está subiendo, el fuerza bajista se recupera de un nuevo mínimo y el fuerza alcista supera el cero.

![Gráfico del indicador Rayos de Elder](../../../../images/indicator_elder_ray.png)

## Véase también

[fuerza alcista](bull_power.md)
[fuerza bajista](bear_power.md)
[Media móvil exponencial](ema.md)
