using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpressXAFTagBoxHelper.Module.Extensions;
using System.ComponentModel;

namespace DevExpressXAFTagBoxHelper.Blazor.Server.Editors.TagBoxEditorHelper
{
    public class ISTagBoxEditorBinding<T> : IDisposable where T : class
    {
        ISBindingList<ISTagBoxEditorDataItem<T>> myChoosenItemsBindingList;
        ISBindingList<ISTagBoxEditorDataItem<T>> myDataBindingList;
        DxTagBoxAdapter myEditorAdapter;

        private bool myIsUpdating = false;
        IList<ISTagBoxEditorDataItem<T>> mySelectedValues;

        public ISTagBoxEditorBinding() { CreateChoosenItemsBindingList(); }


        public ISTagBoxEditorBinding(
            IList<ISTagBoxEditorDataItem<T>> predefinedValues,
            IList<ISTagBoxEditorDataItem<T>> selectedValues = null)
        {
            if((predefinedValues == null) || (predefinedValues.Count == 0))
            {
                return;
            }

            BeginUpdate();
            mySelectedValues = selectedValues;

            myDataBindingList = new();

            AddPredefinedValuesToDataBindingList(predefinedValues);
            CreateChoosenItemsBindingList();
            AddSelectedValuesThoChoosenItemsBindingList();

            EndUpdate();
        }

        public delegate void DataItemAddedDelegate(ISTagBoxEditorDataItem<T> addedItem);
        public delegate void DataItemRemovedDelegate(ISTagBoxEditorDataItem<T> deletedItem);

        public event DataItemAddedDelegate DataItemAdded;

        public event DataItemRemovedDelegate DataItemRemoved;

        private void AddPredefinedValuesToDataBindingList(IList<ISTagBoxEditorDataItem<T>> predefinedValues)
        {
            foreach(var locPredefinedValue in predefinedValues)
            {
                myDataBindingList.Add(locPredefinedValue);
            }
        }

        private void AddSelectedValuesThoChoosenItemsBindingList()
        {
            if(mySelectedValues != null)
            {
                foreach(var locSelectedValue in mySelectedValues)
                {
                    myChoosenItemsBindingList.Add(locSelectedValue);
                }
            }
        }


        private void ChoosenItemsBindingListChanged(object sender, ListChangedEventArgs e)
        {
            if(e.ListChangedType == ListChangedType.ItemAdded)
            {
                if(DataItemAdded != null)
                {
                    ISTagBoxEditorDataItem<T> locCurrentItem = myChoosenItemsBindingList.ElementAt(e.NewIndex);
                    DataItemAdded(locCurrentItem);
                }
                SetValues();
            } else if(e.ListChangedType == ListChangedType.ItemDeleted)
            {
                SetValues();
            }
        }


        private void ChoosenItemsBindingListRemoved(ISTagBoxEditorDataItem<T> deletedItem)
        {
            if(DataItemRemoved != null)
                DataItemRemoved(deletedItem);
        }


        private void CreateChoosenItemsBindingList()
        {
            myChoosenItemsBindingList = new();
            myChoosenItemsBindingList.ListChanged += ChoosenItemsBindingListChanged;
            myChoosenItemsBindingList.Removed += ChoosenItemsBindingListRemoved;
        }


        private void DataBindingListChanged(object sender, ListChangedEventArgs e)
        {
            if(e.ListChangedType == ListChangedType.ItemAdded)
            {
                ISTagBoxEditorDataItem<T> locCurrentItem = myDataBindingList.ElementAt(e.NewIndex);
                DataItemAdded?.Invoke(locCurrentItem);
            }
        }

        private void DataBindingListRemoved(ISTagBoxEditorDataItem<T> deletedItem)
        { DataItemRemoved?.Invoke(deletedItem); }

        private void EditorValueChanged(object sender, EventArgs e)
        {
            var locEditorValues = EditorValues().ToList();

            IEnumerable<string> locDifferences = myChoosenItemsBindingList.Select(
                locChoosenItem => locChoosenItem.Key.ToString())
                .ToList()
                .Except(locEditorValues);

            foreach(var locItemToDelete in locDifferences.ToList())
            {
                var locItemToRemove = myChoosenItemsBindingList.Where(
                    locChoosenItem => locChoosenItem.Key.ToString() == locItemToDelete)
                    .FirstOrDefault();
                if(locItemToRemove != null)
                    myChoosenItemsBindingList.Remove(locItemToRemove);
            }

            foreach(var locItemToAdd in locEditorValues)
            {
                var locFoundItem = myChoosenItemsBindingList.Where(
                    locDataBindingList => locDataBindingList.Key.ToString() == locItemToAdd)
                    .FirstOrDefault();

                if(locFoundItem == null)
                {
                    try
                    {
                        var locGenericType = (T)Convert.ChangeType(locItemToAdd, typeof(T));
                        myChoosenItemsBindingList.Add(new(locGenericType, locItemToAdd, string.Empty));
                    } catch(Exception ex)
                    {
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
        }

        private DxTagBoxModel<ISTagBoxEditorDataItem<T>, string> GetEditorAdapterComponentModel()
        { return ((DxTagBoxModel<ISTagBoxEditorDataItem<T>, string>)myEditorAdapter.ComponentModel); }

        private void SetData()
        {
            if(myEditorAdapter != null && myIsUpdating == false)
            {
                GetEditorAdapterComponentModel().Data = myDataBindingList;
                GetEditorAdapterComponentModel().ValueFieldName = nameof(ISTagBoxEditorDataItem<T>.Key);
                GetEditorAdapterComponentModel().TextFieldName = nameof(ISTagBoxEditorDataItem<T>.DisplayText);
                GetEditorAdapterComponentModel().AllowCustomTags = true;
            }
        }

        private void SetValues()
        {
            if(myEditorAdapter != null && myIsUpdating == false)

            {
                var locChoosenItems = (from myDataItem in myDataBindingList
                    where myChoosenItemsBindingList.Any(locChoosenItem => locChoosenItem.Key == myDataItem.Key)
                    select myDataItem).ToList();
                GetEditorAdapterComponentModel().Values = locChoosenItems.Select(
                    locChoosenItem => locChoosenItem.Key.ToString())
                    .ToList();

                GetEditorAdapterComponentModel().Tags = locChoosenItems.Select(
                    locChoosenItem => locChoosenItem.DisplayText)
                    .ToList();
            }
        }

        public void AddDataItem(ISTagBoxEditorDataItem<T> itemToAdd)
        {
            if(myDataBindingList.Contains(itemToAdd) == false)
            {
                myDataBindingList.Add(itemToAdd);
                SetData();
            }
        }

        public void AddItemToChoose(ISTagBoxEditorDataItem<T> valueToAdd)
        {
            if(myChoosenItemsBindingList.Contains(valueToAdd) == false)
            {
                myChoosenItemsBindingList.Add(valueToAdd);
                SetValues();
            }
        }


        public void BeginUpdate() { myIsUpdating = true; }

        public void Dispose()
        {
            if(myEditorAdapter != null)
            {
                myEditorAdapter.ValueChanged -= EditorValueChanged;
                myEditorAdapter = null;
            }

            myChoosenItemsBindingList.ListChanged -= ChoosenItemsBindingListChanged;
            myChoosenItemsBindingList.Clear();

            if(myDataBindingList != null)
            {
                myDataBindingList.Removed -= DataBindingListRemoved;
                myDataBindingList.ListChanged -= DataBindingListChanged;
                myDataBindingList.Clear();
            }
        }

        public void Editor(ISTagBoxPropertyEditor tagBoxPropertyEditor)
        {
            if(myEditorAdapter == null)
            {
                myEditorAdapter = (DxTagBoxAdapter)tagBoxPropertyEditor.Control;

                if(myDataBindingList == null)
                {
                    myDataBindingList = new();
                    foreach(var locTagBoxEditorDataItem in EditorData())
                    {
                        myDataBindingList.Add(locTagBoxEditorDataItem);
                    }
                } else
                    SetData();


                SetValues();

                myDataBindingList.Removed += DataBindingListRemoved;
                myDataBindingList.ListChanged += DataBindingListChanged;

                myEditorAdapter.ValueChanged += EditorValueChanged;
            }
        }

        public IEnumerable<ISTagBoxEditorDataItem<T>> EditorData()
        {
            if(myEditorAdapter != null)
            {
                var locDataValues = GetEditorAdapterComponentModel().Data.ToList();
                foreach(var locTagBoxEditorDataItem in locDataValues)
                {
                    yield return new ISTagBoxEditorDataItem<T>(
                        locTagBoxEditorDataItem.Key,
                        locTagBoxEditorDataItem.Value,
                        locTagBoxEditorDataItem.DisplayText);
                }
            }
        }


        public IEnumerable<string> EditorValues()
        {
            if(myEditorAdapter != null)
            {
                var locCurrentValues = GetEditorAdapterComponentModel().Values;
                foreach(var locItem in locCurrentValues)
                {
                    yield return locItem;
                }
            }
        }

        public void EndUpdate()
        {
            myIsUpdating = false;
            SetData();
            SetValues();
        }

        public void RemoveChoosenItem(ISTagBoxEditorDataItem<T> itemToRemove)
        {
            if(myChoosenItemsBindingList.Any(locChoosenItem => locChoosenItem.Key == itemToRemove.Key))
            {
                var locItemToRemove = myChoosenItemsBindingList.Where(
                    locChoosenItem => locChoosenItem.Key == itemToRemove.Key)
                    .Single();
                myChoosenItemsBindingList.Remove(locItemToRemove);
                SetValues();
            }
        }

        public void RemoveDataItem(ISTagBoxEditorDataItem<T> itemToRemove)
        {
            if(myDataBindingList.Any(locData => locData.Key == itemToRemove.Key))
            {
                var locItemToRemove = myDataBindingList.Where(locData => locData.Key == itemToRemove.Key).Single();
                myDataBindingList.Remove(locItemToRemove);
                SetData();
            }
        }
    }
}
