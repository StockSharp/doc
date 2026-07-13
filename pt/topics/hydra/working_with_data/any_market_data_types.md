# Quaisquer tipos de dados de mercado

O [Hydra](../../hydra.md) permite utilizar tipos de dados alternativos para obter vários tipos de dados de mercado.

Isto é necessário se a origem não permitir descarregar os dados de mercado necessários. Assim, por exemplo, podem ser usados vários tipos de dados de mercado ao mesmo tempo para construir o **Livro de ordens**.

IMPORTANTE\! O **Livro de ordens** pode ser construído a partir do **Registo de ordens** ou de **Level 1**, desde que estes tipos de dados contenham os melhores preços.

Tenha em conta que os valores de **Level 1** podem ser descarregados de qualquer origem que forneça dados de mercado em tempo real. **Level 1** também pode ser recebido por [conversão](../tasks/converter.md) a partir do **Livro de ordens**.

Para o construir, é necessário:

1. Selecionar o período e o instrumento para os quais pretende obter dados de mercado.![Hydra construir dados de profundidade Nível 1](../../../images/hydra_level1_build_depth_data.png)
2. Selecionar o campo **Construir a partir de** e escolher o tipo de dados necessário![Hydra tipo de construção de dados](../../../images/hydra_type_build_data.png)

   IMPORTANTE\! Se **Livro de ordens, Registo de ordens, Level 1** forem selecionados como origem para construir uma vela, aparece uma seleção de parâmetros adicionais.![Hydra propriedades estendidas de construção de dados](../../../images/hydra_ext_proper_build_data.png)
3. Depois de definir os parâmetros, clique no botão ![Captura de tela de Quaisquer tipos de dados de mercado](../../../images/hydra_candles.png).![Hydra resultado dos dados de profundidade Nível 1](../../../images/hydra_level1_build_depth_data_result.png)

Para construir **Velas**, também está disponível a opção de construir velas de um período maior a partir de velas de um período menor.

Por exemplo, se existirem velas com um período de 1 minuto, pode construir velas com um período de 5 minutos a partir delas, selecionando o tipo adequado na linha **Construir a partir de**.

**Veja o [tutorial em vídeo](../videos/building_order_books.md)**
