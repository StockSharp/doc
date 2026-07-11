# Configuração da Ligação

O **Runner**, além de [exportar definições através do Designer](export_from_designer.md), permite configurar o programa através da sua interface de consola. Para isso, é necessário executar o programa com o comando **setup**:

```cmd
stocksharp.studio.runner setup
```

Será apresentado um menu:

![Configuração da Ligação 1](../../images/runner_setup_1.png)

Ao selecionar o item Connections, o programa entrará no modo de configuração do conector:

![Configuração da Ligação 2](../../images/runner_setup_2.png)

Aqui pode editar uma ligação guardada anteriormente ou criar uma nova:

![Configuração da Ligação 3](../../images/runner_setup_3.png)

Depois de selecionar o tipo pretendido da nova ligação, o programa passará para o menu de edição das respetivas definições:

![Configuração da Ligação 4](../../images/runner_setup_4.png)

Para a [Binance](../api/connectors/crypto_exchanges/binance.md), é necessário introduzir as suas definições principais:

![Configuração da Ligação 5](../../images/runner_setup_5.png)

![Configuração da Ligação 6](../../images/runner_setup_6.png)

![Configuração da Ligação 7](../../images/runner_setup_7.png)

Para verificar a correção dos dados introduzidos, selecione **Verificar**:

![Configuração da Ligação 8](../../images/runner_setup_8.png)

A verificação da ligação será iniciada:

![Configuração da Ligação 9](../../images/runner_setup_9.png)

Em caso de sucesso, será apresentada uma mensagem:

![Configuração da Ligação 10](../../images/runner_setup_10.png)

Depois de introduzir todas as definições e as verificar, deve premir **Guardar**:

![Configuração da Ligação 11](../../images/runner_setup_11.png)

Na pasta Data, será criado um ficheiro **connector.json** (se ainda não tiver sido criado), que conterá as definições guardadas.

Para configurar a integração com o [Telegram](../telegram_services.md), selecione o item de menu:

![Configuração da Ligação 1](../../images/runner_telegram_1.png)

E autentique-se por um método conveniente:

![Configuração da Ligação 2](../../images/runner_telegram_2.png)

Para autenticação por token, introduza o token de [https://stocksharp.ru/profile/](https://stocksharp.ru/profile/):

![Perfil](../../images/profile.png)

Em caso de sucesso, o programa apresentará as opções de operação do Telegram disponíveis:

![Configuração da Ligação 3](../../images/runner_telegram_3.png)
