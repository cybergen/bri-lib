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

    private MeshRenderer _renderer;
    private Quadtree<ColorWrapper> _colorTree;
    private State _currentState;

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
        _currentState = state;
    }

    private class ColorWrapper
    {
        public Color color;
    }
}
