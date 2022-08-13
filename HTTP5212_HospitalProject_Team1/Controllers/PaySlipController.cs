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
    public class PaySlipController : Controller
    {

        public static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PaySlipController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44397/api/");
        }


        // GET: PaySlip/List
        
        public ActionResult List()
        {
            //retrieve a list of payslips from payslipdatacontroller
            //curl https://localhost:44397/api/PaySlipData/ListPaySlips


            string url = "PaySlipData/ListPaySlips";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode); 

            IEnumerable<PaySlipDto> payslips = response.Content.ReadAsAsync<IEnumerable<PaySlipDto>>().Result;
            // Debug.WriteLine("Number of payslips receieved: ");
            //Debug.WriteLine(payslips.Count());

            return View(payslips);
        }

        // GET: PaySlip/Details/5
        
        public ActionResult Details(int id)
        {
            //retrieve one payslip from payslipdatacontroller
            //curl https://localhost:44397/api/PaySlipData/FindPaySlip/{id}

            string url = "PaySlipData/FindPaySlip/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            PaySlipDto selectedpayslip = response.Content.ReadAsAsync<PaySlipDto>().Result;
            //Debug.WriteLine("payslip receieved: ");
            //Debug.WriteLine(selectedpayslip.PaySlipID);

            return View(selectedpayslip);
        }
        
        
        public ActionResult Error()
        {
            return View();
        }

        // GET: PaySlip/New
        
        public ActionResult New()
        {
            //api/payslipdata/addpayslip
            //string url = "employeedata/listemployees";
            //HttpResponseMessage response = client.GetAsync(url).Result;
            //IEnumerable<EmployeeDto> EmployeesOptions = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;


            //return View(EmployeesOptions);
            return View();
        }

        // POST: PaySlip/Create
        [HttpPost]
     
        public ActionResult Create(PaySlip payslip)
        {
            Debug.WriteLine("The json payload is: ");
            //Debug.WriteLine(payslip.PaySlipID);
            //OBJECTIVE: add a new payslip into the sysyem using the API

            //curl -H "Content-Type:application/json" -d @payslip.json https://localhost:44397/api/payslipdata/addpayslip
            string url = "PaySlipData/addpayslip";


            string jsonpayload = jss.Serialize(payslip);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return Redirect("Error");
            }

        }

        // GET: PaySlip/Edit/5
       
        public ActionResult Edit(int id)
        {
            UpdatePaySlip ViewModel = new UpdatePaySlip();


            string url = "PaySlipData/findpayslip/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PaySlipDto SelectedPaySlip = response.Content.ReadAsAsync<PaySlipDto>().Result;
            ViewModel.SelectedPaySlip = SelectedPaySlip;


            url = "employeedata/listemployees";
            response = client.GetAsync(url).Result;
            IEnumerable<EmployeeDto> EmployeesOptions = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;

            ViewModel.EmployeesOptions = EmployeesOptions;
            return View(ViewModel);
        }

        // POST: PaySlip/Update/5
        [HttpPost]
     
        public ActionResult Update(int id, PaySlip payslip)
        {
            //objective: update the payslip info in the system

            //curl -H "Content-Type:application/json" -d @payslip.json https://localhost:44345/api/PaySlipData/updatepayslip
            string url = "PaySlipData/updatepayslip" + id;
            string jsonpayload = jss.Serialize(payslip);

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

        // GET: PaySlip/Delete/5

        public ActionResult DeleteConfirm(int id)
        {
            string url = "PaySlipData/findpayslip/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PaySlipDto selectedpayslip = response.Content.ReadAsAsync<PaySlipDto>().Result;
            return View(selectedpayslip);
        }

        // POST: PaySlip/Delete/5
        [HttpPost]
       
        public ActionResult Delete(int id)
        {
            string url = "PaySlipData/deletepayslip/" + id;
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
