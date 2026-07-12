# Preços de exercício

![Captura de tela de Preços de exercício](../../../../../../images/designer_derivatives_00.png)

O cubo é usado para obter uma lista de opções por um filtro especificado.

### Conectores de entrada

Conectores de entrada

- **Instrumento** - o instrumento, o ativo subjacente.

### Conectores de saída

Conectores de saída

- **Opções** - a lista de opções pelo ativo subjacente.

### Parâmetros

Parâmetros

- **Tipo de opção** - o tipo de opção pode ser opção de compra (call) ou opção de venda (put).
- **Data de vencimento** - a data de vencimento da opção.
- **Strike (menor)** - o deslocamento para a esquerda (menor) a partir do strike central. Se o preço não estiver definido, serão usados todos os strikes com valor central inferior. O deslocamento é calculado em passos de strike; por exemplo, se o passo de strike for 500 u.m., então o deslocamento igual a 3 será 1.500 u.m.
- **Strike (maior)** - o deslocamento para a direita (maior) a partir do strike central. Se o preço não estiver definido, serão usados todos os strikes com valor central superior. O deslocamento é calculado em passos de strike; por exemplo, se o passo de strike for 500 u.m., então o deslocamento igual a 3 será 1.500 u.m.

## Conteúdo recomendado

[Cruzamento](../common/crossing.md)
