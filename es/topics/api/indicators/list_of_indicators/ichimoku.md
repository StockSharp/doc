# Ichimoku

**Ichimoku** es un indicador representado por una combinación de cinco líneas, tres de las cuales son medias móviles y dos son derivadas de estas. Ichimoku identifica la presencia de una tendencia y también indica zonas de soporte/resistencia y retrocesos de tendencia.

Para utilizar el indicador, se debe utilizar la clase [Ichimoku](xref:StockSharp.Algo.Indicators.Ichimoku).
##### Descripción del indicador Ichimoku
  
Gráficamente, el indicador consta de cinco líneas de colores similares a medias móviles simples:  
  
- Tenkan (línea de conversión): la línea más rápida, reacciona primero a los cambios de precios. Su objetivo principal es determinar la dirección de la tendencia a corto plazo. En la versión clásica, retrocede un segmento de 9 compases. Se construye como la mitad de la suma de los precios más alto y más bajo.  
  
- Kijun (línea de base): indica la tendencia a mediano plazo, con un período de 26.  
  
- Senkou A y Senkou B: proyectados y mostrados en 26 períodos hacia el futuro, juntos forman lo que se llama la nube (Kumo), que muestra áreas de soporte y resistencia y es un componente clave del indicador.  
  
- Chikou (lapso rezagado): representa el último precio de cierre, retrasado 26 períodos. Ayuda a confirmar las señales: si cruza el gráfico de abajo hacia arriba, es una señal de compra, y de arriba a abajo, una señal de venta. Básicamente, Chikou actúa como un filtro de tendencias.  

![IndicatorIchimoku](../../../../images/indicatorichimoku.png)

## Véase también

[JMA](jma.md)
