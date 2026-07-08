# Elementos

Dentro de cada cubo é apresentado um ícone que o caracteriza, bem como um nome que pode ser alterado para um nome definido pelo utilizador no painel **Properties**. A dica de contexto do cubo mostra uma descrição da finalidade desse cubo. Ao selecionar um cubo com o rato, pode ver as suas propriedades no painel **Properties** e, se necessário, alterar alguns parâmetros.

![Designer Description of cubes 00](../../../../images/designer_description_of_elements_00.png)

À esquerda e à direita do cubo, as caixas coloridas mostram os parâmetros de entrada (esquerda) e de saída (direita).

Os parâmetros são necessários para preencher o cubo com informações enquanto a estratégia está em execução. Por exemplo, para o cubo [Candles](elements/data_sources/candles.md), o instrumento para o qual pretende construir uma vela é passado para a entrada e as velas construídas são devolvidas na saída. Estas, por sua vez, podem ser usadas como parâmetro de entrada para o elemento [Gráfico](elements/common/chart.md). Ou podem ser passadas para um método que determina o tamanho da vela.

![Designer Description of cubes 01](../../../../images/designer_description_of_elements_01.png)

A cor indica o tipo de dados que é passado nos parâmetros. Diferentes parâmetros em diferentes cubos podem receber e passar tipos de dados diferentes e incompatíveis. A descrição de cada parâmetro é indicada na dica de contexto. Para evitar muitos erros ao ligar parâmetros de tipos diferentes, cada parâmetro tem o seu próprio tipo de dados, que se distingue pela cor. O seguinte conjunto de cores é usado para indicar os parâmetros:

- **Black** - qualquer tipo de dados, normalmente usado como sinal para executar determinadas ações dentro do elemento.
- **Dark green** - o instrumento.
- **Dark cyan** - o livro de ordens.
- **Cyan** - a cotação (um par de preço e volume).
- **Orange red** - velas e estado da vela.
- **Dark goldenrod** - o valor do indicador.
- **Olive** - a ordem.
- **Pale violet red** - erro da ordem.
- **Dark olive green** - negócio próprio.
- **Dodger blue** - o valor da flag (indica o estado e tem dois valores: levantada (true) e baixada (false)).
- **Medium sea green** - um valor numérico, que pode ser definido como número ou percentagem.
- **Dark slate blue** - valores que podem ser comparados (por exemplo, um valor numérico, uma cadeia de texto, um valor de indicador, etc.).
- **Brown** - o portefólio.
- **Deep pink** - opções.
- **Beige** - o lado.
- **Dark khaki** - o negócio.
- **Dark blue** - a estratégia.
- **Chocolate** - a data.
- **Coral** - a hora.
- **Saddle brown** - a posição.
- **Chartreuse** - o estado da ordem.
- **Gainsboro** - o modelo Black-Scholes.
- **Tan** - o modelo Black-Scholes de cabaz.
- **Purple** - a cadeia de texto.

Assim, pode ligar parâmetros das mesmas cores (os mesmos tipos de dados), exceto nos seguintes tipos de parâmetros:

1. O parâmetro **black** pode aceitar quaisquer dados. Na maioria das vezes, esses parâmetros são usados para passar sinais para quaisquer ações dentro do cubo. Por exemplo, o cubo [Variável](elements/data_sources/variable.md) armazena um determinado valor e envia-o para a saída quando recebe um sinal.
2. O parâmetro **dark slate blue** pode receber na entrada vários tipos de dados comparáveis. Por exemplo, valores numéricos, valores de indicadores, cadeias de texto, etc.

Deve notar-se que os tipos de parâmetros podem depender das propriedades do cubo. Por exemplo, para o cubo [Converter](elements/converters/converter.md), o tipo do parâmetro de entrada é automaticamente determinado pelo tipo de dados do cubo de origem de dados para [Converter](elements/converters/converter.md). Ao criar uma ligação, a cor do quadrado no elemento muda automaticamente.

Os parâmetros de saída normalmente permitem várias ligações de saída para diferentes cubos; os parâmetros de entrada geralmente permitem uma ligação, exceto no cubo [Combination](elements/common/combination.md), que permite fundir o fluxo de dados de diferentes cubos num só. O número de ligações simultâneas para um parâmetro é especificado no código-fonte do cubo.

Os cubos para construir esquemas estão divididos em várias categorias, cada uma destinada a ser usada numa determinada parte do esquema.

## Conteúdo recomendado

[Gráfico](elements/common/chart.md)
