using System.Drawing;

namespace QueenGame.Utils.AppColorConverter
{
    public static class AppColorConverter
    {
        // HEX <-> RGB

        public static string ToHexStrFromRGB(Color rgb) => $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
        
        public static Color ToRGBFromHexStr(string hexStr) => ColorTranslator.FromHtml(hexStr);

        // HSL <-> RGB

        public static HSL ToHSLFromRGB(Color rgb)
        {
            double dR, dG, dB, dmax, dmin;

            dR = (double)rgb.R / 255;
            dG = (double)rgb.G / 255;
            dB = (double)rgb.B / 255;

            double h, s, l;

            dmax = Math.Max(Math.Max(dR, dG), dB);
            dmin = Math.Min(Math.Min(dR, dG), dB);

            l = (dmax + dmin) / 2;

            if (dmax - dmin > 0)
            {
                s = (dmax - dmin) / (1 - Math.Abs(2 * l - 1));

                h = 0;
                if (dmax == dR)
                {
                    h = 60 * (((dG - dB) / (dmax - dmin)) % 6);
                }
                else if (dmax == dG)
                {
                    h = 60 * (2 + (dB - dR) / (dmax - dmin));
                }
                else if (dmax == dB)
                {
                    h = 60 * (4 + (dR - dG) / (dmax - dmin));
                }

                if (h < 0)
                {
                    h += 360;
                }
            }
            else
            {
                s = 0;
                h = 0;
            }

            return new HSL(h, s, l);
        }

        public static Color ToRGBFromHSL(HSL hsl)
        {
            double c, x, m;

            c = (1 - Math.Abs(2 * hsl.L - 1)) * hsl.S;
            x = c * (1 - Math.Abs((hsl.H / 60) % 2 - 1));
            m = hsl.L - c / 2;

            (double dR, double dG, double dB) value = (0, 0, 0);

            if (hsl.H >= 0 && hsl.H < 60)
            {
                value = (c, x, 0);
            }
            else if (hsl.H >= 60 && hsl.H < 120)
            {
                value = (x, c, 0);
            }
            else if (hsl.H >= 120 && hsl.H < 180)
            {
                value = (0, c, x);
            }
            else if (hsl.H >= 180 && hsl.H < 240)
            {
                value = (0, x, c);
            }
            else if (hsl.H >= 240 && hsl.H < 300)
            {
                value = (x, 0, c);
            }
            else if (hsl.H >= 300 && hsl.H < 360)
            {
                value = (c, 0, x);
            }

            Color rgb = Color.FromArgb((byte)((value.dR + m)*255), (byte)((value.dG +m) * 255), (byte)((value.dB + m) * 255));

            return rgb;
        }

        // Darken RGB-color

        public static Color Darken(Color rgb, double? degree = 0.7)
        {
            HSL hsl = ToHSLFromRGB(rgb);

            if (degree == null || degree < 0 || degree >1 ) 
            {
                degree = 0.7; 
            }

            hsl = new HSL(hsl.H, hsl.S, hsl.L * degree.Value);

            return ToRGBFromHSL(hsl);
        }
    }
}
