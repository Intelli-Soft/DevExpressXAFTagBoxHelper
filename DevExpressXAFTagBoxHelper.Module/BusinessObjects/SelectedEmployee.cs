
using DevExpress.Xpo;
using System.ComponentModel;


namespace DevExpressXAFTagBoxHelper.Module.BusinessObjects
{
    
    public class SelectedEmployee : XPBaseObject
    { 
        public SelectedEmployee(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
      
        }


        long myEmployeeId;
        long myId;
        EmployeeAssignemt myEmployeeAssignment;

        [Browsable(false)]
        [Key(true)]
        public long Id
        {
            get => myId;
            set => SetPropertyValue(nameof(Id), ref myId, value);
        }

        [Browsable(false)]
        public long EmployeeId
        {
            get => myEmployeeId;
            set => SetPropertyValue(nameof(EmployeeId), ref myEmployeeId, value);
        }

        [Association("Assignment-SelectedEmployee")]
        public EmployeeAssignemt EmployeeAssignemt
        {
            get => myEmployeeAssignment;
            set => SetPropertyValue(nameof(EmployeeAssignemt), ref myEmployeeAssignment, value);
        }

    }
}