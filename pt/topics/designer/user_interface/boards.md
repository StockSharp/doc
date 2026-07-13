# Mercados de negociação

No painel **Editor de mercados**, pode criar **Mercados** e **Bolsas**, e ver ou personalizar os existentes.

![Captura de tela de Mercados de negociação](../../../images/designer_boards.png)

Em [S#](../../api.md), os instrumentos de diferentes origens utilizam um identificador unificado composto pelo código do instrumento e pelo código do mercado. A sintaxe é [**código do instrumento**]@[código do mercado]. Por exemplo, para as acções **AAPL** da bolsa **NASDAQ**, o identificador é **AAPL@NASDAQ**. Cada instrumento está associado a um mercado específico no qual é negociado. No entanto, o instrumento pode ser negociado em diferentes mercados. Neste caso, os códigos dos mercados serão diferentes. Para cada mercado, pode configurar um horário de trabalho com dias úteis e fins-de-semana.

A bolsa pode ter vários mercados com diferentes condições de negociação (hora da sessão, comissão, etc.). Mas cada mercado está associado a uma bolsa específica. Portanto, no **Editor de mercados**, quando selecciona um mercado, a informação sobre a bolsa será automaticamente alterada para a bolsa onde o mercado se encontra. Para cada bolsa, pode definir o código da bolsa, o país e os nomes em russo e em inglês.
