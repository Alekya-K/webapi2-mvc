using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using webapi2.Models;

namespace webapi2.Controllers
{
    public class CrudMvcController : Controller
    {
        HttpClient client = new HttpClient();

        public ActionResult Index()
        {
            List<Employee> emp_list = new List<Employee>();
            client.BaseAddress = new Uri("http://localhost:49925/api/CrudApi");
            var response = client.GetAsync("CrudApi");
            response.Wait();

            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<Employee>>();
                display.Wait();
                emp_list = display.Result;
            }
            return View(emp_list);
        }

        public ActionResult Create(Employee emp)
        {
            client.BaseAddress = new Uri("http://localhost:49925/api/CrudApi");
            var response = client.PostAsJsonAsync<Employee>("CrudApi", emp);
            response.Wait();

            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Create");
        }

        public ActionResult Details(int id)
        {
            Employee e = null;
            client.BaseAddress = new Uri("http://localhost:49925/");

            try
            {
                var response = client.GetAsync("api/CrudApi/" + id.ToString());
                response.Wait();

                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<Employee>();
                    display.Wait();
                    e = display.Result;
                }
                else
                {
                    ViewBag.ErrorMessage = "Error retrieving employee details.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
            }

            return View(e);
        }

       
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Employee e = null;
            client.BaseAddress = new Uri("http://localhost:49925/");

            try
            {
                var response = client.GetAsync("api/CrudApi/" + id.ToString());
                response.Wait();

                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<Employee>();
                    display.Wait();
                    e = display.Result;
                }
                else
                {
                    ViewBag.ErrorMessage = "Error retrieving employee details.";
                    return View(); // Or redirect to an error page
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                return View();
            }

            return View(e);
        }

      
        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            if (employee == null)
            {
                ViewBag.ErrorMessage = "Invalid employee data.";
                return View(); 
            }

            client.BaseAddress = new Uri("http://localhost:49925/");

            try
            {
                var response = client.PutAsJsonAsync("api/CrudApi/" + employee.id.ToString(), employee);
                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details", new { id = employee.id });
                }
                else
                {
                    ViewBag.ErrorMessage = "Error updating employee.";
                    return View(employee);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                return View(employee);
            }
        }

       
        public ActionResult Delete(int id)
        {
            client.BaseAddress = new Uri("http://localhost:49925/");

            try
            {
                var response = client.DeleteAsync("api/CrudApi/" + id.ToString());
                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Error deleting employee.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}