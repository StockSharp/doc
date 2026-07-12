# Força bruta

Para mudar para o modo de otimização de estratégia, clique no botão **Otimização** no separador **Emulação**. O exemplo de otimização será considerado usando a estratégia SMA criada [a partir de cubos](../strategies/using_visual_designer/first_strategy.md).

![Designer Otimização 00](../../../images/designer_optimization_00.png)

Será aberto no espaço de trabalho um separador chamado Otimização + 'Nome da estratégia'. O separador **Otimização** está dividido em duas áreas, **Propriedades** e **Resultado da otimização**:

![Designer Otimização 02](../../../images/designer_optimization_02.png)

- A área **Propriedades** consiste em separadores com várias tabelas. A primeira contém os parâmetros da estratégia, que são [iterados](optimization_parameters.md). A segunda contém as definições de [Genética](genetic.md). A terceira contém as definições de sistema do otimizador. Por exemplo, aí pode alterar o número de threads e núcleos envolvidos na otimização.
- A área **Resultado da otimização** é uma tabela em que cada linha é o resultado do teste da estratégia com parâmetros únicos. Além disso, na área **Resultado da otimização**, existe uma barra de progresso que mostra o progresso da otimização, o tempo decorrido e o tempo estimado até ao fim da otimização. Adicionalmente, existe um separador para apresentar os resultados sob a forma de um [gráfico 3D](3d_chart.md).

A definição dos parâmetros para iteração resulta em mais de 1000 iterações. Depois de iniciar o otimizador, o progresso na parte superior, acima dos resultados, mostrará dados sobre o número planeado de iterações, quantas já foram concluídas e quanto tempo será aproximadamente necessário até à conclusão:

![Designer Otimização 03](../../../images/designer_optimization_03.png)

## Consulte também

[Exemplo de testes históricos](../backtesting/getting_started.md)
