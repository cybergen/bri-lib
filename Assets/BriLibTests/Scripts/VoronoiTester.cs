using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using BriLib;

public class VoronoiTester : MonoBehaviour
{
    public enum State
    {
        None = 0,
        Points = 1,
        VoronoiCells = 2,
        CircumCircles = 3,
        Triangles = 4,
        Broken = 5,
    }

    public int Width;
    public int Height;
    public int VoronoiCount;
    public Material Material;

    private MeshRenderer _renderer;
    private Quadtree<ColorWrapper> _colorTree;
    private State _currentState;
    private Texture2D _texture;

    private void Start()
    {
        _renderer = gameObject.GetComponent<MeshRenderer>();
    }

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

    private void AdvanceToState(State v)
    {
        if ((int)v == (int)_currentState)
        {
            return;
        }
        else if ((int)v < (int)_currentState)
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
        switch (state)
        {
            case State.Points: SetPoints(); break;
            case State.VoronoiCells: AddVoronoiCells(); break;
            case State.CircumCircles: AddCircumCircles(); break;
            case State.Triangles: CalculateTriangles(); break;
            case State.Broken: SeparateMesh(); break;
        }
        _currentState = state;
    }

    private void SetPoints()
    {
        _colorTree = new Quadtree<ColorWrapper>(Width / 2, Height / 2, Width / 2, 5);
        var _rand = new System.Random();
        for (int i = 0; i < VoronoiCount; i++)
        {
            var r = _rand.Next(256) / 256f;
            var g = _rand.Next(256) / 256f;
            var b = _rand.Next(256) / 256f;
            var color = new Color(r, g, b);
            var wrapper = new ColorWrapper { Color = color };

            var x = _rand.Next(Width);
            var y = _rand.Next(Width);

            _colorTree.Insert(x, y, wrapper);
        }

        _texture = new Texture2D(Width, Height, TextureFormat.RGB24, false) { wrapMode = TextureWrapMode.Clamp };

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _texture.SetPixel(x, y, Color.white);

                var colors = _colorTree.GetRange(x, y, 2);
                foreach (var color in colors)
                {
                    _texture.SetPixel(x, y, color.Color);
                }
            }
        }

        _texture.Apply();
        Material.SetTexture("_MainTex", _texture);
    }

    private void AddVoronoiCells()
    {
        throw new NotImplementedException();
    }

    private void AddCircumCircles()
    {
        throw new NotImplementedException();
    }

    private void CalculateTriangles()
    {
        throw new NotImplementedException();
    }

    private void SeparateMesh()
    {
        throw new NotImplementedException();
    }

    private class ColorWrapper
    {
        public Color Color;
    }
}
