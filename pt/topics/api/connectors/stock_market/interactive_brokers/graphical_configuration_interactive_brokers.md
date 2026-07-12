# Configuração gráfica Interactive Brokers

Para todos os produtos [S#](../../../../api.md), a configuração gráfica da ligação é efetuada na [Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md):

![Configurações da API GUI Interactive Brokers](../../../../../images/api_gui_settings_interactivebrokers.png)

- **endereço** - Endereço TWS.
- **Identifier** - ID único. Utilizado quando vários clientes estão ligados a um terminal ou gateway.
- **Real-time** - Se devem ser utilizados dados em tempo real ou "congelados" no servidor da corretora.
- **Nível de registo** - Nível de registo das mensagens do servidor.
- **Campos de dados de mercado** - Campos de dados de mercado que serão recebidos com as mensagens Level1 subscritas.
- **Protocol** - Protocolo SSL para estabelecer a ligação
- **certificado** - Certificado SSL.
- **palavra-passe** - Palavra-passe do certificado SSL.
- **Verificar revogação** - Verificar a revogação do certificado.
- **Validar certificados remotos** - Validar certificados remotos.
- **Nome do host** - O nome do servidor que partilha a ligação SSL.
- **MaxVersion** - MaxVersion
- **Intervalo de verificação da ligação** - Intervalo de verificação do servidor para controlar se a ligação está ativa. Por predefinição, é igual a 1 minuto.
- **Definições de religação** - Mecanismo de controlo das ligações com as definições do sistema de negociação. ([Definições de religação](../../reconnection_settings.md))

## Conteúdo recomendado

[Conectores](../../../connectors.md)

[Configuração gráfica](../../graphical_configuration.md)

[Criar o próprio conector](../../creating_own_connector.md)

[Guardar e carregar definições](../../save_and_load_settings.md)
