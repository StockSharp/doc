# Deslocamento

**Deslocamento** é um indicador auxiliar que desloca o fluxo de valores de entrada por um número especificado de períodos. Não transforma os
dados; apenas os atrasa ou alinha para utilização em cálculos compostos.

Use a classe [Shift](xref:StockSharp.Algo.Indicators.Shift) para aceder ao indicador.

## Descrição

O indicador mantém um buffer dos valores mais recentes e devolve aquele que chegou há **Length** barras. Se o histórico de dados for
mais curto do que o deslocamento necessário, o valor é considerado indefinido.

## Parâmetros

- **Length** - número de períodos pelos quais os dados são deslocados.

## Utilização

- Alinhar no tempo sinais de diferentes indicadores.
- Construir indicadores e estratégias personalizados que exigem entradas atrasadas.
- Criar séries sintéticas, por exemplo, para calcular a diferença entre preços atuais e valores passados.

![Deslocamento](../../../../images/indicator_shift.png)

## Ver também

[PassThrough](pass_through.md)
[Sum](sum_n.md)
