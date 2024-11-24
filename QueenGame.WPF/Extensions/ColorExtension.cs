using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenGame.WPF
{
    public static class ColorExtension
    {
        public static System.Drawing.Color? ToDrawingColor(this System.Windows.Media.Color? mediaColor)
        {
            if (mediaColor != null)
            {
                return System.Drawing.Color.FromArgb(
                    mediaColor.Value.A, 
                    mediaColor.Value.R, 
                    mediaColor.Value.G, 
                    mediaColor.Value.B);
            }
            return null;
        }

        public static System.Drawing.Color ToDrawingColor(this System.Windows.Media.Color mediaColor)
        {
                return System.Drawing.Color.FromArgb(
                    mediaColor.A, 
                    mediaColor.R, 
                    mediaColor.G, 
                    mediaColor.B);
       
        }

        public static System.Windows.Media.Color? ToMediaColor(this System.Drawing.Color? drawingColor)
        {
            if (drawingColor != null)
            {
                return System.Windows.Media.Color.FromArgb(
                    drawingColor.Value.A, 
                    drawingColor.Value.R, 
                    drawingColor.Value.G, 
                    drawingColor.Value.B);
            }
            return null;
        }
        
        public static System.Windows.Media.Color ToMediaColor(this System.Drawing.Color drawingColor)
        {
                return System.Windows.Media.Color.FromArgb(
                    drawingColor.A, 
                    drawingColor.R, 
                    drawingColor.G, 
                    drawingColor.B);
            
        }
    }
}
