# Preços de exercício

![Designer Derivatives 00](../../../../../../images/designer_derivatives_00.png)

O cubo é usado para obter uma lista de opções por um filtro especificado.

### Sockets de entrada

Sockets de entrada

- **Instrument** - o instrumento, o ativo subjacente.

### Sockets de saída

Sockets de saída

- **Options** - a lista de opções pelo ativo subjacente.

### Parâmetros

Parâmetros

- **Option type** - o tipo de opção pode ser Call option ou Put option.
- **Expiry date** - a data de vencimento da opção.
- **Strike (less)** - o deslocamento para a esquerda (menor) a partir do strike central. Se o preço não estiver definido, serão usados todos os strikes com valor central inferior. O deslocamento é calculado em passos de strike; por exemplo, se o passo de strike for 500 u.m., então o deslocamento igual a 3 será 1.500 u.m.
- **Strike (more)** - o deslocamento para a direita (maior) a partir do strike central. Se o preço não estiver definido, serão usados todos os strikes com valor central superior. O deslocamento é calculado em passos de strike; por exemplo, se o passo de strike for 500 u.m., então o deslocamento igual a 3 será 1.500 u.m.

## Conteúdo recomendado

[Cruzamento](../common/crossing.md)
