using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HTTP5212_HospitalProject_Team1.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        public string RoomType { get; set; }

        public int RoomNumber { get; set; }

        public bool Availability { get; set; }

        public bool RoomHasPic { get; set; }

        public string PicExtension { get; set; }

        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

    }

    public class RoomDto
    {
        [Key]
        public int RoomId { get; set; }

        public string RoomType { get; set; }

        public int RoomNumber { get; set; }

        public bool Availability { get; set; }
        public bool RoomHasPic { get; set; }
        public string PicExtension { get; set; }

        public int PatientID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}