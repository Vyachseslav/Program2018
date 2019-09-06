

namespace Kachatel2018.UI
{
    class Effects
    {
        /// <summary>
        /// Apply Blur Effect on the window
        /// </summary>
        /// <param name=”win”>Owner window</param>
        public void ApplyEffect(System.Windows.Window win)
        {
            win.Opacity = 0.6;
        }

        /// <summary>
        /// Remove Blur Effects
        /// </summary>
        /// <param name=”win”>Owner window</param>
        public void ClearEffect(System.Windows.Window win)
        {
            win.Opacity = 1;
        }
    }
}
