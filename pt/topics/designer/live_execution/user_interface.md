# Interface

Depois de adicionar uma estratégia à pasta **Live**, ao fazer duplo clique na estratégia adicionada, abre-se um separador intitulado "Live [Strategy Name]". Ao navegar para este separador, o separador **Live** abre-se automaticamente no **Ribbon**. No separador **Live**, pode especificar o instrumento e o portefólio com os quais a estratégia irá trabalhar. Ao premir o botão **Start**, inicia a live trading para a estratégia; ao premir o botão **Stop**, interrompe-a.

![Designer Interface Live trade 00](../../../images/designer_interface_live_trade_00.png)

O separador da estratégia contém o Strategy Designer para esquemas e elementos de componentes, semelhante ao descrito em [Strategy Designer](../strategies/using_visual_designer/diagram_panel.md). Além disso, o separador inclui o painel [Live Trading Properties](../user_interface/components/live_settings.md), que por predefinição está recolhido e anexado ao lado direito do separador.

Adicionar uma estratégia a **Live** implica copiá-la do código original (no caso de usar [esquemas](../strategies/using_visual_designer.md) ou [código](../strategies/using_code.md)). Por isso, as alterações ao algoritmo dentro da cópia **Live** não afetam o original. Ao iniciar a estratégia, se houver uma discrepância entre **Live** e o original, será apresentado um aviso:

![Designer Interface Live trade 01](../../../images/designer_interface_live_trade_01.png)

- **Yes** significa aplicar as alterações do original à cópia **live**.
- **No** significa ignorar a diferença e iniciar a cópia **live** sem aplicar alterações.
- **Cancel** significa não iniciar nada.

As alterações na cópia **Live** devem ser mínimas, destinadas a testes e posterior transferência para o original. Caso contrário, existe o risco de perder alterações se a cópia **Live** for atualizada para a versão do original.

## Consulte também

[Definições de ligação](../connections_settings.md)
