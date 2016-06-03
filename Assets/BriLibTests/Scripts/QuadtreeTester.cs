using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BriLib;

public class QuadtreeTester : MonoBehaviour
{
    public int Width;
    public int Height;
    public Material Material;
    public int PointSize;
    public Color BgColor;
    public Color PointColor;
    public Color BoundsColor;

    private Texture2D _texture;
    private Quadtree<EmptyPoint> _tree;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var point = Input.mousePosition;
            var hit = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(point), out hit, Mathf.Infinity))
            {
                var uv = hit.textureCoord;
                var x = uv.x * Width;
                var y = uv.y * Height;
                _tree.Insert((int)x, (int)y, new EmptyPoint());
                UpdateTexture();
            }
        }
    }

    private void Initialize()
    {
        _tree = new Quadtree<EmptyPoint>(Width / 2, Height / 2, Width / 2, 3);
        _texture = new Texture2D(Width, Height, TextureFormat.RGB24, false) { wrapMode = TextureWrapMode.Clamp };
        DrawBackground();
        UpdateTexture();
    }

    private void UpdateTexture()
    {
        DrawBoundingBoxes();
        DrawPoints();
        _texture.Apply();
        Material.SetTexture("_MainTex", _texture);
    }

    private void DrawBackground()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _texture.SetPixel(x, y, BgColor);
            }
        }
    }

    private void DrawBoundingBoxes()
    {
        foreach (var box in _tree.RecursiveBounds)
        {
            DrawBoundingBox(box);
        }
    }

    private void DrawPoints()
    {
        foreach (var point in _tree.GetPointRange(Width / 2, Height / 2, Width / 2))
        {
            var startX = (int)Mathf.Max(0, point.X - PointSize / 2);
            var endX = (int)Mathf.Min(Width, point.X + PointSize / 2);
            var startY = (int)Mathf.Max(0, point.Y - PointSize / 2);
            var endY = (int)Mathf.Min(Height, point.Y + PointSize / 2);

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    _texture.SetPixel(x, y, PointColor);
                }
            }
        }
    }

    private void DrawBoundingBox(TwoDimensionalBoundingBox bounds)
    {
        var startX = bounds.X - bounds.Radius;
        var endX = bounds.X + bounds.Radius;
        var startY = bounds.Y - bounds.Radius;
        var endY = bounds.Y + bounds.Radius;

        for (var x = startX; x <= endX; x++)
        {
            for (var subX = -1; subX < 2; subX++)
            {
                _texture.SetPixel((int)x, (int)startY + subX, BoundsColor);
                _texture.SetPixel((int)x, (int)endY + subX, BoundsColor);
            }
        }

        for (var y = startY; y <= endY; y++)
        {
            for (var subY = -1; subY < 2; subY++)
            {
                _texture.SetPixel((int)startX + subY, (int)y, BoundsColor);
                _texture.SetPixel((int)endX + subY, (int)y, BoundsColor);
            }
        }
    }

    private void OnGUI()
    {
        var startX = Screen.width / 2 - 50;
        if (GUI.Button(new Rect(startX, 20, 100, 30), "Initialize"))
        {
            Initialize();
        }
    }

    private class EmptyPoint { }
}
