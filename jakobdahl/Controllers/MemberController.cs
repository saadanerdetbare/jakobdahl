﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web.Mvc;
using jakobdahl.Models;

namespace jakobdahl.Controllers
{
	public class MemberController : SurfaceController
	{
		public ActionResult RenderLogin()
		{
			return PartialView("~/Views/Partials/Login.cshtml", new LoginModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SubmitLogin(LoginModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				if (Membership.ValidateUser(model.Username, model.Password))
				{
					FormsAuthentication.SetAuthCookie(model.Username, false);
					UrlHelper myHelper = new UrlHelper(HttpContext.Request.RequestContext);
					if (myHelper.IsLocalUrl(returnUrl))
					{
						return Redirect(returnUrl);
					}
					else
					{
						return Redirect("/login/");
					}
				}
				else
				{
					ModelState.AddModelError("", "The username or password is incorrect");
				}
			}

			return CurrentUmbracoPage();
		}

		public ActionResult RenderLogout()
		{
			return PartialView("~/Views/Partials/Logout.cshtml", null);
		}

		public ActionResult SubmitLogout()
		{
			TempData.Clear();
			Session.Clear();
			FormsAuthentication.SignOut();
			return RedirectToCurrentUmbracoPage();
		}
	}
}