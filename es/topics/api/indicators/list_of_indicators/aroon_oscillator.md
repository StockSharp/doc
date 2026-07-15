# oscilador Aroon

El **oscilador Aroon** mide la diferencia entre las líneas Aroon alcista y Aroon bajista. Destaca de qué lado del mercado
es dominante y qué tan fuerte es la tendencia actual.

Utilice la clase [AroonOscillator](xref:StockSharp.Algo.Indicators.AroonOscillator) para trabajar con este indicador.

## Descripción

El oscilador oscila entre −100 y +100:

- los valores positivos muestran que Aroon alcista está por encima de Aroon bajista y el mercado está dominado por compradores;
- los valores negativos indican que Aroon bajista está liderando y los bajistas tienen el control;
- las lecturas cercanas a cero reflejan equilibrio o consolidación.

Cuanto más se aleje el valor de cero, más fuerte será el movimiento direccional.

## Parámetros

- **Longitud**: período utilizado para los cálculos subyacentes de Aroon. Los valores más grandes proporcionan lecturas más suaves con una respuesta más lenta.

## Cálculo

1. Calcule las series Aroon alcista y Aroon bajista con el `Length` seleccionado.
2. Resta las dos líneas:
   `oscilador Aroon = Aroon alcista − Aroon bajista`.

## Interpretación

- **oscilador Aroon > 0** — dominio alcista.
- **oscilador Aroon < 0** — dominio bajista.
- **Zero-line cross**: posible cambio en la tendencia predominante.
- **Valores extremos**: fuerte tendencia direccional, utilizada a menudo como filtro direccional.

El oscilador se analiza frecuentemente junto con el indicador base [Aroon](aroon.md) para observar tanto los niveles absolutos como sus
diferencia.

![Gráfico del indicador oscilador Aroon](../../../../images/indicator_aroon_oscillator.png)

## Véase también

[Aroon](aroon.md)
[ADX](adx.md)
[DMI](dmi.md)
