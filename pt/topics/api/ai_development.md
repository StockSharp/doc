# Desenvolvimento Assistido por IA

As ferramentas modernas de IA podem acelerar significativamente o desenvolvimento de estratégias de negociação e conectores usando o StockSharp. Em vez de escrever código do zero, você descreve a tarefa em linguagem natural e a IA gera código funcional usando a API atual.

## Por que usar IA

- **Início rápido** — crie um protótipo de estratégia funcional em minutos, não horas
- **Aprenda a API** — a IA mostrará os métodos e padrões corretos do StockSharp
- **Depuração** — descreva um erro em linguagem simples e obtenha uma correção
- **Refatoração** — melhore o código existente preservando a lógica

## Ferramentas Adequadas

| Ferramenta | Tipo | Recursos |
|------|------|----------|
| [Claude Code](https://claude.com/claude-code) | Agente CLI | Trabalha com o projeto completo, executa comandos, roda testes |
| [OpenAI Codex](https://openai.com/codex) | Agente em nuvem | Execução autônoma de tarefas em ambiente sandbox, integração com GitHub |
| [Cursor](https://cursor.com) | IDE | Assistente de IA integrado, autocompletar, consciência do contexto do projeto |
| [GitHub Copilot](https://github.com/features/copilot) | Plugin de IDE | Completar código no Visual Studio, VS Code, Rider |
| [JetBrains AI](https://www.jetbrains.com/ai/) | Integrado ao Rider | Integração nativa com o Rider e o depurador |

## Princípios Gerais

### 1. Forneça Contexto

Quanto mais contexto a IA tiver, mais preciso será o resultado. Especifique:
- Quais pacotes do StockSharp estão sendo usados
- Qual conector (bolsa/corretora)
- Tipo de estratégia (seguimento de tendência, arbitragem, scalping)
- Restrições (apenas long, tamanho máximo de posição, etc.)

### 2. Usar CLAUDE.md / .cursorrules

Crie um arquivo de regras do projeto na raiz do repositório:

```markdown
# Regras do projeto

- Usar a API StockSharp 5.x
- Framework de destino: .NET 10
- As estratégias herdam de Strategy
- Os conectores implementam MessageAdapter
- Todas as subscrições via Connector.Subscribe()
- Registo de logs via this.AddInfoLog() / this.AddErrorLog()
```

### 3. Abordagem Iterativa

1. Descreva a tarefa em termos gerais
2. Obtenha a primeira versão do código
3. Refine os requisitos e peça melhorias
4. Revise o código: compilação, lógica, tratamento de erros
5. Teste com dados históricos

### 4. Verifique o Resultado

A IA pode usar métodos desatualizados ou inventar APIs inexistentes. Sempre verifique:
- Se o código compila
- Se as classes e métodos usados existem
- Se as assinaturas de assinatura (subscription) e evento estão corretas
- Se o tratamento de erros é adequado

## Seções

- [Escrevendo uma Estratégia com IA](ai_development/strategy_with_ai.md) — guia passo a passo para criar uma estratégia de negociação
- [Escrevendo um Conector com IA](ai_development/connector_with_ai.md) — guia passo a passo para criar um conector de bolsa
