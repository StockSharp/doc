# RemoteManager

O separador **RemoteManager** permite ativar o modo de controlo remoto. Para ativar este modo, deve aceder ao menu de configuração do utilizador.

![Shell gestor remoto 00](../../../images/shell_remotemanager_00.png)

Na janela que aparece, defina o seu **nome de utilizador** e **palavra-passe**.

![Shell gestor remoto 01](../../../images/shell_remotemanager_01.png)

Depois, é necessário ativar o **modo de servidor**.

![Shell gestor remoto 02](../../../images/shell_remotemanager_02.png)

Agora pode ligar-se ao Shell a partir de outro Shell.

Para isso, precisa de executar **outro Shell**. Nele, aceda às definições de ligação.

![Shell gestor remoto 03](../../../images/shell_remotemanager_03.png)

Na janela que abre, configure a ligação FIX.

![Shell gestor remoto 04](../../../images/shell_remotemanager_04.png)

Depois prima o botão Ligar.

![Shell gestor remoto 05](../../../images/shell_remotemanager_05.png)

Quando estiver ligado, todas as estratégias existentes no servidor Shell ficarão disponíveis no cliente Shell.

![Shell gestor remoto 06](../../../images/shell_remotemanager_06.png)

Ao clicar no botão Adicionar, pode adicionar outra estratégia para negociar.

![Shell gestor remoto 07](../../../images/shell_remotemanager_07.png)

Como o cliente Shell suporta vários servidores, ao adicionar uma estratégia deve selecionar o servidor à esquerda. Todas as estratégias disponíveis no servidor aparecerão à direita.

![Shell gestor remoto 08](../../../images/shell_remotemanager_08.png)

Depois de adicionar uma estratégia, ela aparecerá na lista de estratégias.

![Shell gestor remoto 09](../../../images/shell_remotemanager_09.png)

Ao selecionar uma estratégia, haverá separadores à direita com as definições da estratégia, bem como as suas estatísticas.

Depois de alterar as definições da estratégia, certifique-se de clicar no botão Aplicar alterações; caso contrário, as alterações não serão aplicadas à estratégia.

![Shell gestor remoto 10](../../../images/shell_remotemanager_10.png)

Se a estratégia tiver um comando diferente de Start\/Stop, então, para o aplicar, deve defini-lo no campo seguinte.

![Shell gestor remoto 11](../../../images/shell_remotemanager_11.png)

E clicar no botão de envio do comando.

Para definir o seu comando na estratégia, precisa de substituir o método [Strategy.ApplyCommand](xref:StockSharp.Algo.Strategies.Strategy.ApplyCommand(StockSharp.Messages.CommandMessage))**(**[StockSharp.Messages.CommandMessage](xref:StockSharp.Messages.CommandMessage) cmdMsg **)**.

```cs
public virtual void ApplyCommand(CommandMessage cmdMsg)
		
```

A classe base [Strategy](xref:StockSharp.Algo.Strategies.Strategy) controla apenas o início e a paragem da estratégia.

## Conteúdo recomendado

[Definições de ligações](../connections_settings.md)
