# Exportação

O Designer permite exportar qualquer tipo de dados: estratégias, blocos e indicadores. Existem várias formas de exportar:

- No painel **Esquemas**, clique com o botão direito do rato na estratégia, bloco ou indicador. No menu que aparece, selecione **Exportar**.
- No separador **Geral**, prima o botão **Exportar**:

![Designer Exportar estratégias 00](../../../images/designer_export_strategies_00.png)

Depois de premir **Exportar**, dependendo do tipo de conteúdo, será apresentada uma janela:

- para um [esquema](../strategies/using_visual_designer.md):

  ![Designer Exportar estratégias 01](../../../images/designer_export_strategies_01.png)

  - esquema - exportar o esquema tal como está. O modo **Independente** é necessário para esquemas que usam elementos ou indicadores próprios. Neste caso, todos os elementos internos serão exportados dentro do diagrama da estratégia.
  - código - converter o esquema em código C#.
  - DLL - compilar o esquema para uma DLL. Adequado se precisar de manter o código confidencial.

- para [código](../strategies/using_code.md):

  ![Designer Exportar estratégias 02](../../../images/designer_export_strategies_02.png)

  - esquema - exportar o código como um ficheiro JSON, que incluirá tanto o próprio código como as referências necessárias para compilar esse código.
  - código - exportar o código tal como está.
  - DLL - compilar o código para uma DLL. Adequado se precisar de manter o código confidencial.

- para uma [dll](../strategies/using_dll.md), será apresentada uma janela de seleção de ficheiro.

## Consulte também

[Executar estratégias fora do Designer](../live_execution/running_strategies_outside_of_designer.md)
