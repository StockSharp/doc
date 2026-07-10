# OBV

**On-Balance Volume (OBV)** é um indicador técnico desenvolvido por Joseph Granville que usa o volume de negociação para prever alterações de preço, acumulando volume com base na direção do preço.

Para usar o indicador, é necessário usar a classe [OnBalanceVolume](xref:StockSharp.Algo.Indicators.OnBalanceVolume).

## Descrição

On-Balance Volume (OBV) é um indicador cumulativo que adiciona volume quando o preço de fecho sobe e subtrai volume quando o preço de fecho cai. O indicador baseia-se no conceito de que as alterações de volume antecedem as alterações de preço. De acordo com esta teoria, quando o volume aumenta significativamente sem uma alteração de preço correspondente, deve esperar-se que o preço acabe por subir, e vice-versa.

O OBV procura detetar momentos em que o "smart money" (grandes investidores institucionais) está a acumular ou distribuir posições, o que pode antecipar movimentos futuros do preço. O indicador é particularmente útil para identificar divergências entre preço e volume que podem sinalizar potenciais reversões de mercado.

O indicador OBV foi apresentado pela primeira vez por Joseph Granville em 1963 no seu livro "Granville's New Key to Stock Market Profits" e, desde então, tornou-se um dos indicadores de volume mais usados.

## Cálculo

O cálculo do On-Balance Volume é muito simples:

1. Definir o valor inicial do OBV (normalmente 0 ou um número arbitrário):
   ```
   OBV[initial] = 0
   ```

2. Para cada período subsequente:
   ```
   Se Close[current] > Close[previous], então:
       OBV[current] = OBV[previous] + Volume[current]
   Se Close[current] < Close[previous], então:
       OBV[current] = OBV[previous] - Volume[current]
   Se Close[current] = Close[previous], então:
       OBV[current] = OBV[previous]
   ```

Onde:
- Close - preço de fecho
- Volume - volume de negociação

## Interpretação

On-Balance Volume pode ser interpretado da seguinte forma:

1. **Análise da tendência**:
   - OBV em subida indica entrada de volume no mercado (acumulação), o que pode antecipar subida do preço
   - OBV em queda indica saída de volume do mercado (distribuição), o que pode antecipar descida do preço
   - OBV plano indica ausência de movimento direcional do volume, o que pode corresponder a uma tendência lateral

2. **Confirmação da tendência do preço**:
   - Se o OBV se move na mesma direção do preço, isto confirma a tendência atual do preço
   - Se o OBV e o preço se movem em direções opostas, isto pode sinalizar uma potencial reversão de tendência

3. **Divergências**:
   - Divergência altista: o preço forma um novo mínimo, enquanto o OBV forma um mínimo mais alto (sinal de compra)
   - Divergência baixista: o preço forma um novo máximo, enquanto o OBV forma um máximo mais baixo (sinal de venda)

4. **Breakouts do OBV**:
   - Um breakout do OBV de um nível de resistência ou suporte antecede frequentemente um breakout semelhante no gráfico do preço
   - Os traders podem usar breakouts de linhas de tendência do OBV para prever movimentos futuros do preço

5. **Linha de base**:
   - Alguns traders usam médias móveis do OBV como linhas de "base"
   - O cruzamento do OBV com a sua média móvel pode gerar sinais de trading

6. **Padrões de análise técnica**:
   - Padrões clássicos de análise técnica podem formar-se no gráfico do OBV, como "cabeça e ombros", "duplo fundo", etc.
   - Estes padrões podem fornecer sinais de trading adicionais

7. **Picos de volume**:
   - Alterações súbitas e acentuadas no OBV podem indicar mudanças significativas no sentimento de mercado
   - Estes "picos de volume" antecedem frequentemente movimentos substanciais do preço

É importante notar que o OBV é um indicador cumulativo, por isso o seu valor absoluto não tem grande significado. O que importa é a direção do movimento do OBV e a sua relação com o movimento do preço.

![indicator_on_balance_volume](../../../../images/indicator_on_balance_volume.png)

## Ver também

[ADL](accumulation_distribution_line.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[ForceIndex](force_index.md)
[NegativeVolumeIndex](negative_volume_index.md)

