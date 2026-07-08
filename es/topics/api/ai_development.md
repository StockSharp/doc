# Desarrollo asistido por IA

Las herramientas modernas de IA pueden acelerar significativamente el desarrollo de estrategias de trading y conectores con StockSharp. En lugar de escribir código desde cero, usted describe la tarea en lenguaje natural y la IA genera código funcional utilizando la API actual.

## Por qué usar IA

- **Inicio rápido** — cree un prototipo de estrategia funcional en minutos, no en horas
- **Aprender la API** — la IA le mostrará los métodos y patrones correctos de StockSharp
- **Depuración** — describa un error en lenguaje sencillo y obtenga una solución
- **Refactorización** — mejore el código existente conservando la lógica

## Herramientas adecuadas

| Herramienta | Tipo | Características |
|------|------|----------|
| [Claude Code](https://claude.com/claude-code) | Agente CLI | Trabaja con el proyecto completo, ejecuta comandos, ejecuta pruebas |
| [OpenAI Codex](https://openai.com/codex) | Agente en la nube | Ejecución autónoma de tareas en un entorno sandbox, integración con GitHub |
| [Cursor](https://cursor.com) | IDE | Asistente de IA integrado, autocompletado, conciencia del contexto del proyecto |
| [GitHub Copilot](https://github.com/features/copilot) | Plugin de IDE | Autocompletado de código en Visual Studio, VS Code, Rider |
| [JetBrains AI](https://www.jetbrains.com/ai/) | Integrado en Rider | Integración nativa con Rider y el depurador |

## Principios generales

### 1. Proporcione contexto

Cuanto más contexto tenga la IA, más preciso será el resultado. Especifique:
- Qué paquetes de StockSharp se están utilizando
- Qué conector (bolsa/broker)
- Tipo de estrategia (seguimiento de tendencia, arbitraje, scalping)
- Restricciones (solo largo, tamaño máximo de posición, etc.)

### 2. Use CLAUDE.md / .cursorrules

Cree un archivo de reglas del proyecto en la raíz del repositorio:

```markdown
# Reglas del proyecto

- Using StockSharp 5.x API
- Target framework: .NET 10
- Strategies inherit from Strategy
- Connectors implement MessageAdapter
- All subscriptions via Connector.Subscribe()
- Logging via this.AddInfoLog() / this.AddErrorLog()
```

### 3. Enfoque iterativo

1. Describa la tarea en términos generales
2. Obtenga la primera versión del código
3. Refine los requisitos y solicite mejoras
4. Revise el código: compilación, lógica, manejo de errores
5. Pruebe con datos históricos

### 4. Verifique el resultado

La IA puede usar métodos obsoletos o inventar API inexistentes. Verifique siempre:
- Si el código compila
- Si las clases y métodos utilizados existen
- Si las firmas de suscripción y eventos son correctas
- Si el manejo de errores es adecuado

## Secciones

- [Escribir una estrategia con IA](ai_development/strategy_with_ai.md) — guía paso a paso para crear una estrategia de trading
- [Escribir un conector con IA](ai_development/connector_with_ai.md) — guía paso a paso para crear un conector de bolsa
