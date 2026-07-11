# EMV

**Facilidade de movimento (EMV)** é um indicador técnico desenvolvido por Richard Arms que correlaciona a variação do preço com o volume para avaliar a facilidade com que o preço sobe ou desce.

Para usar o indicador, deve ser usada a classe [EaseOfMovement](xref:StockSharp.Algo.Indicators.EaseOfMovement).

## Descrição

O indicador Facilidade de movimento (EMV) foi criado para medir a relação entre movimento do preço e volume. O conceito principal do indicador é que, numa tendência ascendente, o preço deve subir facilmente com pouco volume, enquanto numa tendência descendente o preço também deve descer facilmente com pouco volume.

EMV combina informação sobre intervalo de preços, variação do preço e volume para criar uma medida da "facilidade" do movimento do preço. Valores positivos de EMV indicam que o preço está a subir com relativa facilidade, enquanto valores negativos indicam que o preço está a cair com relativa facilidade.

O indicador é particularmente útil para:
- Confirmar a força ou fraqueza da tendência atual
- Identificar potenciais pontos de reversão
- Detetar divergências com o preço
- Avaliar a "qualidade" do movimento do preço considerando o volume

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de suavização (valor predefinido: 14)

## Cálculo

O cálculo do indicador Facilidade de movimento envolve os seguintes passos:

1. Calcular Movimento do ponto médio:
   ```
   Ponto médio = (High + Low) / 2
   Movimento do ponto médio = Ponto médio[current] - Ponto médio[previous]
   ```

2. Calcular Razão volume-distância (coeficiente volume-distância):
   ```
   Razão volume-distância = Volume / (High - Low)
   ```

3. Calcular EMV de um único período:
   ```
   EMV de 1 período = Movimento do ponto médio / Razão volume-distância
   ```

4. Suavizar para obter o EMV final:
   ```
   EMV = SMA(EMV de 1 período, Length)
   ```

Onde:
- High - preço mais alto da vela
- Low - preço mais baixo da vela
- Volume - volume de negociação
- SMA - média móvel simples

## Interpretação

O indicador EMV pode ser interpretado da seguinte forma:

1. **Cruzamentos da linha zero**:
   - O cruzamento de baixo para cima (de valores negativos para positivos) pode ser visto como um sinal altista, indicando que o preço começa a subir com facilidade
   - O cruzamento de cima para baixo (de valores positivos para negativos) pode ser visto como um sinal baixista, indicando que o preço começa a descer com facilidade

2. **Valores extremos**:
   - Valores positivos elevados indicam que o preço está a subir com muita facilidade (com pouco volume)
   - Valores negativos elevados indicam que o preço está a descer com muita facilidade (com pouco volume)

3. **Divergências**:
   - Divergência altista: o preço forma um novo mínimo, enquanto o EMV forma um mínimo mais alto (pode indicar uma potencial reversão ascendente)
   - Divergência baixista: o preço forma um novo máximo, enquanto o EMV forma um máximo mais baixo (pode indicar uma potencial reversão descendente)

4. **Tendências do EMV**:
   - Valores positivos sustentados confirmam uma tendência ascendente
   - Valores negativos sustentados confirmam uma tendência descendente
   - Oscilações em torno de zero podem indicar uma tendência lateral ou consolidação

5. **Análise de volume**:
   - Se o preço sobe com EMV positivo, isto confirma a força do movimento ascendente
   - Se o preço cai com EMV negativo, isto confirma a força do movimento descendente
   - Se o preço sobe com EMV negativo ou cai com EMV positivo, isto pode indicar instabilidade do movimento atual

![EMV](../../../../images/indicator_ease_of_movement.png)

## Ver também

[ForceIndex](force_index.md)
[BalanceOfPower](balance_of_power.md)
[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
