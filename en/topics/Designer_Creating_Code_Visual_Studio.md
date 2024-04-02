# Using Visual Studio or Rider

When using **Visual Studio** and **JetBrains Rider** to create code that will run in **Designer**, the only option is constant [file importing](Designer_Import_strategies.md). This is due to the special file structure of files created by **Designer**, making it impossible to directly modify the files, even in the case of strategies created in C#. Therefore, if external programs are needed for writing code, it is recommended to use the approach with [dll files](Designer_Creating_strategy_from_dll.md).
