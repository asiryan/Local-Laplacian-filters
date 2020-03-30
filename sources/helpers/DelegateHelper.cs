using System.Drawing;

namespace LocalLaplacianFilters.Helpers
{
    #region Delegates
    /// <summary>
    /// Filter delegate.
    /// </summary>
    /// <param name="bitmap">Bitmap</param>
    /// <returns>Bitmap</returns>
    public delegate Bitmap SingleFilter(Bitmap bitmap);
    /// <summary>
    /// Filter delegate.
    /// </summary>
    /// <param name="bitmap">Bitmap</param>
    /// <returns>Bitmap</returns>
    public delegate Bitmap ComplexFilter(Bitmap[] bitmap);
    #endregion
}
