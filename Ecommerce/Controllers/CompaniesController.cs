﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ecommerce.Clases;
using Ecommerce.Models;

namespace Ecommerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompaniesController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        // GET: Companies
        public ActionResult Index()
        {
            var companies = db.Companies.Include(c => c.Cities).Include(c => c.Departments);
            return View(companies.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(comboHelper.GetCities(0), "CityID", "Name");
            ViewBag.DepartmentID = new SelectList(comboHelper.GetDepartments(), "DepartmentID", "Name");
            return View();
        }

        // POST: Companies/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                
                db.Companies.Add(company);
                db.SaveChanges();
                
                if (company.LogoFile != null)
                {
                    //var pic = string.Empty;
                    var folder = "~/Content/Logos";
                    var file = string.Format("{0}.jpg", company.CompanyID);
                    var response = fileHelper.UploadPhoto(company.LogoFile, folder, file);
                    if (response)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        company.Logo = pic;
                        db.Entry(company).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    
                }

                

                return RedirectToAction("Index");


            }

            ViewBag.CityID = new SelectList(comboHelper.GetCities(company.DepartmentID), "CityID", "Name");
            ViewBag.DepartmentID = new SelectList(comboHelper.GetDepartments(), "DepartmentID", "Name");
            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(comboHelper.GetCities(company.DepartmentID), "CityID", "Name");
            ViewBag.DepartmentID = new SelectList(comboHelper.GetDepartments(), "DepartmentID", "Name");
            return View(company);
        }

        // POST: Companies/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {

                if (company.LogoFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Logos";
                    var file = string.Format("{0}.jpg", company.CompanyID);
                    var response = fileHelper.UploadPhoto(company.LogoFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        company.Logo = pic;
                    }
                }
                    
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(comboHelper.GetCities(company.DepartmentID), "CityID", "Name");
            ViewBag.DepartmentID = new SelectList(comboHelper.GetDepartments(), "DepartmentID", "Name");
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
