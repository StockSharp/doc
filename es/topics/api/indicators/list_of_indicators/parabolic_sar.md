# Parabolic SAR

**Parabolic SAR (SAR)** - Un indicador de tendencia que indica los puntos de parada y reversión del precio, así como la dirección de la tendencia.

Para utilizar el indicador, se debe utilizar la clase [ParabolicSar](xref:StockSharp.Algo.Indicators.ParabolicSar).
##### Cálculo del indicador
  
El precio del punto indicador (SAR) para el siguiente período (vela) se calcula mediante las siguientes fórmulas:
  
SAR(n+1) = SAR(n) + a * (alto - SAR(n)), para una tendencia alcista;  
SAR(n+1) = SAR(n) + a * (bajo - SAR(n)), para una tendencia bajista, donde:

SAR(n+1) — precio para el período n+1;  
SAR(n) — precio para el período n;  
alto y bajo: nuevos máximo y mínimo respectivamente (extremos). Se consideran para el intervalo de tiempo entre la activación de la señal indicadora anterior y el momento actual;
  
a — factor de aceleración.
  
El factor de aceleración es un coeficiente flotante, caracterizado por valores mínimos, máximos y un paso de cambio.
  
El factor toma un valor mínimo igual a un paso en el punto de reversión, y tan pronto como el precio alcanza un nuevo valor extremo según la tendencia (alta o baja), el factor aumenta en un paso. Cuando el factor alcanza su valor máximo, se detiene su crecimiento.
  
![IndicatorParabolicSar](../../../../images/indicatorparabolicsar.png)

## Véase también

[Peak](peak.md)
