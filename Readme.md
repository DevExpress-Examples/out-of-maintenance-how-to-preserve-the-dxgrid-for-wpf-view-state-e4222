<!-- default file list -->
*Files to look at*:

* [MainWindow.xaml](./CS/SaveWpfGridState/MainWindow.xaml) (VB: [MainWindow.xaml](./VB/SaveWpfGridState/MainWindow.xaml))
* [MainWindow.xaml.cs](./CS/SaveWpfGridState/MainWindow.xaml.cs) (VB: [MainWindow.xaml.vb](./VB/SaveWpfGridState/MainWindow.xaml.vb))
* **[RowStateHelper.cs](./CS/SaveWpfGridState/RowStateHelper.cs) (VB: [RowStateHelper.vb](./VB/SaveWpfGridState/RowStateHelper.vb))**
<!-- default file list end -->
# How to save and restore grid rows' state

<p>In this example, we implemented the <b>RowStateHelper</b> class, which allows you to save and restore the group and master rows state, selection, and the focused row.</p>

<p>Below are code snippets demonstrating how to use this class.</p>

```CS
var rowStateHelper = new RowStateHelper(grdMaster, "id");
rowStateHelper.SaveViewInfo();
// ...
rowStateHelper.LoadViewInfo();
```
```VB
Dim rowStateHelper = New RowStateHelper(grdMaster, "id")
rowStateHelper.SaveViewInfo()
' ...
rowStateHelper.LoadViewInfo()
```
