# AI-Assisted Development

Modern AI tools can significantly accelerate the development of trading strategies and connectors using StockSharp. Instead of writing code from scratch, you describe the task in natural language and the AI generates working code using the current API.

## Why Use AI

- **Quick start** — create a working strategy prototype in minutes, not hours
- **Learn the API** — the AI will show you the correct StockSharp methods and patterns
- **Debugging** — describe an error in plain language, get a fix
- **Refactoring** — improve existing code while preserving logic

## Suitable Tools

| Tool | Type | Features |
|------|------|----------|
| [Claude Code](https://claude.com/claude-code) | CLI agent | Works with the full project, executes commands, runs tests |
| [OpenAI Codex](https://openai.com/codex) | Cloud agent | Autonomous task execution in sandbox environment, GitHub integration |
| [Cursor](https://cursor.com) | IDE | Built-in AI assistant, auto-completion, project context awareness |
| [GitHub Copilot](https://github.com/features/copilot) | IDE plugin | Code completion in Visual Studio, VS Code, Rider |
| [JetBrains AI](https://www.jetbrains.com/ai/) | Built into Rider | Native integration with Rider and the debugger |

## General Principles

### 1. Provide Context

The more context the AI has, the more accurate the result. Specify:
- Which StockSharp packages are being used
- Which connector (exchange/broker)
- Strategy type (trend-following, arbitrage, scalping)
- Constraints (long only, maximum position size, etc.)

### 2. Use CLAUDE.md / .cursorrules

Create a project rules file in the repository root:

```markdown
# Project Rules

- Using StockSharp 5.x API
- Target framework: .NET 10
- Strategies inherit from Strategy
- Connectors implement MessageAdapter
- All subscriptions via Connector.Subscribe()
- Logging via this.AddInfoLog() / this.AddErrorLog()
```

### 3. Iterative Approach

1. Describe the task in broad terms
2. Get the first version of the code
3. Refine requirements and ask for improvements
4. Review the code: compilation, logic, error handling
5. Test on historical data

### 4. Verify the Result

AI can use outdated methods or invent non-existent APIs. Always verify:
- Whether the code compiles
- Whether the used classes and methods exist
- Whether subscription and event signatures are correct
- Whether error handling is proper

## Sections

- [Writing a Strategy with AI](ai_development/strategy_with_ai.md) — step-by-step guide to creating a trading strategy
- [Writing a Connector with AI](ai_development/connector_with_ai.md) — step-by-step guide to creating an exchange connector
