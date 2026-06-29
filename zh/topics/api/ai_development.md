# AI 辅助开发

现代 AI 工具可以显著加快使用 StockSharp 开发交易策略和连接器的过程。无需从头编写代码，只需用自然语言描述任务，AI 就能使用当前 API 生成可运行的代码。

## 为什么使用 AI

- **快速开始** — 几分钟内即可创建可运行的策略原型，无需耗费数小时
- **学习 API** — AI 可以展示正确的 StockSharp 方法和设计模式
- **调试** — 用自然语言描述错误并获得修复方案
- **重构** — 在保留原有逻辑的同时改进现有代码

## 适用工具

| 工具 | 类型 | 特点 |
|------|------|----------|
| [Claude Code](https://claude.com/claude-code) | CLI 代理 | 可处理完整项目、执行命令和运行测试 |
| [OpenAI Codex](https://openai.com/codex) | 云端代理 | 在沙盒环境中自主执行任务，并支持 GitHub 集成 |
| [Cursor](https://cursor.com) | IDE | 内置 AI 助手、自动补全和项目上下文感知 |
| [GitHub Copilot](https://github.com/features/copilot) | IDE 插件 | 在 Visual Studio、VS Code 和 Rider 中提供代码补全 |
| [JetBrains AI](https://www.jetbrains.com/ai/) | Rider 内置功能 | 与 Rider 和调试器原生集成 |

## 基本原则

### 1. 提供上下文

AI 获得的上下文越充分，结果就越准确。请明确说明：
- 使用了哪些 StockSharp 包
- 使用哪个连接器（交易所／经纪商）
- 策略类型（趋势跟踪、套利、短线交易）
- 限制条件（仅做多、最大持仓规模等）

### 2. 使用 CLAUDE.md / .cursorrules

在仓库根目录创建项目规则文件：

```markdown
# Project Rules

- Using StockSharp 5.x API
- Target framework: .NET 10
- Strategies inherit from Strategy
- Connectors implement MessageAdapter
- All subscriptions via Connector.Subscribe()
- Logging via this.AddInfoLog() / this.AddErrorLog()
```

### 3. 采用迭代方式

1. 概括描述任务
2. 获取第一版代码
3. 细化需求并要求改进
4. 检查代码：编译、逻辑和错误处理
5. 使用历史数据进行测试

### 4. 验证结果

AI 可能会使用过时的方法，或虚构不存在的 API。务必检查：
- 代码能否编译
- 所使用的类和方法是否存在
- 订阅和事件签名是否正确
- 错误处理是否妥当

## 章节

- [使用 AI 编写策略](ai_development/strategy_with_ai.md) — 创建交易策略的分步指南
- [使用 AI 编写连接器](ai_development/connector_with_ai.md) — 创建交易所连接器的分步指南
