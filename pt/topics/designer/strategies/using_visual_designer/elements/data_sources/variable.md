# Variável

![Designer Variable 00](../../../../../../images/designer_variable_00.png)

O cubo é usado para armazenar valores e passar o valor previamente armazenado mais adiante na cadeia de elementos.

### Sockets de entrada

Sockets de entrada

- **Any data** – o valor do tipo selecionado, que será armazenado em vez do valor predefinido.
- **Trigger** – o sinal (qualquer valor exceto `False`) que determina o momento em que é necessário passar o valor armazenado através do socket de saída.

### Sockets de saída

Sockets de saída

- **Any data** – o valor do tipo selecionado dos dados passados.

### Parâmetros

Parâmetros

- **Data type** - o tipo dos dados armazenados na variável; o tipo dos parâmetros de entrada e saída depende do tipo de dados selecionado.
- **Value** - o valor predefinido armazenado na variável. Este valor é usado se não forem recebidos outros valores na entrada do elemento.
- **Raise on start** - quando a caixa de seleção está selecionada, o valor será passado quando a estratégia for iniciada.

Se o tipo de dados **Instrument** ou **Portfolio** estiver selecionado, o valor predefinido pode estar em falta. Neste caso, se a flag **Parameters** estiver definida nas propriedades, quando a estratégia for executada, estes dados serão retirados das propriedades correspondentes da estratégia.

## Conteúdo recomendado

[Indexador](../converters/indexer.md)
