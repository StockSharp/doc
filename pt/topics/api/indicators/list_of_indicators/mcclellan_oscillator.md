# MCO

**Oscilador de McClellan (MCO)** é um indicador técnico desenvolvido por Sherman e Marian McClellan que mede a amplitude do mercado analisando a diferença entre médias móveis de acções em alta e em queda.

Para utilizar o indicador, é necessário usar a classe [McClellanOscillator](xref:StockSharp.Algo.Indicators.McClellanOscillator).

## Descrição

O Oscilador de McClellan (MCO) é um dos indicadores de amplitude de mercado mais conhecidos, ajudando a avaliar a condição geral do mercado e a identificar potenciais pontos de inversão. Desenvolvido em 1969, tornou-se desde então uma ferramenta crucial para muitos analistas técnicos.

O MCO baseia-se na análise do rácio entre o número de acções em alta e em queda no mercado. O indicador calcula a diferença entre médias móveis exponenciais de 19 e 39 períodos dos avanços líquidos (diferença entre o número de acções em alta e em queda).

O Oscilador de McClellan é particularmente útil para:
- Determinar a direcção geral do mercado
- Identificar condições de sobrecompra e sobrevenda
- Identificar potenciais pontos de inversão
- Confirmar a força ou fraqueza da tendência actual

## Cálculo

O cálculo do Oscilador de McClellan envolve os seguintes passos:

1. Calcular avanços líquidos para cada dia de negociação:
   ```
   avanços líquidos = avanços - declínios
   ```
   onde avanços é o número de acções em alta, declínios é o número de acções em queda.

2. Calcular a média móvel exponencial de 19 períodos de avanços líquidos:
   ```
   EMA19 = EMA(avanços líquidos, 19)
   ```

3. Calcular a média móvel exponencial de 39 períodos de avanços líquidos:
   ```
   EMA39 = EMA(avanços líquidos, 39)
   ```

4. Calcular o Oscilador de McClellan como a diferença entre estas duas EMAs:
   ```
   MCO = EMA19 - EMA39
   ```

## Interpretação

O Oscilador de McClellan pode ser interpretado da seguinte forma:

1. **Cruzamentos da Linha Zero**:
   - O MCO cruzar a linha zero de baixo para cima pode ser visto como um sinal altista, indicando um potencial início de tendência ascendente
   - O MCO cruzar a linha zero de cima para baixo pode ser visto como um sinal baixista, indicando um potencial início de tendência descendente

2. **Valores Extremos**:
   - Valores acima de +100 frequentemente indicam condições de sobrecompra do mercado
   - Valores abaixo de -100 frequentemente indicam condições de sobrevenda do mercado
   - Valores extremos (+150/-150 e acima/abaixo) podem sinalizar uma potencial inversão do mercado

3. **Divergências**:
   - Divergência altista: o índice forma um novo mínimo, enquanto o MCO forma um mínimo mais alto
   - Divergência baixista: o índice forma um novo máximo, enquanto o MCO forma um máximo mais baixo

4. **Estado da Amplitude do Mercado**:
   - Valores positivos do MCO indicam que a maioria das acções no mercado está a subir
   - Valores negativos do MCO indicam que a maioria das acções no mercado está a cair

5. **Aceleração/Desaceleração do Movimento**:
   - Valores crescentes do MCO (positivos ou negativos) indicam aceleração do movimento actual do mercado
   - Valores decrescentes do MCO indicam desaceleração do movimento actual do mercado

6. **Combinação com índice de somatório de McClellan**:
   - índice de somatório de McClellan (MSI) é a soma cumulativa dos valores do MCO
   - O MSI cruzar zero pode confirmar sinais do MCO e indicar alterações de tendência de longo prazo

7. **Padrões Altista/Baixista**:
   - "Cauda altista" - queda rápida do MCO seguida de recuperação rápida, frequentemente indicando um potencial fundo de mercado
   - "Cauda baixista" - subida rápida do MCO seguida de queda rápida, frequentemente indicando um potencial topo de mercado

![indicator_mcclellan_oscillator](../../../../images/indicator_mcclellan_oscillator.png)

## Ver Também

[HighLowIndex](high_low_index.md)

