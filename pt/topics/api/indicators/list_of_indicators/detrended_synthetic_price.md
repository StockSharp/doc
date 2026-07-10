# DSP

**Detrended Synthetic Price (DSP)** é um indicador técnico que remove a tendência geral de uma série de preços, permitindo que os traders se concentrem nas flutuações de preço de curto prazo.

Para usar o indicador, deve ser usada a classe [DetrendedSyntheticPrice](xref:StockSharp.Algo.Indicators.DetrendedSyntheticPrice).

## Descrição

O indicador DSP foi desenvolvido para eliminar a tendência de longo prazo de um gráfico de preços, permitindo aos traders ver com maior clareza ciclos e oscilações de curto prazo. É particularmente útil para identificar oportunidades de trading de curto prazo que podem estar ocultas pela tendência dominante.

A ideia principal do DSP é que, ao remover a tendência da série de preços, torna-se mais fácil identificar os componentes cíclicos do movimento do preço. Isto torna o indicador especialmente valioso para traders especializados em trading de curto prazo e que usam padrões cíclicos.

DSP é útil para:
- Identificar ciclos de mercado de curto prazo
- Determinar potenciais pontos de reversão
- Detetar divergências com o preço
- Criar sistemas de trading baseados na natureza cíclica dos mercados

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo (valor predefinido: 10-20 períodos)

## Cálculo

O cálculo do Detrended Synthetic Price envolve os seguintes passos:

1. Calcular a média móvel do preço ao longo do período especificado:
   ```
   MA = SMA(Price, Length)
   ```

2. Determinar o deslocamento para o cálculo do preço sintético:
   ```
   Offset = (Length / 2) + 1
   ```

3. Calcular o preço sintético subtraindo a média móvel deslocada ao preço atual:
   ```
   DSP = Price - MA[shifted (Length/2) + 1 periods back]
   ```

Onde:
- Price - preço atual (normalmente o preço de fecho)
- MA - média móvel simples
- Length - período de cálculo selecionado

## Interpretação

O indicador DSP oscila em torno da linha zero e pode ser interpretado da seguinte forma:

1. **Cruzamentos da linha zero**:
   - Quando o DSP cruza a linha zero de baixo para cima, pode ser visto como um sinal altista
   - Quando o DSP cruza a linha zero de cima para baixo, pode ser visto como um sinal baixista

2. **Extremos do indicador**:
   - Quando o DSP atinge valores extremamente elevados, pode indicar condições de sobrecompra no mercado
   - Quando o DSP atinge valores extremamente baixos, pode indicar condições de sobrevenda no mercado

3. **Análise cíclica**:
   - Oscilações regulares do DSP podem ser usadas para determinar a periodicidade do ciclo de mercado
   - Alterações na amplitude da oscilação podem indicar mudanças na dinâmica do mercado

4. **Divergências**:
   - Divergência altista: o preço forma um novo mínimo, enquanto o DSP forma um mínimo mais alto
   - Divergência baixista: o preço forma um novo máximo, enquanto o DSP forma um máximo mais baixo

5. **Formação de padrões**:
   - Padrões técnicos (head and shoulders, double bottom, etc.) podem formar-se no gráfico do DSP, fornecendo potencialmente sinais adicionais de trading

![indicator_detrended_synthetic_price](../../../../images/indicator_detrended_synthetic_price.png)

## Ver também

[DetrendedPriceOscillator](dpo.md)
[CenterOfGravityOscillator](center_of_gravity_oscillator.md)
[SineWave](sine_wave.md)
