# Gestão de Ordens

O [S#](../api.md) fornece uma ampla gama de funcionalidades para a gestão eficiente de ordens de negociação em todas as fases do seu ciclo de vida. Esta secção aborda os principais aspetos do trabalho com ordens em aplicações de negociação.

## Funcionalidades Principais

- **Criação de ordens** - formação de vários tipos de ordens de negociação (mercado, limite, stop orders, etc.)
- **Acompanhamento de estado** - receção de informação atualizada sobre o estado atual das ordens
- **Gestão de ordens** - cancelamento, modificação e substituição de ordens existentes
- **Processamento de eventos** - resposta a eventos de registo, execução e cancelamento de ordens
- **Operações em massa** - trabalho eficiente com grupos de ordens

## Ciclo de Vida da Ordem

Cada ordem no S# passa por determinadas fases do ciclo de vida:

1. **Criação** - formar um objeto [Order](xref:StockSharp.BusinessEntities.Order) com os parâmetros necessários
2. **Registo** - enviar a ordem para o sistema de negociação
3. **Execução** - execução parcial ou completa da ordem, formação de negócios
4. **Conclusão** - execução completa, cancelamento ou rejeição da ordem

A API fornece informação detalhada sobre o estado da ordem em cada fase, o que permite construir algoritmos de negociação complexos com controlo de execução preciso.

## Integração com Estratégias de Negociação

O mecanismo de gestão de ordens está estreitamente integrado com componentes para desenvolver estratégias de negociação [Strategy](xref:StockSharp.Algo.Strategies.Strategy), o que permite:

- Encapsular a lógica de gestão de ordens dentro da estratégia
- Acompanhar e processar automaticamente eventos de registo e execução de ordens
- Usar uma abordagem unificada para a gestão de ordens tanto em negociação real como durante testes

## Ver também

[Criar uma Nova Ordem](orders_management/create_new_order.md)

[Criar uma Nova Stop Order](orders_management/create_new_stop_order.md)

[Estados da Ordem](orders_management/orders_states.md)

[Cancelamento de Ordem](orders_management/order_cancel.md)

[Cancelamento em Massa de Ordens](orders_management/orders_mass_cancel.md)

[Substituição de Ordens](orders_management/orders_replacement.md)

[Número da Transação](orders_management/transaction_number.md)
