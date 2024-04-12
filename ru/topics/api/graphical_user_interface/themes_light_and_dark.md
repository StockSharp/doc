# Темы графических компонентов S\#

Для всех графических элементов S\# существует несколько разных тем. Ниже представлены две наиболее популярные темы.

![API GUI Thems 01](../../../images/api_gui_thems_01.png)

![API GUI Thems 02](../../../images/api_gui_thems_02.png)

Для установки темы приложения достаточно написать одну строчку. Например, для установки темной темы VisualStudio 2017 необходимо задать строчку:

```cs
...                 
ApplicationThemeHelper.ApplicationThemeName = Theme.VS2017DarkName;
...
```

Так как все графические элементы S\# базируются на графических элементах **DevExpress**, то к проекту необходимо добавить соответствующие библиотеки **DevExpress** (**DevExpress.Xpf.Core**, **DevExpress.Xpf.Themes.VS2017Dark** и т.д.)
