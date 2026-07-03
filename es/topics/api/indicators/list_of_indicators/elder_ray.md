# Elder Ray

El **Elder Ray Index** es un indicador compuesto de Alexander Elder que combina una media móvil exponencial con el Bull Power.
y osciladores Bear Power. Visualiza el equilibrio entre compradores y vendedores y ayuda a identificar cuándo una de las partes pierde el control.

Utilice la clase [ElderRay](xref:StockSharp.Algo.Indicators.ElderRay) para acceder al indicador.

## Componentes

El indicador devuelve una estructura [ElderRayValue](xref:StockSharp.Algo.Indicators.ElderRayValue) que contiene:

- **EMA** — la media móvil exponencial base de los precios de cierre;
- **Bull Power** — la distancia entre la barra alta y el EMA;
- **Bear Power** — la distancia entre la barra baja y el EMA.

## Parámetros

Elder Ray hereda la configuración de [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage):

- **Length** — período EMA;
- **Alpha** — coeficiente de suavizado, cuando se configura directamente.

## Interpretación

- **Bull Power > 0** junto con un EMA en aumento confirma una tendencia alcista.
- **Bear Power < 0** con un EMA a la baja confirma una tendencia bajista.
- La reducción de Bull Power ante el aumento de precios o la subida de Bear Power ante la caída de precios forman divergencias y advierten sobre reversiones.
- Zero-line crossings de Bull Power o Bear Power marcan el cambio en el control del mercado.

Las decisiones de trading se toman analizando el EMA y ambos osciladores simultáneamente. Por ejemplo, una oportunidad de compra aparece cuando
el EMA está subiendo, el Bear Power se recupera de un nuevo mínimo y el Bull Power supera el cero.

![indicator_elder_ray](../../../../images/indicator_elder_ray.png)

## Véase también

[Bull Power](bull_power.md)
[Bear Power](bear_power.md)
[ExponentialMovingAverage](ema.md)
