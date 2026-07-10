# OBVM

**On Balance Volume Mean (OBVM)** é um indicador técnico que representa uma média móvel do indicador On Balance Volume (OBV), permitindo sinais de tendência mais claros com base no volume.

Para usar o indicador, é necessário usar a classe [OnBalanceVolumeMean](xref:StockSharp.Algo.Indicators.OnBalanceVolumeMean).

## Descrição

On Balance Volume Mean (OBVM) é uma modificação do indicador clássico On Balance Volume (OBV) que aplica uma média móvel aos valores do OBV para suavizar flutuações e identificar tendências mais claras. O indicador mantém o conceito central do OBV - acumulação de volume com base na alteração da direção do preço - mas adiciona uma camada adicional de filtragem.

O OBVM ajuda a eliminar o ruído presente no OBV original e torna mais visíveis as tendências de longo prazo do fluxo de volume. Isto é particularmente útil em mercados voláteis ou ao analisar instrumentos com volumes de negociação irregulares.

A principal vantagem do OBVM é a sua capacidade de gerar sinais de trading mais claros e menos propensos a sinais falsos em comparação com o OBV clássico. O indicador também pode ser usado para identificar cruzamentos entre o OBV e o seu valor médio, fornecendo oportunidades de trading adicionais.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período para o cálculo da média móvel (valor predefinido: 20)

## Cálculo

O cálculo do On Balance Volume Mean envolve os seguintes passos:

1. Calcular o indicador On Balance Volume (OBV) base:
   ```
   Se Close[current] > Close[previous]:
       OBV[current] = OBV[previous] + Volume[current]
   Se Close[current] < Close[previous]:
       OBV[current] = OBV[previous] - Volume[current]
   Se Close[current] = Close[previous]:
       OBV[current] = OBV[previous]
   ```

2. Aplicar a média móvel aos valores do OBV:
   ```
   OBVM = SMA(OBV, Length)
   ```

Onde:
- Close - preço de fecho
- Volume - volume de negociação
- OBV - On Balance Volume
- SMA - média móvel simples
- Length - período da média móvel

Nota: Também podem ser usados outros tipos de médias móveis, como EMA (média móvel exponencial), WMA (média móvel ponderada), etc., em vez de SMA.

## Interpretação

On Balance Volume Mean pode ser interpretado da seguinte forma:

1. **Análise da tendência**:
   - OBVM em subida indica uma tendência altista com forte suporte de volume
   - OBVM em queda indica uma tendência baixista com forte suporte de volume
   - OBVM plano indica ausência de tendência pronunciada

2. **Cruzamentos entre OBV e OBVM**:
   - Quando o OBV cruza o OBVM de baixo para cima, pode ser visto como um sinal altista
   - Quando o OBV cruza o OBVM de cima para baixo, pode ser visto como um sinal baixista
   - Estes cruzamentos indicam frequentemente o início de novas tendências ou movimentos significativos do preço

3. **Divergências**:
   - Divergência altista: o preço forma um novo mínimo, enquanto o OBVM forma um mínimo mais alto
   - Divergência baixista: o preço forma um novo máximo, enquanto o OBVM forma um máximo mais baixo
   - As divergências antecedem frequentemente reversões significativas de tendência

4. **Confirmação da tendência do preço**:
   - Se o OBVM se move na mesma direção do preço, isto confirma a tendência atual do preço
   - Se o OBVM e o preço se movem em direções opostas, isto pode sinalizar uma potencial reversão de tendência

5. **Níveis de suporte e resistência**:
   - O gráfico do OBVM pode formar os seus próprios níveis de suporte e resistência
   - O breakout destes níveis pode anteceder breakouts semelhantes no gráfico do preço

6. **Comparação com outros indicadores de volume**:
   - O OBVM pode ser comparado com outros indicadores de volume para confirmar sinais
   - A consistência de sinais de vários indicadores de volume aumenta a sua fiabilidade

7. **Seleção do parâmetro Length**:
   - Períodos mais curtos (por exemplo, 10-15) tornam o OBVM mais sensível a alterações de curto prazo
   - Períodos mais longos (por exemplo, 30-50) identificam melhor tendências de longo prazo
   - O período ideal depende do horizonte temporal de trading e das características específicas do instrumento

![indicator_on_balance_volume_mean](../../../../images/indicator_on_balance_volume_mean.png)

## Ver também

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[ForceIndex](force_index.md)

