using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BriLib
{
    public class VoronoiDiagram
    {
        public IEnumerable<VoronoiCell> Cells { get; private set; }
        public IEnumerable<Circle> Circumcircles { get; private set; }
        public IEnumerable<Point> IntersectionPoints { get; private set; }

        public void AddVoronoiFacePoint(Point point)
        {

        }

        public void AddExistingVertex(Point point)
        {

        }
    }

    public class VoronoiCell
    {
        public IEnumerable<Triangle> Triangles { get; private set; }

    }

    public class Triangle
    {
        public Point[] Vertices { get; private set; }
    }

    public class Circle
    {
        public Point Center { get; private set; }
        public float Radius { get; private set; }
    }

    public class Point
    {
        public int Dimensionality { get; private set; }
        public float[] Coordinates { get; private set; }

        public Point(float[] coords)
        {
            Coordinates = coords;
        }

        public Point(float x, float y)
        {
            Coordinates = new float[] { x, y };
        }

        public Point(float x, float y, float z)
        {
            Coordinates = new float[] { x, y, z };
        }

        public float Distance(Point other)
        {
            if (Dimensionality != other.Dimensionality) throw new System.Exception("Mismatched dimensionality");

            var total = 0f;
            for (int i = 0; i < Dimensionality; i++)
            {
                total += Coordinates[i].Sq() + other.Coordinates[i].Sq();
            }
            return total.Sqrt();
        }
    }
}
