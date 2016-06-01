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
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var colors = _tree.GetRange(x, y, PointSize);
                foreach (var color in colors)
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
            _texture.SetPixel((int)x, (int)startY, BoundsColor);
            _texture.SetPixel((int)x, (int)endY, BoundsColor);
        }

        for (var y = startY; y <= endY; y++)
        {
            _texture.SetPixel((int)startX, (int)y, BoundsColor);
            _texture.SetPixel((int)endX, (int)y, BoundsColor);
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
