using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HTTP5212_HospitalProject_Team1.Models;


namespace HTTP5212_HospitalProject_Team1.Controllers
{
    public class PatientDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        
        /// <summary>
        /// Returns all patients in the system.
        /// </summary>
        /// <returns>
        /// CONTENT: all patients in the database
        /// </returns>
        /// <example>
        /// GET: api/PatientsData/Listpatient
        /// </example>

        [HttpGet]
        public IEnumerable<PatientDto> ListPatients()
        {
            List<Patient> Patients = db.Patients.ToList();
            List<PatientDto> PatientDtos = new List<PatientDto>();
            Patients.ForEach(a => PatientDtos.Add(new PatientDto()
            {
                PatientID = a.PatientID,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Address = a.Address,
                Phone = a.Phone
            }));
            return PatientDtos;
        }
        
        /// <summary>
        /// Returns patient in the system with specified id.
        /// </summary>
        /// <returns>
        /// CONTENT: patient in the system with specified id
        /// </returns>
        /// <example>
        /// GET: api/PatientsData/FindPatient/5
        /// </example>
        [ResponseType(typeof(Patient))]
        [HttpGet]
        public IHttpActionResult FindPatient(int id)
        {
            Patient Patient = db.Patients.Find(id);
            PatientDto PatientDto = new PatientDto()
            {
                PatientID = Patient.PatientID,
                FirstName = Patient.FirstName,
                LastName = Patient.LastName,
                Address = Patient.Address,
                Phone = Patient.Phone
            };
            if (Patient == null)
            {
                return NotFound();
            }

            return Ok(PatientDto);
        }
        
          /// <summary>
        /// updates patient in the system with specified id.
        /// </summary>
        /// <returns>
        /// CONTENT: update in the system with specified id
        /// </returns>
        /// <example>
        /// PUT: api/PatientsData/UpdatePatient/5
        /// </example>

        [Authorize]
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePatient(int id, Patient patient)
        {
            Debug.WriteLine("I have reached the update Patient method!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is invalid!");
                return BadRequest(ModelState);
            }

            if (id != patient.PatientID)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter " + id);
                Debug.WriteLine("POST parameter " + patient.PatientID);
                return BadRequest();
            }

            db.Entry(patient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    Debug.WriteLine("Patient not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Debug.WriteLine("None of the conditions triggered");

            return StatusCode(HttpStatusCode.NoContent);
        }
        
           /// <summary>
        /// Adds patient in the database.
        /// </summary>
        /// <returns>
        /// CONTENT: Adds patient in the database
        /// </returns>
        /// <example>
        /// POST: api/PatientsData/AddPatient
        /// </example>

        [Authorize]
        [ResponseType(typeof(Patient))]
        [HttpPost]
        public IHttpActionResult AddPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Patients.Add(patient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patient.PatientID }, patient);
        }
        
         /// <summary>
        /// Deletes patient in the database with specified id.
        /// </summary>
        /// <returns>
        /// CONTENT: delete patient in the database with specified id.
        /// </returns>
        /// <example>
        /// DELETE: api/PatientsData/DeletePatient/5
        /// </example>

        [Authorize]
        [ResponseType(typeof(Patient))]
        public IHttpActionResult DeletePatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(patient);
            db.SaveChanges();

            return Ok(patient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.PatientID == id) > 0;
        }
    }
}
