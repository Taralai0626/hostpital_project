using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using HTTP5212_HospitalProject_Team1.Models;
using System.Web.Script.Serialization;
using Microsoft.AspNetCore.Authorization;
using HTTP5212_HospitalProject_Team1.Models.ViewModels;

namespace HTTP5212_HospitalProject_Team1.Controllers
{
    public class ShiftController : Controller
    {

        public static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ShiftController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44397/api/");
        }




        // GET: Shift/List
        public ActionResult List()
        {
            //retrieve a list of shifts from shiftdatacontroller
            //curl https://localhost:44397/api/ShiftData/ListShifts


            string url = "ShiftData/ListShifts";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode); 

            IEnumerable<ShiftDto> shifts = response.Content.ReadAsAsync<IEnumerable<ShiftDto>>().Result;
           // Debug.WriteLine("Number of shifts receieved: ");
            //Debug.WriteLine(shifts.Count());

            return View(shifts);
        }
    
        // GET: Shift/Details/5
        public ActionResult Details(int id)
        {
            //retrieve one shift from shiftdatacontroller
            //curl https://localhost:44397/api/ShiftData/FindShift/{id}

            string url = "ShiftData/FindShift/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            ShiftDto selectedshift = response.Content.ReadAsAsync<ShiftDto>().Result;
            //Debug.WriteLine("shift receieved: ");
            //Debug.WriteLine(selectedshift.ShiftID);

            return View(selectedshift);
        }
        
      
        public ActionResult Error()
        {
            return View();
        }

        // GET: Shift/New
        public ActionResult New()
        {
            //api/ShiftData/AddShift
            //string url = "employeedata/listemployees";
            //HttpResponseMessage response = client.GetAsync(url).Result;
            //IEnumerable<EmployeeDto> EmployeesOptions = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;


            //return View(EmployeesOptions);
            return View();
        }

        // POST: Shift/Create
        [HttpPost]
      
        public ActionResult Create(Shift shift)
        {
            Debug.WriteLine("The json payload is: ");
            //Debug.WriteLine(shift.ShiftID);
            //OBJECTIVE: add a new shift into the sysyem using the API

            //curl -H "Content-Type:application/json" -d @shift.json https://localhost:44397/api/ShiftData/addshift
            string url = "ShiftData/addshift";

          
            string jsonpayload = jss.Serialize(shift);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response =  client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return Redirect("Error");
            }

       

        }

        // POST: Shift/Edit/5

        public ActionResult Edit(int id)
        {

            UpdateShift ViewModel = new UpdateShift();


            string url = "shiftdata/findshift/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ShiftDto SelectedShift = response.Content.ReadAsAsync<ShiftDto>().Result;
            ViewModel.SelectedShift = SelectedShift;

       
            url = "employeedata/listemployees";
            response = client.GetAsync(url).Result;
             IEnumerable<EmployeeDto> EmployeesOptions = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;

            ViewModel.EmployeesOptions = EmployeesOptions;
            return View(ViewModel);
        }

        // POST: Shift/Update/5
        [HttpPost]

        public ActionResult Update(int id, Shift shift)
        {
            //objective: update the shift info in the system
      
            //curl -H "Content-Type:application/json" -d @shift.json https://localhost:44345/api/ShiftData/updateshift
            string url = "shiftdata/updateshift/" + id;
            string jsonpayload = jss.Serialize(shift);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Shift/Delete/5
      
        public ActionResult DeleteConfirm(int id)
        {
            string url = "shiftdata/findshift/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ShiftDto selectedshift = response.Content.ReadAsAsync<ShiftDto>().Result;
            return View(selectedshift);
        }

        // POST: Shift/Delete/5
        [HttpPost]
    
        public ActionResult Delete(int id)
        {
            string url = "shiftdata/deleteshift/" + id;
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
