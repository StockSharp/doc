# KER

**Kaufman Efficiency Ratio (KER)** é um indicador técnico desenvolvido por Perry Kaufman que mede a eficiência do movimento do preço comparando o movimento direccional do preço com a volatilidade global.

Para utilizar o indicador, é necessário usar a classe [KaufmanEfficiencyRatio](xref:StockSharp.Algo.Indicators.KaufmanEfficiencyRatio).

## Descrição

O Kaufman Efficiency Ratio (KER) avalia quão "eficientemente" o preço se move numa direcção específica em comparação com o percurso total que percorre. Representa o rácio entre o movimento direccional líquido do preço e a soma de todas as variações de preço durante um período específico.

O KER foi desenvolvido por Perry Kaufman e foi originalmente usado como componente da média móvel adaptativa de Kaufman (KAMA). No entanto, o próprio KER é uma ferramenta valiosa que ajuda a determinar se o mercado está num estado tendencial ou oscilante.

Os valores do KER oscilam entre 0 e 1:
- Valores próximos de 1 indicam movimento de preço altamente eficiente (tendência forte)
- Valores próximos de 0 indicam movimento de preço ineficiente (mercado lateral ou alta volatilidade)

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período para o cálculo da eficiência (valor predefinido: 10)

## Cálculo

O cálculo do Kaufman Efficiency Ratio envolve os seguintes passos:

1. Calcular o movimento direccional (variação líquida) durante o período:
   ```
   Direction = |Price[current] - Price[current - Length]|
   ```

2. Calcular o movimento total (soma de todas as variações) durante o período:
   ```
   Volatility = Sum(|Price[i] - Price[i-1]|) para i de (current - Length + 1) até current
   ```

3. Calcular o rácio de eficiência:
   ```
   KER = Direction / Volatility
   ```

Onde:
- Price - normalmente o preço de fecho
- Length - período de cálculo
- | | - denota valor absoluto

Se Volatility for zero (o que é improvável), o KER é definido como zero para evitar divisão por zero.

## Interpretação

O Kaufman Efficiency Ratio pode ser interpretado da seguinte forma:

1. **Níveis de Eficiência**:
   - Valores elevados de KER (>0,6) indicam uma tendência forte
   - Valores médios de KER (0,3-0,6) indicam uma tendência moderada
   - Valores baixos de KER (<0,3) indicam um mercado lateral ou alta volatilidade

2. **Alterações do KER**:
   - O crescimento do KER pode sinalizar a formação ou o fortalecimento de uma tendência
   - A queda do KER pode sinalizar enfraquecimento da tendência ou transição para movimento lateral

3. **Estratégias de Negociação**:
   - Durante períodos de alta eficiência (KER elevado), as estratégias de tendência são preferíveis
   - Durante períodos de baixa eficiência (KER baixo), as estratégias de negociação em intervalo são preferíveis

4. **Filtragem de Sinais**:
   - O KER pode ser usado para filtrar sinais de outros indicadores
   - Os sinais de indicadores de tendência são mais fiáveis com KER elevado
   - Os sinais de osciladores são mais fiáveis com KER baixo

5. **Adaptação às Condições de Mercado**:
   - O KER permite adaptar estratégias de negociação a condições de mercado em mudança
   - Os traders podem ajustar dinamicamente parâmetros de outros indicadores com base nos valores do KER

6. **Precursor de Alterações**:
   - Alterações acentuadas do KER antecedem frequentemente novos movimentos de preço
   - A queda do KER após um período de valores elevados pode avisar sobre uma potencial inversão de tendência

![indicator_kaufman_efficiency_ratio](../../../../images/indicator_kaufman_efficiency_ratio.png)

## Ver Também

[KAMA](kama.md)
[ADX](adx.md)
[VHF](vhf.md)
[VIDYA](vidya.md)
