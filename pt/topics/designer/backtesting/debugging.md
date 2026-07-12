# Depuração

Durante o processo de teste de uma estratégia, muitas vezes torna-se necessário verificar que dados entram na entrada de um determinado cubo ou são passados na sua saída. Para isso, o [Designer](../../designer.md) inclui o **Depurador**.

![Designer controle de depuração 00](../../../images/designer_debug_00.png)

Os seguintes botões encontram-se no grupo **Depurador** do friso **Emulação**:

- ![Designer controle de depuração 01](../../../images/designer_debug_01.png)**Adicionar ponto de interrupção** – para adicionar um ponto de interrupção ao elemento selecionado. Os elementos aos quais é adicionado um ponto de interrupção ficam destacados com uma margem vermelha.
- ![Designer controle de depuração 02](../../../images/designer_debug_02.png)**Eliminar ponto de interrupção** – para eliminar um ponto de interrupção.
- ![Designer controle de depuração 03](../../../images/designer_debug_03.png)**Próximo elemento** – quando o ponto de interrupção é acionado, avança para o elemento seguinte do esquema.
- **Passo para saída** – quando o ponto de interrupção é acionado, avança para a saída do elemento atual; é usado para verificar os valores passados na saída do elemento.
- ![Designer controle de depuração 04](../../../images/designer_debug_04.png)**Entrar** – quando o ponto de interrupção é acionado, entra no elemento composto. Abre automaticamente o esquema do elemento composto e para no elemento ao qual os dados são transmitidos primeiro.
- ![Designer controle de depuração 05](../../../images/designer_debug_05.png)**Sair** – quando o ponto de interrupção é acionado e se encontra dentro do elemento composto, sai um nível acima para onde o elemento composto aberto é usado.
- ![Designer controle de depuração 06](../../../images/designer_debug_06.png)**Continuar** – Continua até que o próximo ponto de interrupção seja acionado.

## Conteúdo recomendado

[Pontos de interrupção](debugging/break_points.md)
