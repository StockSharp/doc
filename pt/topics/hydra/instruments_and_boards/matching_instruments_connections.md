# Correspondência instrumentos-ligações

O mesmo instrumento pode ter nomes diferentes em diferentes sistemas de negociação. É possível corresponder o instrumento e as ligações através das quais este instrumento será negociado, e especificar como é identificado no sistema de negociação externo.

Isto permite organizar os dados recebidos e simplificar o armazenamento. Na prática, todos os dados recebidos de várias fontes serão consolidados num único local, não pelo nome da fonte, mas pelo nome do instrumento.

Isto também é útil ao negociar o mesmo instrumento em diferentes boards de negociação ou através de diferentes ligações (ou brokers). Além disso, permite obter dados de uma ligação e efetuar transações através de outra ligação.

Para corresponder instrumentos e ligações, deve:

1. Ir para o separador **Securities** e clicar no botão **Securities and Connections**.![Designer Security mapping 01 00](../../../images/designer_security_mapping_01_00.png)
2. Na lista de ligações, selecione a ligação necessária.![Designer Security mapping 01](../../../images/designer_security_mapping_01.png)
3. Preencha todas as colunas.

   Por exemplo:

   Instrumento de ações APPLE.
   - Ligação - **Interactive Brokers**. Clique no botão ![Designer Creation tool 00](../../../images/designer_creation_tool_00.png), após o que será adicionada uma nova linha. 
   - Nas colunas **Security** code e **Board code**, especifique o código do instrumento e o código da board. Nas colunas **Security code in adapter** e **Board code in adapter**, especifique o código do instrumento e o código da board tal como estão especificados no sistema de negociação externo. Clique em **OK** ![Designer Security mapping 01 01](../../../images/designer_security_mapping_01_01.png)
   - Repetimos os passos para as ligações **Interactive Brokers** e **CQG Continuum** da mesma forma.

   | **Interactive Brokers**                                                           | **CQG Continuum**                                                                 |
   | --------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- |
   | ![Designer Security mapping 01 02](../../../images/designer_security_mapping_01_02.png) | ![Designer Security mapping 01 03](../../../images/designer_security_mapping_01_03.png) |
4. Agora, todos os dados descarregados, no nosso caso para ações APPLE, serão guardados num único local.
