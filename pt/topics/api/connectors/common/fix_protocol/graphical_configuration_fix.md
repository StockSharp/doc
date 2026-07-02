# Configuração gráfica FIX

Para todos os produtos [S#](../../../../api.md), a configuração gráfica da conexão é realizada na [Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md):

![API GUI Settings FIX](../../../../../images/api_gui_settings_fix.png)

- **Address** - Endereço.
- **Dialect** - Dialeto do protocolo FIX.
- **Sender** - Identificador do remetente.
- **Target** - Identificador do destinatário.
- **Login** - Login.
- **Password** - Senha.
- **Portfolios** - Solicitar todos os portfólios ao iniciar.
- **Instruments** - Solicitar todos os instrumentos na conexão.
- **Encoding** - Codificação usada para a transferência de dados.
- **Sequence reset** - Se deve redefinir o contador de identificadores.
- **Date format** - Formato de data.
- **Date and time format** - Formato de data e hora.
- **Time format** - Formato de hora.
- **Receive timeout** - Tempo limite para recebimento de dados.
- **Send timeout** - Tempo limite para envio de dados.
- **Unknown transactions** - Processar execuções desconhecidas geradas por terceiros.
- **Protocol** - Protocolo SSL para estabelecer a conexão.
- **Certificate** - Certificado SSL.
- **Password** - Senha do certificado SSL.
- **Revocation check** - Verificação de revogação de certificado.
- **Check remote** - Verificar certificados remotos.
- **Server name** - Nome do servidor que usa a conexão SSL.
- **Reconnection settings** - Configurações do mecanismo para rastrear a conexão com o sistema de negociação ([Configurações de reconexão](../../reconnection_settings.md)).
- **Heartbeat interval** - Intervalo para notificar o servidor de que a conexão ainda está ativa. O valor padrão é 1 minuto.
- **Unified board code** - Código da bolsa para o instrumento unificado.

## Veja também

[Conectores](../../../connectors.md)

[Configuração gráfica](../../graphical_configuration.md)

[Criando seu próprio conector](../../creating_own_connector.md)

[Salvar e carregar configurações](../../save_and_load_settings.md)
