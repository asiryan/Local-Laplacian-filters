using System.Drawing;

namespace LaplacianHDR.Helpers
{
    /// <summary>
    /// Filter delegate.
    /// </summary>
    /// <param name="bitmap">Bitmap</param>
    /// <returns>Bitmap</returns>
    public delegate Bitmap Filter(Bitmap bitmap);
    /// <summary>
    /// Filter delegate.
    /// </summary>
    /// <param name="bitmap">Bitmap</param>
    /// <returns>Bitmap</returns>
    public delegate Bitmap MultiFilter(Bitmap[] bitmap);
}
