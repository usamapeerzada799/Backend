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
    
    public partial class PersonTest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PersonTest()
        {
            this.PersonIdentification = new HashSet<PersonIdentification>();
            this.PersonTestCollection = new HashSet<PersonTestCollection>();
        }
    
        public int id { get; set; }
        public string title { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<int> patientId { get; set; }
    
        public virtual Patient Patient { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonIdentification> PersonIdentification { get; set; }
        public virtual PersonTest PersonTest1 { get; set; }
        public virtual PersonTest PersonTest2 { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonTestCollection> PersonTestCollection { get; set; }
    }
}
