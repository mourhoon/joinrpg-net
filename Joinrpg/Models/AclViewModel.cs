﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using JoinRpg.DataModel;

namespace JoinRpg.Web.Models
{
  public class AclViewModel
  {
    public int? ProjectAclId { get; set; }
    public int ProjectId { get; set; }
    public int UserId { get; set; }

    [Display(Name="Мастер")]
    public User Master { get; set; }

    [DisplayName("Администратор заявок")]
    public bool CanManageClaims { get; set; }

    [DisplayName("Настраивать поля персонажа")]
    public bool CanChangeFields { get; set; }

    [DisplayName("Настраивать проект")]
    public bool CanChangeProjectProperties { get; set; }

    [DisplayName("Давать доступ другим мастерам")]
    public bool CanGrantRights { get; set; }

    [Display(Name="Редактировать ролевку")]
    public bool CanEditRoles { get; set; }

    [Display(Name = "Принимать взносы")]
    public bool CanAcceptCash { get; set; }

    [Display(Name = "Управлять финансами")]
    public bool CanManageMoney { get; set; }

    [Display(Name = "Игра"), ReadOnly(true)]
    public string ProjectName { get; set; }

    [Display(Name = "Заявок"), ReadOnly(true)]
    public int ClaimsCount { get; set; }

    [Display(Name = "Проблемных"), ReadOnly(true)]
    public int ProblemClaimsCount { get; set; }

    public static AclViewModel FromAcl(ProjectAcl acl)
    {
      return new AclViewModel()
      {
        ProjectId = acl.ProjectId,
        ProjectAclId = acl.ProjectAclId,
        UserId = acl.UserId,
        CanManageClaims = acl.CanManageClaims,
        CanChangeFields = acl.CanChangeFields,
        CanChangeProjectProperties = acl.CanChangeProjectProperties,
        CanGrantRights = acl.CanGrantRights,
        CanEditRoles = acl.CanEditRoles,
        CanAcceptCash = acl.CanAcceptCash,
        CanManageMoney = acl.CanManageMoney,
        Master = acl.User,
        ProjectName = acl.Project.ProjectName,
        ClaimsCount = acl.Project.Claims.Count(claim => claim.ResponsibleMasterUserId == acl.UserId && claim.IsActive)
      };
    }
  }
}
