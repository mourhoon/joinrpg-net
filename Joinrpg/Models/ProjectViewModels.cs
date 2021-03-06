﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using JoinRpg.DataModel;
using JoinRpg.Domain;
using JoinRpg.Web.Models.CommonTypes;

namespace JoinRpg.Web.Models
{
  public class ProjectLinkViewModel
  {
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }
  }
  public abstract class ProjectViewModelBase 
  {
    public int ProjectId { get; set; }

    [DisplayName("Название проекта"), Required]
    public string ProjectName { get; set; }

    [DisplayName("Анонс проекта")]
    public MarkdownViewModel ProjectAnnounce { get; set; }

    [Display(Name = "Заявки открыты?")]
    public bool IsAcceptingClaims { get; set; }
  }
  public class EditProjectViewModel: ProjectViewModelBase
  {
    [DisplayName("Правила подачи заявок")]
    public MarkdownViewModel ClaimApplyRules { get; set; }

    [ReadOnly(true)]
    public string OriginalName { get; set;  }
  }

  public class ProjectDetailsViewModel : ProjectViewModelBase
  {
    [Display(Name="Проект активен?")]
    public bool IsActive { get; set; }
    [Display(Name = "Дата создания")]
    public DateTime CreatedDate { get; set; }
    public IEnumerable<User>  Masters { get; set; }
  }

  public class ProjectListItemViewModel : ProjectViewModelBase
  {
    public bool IsMaster { get; set; }
    public bool IsActive { get; set; }
    public IEnumerable<Claim>  MyClaims {get; set; }
    public int ClaimCount { get; set; }

    public int ProjectRootGroupId { get; set; }

    public bool IsRootGroupAccepting { get; set; }

    public static ProjectListItemViewModel FromProject(Project p, int? user)
    {
      return new ProjectListItemViewModel()
      {
        ProjectId = p.ProjectId,
        IsMaster = p.HasMasterAccess(user),
        IsActive = p.Active,
        ProjectAnnounce = new MarkdownViewModel(p.Details?.ProjectAnnounce),
        ProjectName = p.ProjectName,
        MyClaims = p.Claims.Where(c => c.PlayerUserId == user),
        ClaimCount = p.Claims.Count(c => c.IsActive),
        IsAcceptingClaims = p.IsAcceptingClaims,
        ProjectRootGroupId = p.RootGroup.CharacterGroupId,
        IsRootGroupAccepting = p.RootGroup.IsAvailable
      };
    }

    public static IOrderedEnumerable<T> OrderByDisplayPriority<T>(
      IEnumerable<T> collectionToSort,
      Func<T, ProjectListItemViewModel> getProjectFunc)
    {
      return collectionToSort
        .OrderByDescending(p => getProjectFunc(p)?.IsActive)
        .ThenByDescending(p => getProjectFunc(p)?.IsMaster)
        .ThenByDescending(p => getProjectFunc(p)?.ClaimCount);
    }
  }
}
