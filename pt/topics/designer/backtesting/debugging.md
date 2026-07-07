# Depuração

Durante o processo de teste de uma estratégia, muitas vezes torna-se necessário verificar que dados entram na entrada de um determinado cubo ou são passados na sua saída. Para isso, o [Designer](../../designer.md) inclui o **Debugger**.

![Designer Debug 00](../../../images/designer_debug_00.png)

Os seguintes botões encontram-se no grupo **Debugger** do Ribbon **Emulation**:

- ![Designer Debug 01](../../../images/designer_debug_01.png)**Add breakpoint** – para adicionar um breakpoint ao elemento selecionado. Os elementos aos quais é adicionado um breakpoint ficam destacados com uma margem vermelha.
- ![Designer Debug 02](../../../images/designer_debug_02.png)**Delete breakpoint** – para eliminar um breakpoint.
- ![Designer Debug 03](../../../images/designer_debug_03.png)**Next element** – quando o breakpoint é acionado, avança para o elemento seguinte do esquema.
- **Step to out** – quando o breakpoint é acionado, avança para a saída do elemento atual; é usado para verificar os valores passados na saída do elemento.
- ![Designer Debug 04](../../../images/designer_debug_04.png)**Step in** – quando o breakpoint é acionado, entra no elemento composto. Abre automaticamente o esquema do elemento composto e para no elemento ao qual os dados são transmitidos primeiro.
- ![Designer Debug 05](../../../images/designer_debug_05.png)**Step out** – quando o breakpoint é acionado e se encontra dentro do elemento composto, sai um nível acima para onde o elemento composto aberto é usado.
- ![Designer Debug 06](../../../images/designer_debug_06.png)**Continue** – Continua até que o próximo breakpoint seja acionado.

## Conteúdo recomendado

[Break points](debugging/break_points.md)
