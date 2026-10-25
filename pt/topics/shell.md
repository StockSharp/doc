# Shell

O **Shell** é uma framework gráfica pronta a usar para um terminal de negociação, com o código-fonte totalmente aberto em C#. É construída com [Avalonia](https://avaloniaui.net/) e funciona em Windows, Linux e macOS.

Não precisa de perder tempo a criar uma GUI (interface gráfica de utilizador): a ligação a fontes de dados, as tabelas de carteiras, instrumentos, ordens e negócios, os gráficos, os testes no histórico, a otimização de parâmetros e o registo de trabalho já vêm incluídos. Só falta escrever a própria estratégia.

![Captura de ecrã de Shell](../images/shell_title_00.png)

## Principais funcionalidades

1. Código-fonte completo. Serve tanto para robôs por encomenda como para soluções próprias.
2. **Suporte para mais de 70 ligações** a bolsas e brokers através de [Conectores](api/connectors.md).
3. Quatro áreas de trabalho, entre as quais se alterna com a barra lateral: [Comum](shell/user_interface/common.md), [Tempo real](shell/user_interface/real_time.md), [Emulação](shell/user_interface/emulation.md) e [Registros](shell/user_interface/log.md).
4. Interface flexível: todos os painéis de uma área de trabalho podem ser arrastados, divididos, desacoplados numa janela à parte e voltados a acoplar. A disposição é guardada entre arranques.
5. Teste de estratégias no histórico com estatísticas, gráfico de negócios e curva de rendibilidade.
6. [Otimização](shell/user_interface/optimization.md): pesquisa dos parâmetros da estratégia no mesmo histórico.
7. Guardar e restaurar as definições da estratégia, incluindo as regras de risco.
8. Execução simultânea de várias estratégias: cada uma funciona por conta própria e tem as suas próprias ordens, negócios e estatísticas.
9. Execução de estratégias criadas no [Designer](designer.md) sem escrever código.
10. [Controlo remoto](shell/remote_control.md): início e paragem de estratégias a partir do site ou do bot do Telegram.

## Estratégias incluídas

O Shell inclui quatro estratégias que podem ser executadas logo após o arranque:

| Estratégia | O que faz |
|---|---|
| **SMA strategy** | cruzamento das médias móveis curta e longa |
| **SMA Index strategy** | o mesmo, mas com um instrumento sintético definido por uma expressão |
| **StairsTrend strategy** | entrada a favor da tendência após várias velas seguidas no mesmo sentido |
| **StairsCountertrend strategy** | entrada contra a tendência na mesma situação |

## Conteúdo recomendado

[Instalar o Shell](shell/installing_shell.md)

[Início rápido](shell/quick_start.md)
