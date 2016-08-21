﻿using Markdig;
using Markdig.Renderers;

namespace Joinrpg.Markdown
{
  internal class EntityLinkerExtension : IMarkdownExtension
  {
    private ILinkRenderer LinkRenderers { get; }

    public EntityLinkerExtension(ILinkRenderer linkRenderers)
    {
      LinkRenderers = linkRenderers;
    }

    #region Implementation of IMarkdownExtension

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
      pipeline.InlineParsers.AddIfNotAlready(new LinkerParser(LinkRenderers));
    }

    public void Setup(IMarkdownRenderer renderer)
    {
      renderer.ObjectRenderers.AddIfNotAlready(new LinkerRenderAdapter(LinkRenderers));
    }

    #endregion
  }
}