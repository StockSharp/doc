# Linha de Comandos

O **Runner**, sendo uma aplicação de consola, permite iniciar em diferentes modos especificando parâmetros na linha de comandos. Iniciar o programa sem parâmetros irá apresentar uma mensagem de ajuda com os parâmetros disponíveis:

![Linha de Comandos 1](../../images/runner_command_line_1.png)

Iniciar o **Runner** para teste em dados históricos:

```cmd
b -s SmaStrategy.cs -h "C:\Storage" --hf 20200401 --ht 20200430 --sec AAPL@NASDAQ -r json
```

Parâmetros disponíveis:

- -s - caminho para o ficheiro da estratégia (com extensão cs, json ou dll).
- -t - (opcional) se for selecionado um ficheiro dll e o assembly contiver mais do que uma classe de estratégia, é necessário especificar o tipo pretendido através deste parâmetro.
- -h - caminho para o diretório com dados históricos. Pode ser um endereço de rede no caso de utilização do modo de servidor [server](../hydra_server.md).
- --hl - (opcional) login, usado no modo de servidor [server](../hydra_server.md).
- --hp - (opcional) palavra-passe, usada no modo de servidor [server](../hydra_server.md).
- --hf - data inicial para o teste no formato YYYYMMDD.
- --ht - data final para o teste no formato YYYYMMDD.
- -f - (opcional) formato de armazenamento (Binary ou Csv).
- --sec - (opcional) [Identificador do instrumento](../api/instruments/instrument_identifier.md).
- -r - (opcional) formato do relatório de resultado do teste (json, xml, csv).
- --tm - (opcional) timeout da estratégia.
- --memory - (opcional) tamanho máximo de memória (em megabytes).
- --cpu - (opcional) máscara do processador.
- -l - (opcional) nível de registo (Info, Debug, Error, Warning, Verbose).

Iniciar o **Runner** para otimização:

```cmd
o -s SmaStrategy.cs -h "C:\Storage" --hf 20200401 --ht 20200430 --sec AAPL@NASDAQ -r json -p sma_optimization.json
```
Todos os parâmetros do modo de teste histórico, mais os adicionais:

- -p - caminho para o ficheiro de parâmetros.
- --ol - (opcional) número máximo de iterações.
- --ob - (opcional) número de estratégias testadas em simultâneo.

Formato do ficheiro de parâmetros:

```json
[
	{
	"Name": "SMA_80",
	"Value": "200,201"
	},
	{
	"Name": "SMA_30",
	"From": "40",
	"To": "50",
	"Step": "1"
	},
	{
	"Name": "Security",
	"Value": "AAPL@NASDAQ,MSFT@NASDAQ"
	}
]
```

Iniciar o **Runner** para negociação real:

```cmd
l -s SmaStrategy.cs -c connector.json --tg telegram.json
```

- -c - ficheiro de definições da ligação.
- --tg - ficheiro de definições de integração com o Telegram.
