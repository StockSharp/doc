# DI

**Índice de procura (DI)** é um indicador técnico desenvolvido por James Sibbett que analisa a relação entre preço e volume para avaliar a força da procura e a pressão compradora no mercado.

Para usar o indicador, deve ser usada a classe [DemandIndex](xref:StockSharp.Algo.Indicators.DemandIndex).

## Descrição

O Índice de procura (DI) é um indicador de volume abrangente que avalia a relação entre preço e volume para determinar quão forte é a pressão compradora (procura) em comparação com a pressão vendedora. O indicador baseia-se na suposição de que a relação entre a variação do preço e a variação do volume permite uma avaliação mais precisa da procura do mercado do que observar simplesmente preço ou volume individualmente.

DI procura identificar as seguintes situações de mercado:
- Procura forte (pressão compradora)
- Procura fraca (pressão vendedora)
- Desequilíbrio entre preço e volume (potenciais pontos de reversão)
- Confirmação ou refutação da tendência atual

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo (valor predefinido: 13)

## Cálculo

O cálculo do Índice de procura é bastante complexo e envolve várias etapas:

1. Calcular o componente de preço com base na variação do preço:
   ```
   componente de preço = ((High + Low + Close) / 3) - ((máximo anterior + mínimo anterior + fecho anterior) / 3)
   ```

2. Calcular o componente de volume, tendo em conta a variação relativa do volume.

3. Calcular a procura como a relação entre os componentes de preço e volume:
   ```
   procura bruta = componente de preço / componente de volume
   ```

4. Suavizar os valores obtidos para reduzir ruído:
   ```
   procura suavizada = EMA(procura bruta, Length)
   ```

5. Normalizar o resultado para obter o índice final:
   ```
   Índice de procura = 100 * Normalized(procura suavizada)
   ```

## Interpretação

O Índice de procura pode ser interpretado de várias formas:

1. **Níveis extremos**:
   - Valores positivos elevados indicam procura forte (pressão compradora)
   - Valores negativos elevados indicam procura fraca (pressão vendedora)

2. **Cruzamentos da linha zero**:
   - O cruzamento de baixo para cima pode ser visto como um sinal altista
   - O cruzamento de cima para baixo pode ser visto como um sinal baixista

3. **Divergências**:
   - Divergência altista: o preço forma um novo mínimo, mas o DI forma um mínimo mais alto
   - Divergência baixista: o preço forma um novo máximo, mas o DI forma um máximo mais baixo

4. **Tendências do DI**:
   - Valores positivos sustentados de DI confirmam uma tendência ascendente
   - Valores negativos sustentados de DI confirmam uma tendência descendente

5. **Valores extremos**:
   - Valores muito elevados ou muito baixos podem indicar condições de sobrecompra ou sobrevenda no mercado

Usar o Índice de procura é mais eficaz quando combinado com outros indicadores e métodos de análise para filtrar sinais falsos.

![indicator_demand_index](../../../../images/indicator_demand_index.png)

## Ver também

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[BalanceOfPower](balance_of_power.md)
