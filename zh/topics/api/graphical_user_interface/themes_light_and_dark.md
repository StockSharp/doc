# S# 图形组件的主题

对于所有 S# 图形元素，都有几种不同的主题。下面是两个最受欢迎的主题。

![API GUI 主题 01](../../../images/api_gui_thems_01.png)

![API GUI 主题 02](../../../images/api_gui_thems_02.png)

要安装应用程序主题，只需写一行代码。例如，要设置 VisualStudio 2017 暗色主题，您需要指定以下代码行：

```cs
...
ThemeExtensions.ApplyDefaultTheme();
...
```

由于所有 S# 图形元素都是基于 **DevExpress** 图形元素的，因此您需要添加相应的 **DevExpress** 库（**DevExpress.Xpf.Core**、**DevExpress.Xpf.Themes.VS2017Dark** 等）。
