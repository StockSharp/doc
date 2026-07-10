# Interactive Brokers

**Interactive Brokers** - plataforma de negociação para negociar ativos financeiros, incluindo ações, opções, futuros, EFPs, opções sobre futuros, forex, obrigações e fundos.

Antes de escrever robôs de negociação para esta plataforma de negociação, leia as ligações em [Conectores](../../connectors.md).

## Configuração TWS Interactive Brokers

1. Tem de permitir ligações a partir de outros programas (como o algoritmo de negociação em [S#](../../../api.md)). Para o fazer, abra o menu de definições "File -\> Global configuration...". Selecione "Configuration -\> API -\> Settings" na nova janela:

   ![ib configurações](../../../../images/ib_settings.png)
2. Ative o modo "Enable ActiveX and Socket Clients".
3. Adicione também o endereço do computador que irá executar o algoritmo (endereço local: 127.0.0.1). Isto elimina a necessidade de confirmar a permissão de ligação ao terminal sempre que iniciar o algoritmo.

## Ver também

[Conectores](../../connectors.md)

[Configuração gráfica](../graphical_configuration.md)

[Guardar e carregar definições](../save_and_load_settings.md)

[Criar o próprio conector](../creating_own_connector.md)

[Gestão de Ordens](../../orders_management.md)

[Criar nova ordem](../../orders_management/create_new_order.md)

[Criar nova ordem stop](../../orders_management/create_new_stop_order.md)

[Inicialização do adaptador Interactive Brokers](interactive_brokers/adapter_initialization_interactive_brokers.md)
