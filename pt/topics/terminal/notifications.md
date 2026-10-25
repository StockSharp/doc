# Notificações

O [Terminal](../terminal.md) informa sobre os eventos importantes com janelas pop-up no canto do ecrã: ligação estabelecida ou interrompida, negócio próprio realizado, ordem que não foi possível cancelar. Um erro de ligação é apresentado numa janela de aviso.

O Terminal também pode informar de outras formas sobre os eventos das ordens e da ligação. Escolhem-se na janela [Configurações](settings/common_settings.md) (**Configurações → Configurações**), no grupo **Notificação**, para cada evento separadamente:

- **Mudança de ordem**: a ordem mudou;
- **Correspondência**: a ordem foi executada;
- **Erro**: a ordem não foi aceite;
- **Perdido** e **Restaurado**: a ligação perdeu-se e foi restabelecida.

Para cada evento é possível escolher como informar sobre ele:

- **Som** - será reproduzido um sinal sonoro.
- **Janela pop-up** - aparecerá uma pequena janela com uma mensagem no canto do ecrã.
- **Ficheiro de registo** - a mensagem será escrita no registo, que se vê no painel [Registos](user_interface/logs.md).
- **Telegram** - a mensagem será enviada para o canal do Telegram indicado no mesmo grupo de definições. Os canais são obtidos da conta StockSharp, pelo que, para este método, é preciso iniciar sessão no site.

Se não for escolhido nenhum método, o evento não é comunicado.

## Ver também

[Definições gerais](settings/common_settings.md)

[Menu principal](user_interface/ribbon.md)
