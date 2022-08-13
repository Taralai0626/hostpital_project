using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTTP5212_HospitalProject_Team1.Models
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        //---NO NEED FOREIGN KEY
        //one patient can have multiple responsile employee (employee.cs have patient as foreign key)
        //one patient can have multiple appointment (appointment.cs have patient as foreign key)
        //one patient can have multiple perscription (perscription.cs have patient as foreign key)
        //one patient can have one room (foreign key is already made in room.cs)

    }
    public class PatientDto
    {
        public int PatientID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
    }


}