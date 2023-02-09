namespace HexBOT.Protocol.Utils
{
    internal class Math
    {
        public static double ConvertFixedPoint(int fixedPoint)
        {
            return (double)(fixedPoint / 32.0D);
        }

        public static int ConvertFixedPoint(double fixedPoint)
        {
            return (int)(fixedPoint * 32.0D);
        }
    }
}
