# CHOP

**Choppiness Index (CHOP)** é um indicador concebido para determinar se o mercado está em movimento lateral (dentro de um intervalo) ou num estado tendencial.

Para usar o indicador, deve ser usada a classe [ChoppinessIndex](xref:StockSharp.Algo.Indicators.ChoppinessIndex).

## Descrição

O Choppiness Index (CHOP) foi criado para avaliar quantitativamente a volatilidade e determinar a natureza do movimento do mercado. Ao contrário de muitos outros indicadores, o CHOP não se destina a identificar a direção da tendência nem a gerar sinais de compra ou venda. Em vez disso, ajuda os traders a determinar se o mercado está em consolidação (movimento lateral) ou numa tendência direcional.

O indicador CHOP oscila entre 0 e 100:
- Valores mais próximos de 100 indicam forte consolidação (elevado "choppiness")
- Valores mais próximos de 0 indicam uma forte tendência direcional (baixo "choppiness")

CHOP é particularmente útil para:
- Determinar uma estratégia de trading adequada com base no caráter do mercado
- Identificar transições de movimento lateral para tendência e vice-versa
- Confirmar ou refutar sinais de outros indicadores
- Evitar sinais falsos durante a consolidação

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo (valor predefinido: 14)

## Cálculo

O cálculo do Choppiness Index envolve os seguintes passos:

1. Calcular a soma dos intervalos verdadeiros ao longo do período selecionado:
   ```
   Sum of TR = Sum(TR(i)) para i de 1 até Length
   ```

2. Calcular o High mais alto e o Low mais baixo ao longo do período selecionado:
   ```
   Highest High = valor High máximo durante o período Length
   Lowest Low = valor Low mínimo durante o período Length
   ```

3. Calcular o índice CHOP:
   ```
   CHOP = 100 * LOG10(Sum TR / (Highest High - Lowest Low)) / LOG10(Length)
   ```

Onde:
- TR - intervalo verdadeiro de cada vela
- Length - período selecionado
- LOG10 - logaritmo decimal

## Interpretação

- **Valores elevados de CHOP (acima de 60-70)** indicam que o mercado está em movimento lateral (consolidação). Durante este período, é melhor evitar estratégias de tendência e considerar estratégias de trading em intervalo.

- **Valores baixos de CHOP (abaixo de 30-40)** indicam uma forte tendência direcional. Este é um bom momento para usar estratégias de tendência e seguir o movimento do preço.

- **Transições entre valores altos e baixos** podem indicar uma alteração no caráter do mercado. Uma queda do CHOP a partir de valores elevados pode sinalizar o início de uma nova tendência. Uma subida do CHOP a partir de valores baixos pode avisar sobre esgotamento da tendência e transição para consolidação.

- **Definição de níveis de limiar**: normalmente, são usados os seguintes níveis de limiar:
  - Acima de 60-70: elevado "choppiness" (movimento lateral)
  - 30-60: "choppiness" moderado (estado de transição)
  - Abaixo de 30: baixo "choppiness" (tendência forte)

![indicator_choppiness_index](../../../../images/indicator_choppiness_index.png)

## Ver também

[ATR](atr.md)
[ADX](adx.md)
[VHF](vhf.md)
[Intervalo verdadeiro](true_range.md)
