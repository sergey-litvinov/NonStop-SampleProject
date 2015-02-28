using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using ContactApp.Data;

namespace ContactApp.Controllers
{
    public class ApiController : Controller
    {
		private EfContext _efContext = new EfContext();

		/// <summary>
		/// Find item
		/// </summary>
		/// <param name="query">Conact name or email</param>
	    public ActionResult Find(string query)
		{
			IQueryable<Contact> dbQuery = _efContext.Contacts;
			if (!string.IsNullOrWhiteSpace(query))
			{
				dbQuery = dbQuery.Where(c => c.Name.Contains(query) || c.Email.Contains(query));
			}

		    var contacts = dbQuery.ToList();
			foreach (var contact in contacts)
			{
				if (string.IsNullOrWhiteSpace(contact.Photo))
				{
					contact.Photo = "/Content/default.jpg";
				}
			}
		    return Json(contacts, JsonRequestBehavior.AllowGet);
	    }

	    public ActionResult Create(Contact item)
	    {
		    if (!this.ModelState.IsValid)
		    {
			    return Content("Model is invalid");
		    }

		    _efContext.Contacts.Add(item);
		    _efContext.SaveChanges();

		    return Json(item);
	    }

		public ActionResult Update(Contact item)
		{
			if (!this.ModelState.IsValid || item.Id <= 0)
			{
				return Content("Model is invalid");
			}

			_efContext.Contacts.Attach(item);
			_efContext.Entry(item).State = EntityState.Modified;
			
			_efContext.SaveChanges();

			return Json(item);
		}

	    public ActionResult Delete(int id)
	    {
		    var item = _efContext.Contacts.Find(id);

		    if (item == null)
		    {
			    return Content("We can't find item with id " + id);
		    }

		    _efContext.Contacts.Remove(item);

		    _efContext.SaveChanges();

			return new HttpStatusCodeResult(HttpStatusCode.OK);
	    }

		public ActionResult Get(int id)
		{
			var item = _efContext.Contacts.Find(id);

			if (item == null)
			{
				return Content("We can't find item with id " + id);
			}

			return Json(item, JsonRequestBehavior.AllowGet);
		}


		public ActionResult UploadFile(int id)
		{
			string root = Server.MapPath("~/Content/");
			foreach (string file in Request.Files)
			{
				var extension = Path.GetExtension(file);
				if (!string.IsNullOrWhiteSpace(extension))
				{
					extension = "." + extension;
				}
				else
				{
					extension = ".jpg";
				}
				var fileName = Guid.NewGuid().ToString().Replace("-", "") + extension;
				var filePath = Path.Combine(root, fileName);

				using (var stream = System.IO.File.Create(filePath))
				{
					var inputStream = Request.Files[file].InputStream;
					inputStream.CopyTo(stream);
				}

				var ad = _efContext.Contacts.Find(id);
				ad.Photo = "/Content/" + fileName;
				_efContext.SaveChanges();
				return Json(ad);
			}

			return Content("No files were passed");
		}
	    protected override void Dispose(bool disposing)
	    {
			_efContext.Dispose();
		    base.Dispose(disposing);
	    }
    }
}