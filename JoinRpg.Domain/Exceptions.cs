﻿using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using JoinRpg.DataModel;
using JoinRpg.Helpers;

namespace JoinRpg.Domain
{
  public abstract  class JoinRpgBaseException : ApplicationException
  {
    protected JoinRpgBaseException(string message) : base(message)
    {
    }
  }

  public class NoAccessToProjectException : JoinRpgBaseException
  {
    [PublicAPI]
    public Project Project { get; }
    [PublicAPI]
    public int? UserId { get; }

    public NoAccessToProjectException(Project project, int? userId, Expression<Func<ProjectAcl, bool>> accessExpression)
      : base($"No access to project {project.ProjectName} for user {userId}. Required access: {accessExpression.AsPropertyName()}")
    {
      Project = project;
      UserId = userId;
    }

    public NoAccessToProjectException(Project project, int? userId)
      : base($"No access to project {project.ProjectName} for user {userId}")
    {
      Project = project;
      UserId = userId;
    }
  }

  public class ValueAlreadySetException : JoinRpgBaseException
  {
    public ValueAlreadySetException(string message) : base(message)
    {
    }
  }
}