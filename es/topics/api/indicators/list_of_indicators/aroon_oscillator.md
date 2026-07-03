# Aroon Oscillator

El **Aroon Oscillator** mide la diferencia entre las líneas Aroon Up y Aroon Down. Destaca de qué lado del mercado
es dominante y qué tan fuerte es la tendencia actual.

Utilice la clase [AroonOscillator](xref:StockSharp.Algo.Indicators.AroonOscillator) para trabajar con este indicador.

## Descripción

El oscilador oscila entre −100 y +100:

- los valores positivos muestran que Aroon Up está por encima de Aroon Down y el mercado está dominado por compradores;
- los valores negativos indican que Aroon Down está liderando y los bajistas tienen el control;
- las lecturas cercanas a cero reflejan equilibrio o consolidación.

Cuanto más se aleje el valor de cero, más fuerte será el movimiento direccional.

## Parámetros

- **Length**: período utilizado para los cálculos subyacentes de Aroon. Los valores más grandes proporcionan lecturas más suaves con una respuesta más lenta.

## Cálculo

1. Calcule las series Aroon Up y Aroon Down con el `Length` seleccionado.
2. Resta las dos líneas:  
   `Aroon Oscillator = Aroon Up − Aroon Down`.

## Interpretación

- **Aroon Oscillator > 0** — dominio alcista.
- **Aroon Oscillator < 0** — dominio bajista.
- **Zero-line cross**: posible cambio en la tendencia predominante.
- **Valores extremos**: fuerte tendencia direccional, utilizada a menudo como filtro direccional.

El oscilador se analiza frecuentemente junto con el indicador base [Aroon](aroon.md) para observar tanto los niveles absolutos como sus
diferencia.

![indicator_aroon_oscillator](../../../../images/indicator_aroon_oscillator.png)

## Véase también

[Aroon](aroon.md)
[ADX](adx.md)
[DMI](dmi.md)
