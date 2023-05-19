using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DevExpressXAFTagBoxHelper.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    
    public class EmployeeAssignemt : XPBaseObject
    {   public EmployeeAssignemt(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            
        }

        string myDescription;
        long myId;

        [Browsable(false)]
        [Key(true)]
        public long Id
        {
            get => myId;
            set => SetPropertyValue(nameof(Id), ref myId, value);
        }

        [VisibleInDashboards(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(true)]
        [CollectionOperationSet(AllowAdd = true, AllowRemove = true)]
        [Association("Assignment-SelectedEmployee", typeof(SelectedEmployee))]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<SelectedEmployee> SelectedEmployees => GetCollection<SelectedEmployee>(nameof(SelectedEmployees));

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleRequiredField]
        public string Description
        {
            get => myDescription;
            set => SetPropertyValue(nameof(Description), ref myDescription, value);
        }

    }
}