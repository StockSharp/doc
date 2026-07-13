# Proteção de Posição

## Introdução

Esta modificação da estratégia SMA implementa um mecanismo para proteger posições abertas usando um controlador de proteção local. Esta abordagem permite uma gestão flexível do risco e o fecho automático de posições quando determinadas condições são cumpridas.

## Componentes Principais da Proteção de Posição

### Controlador de Proteção

A estratégia usa dois objetos principais para a proteção de posição:

```cs
// Declaração dos controladores de proteção
private readonly ProtectiveController _protectiveController = new();
private IProtectivePositionController _posController;

// Este código inicializa o controlador de proteção principal e cria um marcador
// para um controlador de posição específico. ProtectiveController gere todas as posições,
// enquanto IProtectivePositionController é responsável por uma posição específica.
```

- `_protectiveController`: O controlador principal que gere a proteção de todas as posições.
- `_posController`: Controlador de uma posição específica.

### Inicialização da Proteção

Ao abrir uma nova posição ou modificar uma existente, o controlador de proteção é inicializado:

```cs
// Inicialização do controlador de proteção para uma nova posição
this.WhenOwnTradeReceived()
	.Do(t =>
	{
		// ... (outro código)

		if (TakeValue.IsSet() || StopValue.IsSet())
		{
			_posController ??= _protectiveController.GetController(
				security.ToSecurityId(),
				portfolio.Name,
				new LocalProtectiveBehaviourFactory(security.PriceStep, security.Decimals),
				TakeValue, StopValue, true, default, default, true);
		}

		var info = _posController?.Update(t.Trade.Price, t.GetPosition());

		if (info is not null)
			ActiveProtection(info.Value);
	})
	.Apply(this);

// Este código cria e inicializa um controlador de proteção para uma nova posição
// ao receber informação sobre um novo negócio. Também atualiza a informação
// sobre a posição no controlador e ativa a proteção, se necessário.
```

Isto cria um controlador para uma posição específica com os parâmetros de take-profit e stop-loss indicados.

### Atualizar Informação da Posição

```cs
var info = _posController?.Update(t.Trade.Price, t.GetPosition());

if (info is not null)
	ActiveProtection(info.Value);
```

Isto permite ao controlador acompanhar o estado atual da posição e ajustar as ordens de proteção conforme necessário.

### Verificar Condições de Ativação da Proteção

No método que processa novos dados (por exemplo, ao receber uma nova vela), são verificadas as condições para ativar ordens de proteção:

```cs
// Verificar condições de ativação da proteção no método ProcessCandle
var info = _posController?.TryActivate(candle.ClosePrice, CurrentTime);

if (info is not null)
	ActiveProtection(info.Value);

// Este código verifica se uma ordem de proteção precisa de ser ativada com base
// no preço atual (neste caso, o preço de fecho da vela) e no tempo.
// Se as condições forem cumpridas, o método ActiveProtection é chamado.
```

Aqui, o preço de fecho da vela é usado como preço atual, mas pode ser qualquer valor de preço relevante (por exemplo, o preço do último negócio ou o spread atual no livro de ordens).

### Ativar uma Ordem de Proteção

Se as condições para ativar uma ordem de proteção forem cumpridas, a lógica correspondente é acionada:

```cs
// Método para ativar uma ordem de proteção
private void ActiveProtection((bool isTake, Sides side, decimal price, decimal volume, OrderCondition condition) info)
{
	// enviar uma ordem de proteção (de fecho de posição) como uma ordem normal
	RegisterOrder(this.CreateOrder(info.side, info.price, info.volume));
}

// Este método cria e regista uma ordem para fechar a posição
// com base na informação recebida do controlador de proteção.
```

Este método cria e regista uma ordem para fechar a posição de acordo com os parâmetros devolvidos pelo controlador de proteção.

## Comparação com Ordens Stop do Lado do Servidor

### Vantagens das Ordens Stop do Lado do Servidor

1. As ordens stop (stop loss e take profit) são enviadas diretamente para o broker.
2. O broker monitoriza de forma independente o cumprimento das condições de stop.
3. Quando um stop é acionado, o broker coloca automaticamente uma ordem de mercado ou limite.

### Vantagens da Abordagem Local

1. **Flexibilidade**: Capacidade de implementar lógica de proteção complexa indisponível nos stops padrão do lado do servidor.
2. **Confidencialidade**: A informação sobre níveis de stop não é transmitida ao broker, o que pode ser importante em alguns mercados.
3. **Velocidade de Reação**: Reação potencialmente mais rápida a alterações das condições de mercado.
4. **Adaptabilidade**: Capacidade de ajustar dinamicamente os níveis de proteção com base em dados de mercado ou na lógica da estratégia.
5. **Independência da implementação do broker/bolsa**: A abordagem local funciona da mesma forma independentemente de o broker ou a bolsa suportar todos os tipos necessários de ordens de proteção.
6. **Teste em dados históricos**: Capacidade de testar completamente a estratégia com proteção de posição em dados históricos, o que é impossível com stops do lado do servidor.

### Desvantagens da Abordagem Local

1. **Dependência da funcionalidade do terminal de negociação**: Se o terminal for desligado, a proteção não funcionará.
2. **Carga do sistema**: Requer cálculos constantes no lado do cliente.
3. **Atrasos**: Possíveis atrasos na colocação de uma ordem depois de as condições de proteção serem acionadas.

### Desvantagens das Ordens Stop do Lado do Servidor

1. **Dependência da implementação do broker/bolsa**: Nem todos os brokers ou bolsas suportam todos os tipos de ordens de proteção, o que pode limitar a funcionalidade da estratégia.
2. **Impossibilidade de testar completamente em dados históricos**: Os stops do lado do servidor não podem ser modelados com precisão ao testar em dados históricos, dificultando a avaliação da eficácia real da estratégia.
3. **Flexibilidade limitada**: Normalmente estão disponíveis apenas tipos básicos de ordens stop, limitando as possibilidades de implementar mecanismos de proteção complexos.

## Conclusão

Usar um controlador de proteção local na estratégia SMA permite uma gestão eficaz do risco de posições abertas. Esta abordagem fornece flexibilidade na definição dos parâmetros de proteção e reação rápida a alterações das situações de mercado, o que é crítico para uma negociação bem-sucedida.
