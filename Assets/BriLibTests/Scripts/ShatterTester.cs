using System;
using System.Collections.Generic;
using UnityEngine;
using BriLib;
using Point = BriLib.TwoDimensionalPoint<ShatterTester.ColorWrapper>;
using PointCollection = BriLib.IObservableCollection<BriLib.TwoDimensionalPoint<ShatterTester.ColorWrapper>>;

public class ShatterTester : TextureWriteTester
{
    public enum State
    {
        None = 0,
        Points = 1,
        VoronoiCells = 2,
        CircumCircles = 3,
        IntersectionPoints = 4,
        Triangles = 5,
        Broken = 6,
    }

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
    private State _currentState;

    private void OnGUI()
    {
        var startX = 20;
        var width = 100;
        var startY = Screen.height - 30;
        var height = 30;
        var currentX = startX;

        var enumValues = Enum.GetValues(typeof(State));
        var enumNames = Enum.GetNames(typeof(State));

        for (int i = 1; i < enumValues.Length; i++)
        {
            if (GUI.Button(new Rect(currentX, startY, width, height), enumNames[i]))
            {
                AdvanceToState((State)enumValues.GetValue(i));
            }

            currentX += width + startX;
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

    private void AdvanceToState(State v)
    {
        if ((int)v <= (int)_currentState)
        {
            _currentState = State.None;
            AdvanceToState(v);
        }
        else
        {
            while ((int)_currentState < (int)v)
            {
                var newState = (int)_currentState + 1;
                SetState((State)newState);
            }
        }
    }

    private void SetState(State state)
    {
        Debug.Log("Setting state to " + state);
        if (state == State.Points) Initialize();
        _currentState = state;
        UpdateTexture();
        if (state == State.Broken) SeparateMesh();
    }

    protected override void UpdateTexture()
    {
        DrawBackground();
        DrawPoints();
        if ((int)_currentState >= (int)State.VoronoiCells) DrawVoronoi();
        if ((int)_currentState >= (int)State.CircumCircles) DrawCircumcircles();
        if ((int)_currentState >= (int)State.IntersectionPoints) ShowIntersections();
        if ((int)_currentState >= (int)State.Triangles) DrawTriangles();
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

    private void DrawCircumcircles()
    {
        //foreach (var circle in _voronoi.Circumcircles)
        //{
        //    DrawCircle(circle.Center.Coordinates[0], circle.Center.Coordinates[1], circle.Radius, CircleColor);
        //}
    }

    private void ShowIntersections()
    {
        //foreach (var intersection in _voronoi.IntersectionPoints)
        //{
        //    DrawPoint(
        //        (int)intersection.Coordinates[0], 
        //        (int)intersection.Coordinates[1],  
        //        IntersectionPointSize, 
        //        IntersectionColor);
        //}
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
