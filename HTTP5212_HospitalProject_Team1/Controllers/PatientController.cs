using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTP5212_HospitalProject_Team1.Models;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.Net.Http;


namespace HTTP5212_HospitalProject_Team1.Controllers
{
    public class PatientController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PatientController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44397/api/");
        }


        // GET: Patient/List
        public ActionResult List()
        {
            
            string url = "PAtientData/ListPatient";
            HttpResponseMessage response = client.GetAsync(url).Result;
           

            IEnumerable<PatientDto> employee = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;
           

            return View(employee);
        }

        // GET: Patient/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            //HttpClient client = new HttpClient() { };
            string url = "PatientData/findPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine(response.StatusCode);

            PatientDto selectedpatient = response.Content.ReadAsAsync<PatientDto>().Result;
            //Debug.WriteLine("The number of Employees is:");
            //Debug.WriteLine(employees.Count());

            return View(selectedpatient);
        }

        public ActionResult Error()
        {

            return View();
        }


        // GET: Employee/New
        public ActionResult New()
        {

            return View();
        }


        // GET: Patient/Create
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            string url = "PatientData/adpatient";

            //JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(patient);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Patient/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = "PatientData/findPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto selectedpatient = response.Content.ReadAsAsync<PatientDto>().Result;
            return View(selectedpatient);
        }

        // POST: Patient/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Update(int id, Patient patient)
        {
            string url = "PatientData/UpdatePatient/" + id;
            Debug.WriteLine(url + "This?");
            string jsonpayload = jss.Serialize(patient);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(url);
            Debug.WriteLine(content);
            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Patient/Delete/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Patientdata/findPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto selectedemployee = response.Content.ReadAsAsync<PatientDto>().Result;
            return View(selectedemployee);
        }

        // POST: Patient/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "patientdata/deletepatient/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
