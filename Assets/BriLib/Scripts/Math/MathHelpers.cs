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

        public static int GetPosNeg(System.Random rand)
        {
            return rand.Next(2) > 0 ? 1 : -1;
        }

        public static float AbsFloat(float value)
        {
            return value < 0 ? value * -1f : value;
        }

        public static bool FloatCompare(float a, float b)
        {
            return AbsFloat(a - b) < 1E-05F;
        }

        public static bool LessThanEqualFloat(float a, float b)
        {
            return a < b || FloatCompare(a, b);
        }

        public static bool GreaterThanEqualFloat(float a, float b)
        {
            return a > b || FloatCompare(a, b);
        }
    }
}
