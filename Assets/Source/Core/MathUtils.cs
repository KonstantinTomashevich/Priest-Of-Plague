namespace PriestOfPlague.Source.Core
{
    public static class MathUtils
    {
        public static float AngleFrom_0_360To_m180_180 (float angle)
        {
            return angle > 180.0f ? angle - 360.0f : angle;
        }
    }
}