# DPO

**Detrended Price Oscillator (DPO)** é um oscilador que elimina tendências de preço na tentativa de estimar a duração dos ciclos de preço de pico a pico ou de vale a vale. Ao contrário de outros osciladores, como a convergência estocástica ou a convergência/divergência de médias móveis (MACD), o DPO não é um indicador de momentum. Destaca picos e vales no preço, que são usados para estimar pontos de entrada e saída.

Para usar o indicador, deve ser usada a classe [DetrendedPriceOscillator](xref:StockSharp.Algo.Indicators.DetrendedPriceOscillator).

O Detrended Price Oscillator é calculado subtraindo uma média móvel simples (SMA) ao valor atual do preço. O comprimento da média móvel é determinado pelo utilizador.

![IndicatorDetrendedPriceOscillator](../../../../images/indicatordetrendedpriceoscillator.png)

## Ver também

[DMI](dmi.md)
