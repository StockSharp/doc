# 使用 Visual Studio 或 Rider

使用 **Visual Studio** 或 **JetBrains Rider** 编写将在 **Designer** 中运行的代码时，只能不断地执行[文件导入](../../../export_import/import.md)。这是因为 **Designer** 创建的文件采用特殊结构，即使策略使用 C# 编写，也无法直接在外部开发环境中修改这些文件。因此，如果需要使用外部程序编写代码，建议采用 [DLL 文件](../../using_dll.md)方式。
