namespace BriLib
{
    public class AxisAlignedBoundingBox
    {
        public float X;
        public float Y;
        public float Radius;

        public AxisAlignedBoundingBox(float centerX, float centerY, float radius)
        {
            X = centerX;
            Y = centerY;
            Radius = radius;
        }

        public bool Intersects(float x, float y)
        {
            return (x >= X - Radius && x < X + Radius && y >= Y - Radius && y < Y + Radius);
        }

        public bool Intersects(AxisAlignedBoundingBox box)
        {
            var boxLeft = box.X - box.Radius;
            var boxRight = box.X + box.Radius;
            var boxTop = box.Y + box.Radius;
            var boxBot = box.Y - box.Radius;

            var myLeft = X - Radius;
            var myRight = X + Radius;
            var myBot = Y - Radius;
            var myTop = Y + Radius;

            var xInter = (boxLeft >= myLeft && boxLeft <= myRight || boxRight >= myLeft && boxRight <= myRight);
            var yInter = (boxBot >= myBot && boxBot <= myTop || boxTop >= myBot && boxTop <= myTop);
            return xInter && yInter;
        }
    }
}
