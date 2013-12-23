using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crossrail.ObservationForm.Domain;
using Crossrail.ObservationForm.Mvc.Mailers;
using Crossrail.ObservationForm.Mvc.Models;

namespace Crossrail.ObservationForm.Mvc.Controllers
{
    /// <summary>
    /// NOTE: At present only the create and index methods are used. The edit, details and index
    /// views are probably incomplete at this stage and do not work.
    /// </summary>

    public class ObservationsController : BaseController
    {
        // GET: /Observations/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Thanks()
        {
            ViewBag.Thanks = "Thank you for your submission";
            return View();
        }

        public ActionResult Administration()
        {
            IQueryable<Observation> observations = UnitOfWork.ObservationService
                .GetAll()
                .OrderByDescending(o => o.ObservationDate);
            
            return View(observations);
        }

        //
        // GET: /Observations/Create

        public ActionResult Create(HttpPostedFileBase file)
        {
            PopulateViewBag();
            return View();
        }

        private void PopulateViewBag()
        {
            ViewBag.Contracts = UnitOfWork.ContractService.GetAll().ToList().Select(x => new SelectListItem
            {
                Value = x.ContractId.ToString(),
                Text = x.Name
            });

            ViewBag.Types = UnitOfWork.ObservationService.GetAllTypes().ToList().Select(x => new SelectListItem
            {
                Value = x.ObservationTypeId.ToString(),
                Text = x.Name
            });

            ViewBag.Categories = UnitOfWork.ObservationService.GetAllCategories().ToList().Select(x => new SelectListItem
            {
                Value = x.ObservationCategoryId.ToString(),
                Text = x.Name
            });

            ViewBag.Time = null;
        }

        //
        // POST: /Observations/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Observation model, HttpPostedFileBase file)
        {
            //Extra complex validation is performed within the service to method,
            //then the model state validity is used.

            Observation observation = UnitOfWork.ObservationService.Add(
                new ModelStateWrapper(ModelState), model, file);
            
            if (observation != null)
            {
                //Email if specified

                if (!string.IsNullOrWhiteSpace(observation.Email))
                {
                    var observationsMailer = new ObservationsMailer();
                    observationsMailer.Details(observation).Send();
                }

                return RedirectToAction("Thanks");
            }
            
            PopulateViewBag();
            return View(model);
        }

        //
        // GET: /Observations/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var observation = UnitOfWork.ObservationService.GetById(id);

            if (observation == null)
            {
                return HttpNotFound();
            }

            PopulateViewBag();
            return View(observation);
        }

        //
        // POST: /Observations/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Observation model,  HttpPostedFileBase file)
        {
            //Assign the ID manually from the URL since we cannot change the default
            //"id" parameter name in the GET without either creating a custom binder + attribute
            //or change the routing set-up.

            model.ObservationId = id;

            Observation observation = UnitOfWork.ObservationService.Edit(
                new ModelStateWrapper(ModelState), model, file);

            if (observation != null)
            {
                //No need to send an email on the edit? Can we remove
                //the field from the view?

                return RedirectToAction("Administration");
            }

            PopulateViewBag();
            return View(model);
        }

        //
        // GET: /Observations/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Observation observation = UnitOfWork.ObservationService.GetById(id);

            if (observation == null)
            {
                return HttpNotFound();
            }

            return View(observation);
        }

        // GET: /Observations/Details/5

        public ActionResult Details(int id = 0)
        {
            Observation observation = UnitOfWork.ObservationService.GetById(id);

            if (observation == null)
            {
                return HttpNotFound();
            }

            return View(observation);
        }

        //
        // POST: /Observations/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UnitOfWork.ObservationService.Remove(id);
            UnitOfWork.SaveChanges();

            return View();
        }
    }
}


