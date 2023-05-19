using DevExpress.ExpressApp;
using DevExpress.XtraRichEdit.Import.Html;
using DevExpressXAFTagBoxHelper.Blazor.Server.Editors;
using DevExpressXAFTagBoxHelper.Blazor.Server.Editors.TagBoxEditorHelper;
using DevExpressXAFTagBoxHelper.Module.BusinessObjects;
using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace DevExpressXAFTagBoxHelper.Blazor.Server.Controllers
{
    public partial class EmployeeAssignmentController : ObjectViewController<DetailView, EmployeeAssignemt>
    {
        List<ISTagBoxEditorDataItem<string>> myAllAvailableEmployees;
        List<ISTagBoxEditorDataItem<string>> mySelectedEmployees;
        ISTagBoxEditorBinding<string> myTagBoxEditor;


        public EmployeeAssignmentController() { InitializeComponent(); }

        private void DeactivateEditors()
        {
            if(myTagBoxEditor != null)
            {
                myTagBoxEditor.DataItemAdded -= TagBoxEditor_DataItemAdded;
                myTagBoxEditor.DataItemRemoved -= TagBoxEditor_DataItemRemoved;
            }
        }

        private void LoadData()
        {
            if(myAllAvailableEmployees == null)
            {
                myAllAvailableEmployees = ObjectSpace.GetObjects<Employee>()
                    .OrderBy(locEmployee => locEmployee.FullName)
                    .Select(
                        locEmployee => new ISTagBoxEditorDataItem<string>().TryConvertObjetToDataItem(
                            locEmployee,
                            locEmployee.FullName))
                    .ToList();
            }

            mySelectedEmployees = ViewCurrentObject.SelectedEmployees
                .Select(
                    locEmployee => new ISTagBoxEditorDataItem<string>().TryConvertObjetToDataItem(
                        locEmployee,
                        nameof(locEmployee.EmployeeId),
                        string.Empty))
                .ToList();

            myTagBoxEditor = new(myAllAvailableEmployees, mySelectedEmployees);
            myTagBoxEditor.DataItemAdded += TagBoxEditor_DataItemAdded;
            myTagBoxEditor.DataItemRemoved += TagBoxEditor_DataItemRemoved;


            View.CustomizeViewItemControl<ISTagBoxPropertyEditor>(
                this,
                myTagBoxEditor.Editor,
                nameof(EmployeeAssignemt.SelectedEmployees));
        }

        private void ObjectSpace_Reloaded(object sender, EventArgs e)
        {
            ViewCurrentObject.Reload();
            DeactivateEditors();
            LoadData();
        }

        private void TagBoxEditor_DataItemAdded(ISTagBoxEditorDataItem<string> addedItem)
        {
            var locId = addedItem.Key;
            var locEmployeeToAssign = ObjectSpace.GetObjects<Employee>()
                .Where(locEmployee => locEmployee.Id.ToString() == locId)
                .SingleOrDefault();
            if(locEmployeeToAssign != null)
            {
                var locEmployeeToAdd = new SelectedEmployee(ViewCurrentObject.Session)
                {
                    EmployeeId = locEmployeeToAssign.Id,
                    EmployeeAssignemt = this.ViewCurrentObject
                };
                ViewCurrentObject.SelectedEmployees.Add(locEmployeeToAdd);
                ObjectSpace.SetModified(locEmployeeToAdd);
            }
        }

        private void TagBoxEditor_DataItemRemoved(ISTagBoxEditorDataItem<string> deletedItem)
        {
            var locId = deletedItem.Key;
            var locAssignedEmployee = ViewCurrentObject.SelectedEmployees
                .Where(
                    locSelectedEmplyoee => locSelectedEmplyoee.EmployeeId.ToString() == locId &&
                        locSelectedEmplyoee.EmployeeAssignemt == this.ViewCurrentObject)
                .SingleOrDefault();
            if(locAssignedEmployee != null)
            {
                ViewCurrentObject.SelectedEmployees.Remove(locAssignedEmployee);
                ObjectSpace.SetModified(locAssignedEmployee);
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            LoadData();
            ObjectSpace.Reloaded += ObjectSpace_Reloaded;
        }

        protected override void OnDeactivated()
        {
            DeactivateEditors();
            ObjectSpace.Reloaded -= ObjectSpace_Reloaded;
            base.OnDeactivated();
        }


        protected override void OnViewControlsCreated() { base.OnViewControlsCreated(); }
    }
}
