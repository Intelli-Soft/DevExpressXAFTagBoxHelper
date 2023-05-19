using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpressXAFTagBoxHelper.Blazor.Server.Editors.TagBoxEditorHelper;
using System.Collections;

namespace DevExpressXAFTagBoxHelper.Blazor.Server.Editors
{
    [PropertyEditor(typeof(IList), nameof(ISTagBoxPropertyEditor), false)]
    public class ISTagBoxPropertyEditor : BlazorPropertyEditorBase
    {
        public ISTagBoxPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model)
        {
        }

        protected override IComponentAdapter CreateComponentAdapter()
        {
            var locComponentModel = new DxTagBoxModel<ISTagBoxEditorDataItem<string>, string>();
            return new DxTagBoxAdapter<ISTagBoxEditorDataItem<string>, string>(locComponentModel);
        }

        protected override bool IsMemberSetterRequired() => false;


        protected override void ReadValueCore()
        {
            /// This empty override void is important, that the built in logic does not try to read the data by it's own
        }

        protected override void WriteValueCore()
        {
            /// This empty override void is important, that the built in logic does not try to set the data by it's own
        }
    }
}
