using System.Threading.Tasks;

namespace Fritz.StreamLib.Core
{
  /// <summary>
  /// This class gives all assemblies access to hub functions
  /// </summary>
  public interface IHubAccessor
  {
		Task AlertFritz();
  }
}
