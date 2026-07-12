# Elementos

Dentro de cada cubo é apresentado um ícone que o caracteriza, bem como um nome que pode ser alterado para um nome definido pelo utilizador no painel **Propriedades**. A dica de contexto do cubo mostra uma descrição da finalidade desse cubo. Ao selecionar um cubo com o rato, pode ver as suas propriedades no painel **Propriedades** e, se necessário, alterar alguns parâmetros.

![Designer Descrição dos cubos 00](../../../../images/designer_description_of_elements_00.png)

À esquerda e à direita do cubo, as caixas coloridas mostram os parâmetros de entrada (esquerda) e de saída (direita).

Os parâmetros são necessários para preencher o cubo com informações enquanto a estratégia está em execução. Por exemplo, para o cubo [Velas](elements/data_sources/candles.md), o instrumento para o qual pretende construir uma vela é passado para a entrada e as velas construídas são devolvidas na saída. Estas, por sua vez, podem ser usadas como parâmetro de entrada para o elemento [Gráfico](elements/common/chart.md). Ou podem ser passadas para um método que determina o tamanho da vela.

![Designer Descrição dos cubos 01](../../../../images/designer_description_of_elements_01.png)

A cor indica o tipo de dados que é passado nos parâmetros. Diferentes parâmetros em diferentes cubos podem receber e passar tipos de dados diferentes e incompatíveis. A descrição de cada parâmetro é indicada na dica de contexto. Para evitar muitos erros ao ligar parâmetros de tipos diferentes, cada parâmetro tem o seu próprio tipo de dados, que se distingue pela cor. O seguinte conjunto de cores é usado para indicar os parâmetros:

- **Preto** - qualquer tipo de dados, normalmente usado como sinal para executar determinadas ações dentro do elemento.
- **Verde escuro** - o instrumento.
- **Ciano escuro** - o livro de ordens.
- **Ciano** - a cotação (um par de preço e volume).
- **Vermelho-alaranjado** - velas e estado da vela.
- **Dourado escuro** - o valor do indicador.
- **Oliva** - a ordem.
- **Vermelho-violeta pálido** - erro da ordem.
- **Verde-oliva escuro** - negócio próprio.
- **Azul vivo** - o valor do sinalizador (indica o estado e tem dois valores: ativado (true) e desativado (false)).
- **Verde-mar médio** - um valor numérico, que pode ser definido como número ou percentagem.
- **Azul-ardósia escuro** - valores que podem ser comparados (por exemplo, um valor numérico, uma cadeia de texto, um valor de indicador, etc.).
- **Castanho** - o portefólio.
- **Rosa intenso** - opções.
- **Bege** - o lado.
- **Caqui escuro** - o negócio.
- **Azul escuro** - a estratégia.
- **Chocolate** - a data.
- **Coral** - a hora.
- **Castanho-sela** - a posição.
- **Verde chartreuse** - o estado da ordem.
- **Cinzento claro** - o modelo Black-Scholes.
- **Castanho-claro** - o modelo Black-Scholes de cabaz.
- **Púrpura** - a cadeia de texto.

Assim, pode ligar parâmetros das mesmas cores (os mesmos tipos de dados), exceto nos seguintes tipos de parâmetros:

1. O parâmetro **preto** pode aceitar quaisquer dados. Na maioria das vezes, esses parâmetros são usados para passar sinais para quaisquer ações dentro do cubo. Por exemplo, o cubo [Variável](elements/data_sources/variable.md) armazena um determinado valor e envia-o para a saída quando recebe um sinal.
2. O parâmetro **azul-ardósia escuro** pode receber na entrada vários tipos de dados comparáveis. Por exemplo, valores numéricos, valores de indicadores, cadeias de texto, etc.

Deve notar-se que os tipos de parâmetros podem depender das propriedades do cubo. Por exemplo, para o cubo [Conversor](elements/converters/converter.md), o tipo do parâmetro de entrada é automaticamente determinado pelo tipo de dados do cubo de origem de dados para [Conversor](elements/converters/converter.md). Ao criar uma ligação, a cor do quadrado no elemento muda automaticamente.

Os parâmetros de saída normalmente permitem várias ligações de saída para diferentes cubos; os parâmetros de entrada geralmente permitem uma ligação, exceto no cubo [Combinação](elements/common/combination.md), que permite fundir o fluxo de dados de diferentes cubos num só. O número de ligações simultâneas para um parâmetro é especificado no código-fonte do cubo.

Os cubos para construir esquemas estão divididos em várias categorias, cada uma destinada a ser usada numa determinada parte do esquema.

## Conteúdo recomendado

[Gráfico](elements/common/chart.md)
