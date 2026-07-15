# DC

**canais de Donchian (DC)** é um indicador técnico desenvolvido pelo operador Richard Donchian, composto por uma banda superior e uma inferior (limites do canal) com base nos valores máximos e mínimos do preço durante um período específico.

Para usar o indicador, deve ser usada a classe [DonchianChannels](xref:StockSharp.Algo.Indicators.DonchianChannels).

## Descrição

canais de Donchian são um indicador simples mas eficaz de volatilidade e tendência. O indicador é composto por três linhas:
- Linha superior: máximo mais alto ao longo do período selecionado
- Linha inferior: mínimo mais baixo ao longo do período selecionado
- Linha média: valor médio entre as linhas superior e inferior

Este indicador foi usado pela primeira vez por Richard Donchian na sua regra do canal de 4 semanas, segundo a qual ocorre um sinal de compra quando o preço excede o máximo mais alto de 4 semanas, e ocorre um sinal de venda quando o preço cai abaixo do mínimo mais baixo de 4 semanas.

canais de Donchian são úteis para:
- Identificar a volatilidade do mercado
- Determinar níveis de suporte e resistência
- Gerar sinais de rutura
- Definir o intervalo atual de negociação

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Período** - período de cálculo (valor predefinido: 20)

## Cálculo

O cálculo dos canais de Donchian é bastante simples:

1. Linha superior do canal:
   ```
   linha superior = máxima mais alta durante o período Length
   ```

2. Linha inferior do canal:
   ```
   linha inferior = mínima mais baixa durante o período Length
   ```

3. Linha média do canal:
   ```
   linha média = (linha superior + linha inferior) / 2
   ```

## Interpretação

canais de Donchian podem ser usados de várias formas:

1. **Estratégias de rutura**:
   - A quebra acima da linha superior do canal pode ser vista como um sinal de compra
   - A quebra abaixo da linha inferior do canal pode ser vista como um sinal de venda

2. **Determinação da tendência**:
   - Se o preço estiver na metade superior do canal (acima da linha média), pode inferir-se uma tendência ascendente
   - Se o preço estiver na metade inferior do canal (abaixo da linha média), pode inferir-se uma tendência descendente

3. **Níveis de suporte e resistência**:
   - A linha superior do canal pode servir como nível de resistência
   - A linha inferior do canal pode servir como nível de suporte

4. **Medição da volatilidade**:
   - A largura do canal (diferença entre as linhas superior e inferior) indica a volatilidade do mercado
   - A expansão do canal indica aumento da volatilidade
   - A contração do canal indica diminuição da volatilidade

5. **Estratégias contra a tendência**:
   - Alguns operadores usam sinais opostos, esperando que o preço regresse à linha média após atingir os limites do canal

![Gráfico do indicador DC](../../../../images/indicator_donchian_channels.png)

## Ver também

[BollingerBands](bollinger_bands.md)
[KeltnerChannels](keltner_channels.md)
[Valor máximo](highest.md)
[Valor mínimo](lowest.md)
