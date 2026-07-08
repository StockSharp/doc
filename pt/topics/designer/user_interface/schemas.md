# Painel Schemes

Para abrir o painel **Schemes**, tem de clicar no botão **Schemes** no separador **Common**. O painel **Schemes** contém uma árvore de scripts, agrupados em pastas por finalidade. Os esquemas de estratégias e os blocos personalizados não são diferentes. São editados com um editor comum, [Designer de estratégias](../strategies/using_visual_designer/diagram_panel.md). No entanto, para evitar confusão entre eles, estão divididos em duas listas independentes e guardados em pastas diferentes (estratégias na pasta **Backtest**, blocos personalizados na pasta **Custom Blocks**). A seleção de um esquema para edição é feita com duplo clique no item necessário na lista. O esquema selecionado será então aberto no designer para visualização e edição. Abaixo encontra-se uma descrição das pastas no painel **Schemes**:

![Designer Panel Circuits 00](../../../images/designer_panel_circuits_00.png)

1. A pasta **Backtest** contém estratégias de negociação criadas tanto como esquemas a partir de um conjunto de elementos e ligações entre eles, como a partir de código. Pode adicionar uma nova estratégia premindo o botão **Add** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) no separador **Common** e selecionando **Strategy**. Ou clicando com o botão direito do rato na pasta **Backtest** no painel **Schemes** e premindo o botão **Add** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) no menu pendente. Na janela que se abre, selecione exatamente como pretende criar uma estratégia.
   
    ![Designer Panel Circuits 04](../../../images/designer_panel_circuits_04.png)
   
    As estratégias podem ser criadas usando um designer visual sem programação ou usando o editor de código-fonte integrado. Além disso, podem ser ligados ficheiros DLL externos com estratégias escritas no Microsoft Visual Studio. As informações detalhadas sobre **Strategies** são descritas na secção [Usar blocos](../strategies/using_visual_designer.md).

2. A pasta **Own elements** contém elementos que representam uma funcionalidade completa e podem ser usados em vários esquemas ou num esquema várias vezes com valores de propriedades diferentes. Esses conjuntos de elementos podem ser extraídos para um bloco separado, que será depois usado como qualquer elemento padrão. **Custom block** é um esquema normal que é guardado/carregado/editado como qualquer esquema de estratégia. Adicione um novo elemento composto premindo o botão **Add** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) no separador **Common** e selecionando **Custom Blocks**. Ou clicando com o botão direito do rato na pasta **Custom Blocks** no painel **Schemes** e premindo o botão **Add** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) no menu pendente. Ao adicionar novos blocos personalizados, estes são adicionados automaticamente à **Element Palette**, no grupo **Custom Blocks**, e podem ser usados na criação de outros esquemas de estratégias e blocos personalizados. As informações detalhadas sobre **Custom Blocks** são descritas na secção [Creating composite elements](../strategies/using_visual_designer/composite_elements.md).

3. A pasta **Live** contém estratégias adicionadas para negociação. As estratégias iniciadas são marcadas com o ícone ![Designer Panel Circuits 02](../../../images/designer_panel_circuits_02.png), e as que estão paradas são marcadas com o ícone ![Designer Panel Circuits 03](../../../images/designer_panel_circuits_03.png). A forma de adicionar estratégias à pasta **Live** e de as iniciar é descrita na secção [Live trading](../live_execution/getting_started.md).

4. A pasta **Indicators** contém os seus próprios indicadores para estratégias de negociação, escritos por si. Não é possível criar novos indicadores com esquemas; apenas estão disponíveis código e ficheiros DLL externos. O uso de indicadores personalizados em esquemas está disponível através do bloco [Indicador](../strategies/using_visual_designer/elements/common/indicator.md) ao selecionar o tipo de indicador.

5. A pasta **Remote** contém estratégias localizadas num servidor remoto.

## Ver Também

[Painel Logs](logs.md)
