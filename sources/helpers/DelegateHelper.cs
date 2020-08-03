using System.Drawing;

namespace LaplacianHDR.Helpers
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
