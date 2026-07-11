# UDP Dumper

O **UDPDumper** grava pacotes UDP. Pode ser utilizado para verificar as definições de rede fornecidas por uma corretora ou bolsa e para recolher dados para testes posteriores de conectores baseados em UDP, como [FAST](api/connectors/common/fast_protocol.md) ou SBE.

Instale o UDPDumper através do [Instalador](installer.md).

## Configuração e execução

1. No primeiro arranque, a aplicação apresenta o seguinte:![Captura de tela de UDP Dumper 1](../images/dumper_1.png)
2. Para adicionar fluxos de rede, adicione-os manualmente ou carregue todos os fluxos a partir dos ficheiros de configuração da bolsa. Para isso, clique no botão:![Captura de tela de UDP Dumper 2](../images/dumper_2.png)
3. Na janela apresentada, deve localizar o ficheiro de configuração pretendido da bolsa e abri-lo:![Captura de tela de UDP Dumper 3](../images/dumper_3.png)
4. Todos os fluxos com definições de endereços IP e portas serão carregados a partir de um ficheiro:![Captura de tela de UDP Dumper 4](../images/dumper_4.png)
5. Selecione os fluxos necessários e clique no botão para iniciar a transferência:![Captura de tela de UDP Dumper 5](../images/dumper_5.png)
6. Se as definições estiverem corretas, o programa começará a receber datagramas UDP e a gravá-los no disco. A aplicação mostrará o número de bytes recebidos para cada fluxo:![Captura de tela de UDP Dumper 6](../images/dumper_6.png)
7. O **UDPDumper** tem uma interface gráfica. Se precisar de o executar sem interface gráfica, por exemplo em Linux, utilize o **UDPDumper.Console**, a versão de consola multiplataforma.

   A aplicação **UDPDumper.Console** recebe como parâmetro o caminho para o ficheiro criado pela versão com interface gráfica (exatamente a versão com interface gráfica, e **não uma configuração da bolsa**):

   ```cs
   		StockSharp.UdpDumper.Console.exe settings.json
   		
   ```
8. Para testar um conector nos dados recolhidos, utilize o modo dump. Para mais detalhes, consulte [Modo dump](api/connectors/common/fast_protocol/dump_mode.md).
