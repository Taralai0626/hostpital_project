using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using HTTP5212_HospitalProject_Team1.Models;
using System.Web.Script.Serialization;


namespace HTTP5212_HospitalProject_Team1.Controllers
{
    public class EmployeeController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static EmployeeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44397/api/");
        }



        // GET: Employee/List
        public ActionResult List()
        {
            //HttpClient client = new HttpClient(){ };
            string url = "EmployeeData/ListEmployee";
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<EmployeeDto> employee = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
            //Debug.WriteLine("The number of Employees is:");
            //Debug.WriteLine(employees.Count());

            return View(employee);
        }

        // GET: Employee/Details/5
    
        public ActionResult Details(int id)
        {
            //HttpClient client = new HttpClient() { };
            string url = "EmployeeData/findEmployee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine(response.StatusCode);

          EmployeeDto selectedemployee = response.Content.ReadAsAsync<EmployeeDto>().Result;
            //Debug.WriteLine("The number of Employees is:");
            //Debug.WriteLine(employees.Count());

            return View(selectedemployee);
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

        // POST: Employee/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            string url = "EmployeeData/addemployee";

            //JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(employee);

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

            // GET: Employee/Edit/5
            public ActionResult Edit(int id)
        {
            string url = "EmployeeData/findEmployee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            EmployeeDto selectedemployee = response.Content.ReadAsAsync<EmployeeDto>().Result;
            return View(selectedemployee);
        }

        // POST: Employee/Update/5
        [Authorize]
        [HttpPost]
        public ActionResult Update(int id, Employee employee)
        {
            string url = "EmployeeData/UpdateEmployee/" + id;
            Debug.WriteLine(url + "This?");
            string jsonpayload = jss.Serialize(employee);
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

        // GET: Employee/Delete/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
             string url = "employeedata/findemployee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            EmployeeDto selectedemployee = response.Content.ReadAsAsync<EmployeeDto>().Result;
            return View(selectedemployee);
        }

        // POST: Employee/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "employeedata/deleteemployee/"+id;
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
