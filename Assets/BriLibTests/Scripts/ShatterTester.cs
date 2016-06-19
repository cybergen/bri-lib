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
    public int PointSize;
    public int IntersectionPointSize;
    public Color CircleColor;
    public Color IntersectionColor;
    public Color TriangleColor;
    public MeshFilter MeshFilter;

    private Quadtree<ColorWrapper> _colorTree;
    private VoronoiDiagram _voronoi;
    private Dictionary<ColorWrapper, BriLib.Point> _pointMap;
    private Triangulation _delaunay;
    private State _currentState;

    private void OnGUI()
    {
        var startX = 20;
        var width = 100;
        var startY = Screen.height - 70;
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

    protected override void OnMouseClick(int x, int y)
    {
        base.OnMouseClick(x, y);

        if (_colorTree == null) return;

        var _rand = new System.Random();
        var r = _rand.Next(256) / 256f;
        var g = _rand.Next(256) / 256f;
        var b = _rand.Next(256) / 256f;
        var color = new Color(r, g, b);
        var wrapper = new ColorWrapper { Color = color };
        
        _colorTree.Insert(x, y, wrapper);
        _delaunay.delaunayPlace(new Pnt(x, y));
    }

    protected override void Initialize()
    {
        base.Initialize();
        _colorTree = new Quadtree<ColorWrapper>(Width / 2, Height / 2, Width / 2, 5);
        _voronoi = new VoronoiDiagram();
        _pointMap = new Dictionary<ColorWrapper, BriLib.Point>();
        var points = new List<Pnt>
        {
            new Pnt(-10000, -10000),
            new Pnt(10000, -10000),
            new Pnt(0, 10000)
        };
        _delaunay = new Triangulation(new Triangle(points));
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

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var point = _colorTree.GetNearestNeighbor(x, y);
                if (point == null) continue;
                _texture.SetPixel(x, y, point.Color);
            }
        }
    }

    private void DrawPoints()
    {
        if (_colorTree == null) return;

        foreach (var point in _colorTree.GetPointRange(Width / 2, Height / 2, Width / 2))
        {
            DrawPoint((int)point.X, (int)point.Y, PointSize, point.StoredObject.Color);
            if (_pointMap.ContainsKey(point.StoredObject)) continue;

            var voronoiPoint = new BriLib.Point(point.X, point.Y);
            _voronoi.AddVoronoiFacePoint(voronoiPoint);
            _pointMap.Add(point.StoredObject, voronoiPoint);
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
        foreach (var triangle in _delaunay.Triangles)
        {
            Debug.Log("Got triangle: " + triangle);
            var enumerator = triangle.GetEnumerator();
            if (!enumerator.MoveNext()) continue;
            var old = enumerator.Current;
            while (enumerator.MoveNext())
            {
                var newPoint = enumerator.Current;
                DrawLine(old[0], old[1], newPoint[0], newPoint[1], TriangleColor);
                old = newPoint;
            }
        }
    }

    private void SeparateMesh()
    {
        throw new NotImplementedException();
    }

    public class ColorWrapper
    {
        public Color Color;
    }
}
