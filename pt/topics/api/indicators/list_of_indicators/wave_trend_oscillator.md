# WTO

**Oscilador de tendência de onda (WTO)** é um indicador técnico desenvolvido para identificar níveis de sobrecompra e sobrevenda do mercado, bem como para detetar flutuações cíclicas de preço. O WTO combina elementos de canais e osciladores, tornando-se uma ferramenta eficaz para identificar o momentum do mercado e potenciais pontos de inversão.

Para usar o indicador, é necessário usar a classe [WaveTrendOscillator](xref:StockSharp.Algo.Indicators.WaveTrendOscillator).

## Descrição

O Oscilador de tendência de onda foi concebido para filtrar o ruído do mercado e destacar os movimentos principais do preço. O indicador oscila em torno da linha zero, criando padrões de onda que se correlacionam com movimentos cíclicos do preço.

Características principais do WTO:
- Oscilações em torno da linha zero, em que valores positivos indicam uma tendência ascendente e valores negativos indicam uma tendência descendente
- Níveis de sobrecompra (normalmente acima de +60) e sobrevenda (normalmente abaixo de -60)
- Capacidade de filtrar ruído de preço e destacar movimentos principais

Principais sinais do indicador:
- Cruzamento da linha zero (alteração da direção da tendência)
- Saída de zonas de sobrecompra/sobrevenda
- Divergências entre WTO e preço (potenciais inversões)
- Padrões de onda específicos

## Parâmetros

- **EsaPeriod** - período da EMA para calcular o valor ESA (tipicamente 10)
- **DPeriod** - período para calcular o desvio (tipicamente 21)
- **AveragePeriod** - período para calcular a média do oscilador final (tipicamente 4)

## Cálculo

O cálculo do Oscilador de tendência de onda envolve vários passos:

1. Calcular o preço típico:
   ```
   AP = (High + Low + Close) / 3
   ```

2. Criar o valor suavizado e absoluto da primeira medição:
   ```
   ESA = EMA(AP, EsaPeriod)
   D = EMA(Abs(AP - ESA), DPeriod)
   ```

3. Calcular a primeira linha do oscilador:
   ```
   CI = (AP - ESA) / (0.015 * D)
   ```

4. Suavizar o oscilador para obter o valor final do WTO:
   ```
   WTO = EMA(CI, AveragePeriod)
   ```

Os valores típicos para os parâmetros do indicador são: EsaPeriod = 10, DPeriod = 21, AveragePeriod = 4, mas podem ser adaptados a diferentes períodos temporais e instrumentos.

![Gráfico do indicador WTO](../../../../images/indicator_wave_trend_oscillator.png)

## Ver também

[MACD](macd.md)
[Oscilador estocástico](stochastic_oscillator.md)
