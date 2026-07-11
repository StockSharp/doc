# Themes der grafischen S#-Komponenten

Für alle grafischen S#-Elemente gibt es mehrere unterschiedliche Themes. Unten sind die zwei beliebtesten Themes dargestellt.

![API GUI Themen 01](../../../images/api_gui_thems_01.png)

![API GUI Themen 02](../../../images/api_gui_thems_02.png)

Um das Anwendungstheme zu installieren, reicht eine Zeile. Um beispielsweise das dunkle Theme VisualStudio 2017 festzulegen, müssen Sie die folgende Zeile angeben:

```cs
...
ThemeExtensions.ApplyDefaultTheme();
...
```

Da alle grafischen S#-Elemente auf grafischen **DevExpress**-Elementen basieren, müssen Sie die entsprechenden **DevExpress**-Bibliotheken hinzufügen (**DevExpress.Xpf.Core**, **DevExpress.Xpf.Themes.VS2017Dark** usw.)
