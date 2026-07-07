# Instrumentos de Trabalho

## Descrição

O método `GetWorkingSecurities()` na classe base `Strategy` é usado para obter uma lista de instrumentos e tipos de dados que a estratégia usa na sua operação. Este método desempenha um papel importante ao trabalhar com o [Designer](../../designer.md).

## Finalidade

A principal finalidade do método é fornecer ao Designer informação sobre quais os instrumentos e tipos de dados necessários para a estratégia funcionar. Isto permite ao Designer:

1. Verificar a disponibilidade dos dados históricos necessários no armazenamento antes de iniciar os testes
2. Carregar automaticamente os dados necessários quando disponíveis
3. Configurar corretamente as subscrições ao iniciar a estratégia

## Implementação

Na classe base `Strategy`, o método devolve uma coleção vazia. Para trabalhar corretamente com o Designer, recomenda-se substituí-lo na sua estratégia:

```cs
public override IEnumerable<(Security sec, DataType dt)> GetWorkingSecurities()
{
	// Devolver uma lista de pares (instrumento, tipo de dados) usados pela estratégia
	return new[] 
	{ 
		(Security, CandleType),
		// Outros pares instrumento-tipo de dados se a estratégia usar vários
	};
}
```

## Importância de Substituir o Método

Se o método `GetWorkingSecurities()` não for substituído na sua estratégia:

- O Designer não conseguirá verificar automaticamente os dados necessários
- Se os dados históricos necessários estiverem ausentes no armazenamento, o Designer não emitirá avisos
- A estratégia pode ser iniciada para teste, mas não serão apresentados resultados
- O utilizador não receberá qualquer informação sobre o motivo da ausência de resultados

## Exemplo de Utilização

```cs
public class MySmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleType;
	
	public DataType CandleType
	{
		get => _candleType.Value;
		set => _candleType.Value = value;
	}
	
	public MySmaStrategy()
	{
		_candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
	}
	
	// Substituir o método para trabalhar corretamente com o Designer
	public override IEnumerable<(Security sec, DataType dt)> GetWorkingSecurities()
	{
		return new[] { (Security, CandleType) };
	}
	
	// O resto do código da estratégia...
}
```

## Conclusão

Embora o método `GetWorkingSecurities()` não seja obrigatório para implementar a funcionalidade básica de uma estratégia, substituí-lo é fortemente recomendado para trabalhar corretamente com o StockSharp Designer. Isto ajuda a evitar situações em que uma estratégia é iniciada para teste mas não apresenta resultados devido à ausência dos dados históricos necessários.

