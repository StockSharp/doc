# Executar estratégias a partir do Designer

O [Shell](../shell.md) consegue executar estratégias criadas no [Designer](../designer.md) e exportadas através da [Exportação](../designer/export_import/export.md) de estratégias. Para isso não é preciso escrever código: o esquema é executado pelo mesmo motor que no Designer.

## Importação

Abra a área [Tempo real](user_interface/real_time.md) ou [Emulação](user_interface/emulation.md), prima **Adicionar** e, na janela que se abre, **Importar**.

![Shell seleção de estratégia](../../images/shell_add_strategy_00.png)

Selecione o ficheiro do esquema exportado do Designer. Aparece na lista junto às estratégias incluídas, com o nome que tinha no Designer. Junto à lista é mostrado como terminou a importação: esquema importado, importação cancelada ou importação falhada com a indicação da causa.

Os esquemas importados são guardados na pasta `Data` junto à aplicação e continuam na lista depois de reiniciar.

## Arranque

A partir daí, trabalha-se com o esquema tal como com uma estratégia incluída: selecione-o, prima **OK**, preencha o instrumento, a carteira, o volume e os parâmetros do esquema no painel **Estratégia** e prima **Iniciar**.

Na área [Emulação](user_interface/emulation.md), o esquema é executado no histórico e dá as mesmas estatísticas, o mesmo gráfico e as mesmas listas de ordens e negócios que qualquer outra estratégia.

## Conteúdo recomendado

[Criar estratégia](create_strategy.md)

[Tempo real](user_interface/real_time.md)
