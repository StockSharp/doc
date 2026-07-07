# Interface de utilizador

Para executar o teste no histórico, deve selecionar uma estratégia cujo esquema será testado no histórico. A estratégia é selecionada no painel [Schemas](../user_interface/schemas.md), na pasta da estratégia, fazendo duplo clique na estratégia pretendida. Quando seleciona uma estratégia para a área de trabalho, aparece um novo separador com a estratégia; ao mudar para esse separador, o separador **Emulation** abre automaticamente no Ribbon.

![Designer Interface Backtesting 00](../../../images/designer_interface_backtesting_00.png)

No separador **Emulation**, pode alterar o nome da estratégia e atribuir-lhe uma breve descrição.

Para executar o teste no histórico, no **separador Emulation** especifique o caminho para os dados históricos no campo Market Data e defina o período de teste. A estratégia para teste é iniciada clicando no **botão Start** ![Designer Interface Backtesting 01](../../../images/designer_interface_backtesting_01.png). Depois de iniciar a estratégia para teste, ficam ativos o botão **Pause** ![Designer Interface Backtesting 02](../../../images/designer_interface_backtesting_02.png), que suspende o teste, e o botão **Stop** ![Designer Interface Backtesting 03](../../../images/designer_interface_backtesting_03.png), que para completamente o teste. Ao editar a estratégia, são úteis os botões **Undo(Ctrl+Z)** ![Designer Interface Backtesting 04](../../../images/designer_interface_backtesting_04.png), que cancela a última ação, **Redo(Ctrl+Y)** ![Designer Interface Backtesting 05](../../../images/designer_interface_backtesting_05.png), que repõe a ação anulada, e **Refresh(Ctrl+R)** ![Designer Interface Backtesting 06](../../../images/designer_interface_backtesting_06.png), que atualiza completamente o esquema. Além disso, no **separador Emulation** pode usar o **Debugger** ([Depuração](debugging.md)) ou executar a **Optimization** da estratégia.

O separador da estratégia selecionada contém, por predefinição, os seguintes painéis:

- O painel **Scheme**, no qual é realizado o principal processo de trabalho de conceção da estratégia e dos seus componentes, combinando cubos e linhas de ligação. O Scheme é descrito em detalhe na secção [Painel do diagrama](../strategies/using_visual_designer/diagram_panel.md).
- Painel de elementos informativos, que contém o **Chart**, **Orders**, **Trades**, **Statistics** e outros componentes. Pode adicionar o componente necessário selecionando-o no separador **Emulation**, no grupo **Components**.
- O painel **Properties** fica recolhido por predefinição no lado direito do separador da estratégia. No painel **Properties**, pode configurar as definições gerais de **Emulation**. Por exemplo, o **Market-data storage format** pode ser definido como **BIN** ou **CSV**, dependendo do formato de ficheiro do armazenamento selecionado. O tipo de dados pode ser Ticks ou Candles. Se Ticks estiver selecionado, os candles serão formados a partir dos ticks especificados em [Definições de backtesting](../user_interface/components/backtesting_settings.md).

## Conteúdo recomendado

[Definições de backtesting](../user_interface/components/backtesting_settings.md)
