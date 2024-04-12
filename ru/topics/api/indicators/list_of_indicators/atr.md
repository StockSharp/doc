# ATR

**Средний истинный диапазон (Average True Range, ATR)** — индикатор показывающий уровень текущей волатильности. 

Для использования индикатора необходимо использовать класс [AverageTrueRange](xref:StockSharp.Algo.Indicators.AverageTrueRange). 
##### Расчет индикатора  
  
Расчет индикатора начинается с определения истинного диапазона (True Range, TR), который вычисляется как максимум из следующих трех величин:  
- разность между текущими максимумом и минимумом;  
- разность между текущим максимумом и предыдущей ценой закрытия (абсолютная величина);  
- разность между текущим минимумом и предыдущей ценой закрытия (абсолютная величина).  

TRt = max (High(t)\-Low(t) ; High(t) \- Close(t\-1) ; Close(t\-1)\-Low(t))  

Абсолютная величина используется для обеспечения положительных значений, так как мы интересуемся расстоянием между двумя точками, а не направлением движения цен.  
  
На основе этого показателя рассчитывается уже ATR. У него есть единственный параметр \- это период N. По умолчанию берется 14-периодный индикатор, но его можно настроить под собственную стратегию. Вот как выглядит формула (это одна из форм экспоненциальной скользящей средней)  
  
ATR(t) = ((ATR(t\-1) x (N\-1)) + TR(t)) \/ N  

![IndicatorAverageTrueRange](../../../../images/indicatoraveragetruerange.png)

## См. также

[AO](ao.md)