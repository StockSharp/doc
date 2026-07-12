# Interface de utilizador

Para executar o teste no histórico, deve selecionar uma estratégia cujo esquema será testado no histórico. A estratégia é selecionada no painel [Esquemas](../user_interface/schemas.md), na pasta da estratégia, fazendo duplo clique na estratégia pretendida. Quando seleciona uma estratégia para a área de trabalho, aparece um novo separador com a estratégia; ao mudar para esse separador, o separador **Emulação** abre automaticamente na faixa de opções.

![Designer Interface de testes históricos 00](../../../images/designer_interface_backtesting_00.png)

No separador **Emulação**, pode alterar o nome da estratégia e atribuir-lhe uma breve descrição.

Para executar o teste no histórico, no separador **Emulação** especifique o caminho para os dados históricos no campo **Dados de mercado** e defina o período de teste. A estratégia para teste é iniciada clicando no botão **Iniciar** ![Designer Interface de testes históricos 01](../../../images/designer_interface_backtesting_01.png). Depois de iniciar a estratégia para teste, ficam ativos o botão **Pausa** ![Designer Interface de testes históricos 02](../../../images/designer_interface_backtesting_02.png), que suspende o teste, e o botão **Parar** ![Designer Interface de testes históricos 03](../../../images/designer_interface_backtesting_03.png), que para completamente o teste. Ao editar a estratégia, são úteis os botões **Anular (Ctrl+Z)** ![Designer Interface de testes históricos 04](../../../images/designer_interface_backtesting_04.png), que cancela a última ação, **Refazer (Ctrl+Y)** ![Designer Interface de testes históricos 05](../../../images/designer_interface_backtesting_05.png), que repõe a ação anulada, e **Atualizar (Ctrl+R)** ![Designer Interface de testes históricos 06](../../../images/designer_interface_backtesting_06.png), que atualiza completamente o esquema. Além disso, no separador **Emulação** pode usar o **Depurador** ([Depuração](debugging.md)) ou executar a **Otimização** da estratégia.

O separador da estratégia selecionada contém, por predefinição, os seguintes painéis:

- O painel **Esquema**, no qual é realizado o principal processo de trabalho de conceção da estratégia e dos seus componentes, combinando cubos e linhas de ligação. O esquema é descrito em detalhe na secção [Painel do diagrama](../strategies/using_visual_designer/diagram_panel.md).
- Painel de elementos informativos, que contém o **Gráfico**, **Ordens**, **Negócios**, **Estatísticas** e outros componentes. Pode adicionar o componente necessário selecionando-o no separador **Emulação**, no grupo **Componentes**.
- O painel **Propriedades** fica recolhido por predefinição no lado direito do separador da estratégia. No painel **Propriedades**, pode configurar as definições gerais de **Emulação**. Por exemplo, o **Formato de armazenamento de dados de mercado** pode ser definido como **BIN** ou **CSV**, dependendo do formato de ficheiro do armazenamento selecionado. O tipo de dados pode ser Ticks ou Candles. Se Ticks estiver selecionado, os candles serão formados a partir dos ticks especificados em [Definições de testes históricos](../user_interface/components/backtesting_settings.md).

## Conteúdo recomendado

[Definições de testes históricos](../user_interface/components/backtesting_settings.md)
