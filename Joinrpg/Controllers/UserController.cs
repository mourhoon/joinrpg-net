﻿using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Mvc;
using JoinRpg.DataModel;
using JoinRpg.Domain;
using JoinRpg.Helpers;
using JoinRpg.Helpers.Web;
using JoinRpg.Web.Models;
using Microsoft.AspNet.Identity;

namespace JoinRpg.Web.Controllers
{
    public class UserController : Common.ControllerBase
    {

      [HttpGet]
      // GET: User
      public async Task<ActionResult> Details(int userId)
      {
        var user = await UserManager.FindByIdAsync(userId);
        var currentUser = User.Identity.IsAuthenticated ? await GetCurrentUserAsync() : null;

        var userProfileViewModel = new UserProfileViewModel()
        {
          DisplayName = user.DisplayName,
          ThisUserProjects = user.ProjectAcls,
          UserId = user.UserId,
          Reason = currentUser != null
          ? (AccessReason)user.GetProfileAccess(currentUser)
          : AccessReason.NoAccess,
          Details = UserProfileDetailsViewModel.FromUser(user, currentUser)
        };


        if (currentUser != null)
        {
          userProfileViewModel.CanGrantAccessProjects = currentUser.GetProjects(acl => acl.CanGrantRights);
          userProfileViewModel.Claims =
            user.Claims.Where(claim => claim.HasAnyAccess(currentUser.UserId))
              .Select(ClaimListItemViewModel.FromClaim);

        }

        return View(userProfileViewModel);
      }


      [Authorize]
      // GET: User preferred name. I.e. to display in page header.
      public ActionResult PreferredName()
      {
        var userId = CurrentUserId;
        var user =   UserManager.FindById(userId);

        var userProfileViewModel = new UserProfileViewModel()
        {
          DisplayName = user.DisplayName,
          ThisUserProjects = user.ProjectAcls,
          UserId = user.UserId,
          Hash = user.Email.GravatarHash()
        };

        return PartialView(userProfileViewModel);
      }

        public UserController(ApplicationUserManager userManager) : base(userManager)
      {
      }

      [HttpGet,Authorize]
      public ActionResult Me()
      {
        return RedirectToAction("Details", new {UserId = CurrentUserId});
      }

      public ActionResult GetAvatar(int userId)
      {
        var hash = UserManager.FindById(userId).Email.GravatarHash();
      return Content($"https://www.gravatar.com/avatar/{hash}?d=identicon&s=64");
    }
    }
}