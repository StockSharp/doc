# Boards de negociação

No painel **Board editor**, pode criar **Boards** e **Exchanges**, e ver ou personalizar os existentes.

![Designer Boards](../../../images/designer_boards.png)

Em [S#](../../api.md), os instrumentos de diferentes origens utilizam um identificador unificado composto pelo código do instrumento e pelo código do board. A sintaxe é [**código do instrumento**]@[código do board]. Por exemplo, para as acções **AAPL** da bolsa **NASDAQ**, o identificador é **AAPL@NASDAQ**. Cada instrumento está associado a um board específico no qual é negociado. No entanto, o instrumento pode ser negociado em diferentes boards. Neste caso, os códigos dos boards serão diferentes. Para cada board, pode configurar um horário de trabalho com dias úteis e fins-de-semana.

A bolsa pode ter vários boards com diferentes condições de negociação (hora da sessão, comissão, etc.). Mas cada board está associado a uma bolsa específica. Portanto, no **Board editor**, quando selecciona um board, a informação sobre a bolsa será automaticamente alterada para a bolsa onde o board se encontra. Para cada bolsa, pode definir o código da bolsa, o país e os nomes em russo e em inglês.
