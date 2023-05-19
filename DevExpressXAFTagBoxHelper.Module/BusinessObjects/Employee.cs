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
    [ImageName("BO_Contact")]
    [DefaultProperty(nameof(FullName))]
    
    public class Employee : XPBaseObject
    {
        public Employee(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
                
        }


        string myLastName;
        string myFirstName;
        long myId;

        [Browsable(false)]
        [Key(true)]
        public long Id
        {
            get => myId;
            set => SetPropertyValue(nameof(Id), ref myId, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleRequiredField]
        public string FirstName
        {
            get => myFirstName;
            set => SetPropertyValue(nameof(FirstName), ref myFirstName, value);
        }
        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleRequiredField]
        public string LastName
        {
            get => myLastName;
            set => SetPropertyValue(nameof(LastName), ref myLastName, value);
        }

        public string FullName { get => ObjectFormatter.Format($"{LastName}, {FirstName}", this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty); }    


    }
}