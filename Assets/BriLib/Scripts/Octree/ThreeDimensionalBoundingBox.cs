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

            var myLeft = X - Radius;
            var myRight = X + Radius;
            var myBottom = Y - Radius;
            var myTop = Y + Radius;
            var myFront = Z - Radius;
            var myBack = Z + Radius;

            var xIsInside = (myLeft >= lowX && myRight <= highX);
            var yIsInside = (myBottom >= lowY && myTop <= highY);
            var zIsInside = (myFront >= lowZ && myBack <= highZ);

            return topLeftBack || topLeftFront || bottomLeftBack || bottomLeftFront
                || topRightBack || topRightFront || bottomRightBack || bottomRightFront
                || (xIsInside && yIsInside && zIsInside);
        }

        public float BoundsDistance(float x, float y, float z)
        {
            var xDist = System.Math.Abs(x - X) - Radius;
            var yDist = System.Math.Abs(y - Y) - Radius;
            var zDist = System.Math.Abs(z - Z) - Radius;

            if (yDist > 0 && xDist <= 0 && zDist <= 0) return yDist;
            if (yDist <= 0 && xDist > 0 && zDist <= 0) return xDist;
            if (yDist <= 0 && xDist <= 0 && zDist > 0) return zDist;
            if (yDist > 0 && xDist > 0 && zDist <= 0) return (yDist.Sq() + xDist.Sq()).Sqrt();
            if (yDist > 0 && xDist <= 0 && zDist > 0) return (yDist.Sq() + zDist.Sq()).Sqrt();
            if (yDist <= 0 && xDist > 0 && zDist > 0) return (xDist.Sq() + zDist.Sq()).Sqrt();
            if (yDist <= 0 && xDist <= 0 && zDist <= 0) return System.Math.Min(xDist, yDist);

            return (xDist.Sq() + yDist.Sq() + zDist.Sq()).Sqrt();
        }

        public override string ToString()
        {
            var vectorString = string.Format("[x={0}, y={1}, z={2}]", X, Y, Z);
            return string.Format("[ThreeDimensionalBoundBox Center={0}, Radius={1}]", vectorString, Radius);
        }
    }
}
