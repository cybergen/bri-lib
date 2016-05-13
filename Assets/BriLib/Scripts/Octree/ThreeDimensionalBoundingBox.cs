namespace BriLib
{
    public class ThreeDimensionalBoundingBox
    {
        public float X;
        public float Y;
        public float Z;
        public float Radius;

        public ThreeDimensionalBoundingBox(float x, float y, float z, float radius)
        {
            X = x;
            Y = y;
            Z = z;
            Radius = radius;
        }

        public bool Intersects(float x, float y, float z)
        {
            var xIn = x >= X - Radius && x <= X + Radius;
            var yIn = y >= Y - Radius && y <= Y + Radius;
            var zIn = z >= Z - Radius && z <= Z + Radius;
            return xIn && yIn && zIn;
        }

        public bool Intersects(ThreeDimensionalBoundingBox otherBox)
        {
            var lowX = otherBox.X - otherBox.Radius;
            var highX = otherBox.X + otherBox.Radius;
            var lowY = otherBox.Y - otherBox.Radius;
            var highY = otherBox.Y + otherBox.Radius;
            var lowZ = otherBox.Z - otherBox.Radius;
            var highZ = otherBox.Z + otherBox.Radius;

            var topLeftBack = Intersects(lowX, highY, highZ);
            var topLeftFront = Intersects(lowX, highY, lowZ);
            var bottomLeftBack = Intersects(lowX, lowY, highZ);
            var bottomLeftFront = Intersects(lowX, lowY, lowZ);
            var topRightBack = Intersects(highX, highY, highZ);
            var topRightFront = Intersects(highX, highY, lowZ);
            var bottomRightBack = Intersects(highX, lowY, highZ);
            var bottomRightFront = Intersects(highX, lowY, lowZ);

            return topLeftBack || topLeftFront || bottomLeftBack || bottomLeftFront 
                || topRightBack || topRightFront || bottomRightBack || bottomRightFront;
        }
    }
}
