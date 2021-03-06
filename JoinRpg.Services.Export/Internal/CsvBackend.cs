using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinRpg.Services.Export.Internal
{
  internal class CsvBackend : IGeneratorBackend
  {
    private StringBuilder Builder { get;  }= new StringBuilder();
    public string ContentType => "text/csv";

    public string FileExtension => "csv";

    public void WriteRow(IEnumerable<Cell> cells)
      => Builder.AppendLine(string.Join(";", cells.Select(GetContentForCsv)));

    private static string GetContentForCsv(Cell c) => "\"" + c.Content.Replace("\"", "\"\"") + "\"";

    public byte[] Generate() => Encoding.UTF8.GetBytes(Builder.ToString());
  }
}