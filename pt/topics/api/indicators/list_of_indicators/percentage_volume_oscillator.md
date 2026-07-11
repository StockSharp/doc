# PVO

**Oscilador percentual de volume (PVO)** é um indicador técnico semelhante ao MACD, mas aplicado ao volume de negociação em vez do preço, expressando a diferença entre médias móveis exponenciais rápida e lenta do volume como percentagem.

Para usar o indicador, é necessário usar a classe [PercentageVolumeOscillator](xref:StockSharp.Algo.Indicators.PercentageVolumeOscillator).

## Descrição

O oscilador percentual de volume (PVO) é uma modificação do indicador MACD (convergência/divergência de médias móveis), aplicada ao volume de negociação em vez do preço. Tal como o PPO (oscilador percentual de preço), o PVO expressa a diferença entre médias móveis exponenciais rápida e lenta como percentagem, em vez de unidades absolutas. Isto torna o PVO particularmente útil ao comparar instrumentos diferentes com níveis de volume distintos ou ao analisar um único instrumento durante um período alargado.

O PVO consiste em três componentes:
1. **Linha PVO** - diferença percentual entre a EMA rápida e a EMA lenta do volume
2. **Linha de sinal** - EMA da linha PVO
3. **Histograma** - diferença entre a linha PVO e a linha de sinal

O indicador PVO ajuda a identificar anomalias de volume que podem anteceder movimentos significativos do preço. Também é útil para confirmar tendências de preço e identificar potenciais pontos de reversão.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **ShortPeriod** - período para calcular a EMA curta do volume (valor predefinido: 12)
- **LongPeriod** - período para calcular a EMA longa do volume (valor predefinido: 26)

## Cálculo

O cálculo do oscilador percentual de volume envolve os seguintes passos:

1. Calcular as médias móveis exponenciais curta e longa do volume:
   ```
   EMA curta = EMA(Volume, ShortPeriod)
   EMA longa = EMA(Volume, LongPeriod)
   ```

2. Calcular a linha PVO como a diferença percentual entre a EMA curta e a EMA longa:
   ```
   Linha PVO = ((EMA curta - EMA longa) / EMA longa) * 100
   ```

3. Calcular a linha de sinal (normalmente EMA de 9 períodos da linha PVO):
   ```
   Linha de sinal = EMA(Linha PVO, 9)
   ```

4. Calcular o histograma:
   ```
   Histograma = Linha PVO - Linha de sinal
   ```

Onde:
- Volume - volume de negociação
- EMA - média móvel exponencial
- ShortPeriod - período da EMA curta
- LongPeriod - período da EMA longa

## Interpretação

O oscilador percentual de volume pode ser interpretado da seguinte forma:

1. **Cruzamentos da linha zero**:
   - O cruzamento da linha PVO da linha zero de baixo para cima indica aceleração do volume acima da média, o que pode anunciar um movimento altista
   - O cruzamento da linha PVO da linha zero de cima para baixo indica desaceleração do volume abaixo da média, o que pode anunciar um movimento baixista

2. **Cruzamentos da linha de sinal**:
   - O cruzamento da linha PVO da linha de sinal de baixo para cima pode ser visto como um sinal altista
   - O cruzamento da linha PVO da linha de sinal de cima para baixo pode ser visto como um sinal baixista

3. **Divergências**:
   - Divergência altista: o preço forma um novo mínimo, enquanto o PVO forma um mínimo mais alto
   - Divergência baixista: o preço forma um novo máximo, enquanto o PVO forma um máximo mais baixo

4. **Valores extremos**:
   - Valores muito elevados do PVO podem indicar volume excessivo, frequentemente observado em topos de mercado ou em pânico
   - Valores muito baixos do PVO podem indicar volume insuficiente, frequentemente observado em pausas do mercado

5. **Análise do histograma**:
   - Histograma positivo crescente indica reforço do momentum altista do volume
   - Histograma negativo crescente indica reforço do momentum baixista do volume
   - Contração do histograma indica enfraquecimento do momentum de volume atual

6. **Confirmação da tendência do preço**:
   - PVO em subida confirma uma tendência ascendente do preço
   - PVO em queda confirma uma tendência descendente do preço
   - Divergência entre a direção do PVO e o preço pode sinalizar uma potencial reversão

7. **Picos de volume**:
   - Saltos acentuados do PVO indicam alterações significativas de volume, frequentemente acompanhando eventos de mercado importantes
   - Estes picos podem anteceder ou acompanhar ruturas de níveis-chave de preço

![PVO](../../../../images/indicator_percentage_volume_oscillator.png)

## Ver também

[PPO](percentage_price_oscillator.md)
[OBV](on_balance_volume.md)
[MACD](macd.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
