namespace BriLib
{
    public static class MathHelpers
    {
        public static float Distance(float xA, float yA, float xB, float yB)
        {
            return ((xA - xB).Sq() + (yA - yB).Sq()).Sqrt();
        }

        public static float Distance(float xA, float yA, float zA, float xB, float yB, float zB)
        {
            return ((xA - xB).Sq() + (yA - yB).Sq() + (zA - zB).Sq()).Sqrt();
        }

        public static float GetRandom(float max, System.Random rand)
        {
            return (float)rand.Next(1000) / 1000f * max;
        }
    }
}
