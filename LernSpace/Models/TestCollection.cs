//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LernSpace.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TestCollection
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TestCollection()
        {
            this.PatientTestCollectionFeedback = new HashSet<PatientTestCollectionFeedback>();
        }
    
        public int id { get; set; }
        public int collectId { get; set; }
        public int testId { get; set; }
        public Nullable<int> op1 { get; set; }
        public Nullable<int> op2 { get; set; }
        public Nullable<int> op3 { get; set; }
    
        public virtual Collection Collection { get; set; }
        public virtual Collection Collection1 { get; set; }
        public virtual Collection Collection2 { get; set; }
        public virtual Collection Collection3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientTestCollectionFeedback> PatientTestCollectionFeedback { get; set; }
        public virtual Test Test { get; set; }
    }
}
