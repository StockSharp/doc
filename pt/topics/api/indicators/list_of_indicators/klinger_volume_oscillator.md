# KVO

**Klinger Volume Oscillator (KVO)** é um indicador técnico desenvolvido por Stephen Klinger que usa volume e preço para identificar tendências de longo prazo e inversões de curto prazo no mercado.

Para utilizar o indicador, é necessário usar a classe [KlingerVolumeOscillator](xref:StockSharp.Algo.Indicators.KlingerVolumeOscillator).

## Descrição

O Klinger Volume Oscillator (KVO) foi criado por Stephen Klinger para medir a divergência entre volume e preço. O indicador baseia-se no conceito de que o movimento do preço é confirmado pelo volume. O KVO procura determinar não só a direcção da tendência, mas também a sua força e potenciais pontos de inversão.

O KVO combina informação de preço com volume usando um indicador Volume Force que considera tanto a direcção como a magnitude do movimento do preço, bem como o volume de negociação. Em seguida, aplica médias móveis exponenciais (EMA) com dois períodos diferentes a este fluxo monetário e calcula a diferença entre elas.

O indicador é um oscilador que flutua acima e abaixo da linha zero. Valores positivos do KVO indicam que os compradores controlam o mercado, enquanto valores negativos indicam que os vendedores têm vantagem.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **ShortPeriod** - período para calcular a EMA curta (valor predefinido: 34)
- **LongPeriod** - período para calcular a EMA longa (valor predefinido: 55)

## Cálculo

O cálculo do Klinger Volume Oscillator envolve vários passos:

1. Determinar a tendência para cada período:
   ```
   Trend = +1, if (High + Low + Close) > (High[previous] + Low[previous] + Close[previous])
   Trend = -1, otherwise
   ```

2. Calcular o indicador Volume Force:
   ```
   Volume Force = Volume * Trend * abs(2 * ((Close - Low) - (High - Close)) / (High - Low))
   ```
   Se (High - Low) for zero, Volume Force é definido como o volume multiplicado pela tendência.

3. Calcular a EMA para dois períodos:
   ```
   EMA curta = EMA(Força de volume, ShortPeriod)
   EMA longa = EMA(Força de volume, LongPeriod)
   ```

4. Cálculo final do KVO:
   ```
   KVO = EMA curta - EMA longa
   ```

5. Calcular a linha de sinal (opcional):
   ```
   Linha de sinal = EMA(KVO, 13)
   ```

Onde:
- High, Low, Close - preços máximo, mínimo e de fecho
- Volume - volume de negociação
- EMA - média móvel exponencial
- ShortPeriod - período para a EMA curta
- LongPeriod - período para a EMA longa

## Interpretação

O Klinger Volume Oscillator pode ser interpretado da seguinte forma:

1. **Cruzamentos da Linha Zero**:
   - O KVO cruzar a linha zero de baixo para cima pode ser visto como um sinal altista
   - O KVO cruzar a linha zero de cima para baixo pode ser visto como um sinal baixista

2. **Cruzamentos da Linha de Sinal**:
   - O KVO cruzar a linha de sinal de baixo para cima pode ser visto como um sinal de entrada altista
   - O KVO cruzar a linha de sinal de cima para baixo pode ser visto como um sinal de entrada baixista

3. **Divergências**:
   - Divergência altista: o preço forma um novo mínimo, enquanto o KVO forma um mínimo mais alto
   - Divergência baixista: o preço forma um novo máximo, enquanto o KVO forma um máximo mais baixo

4. **Confirmação da Tendência**:
   - Valores positivos do KVO confirmam uma tendência ascendente
   - Valores negativos do KVO confirmam uma tendência descendente

5. **Força da Tendência**:
   - O aumento do valor do KVO (tanto positivo como negativo) indica fortalecimento da tendência actual
   - A diminuição do valor do KVO indica enfraquecimento da tendência actual

6. **Potenciais Inversões**:
   - Valores extremos do KVO podem indicar condições de sobrecompra ou sobrevenda do mercado e potencial inversão
   - O abrandamento da subida ou queda do KVO pode anteceder uma inversão de tendência

7. **Volume e Preço**:
   - O KVO permite avaliar a consistência entre o movimento do preço e do volume
   - Volume forte na direcção da tendência conduz a valores mais extremos do KVO

![indicator_klinger_volume_oscillator](../../../../images/indicator_klinger_volume_oscillator.png)

## Ver Também

[OBV](on_balance_volume.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[ADL](accumulation_distribution_line.md)
[ForceIndex](force_index.md)
