namespace QueenGame.Utils.AppColorConverter
{
    public class HSL
    {
        public double H { get; }
        public double S { get; }
        public double L { get; }

        public HSL(double h, double s, double l)
        {
            H = h >= 0 ? h % 360 : (360 - ((-h) % 360));
            S = s < 0 ? 0 : (s > 1 ? 1 : s);
            L = l < 0 ? 0 : (l > 1 ? 1 : l);
        }
    }
}
