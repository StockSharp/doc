# KC

**Keltner Channels (KC)** é um indicador técnico composto por um conjunto de bandas de volatilidade que usa uma média móvel exponencial (EMA) como linha central e o intervalo verdadeiro médio (ATR) para determinar a largura do canal.

Para utilizar o indicador, é necessário usar a classe [KeltnerChannels](xref:StockSharp.Algo.Indicators.KeltnerChannels).

## Descrição

Os Keltner Channels consistem em três linhas:
1. **Linha Central**: normalmente representada por uma EMA de 20 períodos
2. **Banda Superior**: linha central mais um multiplicador do ATR
3. **Banda Inferior**: linha central menos o mesmo multiplicador do ATR

O indicador foi desenvolvido por Chester Keltner nos anos 1960 e posteriormente modificado por Linda Raschke, que substituiu a média móvel simples (SMA) por uma média móvel exponencial (EMA) e começou a usar o ATR em vez do intervalo High-Low para calcular a largura do canal.

Os Keltner Channels ajudam os traders a determinar a direcção da tendência e potenciais níveis de suporte e resistência. Também são usados para identificar condições de sobrecompra e sobrevenda quando o preço toca ou rompe a banda superior ou inferior, respectivamente.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período para calcular EMA e ATR (valor predefinido: 20)
- **Multiplier** - multiplicador para o ATR, que determina a largura do canal (valor predefinido: 2,0)

## Cálculo

O cálculo dos Keltner Channels envolve os seguintes passos:

1. Calcular a média móvel exponencial:
   ```
   Linha média = EMA(Preço, Período)
   ```

2. Calcular o intervalo verdadeiro médio:
   ```
   ATR = intervalo verdadeiro médio durante o período Length
   ```

3. Calcular as bandas superior e inferior:
   ```
   Banda superior = Linha média + (Multiplicador * ATR)
   Banda inferior = Linha média - (Multiplicador * ATR)
   ```

Onde:
- Price - normalmente o preço de fecho
- EMA - média móvel exponencial
- ATR - intervalo verdadeiro médio
- Length - período para o cálculo da EMA e do ATR
- Multiplier - multiplicador que determina a largura do canal

## Interpretação

Os Keltner Channels podem ser interpretados da seguinte forma:

1. **Direcção da Tendência**:
   - Quando as três linhas apontam para cima, isto indica uma tendência ascendente
   - Quando as três linhas apontam para baixo, isto indica uma tendência descendente
   - O movimento horizontal das linhas indica uma tendência lateral

2. **Rompimentos**:
   - O preço romper acima da banda superior pode indicar momentum ascendente forte
   - O preço romper abaixo da banda inferior pode indicar momentum descendente forte
   - Os rompimentos são frequentemente usados como sinais de entrada na direcção do rompimento

3. **Regressos à Linha Média**:
   - Depois de romper a banda superior ou inferior, o preço regressa frequentemente à linha média
   - A linha média pode servir como nível de suporte ou resistência

4. **Condições de Sobrecompra e Sobrevenda**:
   - Preço perto ou para lá da banda superior pode indicar condições de sobrecompra
   - Preço perto ou para lá da banda inferior pode indicar condições de sobrevenda
   - Em mercados tendenciais, o preço pode permanecer em zonas "extremas" durante períodos prolongados

5. **Contracção e Expansão do Canal**:
   - O estreitamento do canal (diminuição da distância entre as bandas) indica diminuição da volatilidade, frequentemente antecedendo um movimento de preço forte
   - A expansão do canal indica aumento da volatilidade

6. **Estratégias de Negociação**:
   - Estratégia "Edge to Middle": abrir uma posição quando o preço toca a banda superior ou inferior, tendo como alvo a linha média
   - Estratégia de rompimento: abrir uma posição quando o preço rompe a banda superior ou inferior, esperando continuação do movimento na mesma direcção
   - Estratégia "Middle to Edge": abrir uma posição quando o preço ressalta da linha média, tendo como alvo a banda superior ou inferior

![indicator_keltner_channels](../../../../images/indicator_keltner_channels.png)

## Ver Também

[BollingerBands](bollinger_bands.md)
[DonchianChannels](donchian_channels.md)
[EMA](ema.md)
[ATR](atr.md)
