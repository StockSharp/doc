# Configuração gráfica do cTrader

Para todos os produtos StockSharp, a configuração gráfica da ligação é efetuada no formulário de ecrã [Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md):

![Configurações da API GUI cTrader](../../../../../images/api_gui_settings_ctrader.png)

- **Demonstração** - Ligação à negociação de demonstração.

Autorização OAuth:

O cTrader fornece apenas o método de autorização OAuth.

Processo de autorização OAuth:

1. Ao clicar no botão "Check", será aberta uma janela:

   ![OAuth Start](../../../../../images/oauth_start.png)

2. Depois de clicar em "Start", o utilizador será redirecionado para o site do cTrader para iniciar sessão. No site do cTrader, é necessário permitir que a aplicação StockSharp aceda às operações de negociação:

   ![Login cTrader](../../../../../images/api_gui_settings_ctrader_2.png)

3. Depois disso, será redirecionado de volta para o site da StockSharp, e o programa iniciará sessão automaticamente.

## Ver também

[Conectores](../../../connectors.md)

[OAuth](../../oauth.md)

[Configuração gráfica](../../graphical_configuration.md)

[Criar o próprio conector](../../creating_own_connector.md)

[Guardar e carregar definições](../../save_and_load_settings.md)
