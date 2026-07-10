# KPO

**Kase Peak Oscillator (KPO)** é um indicador técnico desenvolvido por Celia Kase que combina momentum e volatilidade para identificar potenciais máximos e mínimos do mercado.

Para utilizar o indicador, é necessário usar a classe [KasePeakOscillator](xref:StockSharp.Algo.Indicators.KasePeakOscillator).

## Descrição

O Kase Peak Oscillator (KPO) é uma ferramenta para determinar condições de sobrecompra e sobrevenda do mercado e identificar potenciais pontos de inversão. Foi desenvolvido pela trader e engenheira Celia Kase como parte da sua metodologia de negociação.

O KPO baseia-se no conceito de que os máximos e mínimos do mercado se formam quando o momentum do movimento do preço começa a esgotar-se. O oscilador usa uma combinação de indicadores de momentum e volatilidade para identificar estes pontos de viragem principais.

O indicador é um oscilador adimensional que flutua em torno da linha zero. Valores positivos indicam momentum ascendente, enquanto valores negativos indicam momentum descendente. Valores extremos do oscilador coincidem frequentemente com máximos e mínimos no gráfico de preços.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **ShortPeriod** - período curto para o cálculo do momentum (valor predefinido: 10)
- **LongPeriod** - período longo para o cálculo do momentum (valor predefinido: 30)

## Cálculo

O cálculo do Kase Peak Oscillator envolve vários passos:

1. Calcular o momentum de curto prazo com base no período curto:
   ```
   Short Momentum = EMA(Price, ShortPeriod) - EMA(Price, ShortPeriod)[previous]
   ```

2. Calcular o momentum de longo prazo com base no período longo:
   ```
   Long Momentum = EMA(Price, LongPeriod) - EMA(Price, LongPeriod)[previous]
   ```

3. Calcular a volatilidade actual:
   ```
   Volatility = ATR(ShortPeriod)
   ```

4. Normalizar o momentum relativamente à volatilidade:
   ```
   Normalized Short Momentum = Short Momentum / Volatility
   Normalized Long Momentum = Long Momentum / Volatility
   ```

5. Cálculo final do KPO:
   ```
   KPO = Normalized Short Momentum - Normalized Long Momentum
   ```

Onde:
- Price - normalmente o preço de fecho
- EMA - média móvel exponencial
- ATR - intervalo verdadeiro médio
- ShortPeriod - período curto de cálculo
- LongPeriod - período longo de cálculo

## Interpretação

O Kase Peak Oscillator pode ser interpretado da seguinte forma:

1. **Cruzamentos da Linha Zero**:
   - O cruzamento de baixo para cima pode ser visto como um sinal altista
   - O cruzamento de cima para baixo pode ser visto como um sinal baixista

2. **Valores Extremos**:
   - Valores positivos elevados podem indicar condições de sobrecompra do mercado e uma potencial inversão descendente
   - Valores negativos elevados podem indicar condições de sobrevenda do mercado e uma potencial inversão ascendente

3. **Divergências**:
   - Divergência altista (o preço forma um novo mínimo, enquanto o KPO forma um mínimo mais alto) pode sinalizar uma próxima inversão ascendente
   - Divergência baixista (o preço forma um novo máximo, enquanto o KPO forma um máximo mais baixo) pode sinalizar uma próxima inversão descendente

4. **Cruzamentos dos Componentes**:
   - Quando o momentum de curto prazo cruza o momentum de longo prazo de baixo para cima, pode ser visto como um sinal altista
   - Quando o momentum de curto prazo cruza o momentum de longo prazo de cima para baixo, pode ser visto como um sinal baixista

5. **Aceleração e Desaceleração**:
   - O aumento da inclinação do KPO indica aceleração do momentum
   - A diminuição da inclinação do KPO indica desaceleração do momentum, que pode anteceder uma inversão

6. **Combinação com Outros Indicadores**:
   - O KPO é frequentemente usado com outros indicadores técnicos para confirmar sinais
   - É particularmente eficaz quando combinado com indicadores de tendência e níveis de suporte/resistência

![indicator_kase_peak_oscillator](../../../../images/indicator_kase_peak_oscillator.png)

## Ver Também

[MomentumOscillator](momentum.md)
[MACD](macd.md)
[PrettyGoodOscillator](pretty_good_oscillator.md)
[ATR](atr.md)
