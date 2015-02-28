using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ContactApp.Data;

namespace ContactApp
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			// create db on start
			using (var context = new EfContext())
			{
				context.Database.CreateIfNotExists();
			}
		}
	}
}
