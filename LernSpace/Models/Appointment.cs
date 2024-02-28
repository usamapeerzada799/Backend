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
    
    public partial class Appointment
    {
        public int id { get; set; }
        public int userId { get; set; }
        public Nullable<int> testId { get; set; }
        public Nullable<int> pracId { get; set; }
        public int patientId { get; set; }
        public System.DateTime appointmentDate { get; set; }
        public System.DateTime nextAppointDate { get; set; }
        public string feedback { get; set; }
    
        public virtual Patient Patient { get; set; }
        public virtual Practice Practice { get; set; }
        public virtual Test Test { get; set; }
        public virtual User User { get; set; }
    }
}
