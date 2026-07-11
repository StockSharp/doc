# HLI

**índice máximo-mínimo (HLI)** é um indicador técnico que mede o rácio entre o número de ações que atingem novos máximos e o número de ações que atingem novos mínimos ao longo de um período específico.

Para utilizar o indicador, é necessário usar a classe [HighLowIndex](xref:StockSharp.Algo.Indicators.HighLowIndex).

## Descrição

O índice máximo-mínimo (HLI) é um indicador de amplitude de mercado que analisa a atividade global do mercado comparando o número de instrumentos que atingem novos máximos com o número que atinge novos mínimos. Isto permite avaliar a força ou fraqueza interna do mercado.

A ideia principal do indicador é que um mercado saudável é caracterizado por mais títulos a atingirem novos máximos do que novos mínimos. Pelo contrário, um mercado em enfraquecimento terá mais títulos a atingirem novos mínimos.

O HLI é particularmente útil para:
- Avaliar a condição geral do mercado
- Identificar divergências entre o índice e instrumentos individuais do mercado
- Determinar potenciais pontos de inversão do mercado
- Confirmar sinais de outros indicadores

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo (valor predefinido: 14)

## Cálculo

O cálculo do índice máximo-mínimo envolve os seguintes passos:

1. Contar o número de títulos que atingem novos máximos ao longo do período Length:
   ```
   New Highs = número de instrumentos que atingem novos máximos durante o período Length
   ```

2. Contar o número de títulos que atingem novos mínimos ao longo do período Length:
   ```
   New Lows = número de instrumentos que atingem novos mínimos durante o período Length
   ```

3. Calcular o índice máximo-mínimo como o rácio entre a diferença de novos máximos e mínimos e a sua soma:
   ```
   HLI = ((New Highs - New Lows) / (New Highs + New Lows)) * 100
   ```

Nota: Se (New Highs + New Lows) for igual a zero, o HLI é definido como zero para evitar divisão por zero.

## Interpretação

O índice máximo-mínimo é interpretado da seguinte forma:

1. **Intervalo de Valores**:
   - O HLI oscila entre -100 e +100
   - Valores positivos indicam mais títulos a atingir novos máximos do que novos mínimos
   - Valores negativos indicam mais títulos a atingir novos mínimos do que novos máximos

2. **Cruzamentos da Linha Zero**:
   - A transição de valores negativos para positivos pode ser vista como um sinal de alta
   - A transição de valores positivos para negativos pode ser vista como um sinal de baixa

3. **Valores Extremos**:
   - Valores próximos de +100 indicam um mercado de alta forte (possível condição de sobrecompra)
   - Valores próximos de -100 indicam um mercado de baixa forte (possível condição de sobrevenda)

4. **Divergências**:
   - Divergência de alta: o índice de mercado atinge um novo mínimo, mas o HLI forma um mínimo mais alto
   - Divergência de baixa: o índice de mercado atinge um novo máximo, mas o HLI forma um máximo mais baixo

5. **Tendências do HLI**:
   - Crescimento sustentado do HLI indica fortalecimento de um mercado de alta
   - Queda sustentada do HLI indica fortalecimento de um mercado de baixa

6. **Confirmação da Tendência de Mercado**:
   - Se o índice de mercado sobe e o HLI também sobe, isto confirma a força de uma tendência de alta
   - Se o índice de mercado cai e o HLI também cai, isto confirma a força de uma tendência de baixa

![indicator_high_low_index](../../../../images/indicator_high_low_index.png)

## Ver Também

[McClellanOscillator](mcclellan_oscillator.md)

