# Configuração da Ligação

O **Runner**, além de [exportar definições através do Designer](export_from_designer.md), permite configurar o programa através da sua interface de consola. Para isso, é necessário executar o programa com o comando **setup**:

```cmd
stocksharp.studio.runner setup
```

Será apresentado um menu:

![runner_setup_1](../../images/runner_setup_1.png)

Ao selecionar o item Connections, o programa entrará no modo de configuração do conector:

![runner_setup_2](../../images/runner_setup_2.png)

Aqui pode editar uma ligação guardada anteriormente ou criar uma nova:

![runner_setup_3](../../images/runner_setup_3.png)

Depois de selecionar o tipo pretendido da nova ligação, o programa passará para o menu de edição das respetivas definições:

![runner_setup_4](../../images/runner_setup_4.png)

Para a [Binance](../api/connectors/crypto_exchanges/binance.md), é necessário introduzir as suas definições principais:

![runner_setup_5](../../images/runner_setup_5.png)

![runner_setup_6](../../images/runner_setup_6.png)

![runner_setup_7](../../images/runner_setup_7.png)

Para verificar a correção dos dados introduzidos, selecione **Check**:

![runner_setup_8](../../images/runner_setup_8.png)

A verificação da ligação será iniciada:

![runner_setup_9](../../images/runner_setup_9.png)

Em caso de sucesso, será apresentada uma mensagem:

![runner_setup_10](../../images/runner_setup_10.png)

Depois de introduzir todas as definições e as verificar, deve premir **Save**:

![runner_setup_11](../../images/runner_setup_11.png)

Na pasta Data, será criado um ficheiro **connector.json** (se ainda não tiver sido criado), que conterá as definições guardadas.

Para configurar a integração com o [Telegram](../telegram_services.md), selecione o item de menu:

![runner_telegram_1](../../images/runner_telegram_1.png)

E autentique-se por um método conveniente:

![runner_telegram_2](../../images/runner_telegram_2.png)

Para autenticação por token, introduza o token de [https://stocksharp.ru/profile/](https://stocksharp.ru/profile/):

![Profile](../../images/profile.png)

Em caso de sucesso, o programa apresentará as opções de operação do Telegram disponíveis:

![runner_telegram_3](../../images/runner_telegram_3.png)
