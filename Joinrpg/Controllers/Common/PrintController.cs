﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using JoinRpg.Data.Interfaces;
using JoinRpg.DataModel;
using JoinRpg.Helpers.Web;
using JoinRpg.Services.Interfaces;
using JoinRpg.Web.Models;
using JoinRpg.Web.Models.Print;

namespace JoinRpg.Web.Controllers.Common
{
  [Authorize]
  public class PrintController : ControllerGameBase
  {
    private IPlotRepository PlotRepository { get; }

    public PrintController(ApplicationUserManager userManager, IProjectRepository projectRepository, IProjectService projectService, IExportDataService exportDataService, IPlotRepository plotRepository) : base(userManager, projectRepository, projectService, exportDataService)
    {
      PlotRepository = plotRepository;
    }


    public async Task<ActionResult> Character(int projectid, int characterid)
    {
      var character = await ProjectRepository.GetCharacterWithGroups(projectid, characterid);
      var error = WithCharacter(character);
      if (error != null) return error;

      return View(new PrintCharacterViewModel(CurrentUserId, character, await PlotRepository.GetPlotsForCharacter(character)));
    }

    //TODO: Split this into printing envelope and printing content
    public async Task<ActionResult> CharacterList(int projectid, string characterIds)
    {
      var characters = (await ProjectRepository.LoadCharacters(projectid, characterIds.UnCompressIdList().ToArray()));

      var error = await AsMaster(characters, projectid);
      if (error != null) return error;

      var plotElements = (await PlotRepository.GetPlotsWithTargetAndText(projectid)).SelectMany(p => p.Elements).ToArray();

      var viewModel =
        characters.Select(
          c => new PrintCharacterViewModel(CurrentUserId, c, plotElements)).ToArray();

      return View(viewModel);
    }

    public async Task<ActionResult> Index(int projectid)
    {
      var characters = (await ProjectRepository.GetCharacters(projectid)).Where(c => c.IsActive).ToList();
      var error = await AsMaster(characters, projectid);
      if (error != null) return error;

      return
        View(new PrintIndexViewModel(projectid,
          characters.Select(c => c.CharacterId)));
    }

    public async Task<ActionResult> HandoutReport(int projectid)
    {
      var plotElements =
        (await PlotRepository.GetPlotsWithTargetAndText(projectid)).SelectMany(p => p.Elements)
          .Where(e => e.ElementType == PlotElementType.Handout)
          .ToArray();

      var characters = (await ProjectRepository.GetCharacters(projectid)).Where(c => c.IsActive).ToList();
      var error = await AsMaster(characters, projectid);
      if (error != null) return error;

      return View(new HandoutReportViewModel(plotElements, characters));
    }
  }
}