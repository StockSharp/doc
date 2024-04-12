# Использование Visual Studio или Rider

При использовании **Visual Studio** и **JetBrains Rider** для создания кода, запускаемого в **Дизайнере** возможен вариант только с постоянным [импортированием файлов](../../export_import/import.md). Это связано с тем, что изменять файлы, создаваемые **Дизайнером**, напрямую невозможно из-за особой структуры файлов (даже в случае стратегий, созданных на C#). Поэтому, если требуется наличие внешних программ для написания кода, рекомендуется использовать подход с [dll файлами](../using_dll.md).