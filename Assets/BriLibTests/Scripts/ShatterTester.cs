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

    protected override void OnMouseClick(int x, int y)
    {
        base.OnMouseClick(x, y);

        Debug.Log("Got mouse click at x: " + x + ", y: " + y);

        if (_colorTree == null) return;

        var _rand = new System.Random();
        var r = _rand.Next(256) / 256f;
        var g = _rand.Next(256) / 256f;
        var b = _rand.Next(256) / 256f;
        var color = new Color(r, g, b);
        var wrapper = new ColorWrapper { Color = color };

        _colorTree.Insert(x, y, wrapper);
    }

    protected override void OnMouseClickWorld(Vector3 point)
    {
        base.OnMouseClickWorld(point);
        var localPoint = transform.InverseTransformPoint(point);
        _delaunay.delaunayPlace(new Pnt(localPoint.x, localPoint.z));
    }

    protected override void Initialize()
    {
        base.Initialize();
        _colorTree = new Quadtree<ColorWrapper>(Width / 2, Height / 2, Width / 2, 5);
        _voronoi = new VoronoiDiagram();
        _pointMap = new Dictionary<ColorWrapper, BriLib.Point>();
        var tri = new Triangle(
                new Pnt(-10000, -10000),
                new Pnt(10000, -10000),
                new Pnt(Width / 2, 10000)
            );
        _delaunay = new Triangulation(tri);

        //_delaunay = new Triangulation();
        //var verts = MeshFilter.mesh.vertices;
        //var tris = MeshFilter.mesh.triangles;

        //var triList = new List<Triangle>();

        //for (int i = 0; i < tris.Length; i += 3)
        //{
        //    var s = "[Point=X:" + verts[tris[i]].x + ",Y:" + verts[tris[i]].y + ",Z:" + verts[tris[i]].z + "]";
        //    s += ",[Point=X:" + verts[tris[i + 1]].x + ",Y:" + verts[tris[i + 1]].y + ",Z:" + verts[tris[i + 1]].z + "]";
        //    s += ",[Point=X:" + verts[tris[i + 2]].x + ",Y:" + verts[tris[i + 2]].y + ",Z:" + verts[tris[i + 2]].z + "]";
        //    Debug.Log("Got tri with points: " + s);
        //    var pnt = new Pnt(verts[tris[i]].x, verts[tris[i]].z);
        //    var pnt2 = new Pnt(verts[tris[i + 1]].x, verts[tris[i + 1]].z);
        //    var pnt3 = new Pnt(verts[tris[i + 2]].x, verts[tris[i + 2]].z);
        //    var tri = new Triangle(pnt, pnt2, pnt3);
        //    triList.Add(tri);
        //}

        //_delaunay.AddExistingTriangles(triList);

        UpdateTexture();
    }

    public void drawAllVoronoi(boolean withFill, boolean withSites)
    {
        // Keep track of sites done; no drawing for initial triangles sites
        HashSet<Pnt> done = new HashSet<Pnt>(initialTriangle);
        for (Triangle triangle : dt)
            for (Pnt site: triangle)
            {
                if (done.contains(site)) continue;
                done.add(site);
                List<Triangle> list = dt.surroundingTriangles(site, triangle);
                Pnt[] vertices = new Pnt[list.size()];
                int i = 0;
                for (Triangle tri: list)
                    vertices[i++] = tri.getCircumcenter();
                draw(vertices, withFill ? getColor(site) : null);
                if (withSites) draw(site);
            }
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
        var oldPoint = transform.TransformPoint(new Vector3((float)old[0], this.transform.position.y + 0.01f, (float)old[1]));
        var newP = transform.TransformPoint(new Vector3((float)newPoint[0], this.transform.position.y + 0.01f, (float)newPoint[1]));
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
