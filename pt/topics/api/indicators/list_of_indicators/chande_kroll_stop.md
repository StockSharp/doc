# CKS

**stop Chande Kroll (CKS)** é um indicador para determinar níveis de stop-loss, desenvolvido por Tushar Chande e Stanley Kroll, que se adapta à volatilidade do mercado e ajuda os traders a definir pontos de saída de posições.

Para usar o indicador, deve ser usada a classe [ChandeKrollStop](xref:StockSharp.Algo.Indicators.ChandeKrollStop).

## Descrição

O indicador stop Chande Kroll foi desenvolvido como uma ferramenta dinâmica para definir níveis de stop-loss que responde a alterações na volatilidade e na tendência do mercado. É composto por duas linhas: uma linha de stop superior (para posições short) e uma linha de stop inferior (para posições long).

A principal vantagem do CKS está na sua capacidade de se adaptar às condições atuais do mercado. Durante períodos de elevada volatilidade, as linhas de stop são posicionadas mais longe do preço, ajudando a evitar o encerramento prematuro da posição devido ao ruído do mercado. Durante períodos de baixa volatilidade, as linhas de stop aproximam-se do preço, proporcionando um seguimento de tendência mais apertado.

CKS é particularmente útil para:
- Determinar níveis de stop-loss para posições long e short
- Seguir a tendência com controlo de risco adaptativo
- Identificar potenciais pontos de reversão da tendência
- Criar sistemas de trading mecânicos com regras de saída claras

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Period** - período principal para calcular extremos (valor predefinido: 10)
- **Multiplier** - multiplicador para ATR, que determina a distância aos extremos (valor predefinido: 1,5)
- **StopPeriod** - período para calcular níveis de stop (valor predefinido: 20)

## Cálculo

O cálculo do stop Chande Kroll envolve os seguintes passos:

1. Determinar os extremos máximo e mínimo ao longo do Period:
   ```
   HighestHigh = valor High mais alto durante Period
   LowestLow = valor Low mais baixo durante Period
   ```

2. Calcular o intervalo verdadeiro médio (ATR) ao longo do Period:
   ```
   ATR = valor TR médio durante Period
   ```

3. Calcular as bandas superior e inferior:
   ```
   Banda superior = HighestHigh - (Multiplicador * ATR)
   Banda inferior = LowestLow + (Multiplicador * ATR)
   ```

4. Determinar as linhas de stop finais com base no StopPeriod:
   ```
   Upper Stop = valor mais alto da banda superior durante StopPeriod
   Lower Stop = valor mais baixo da banda inferior durante StopPeriod
   ```

## Interpretação

- **Upper Stop** é usado para posições short. Se o preço de fecho ultrapassar o upper stop, isto pode ser considerado um sinal para fechar uma posição short ou abrir uma posição long.

- **Lower Stop** é usado para posições long. Se o preço de fecho cair abaixo do lower stop, isto pode ser considerado um sinal para fechar uma posição long ou abrir uma posição short.

- **Cruzamento do preço com as linhas de stop** pode indicar uma potencial reversão da tendência ou o início de um novo momentum.

- **Alterações bruscas nas linhas de stop** podem ocorrer com alterações significativas da volatilidade do mercado.

- **Utilização com outros indicadores**: o CKS funciona melhor em combinação com outros indicadores de tendência e momentum que ajudam a determinar a direção de entrada no mercado.

![indicator_chande_kroll_stop](../../../../images/indicator_chande_kroll_stop.png)

## Ver também

[ATR](atr.md)
[ParabolicSAR](parabolic_sar.md)
[DonchianChannels](donchian_channels.md)
