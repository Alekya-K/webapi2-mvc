using System.Collections.Generic;
using System.Web.Http;
using webapi2.Models;


using System.Linq;
using System.Net;
using System.Net.Http;
using System;

namespace webapi2.Controllers
{
    public class CrudApiController : ApiController
    {
        webapi_crudDBEntities1 db = new webapi_crudDBEntities1();

        [System.Web.Http.HttpGet]
        public IHttpActionResult GetEmployees()
        {
            List<Employee> list = db.Employees.ToList();
            return Ok(list);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CrudApi/{id}")]
        public IHttpActionResult GetEmployeeById(int id)
        {
            var emp = db.Employees.Where(model => model.id == id).FirstOrDefault();
            return Ok(emp);
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult EmpInsert(Employee e)
        {
            db.Employees.Add(e);
            db.SaveChanges();
            return Ok();
        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("api/CrudApi/{id}")]
        public IHttpActionResult UpdateEmployee(int id, Employee e)
        {
            db.Entry(e).State = System.Data.Entity.EntityState.Modified;
            if (id != e.id)
            {
                return BadRequest("Invalid employee ID.");
            }

            var existingEmp = db.Employees.Find(id);
            if (existingEmp == null)
            {
                return NotFound();
            }

            // Update the properties
            //existingEmp.name = e.name;
            //existingEmp.gender = e.gender;
            //existingEmp.designation = e.designation;
            //existingEmp.age = e.age;
            //existingEmp.salary = e.salary;

            db.Entry(existingEmp).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Database Update Error: " + ex.Message);
                return InternalServerError(ex);
            }


        }
        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("api/CrudApi/{id}")]
        public IHttpActionResult DeleteEmployee(int id)
        {
            var employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            db.SaveChanges();

            return Ok();
        }

    }
}