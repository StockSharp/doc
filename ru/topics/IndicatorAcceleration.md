# A\/D

**Замедления\/Ускорения (Acceleration\/Deceleration , A\/D)** является осциллятором, который был создан Биллом Уильямсом. Он измеряет ускорение и замедление импульса тренда.

Для использования индикатора необходимо использовать класс [Acceleration](xref:StockSharp.Algo.Indicators.Acceleration). 
##### Расчет  
  
Гистограмма А/\D — это разность между значением 5/\34 гистограммы движущей силы и 5-периодным простым скользящим средним, взятым от этой гистограммы.  Значения взяты для классического осциллятора, в настройках всегда можно указать свои собственные параметры.

MEDIAN PRICE = (HIGH + LOW) / 2  
AO = SMA (MEDIAN PRICE, 5) — SMA (MEDIAN PRICE, 34)  
A\/D = AO — SMA (AO, 5)  
  
где:  
  
MEDIAN PRICE — медианная цена;  
HIGH — максимальная цена бара;  
LOW — минимальная цена бара;  
SMA — простое скользящее среднее;  
AO — индикатор [Awesome Oscillator](IndicatorAwesomeOscillator.md).  

В качестве параметров задаются значения периодов SMA.

![IndicatorAcceleration](../images/IndicatorAcceleration.png)

## См. также

[Alligator](IndicatorAlligator.md)
