# Parabolic SAR

**Parabolic SAR (SAR)** \- Трендовый индикатор, который указывает точки остановки и разворота цены, а также направление тренда. 

Для использования индикатора необходимо использовать класс [ParabolicSar](xref:StockSharp.Algo.Indicators.ParabolicSar). 
##### Расчет индикатора  
  
Цена точки индикатора (SAR) для очередного периода (свечки) вычисляется по следующим формулам:  
  
SAR(n+1) = SAR(n) + a * (high — SAR(n)), для восходящего тренда;  
SAR(n+1) = SAR(n) + a * (low — SAR(n)), для нисходящего тренда, где:  

SAR(n+1) — цена для периода n+1;  
SAR(n) — цена для периода n;  
high и low — новый максимум и минимум соответственно (экстремумы). Учитываются на временном промежутке между срабатыванием предыдущего сигнала индикатора и текущим моментом;  
  
a — фактор ускорения.  
  
Фактор ускорения — плавающий коэффициент, характеризующийся минимальным, максимальным значением и шагом изменения.  
  
Минимальное значение равное одному шагу фактор принимает в точке разворота, и как только цена достигнет нового экстремального значения по тренду (high или low), фактора увеличивается на шаг. При достижении фактором своего максимального значения, его рост приостанавливается.  
  
![IndicatorParabolicSar](../../../../images/indicatorparabolicsar.png)

## См. также

[Peak](peak.md)