# DevExpress XAF TagBoxHelper

This project was created to help people doing complex work with TagBoxes in DevExpress XAF Blazor

There is a very good (original) apporach by DevExpress, how to work with TagBoxes:
https://github.com/DevExpress-Examples/XAF-Blazor-How-to-use-a-TagBox-to-view-and-edit-a-collection-property-in-Detail-Views

# Why to create an other aproach?
I found out, that the original aproach, used by DevExpress in this example does not work with more complex scenarios.
For example, when multiple Tagboxes are needed by a single view. Or Tagboxes rely on each other.

# What is needed?
You need to use three 3 files to get the setup
1) In the Module project, under "Extensions", there is a file called ISBIndingLinst.cs
This file extends the System.Componentmodel.BindingList<T> to remove data
2) In the Blazor project, under "Editors\TagBoxEditorHelper" there are two files.
The first file: "ISTagBoxEditorDataItem.cs" is the used DataItem, which is influenced by the DataItem, which is used by DevExpress
The second file: "ISTagBoxEditorBinding.cs" is doing all the binding "magic"
Here, all the data get's controlled
3) On the root of the Editors folder, there is the file "ISTagBoxPropertyEditor.cs", which is the Property Editor itself.
  
 # Implementation steps
 1) Copy the above files into your project.
 2) Create an ObjectViewController for DetailView of your Business Object, where the TagBox(es) are on.
 3) Create two lists of ISTagBoxEditorDataItem(s), one list holds all available items, the second list holds all the selected items
 4) Bind the lists to the ISTagBoxEditorBinding and expose the event handler
 5) Set the "View.CustomizeViewItemControl" to use the ISTagBoxPropertyEditor and the parameters to the list, where the selected items get stored.
 6) Set the PropertyEditor in the Model to the ISTagBoxPropertyEditor
  
  # The events
  There are two events, which get fired, when something happens
  1) DataItemAdded Event 
  This event get's fired, when an item get's added to the selected list. (here you add the code to add data to the Object store)
  And you can also add relied data to the selectable items.
  2)  DataItemRemoved Event
  This event get's fired, when an item get's removed from the selected list (here you add the code to remove data from the Object store)
  
  Imagine, that you have for example a list of suppliers and each supplier holds different brands, then it is possible to add all brands to a second 
  TagBox, based on the chosen value of an other TagBox.
  
  
  # Others
  Don't forget to look at my other project to use type safe localization of XAF Applications:
  https://github.com/Intelli-Soft/XAFCodeGenCustomLocalization
  
  I would be happy to get a pizza.... https://www.buymeacoffee.com/IntelliSoft

