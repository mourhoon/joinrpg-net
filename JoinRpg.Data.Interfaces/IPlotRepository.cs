﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JoinRpg.DataModel;

namespace JoinRpg.Data.Interfaces
{
  public interface IPlotRepository : IDisposable
  {

    Task<List<PlotFolder>> GetPlots(int project);

    Task<PlotFolder> GetPlotFolderAsync(int projectId, int plotFolderId);
    Task<IList<PlotElement>> GetPlotsForCharacter(Character character);
  }
}
