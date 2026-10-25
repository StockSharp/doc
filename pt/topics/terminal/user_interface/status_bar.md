# Barra de estado

Na parte inferior da janela do [Terminal](../../terminal.md) está a barra de estado. Da esquerda para a direita contém:

- **estado**: **Desconectado**, **Conectando**, **Conectado**, **Desconectando** ou o texto do erro de ligação. É aqui também que o Terminal escreve o resultado da última ação, por exemplo, «Disposição guardada.» ou «Instrumento criado: …»;
- **notícias da StockSharp**: os títulos das notícias do site, que podem ser percorridos com os botões de seta;
- **contadores de avisos e erros** do registo: aparecem quando o registo contém avisos ou erros;
- **última mensagem do registo**: um clique direito nela copia o seu texto para a área de transferência. O registo completo está no painel [Registos](logs.md);
- **ticks por segundo** (tps): quantos negócios de mercado (ticks) o Terminal recebeu no último segundo;
- **latência** (ms): quantos milissegundos depois da sua hora no mercado chegaram o último tick ou a última alteração de Level 1;
- **memória** ocupada pelos dados do Terminal, em megabytes;
- **tempo de funcionamento** do programa desde o arranque;
- o botão **Diretório de registos**: abre a pasta na qual o Terminal escreve o registo.

![Terminal barra de estado 00](../../../images/terminal_status_bar_00.png)

Enquanto o Terminal não está ligado, os ticks e a latência não são contados: na barra aparecem **0 tps** e **— ms**.

## Ver também

[Registos](logs.md)

[Menu principal](ribbon.md)
