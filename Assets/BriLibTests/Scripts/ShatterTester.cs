using System.Collections.Generic;
using UnityEngine;
using BriLib;

public class ShatterTester : TextureWriteTester
{
    public int VoronoiCount;
    public int IntersectionPointSize;
    public Color CircleColor;
    public Color IntersectionColor;
    public MeshFilter MeshFilter;

    private Quadtree<ColorWrapper> _colorTree;
    private VoronoiDiagram _voronoi;
    private Dictionary<ColorWrapper, BriLib.Point> _pointMap;
    private Triangulation _delaunay;
    private Triangle _initial;

    private void OnGUI()
    {
        var width = 100;
        var startY = Screen.height - 30;
        var height = 30;
        var gap = 20;
        var x = Screen.width / 2 - gap - gap / 2 - width * 2;

        if (GUI.Button(new Rect(x, startY, width, height), "Initialize"))
        {
            Initialize();
        }

        x += width + gap;
        if (GUI.Button(new Rect(x, startY, width, height), "Toggle Points"))
        {
            drawPoints = !drawPoints;
        }

        x += width + gap;
        if (GUI.Button(new Rect(x, startY, width, height), "Toggle Voronoi"))
        {
            drawVoronoi = !drawVoronoi;
        }

        x += width + gap;
        if (GUI.Button(new Rect(x, startY, width, height), "Toggle Delaunay"))
        {
            drawLines = !drawLines;
        }
    }

    protected override void OnMouseClickWorld(Vector3 point)
    {
        base.OnMouseClickWorld(point);

        if (_colorTree == null) return;

        var localPoint = transform.InverseTransformPoint(point);
        _delaunay.delaunayPlace(new Pnt(localPoint.x, localPoint.z));

        var _rand = new System.Random();
        var r = _rand.Next(256) / 256f;
        var g = _rand.Next(256) / 256f;
        var b = _rand.Next(256) / 256f;
        var color = new Color(r, g, b);
        var wrapper = new ColorWrapper { Color = color };

        _colorTree.Insert(localPoint.x, localPoint.z, wrapper);

    }

    protected override void Initialize()
    {
        base.Initialize();
        _colorTree = new Quadtree<ColorWrapper>(0, 0, Width / 2, 5);
        _voronoi = new VoronoiDiagram();
        _pointMap = new Dictionary<ColorWrapper, BriLib.Point>();
        _initial = new Triangle(
                new Pnt(-10000, -10000),
                new Pnt(10000, -10000),
                new Pnt(Width / 2, 10000)
            );
        _delaunay = new Triangulation(_initial);

        ClearTris();
        ClearLines();
        ClearPoints();
        UpdateTexture();
    }

    protected override void UpdateTexture()
    {
        DrawBackground();
        DrawPoints();
        DrawVoronoi();
        DrawTriangles();
        base.UpdateTexture();
    }

    private void DrawVoronoi()
    {
        if (_colorTree == null) return;

        ClearTris();

        // Keep track of sites done; no drawing for initial triangles sites
        HashSet<Pnt> done = new HashSet<Pnt>(_initial);
        foreach (var triangle in _delaunay.Triangles)
        {
            foreach (var site in triangle)
            {
                if (done.Contains(site)) continue;
                var color = _colorTree.GetNearestNeighbor((float)site[0], (float)site[1]).Color;

                done.Add(site);
                List<Triangle> list = _delaunay.surroundingTriangles(site, triangle);
                Pnt[] vertices = new Pnt[list.Count];
                int i = 0;
                foreach (var tri in list) vertices[i++] = tri.getCircumcenter();
                
                if (vertices.Length < 3) return;
                var vectors = new Vector3[vertices.Length];
                var firstPoint = GetPoint(vertices[0]);

                for (int j = 2; j < vertices.Length; j++)
                {
                    var vector = new Vector3[3];
                    vector[0] = firstPoint;
                    vector[1] = GetPoint(vertices[j - 1]);
                    vector[2] = GetPoint(vertices[j]);
                    DrawTriangle(vector, color);
                }
            }
        }
    }

    private Vector3 GetPoint(Pnt point)
    {
        return transform.TransformPoint((float)point[0], transform.position.y + 0.01f, (float)point[1]);
    }

    private void DrawPoints()
    {
        if (_colorTree == null) return;
        ClearPoints();

        foreach (var point in _colorTree.GetPointRange(0, 0, Width / 2))
        {
            var p = GetPoint(new Pnt(point.X, point.Y));
            DrawGLPoint(p, point.StoredObject.Color);
        }
    }

    private void DrawTriangles()
    {
        ClearLines();

        foreach (var triangle in _delaunay.Triangles)
        {
            Debug.Log("Got triangle: " + triangle);
            var enumerator = triangle.GetEnumerator();
            if (!enumerator.MoveNext()) continue;
            var old = enumerator.Current;
            var first = old;
            while (enumerator.MoveNext())
            {
                var newPoint = enumerator.Current;
                MakeDrawLine(old, newPoint);
                old = newPoint;
            }
            if (first != null && old != null)
            {
                MakeDrawLine(old, first);
            }
        }
    }

    private void MakeDrawLine(Pnt old, Pnt newPoint)
    {
        var oldPoint = transform.TransformPoint(new Vector3((float)old[0], transform.position.y + 0.02f, (float)old[1]));
        var newP = transform.TransformPoint(new Vector3((float)newPoint[0], transform.position.y + 0.02f, (float)newPoint[1]));
        DrawLine(oldPoint, newP);
    }

    private void SeparateMesh()
    {
        if (_colorTree == null) return;

        foreach (var point in _colorTree.GetPointRange(Width / 2, Height / 2, Width / 2))
        {
            var pnt = new Pnt(point.X, point.Y);
            var tri = _delaunay.locate(pnt);
            var tris = _delaunay.surroundingTriangles(pnt, tri);

        }
    }

    public class ColorWrapper
    {
        public Color Color;
    }
}
