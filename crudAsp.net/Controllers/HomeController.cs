using crudAsp.net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace crudAsp.net.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEmployees()
        {
            using (mydatabaseEntities1 dc = new mydatabaseEntities1())
            {
                var employees = dc.Employees.OrderBy(a => a.FirstName).ToList();
                return Json(new { data = employees }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Save(int id)
        {
            using (mydatabaseEntities1 dc = new mydatabaseEntities1())
            {
                var v = dc.Employees.Where(a => a.EmployeeId == id).FirstOrDefault();
                return View(v);
            }
            
        }

        [HttpPost]
        public ActionResult save(Employee emp)
        {
            bool status = false;

            if (ModelState.IsValid)
            {
                using (mydatabaseEntities1 dc = new mydatabaseEntities1())
                {
                    if(emp.EmployeeId > 0)
                    {
                        //update
                        var v = dc.Employees.Where(a => a.EmployeeId == emp.EmployeeId).FirstOrDefault();
                        if(v != null)
                        {
                            v.FirstName = emp.FirstName;
                            v.LastName = emp.LastName;
                            v.EmailId = emp.EmailId;
                            v.City = emp.City;
                            v.Country = emp.Country;

                        }
                    }
                    else
                    {
                        //save
                        dc.Employees.Add(emp);
                    }
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult {  Data = new {status = status}};
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (mydatabaseEntities1 dc = new mydatabaseEntities1())
            {
                var v = dc.Employees.Where(a => a.EmployeeId == id).FirstOrDefault();
                if(v != null)
                {
                    return View(v);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteEmployee(int id)
        {
            bool status = false;

            using (mydatabaseEntities1 dc = new mydatabaseEntities1())
            {
                var v = dc.Employees.Where(a => a.EmployeeId == id).FirstOrDefault();
                if(v != null)
                {
                    dc.Employees.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }

            return new JsonResult { Data = new { status = status } };
        }
    }
}