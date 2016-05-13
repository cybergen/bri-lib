using System;
using System.Collections.Generic;

namespace BriLib
{
    public class Quadtree<T>
    {
        public enum Quadrant
        {
            NorthWest = 0,
            NorthEast = 1,
            SouthEast = 2,
            SouthWest = 3,
        }

        private int _maxObjectsPerNode;
        private TwoDimensionalBoundingBox _boundingBox;
        private List<Point<T>> _children;
        private Quadtree<T>[] _subtrees;
        
        public Quadtree(TwoDimensionalBoundingBox boundingBox, int maxObjectsPerNode)
        {
            _maxObjectsPerNode = maxObjectsPerNode;
            _boundingBox = boundingBox;
            _children = new List<Point<T>>();
            _subtrees = new Quadtree<T>[4];
        }

        public Quadtree(float centerX, float centerY, float radius, int maxObjectsPerNode)
            : this(new TwoDimensionalBoundingBox(centerX, centerY, radius), maxObjectsPerNode) { }

        public Quadrant GetQuadrant(float x, float y)
        {
            if (x < _boundingBox.X && y >= _boundingBox.Y) return Quadrant.NorthEast;
            if (x >= _boundingBox.X && y >= _boundingBox.Y) return Quadrant.NorthWest;
            if (x < _boundingBox.X && y < _boundingBox.Y) return Quadrant.SouthWest;
            return Quadrant.SouthEast;
        }

        public void Insert(float x, float y, T obj)
        {
            if (_children.Count >= _maxObjectsPerNode)
            {
                Subdivide();
                _subtrees[(int)GetQuadrant(x, y)].Insert(x, y, obj);
            }
            else
            {
                _children.Add(new Point<T>(x, y, obj));
            }
        }

        public IEnumerable<T> GetRange(float x, float y, float radius)
        {
            return GetRange(new TwoDimensionalBoundingBox(x, y, radius));
        }

        public IEnumerable<T> GetRange(TwoDimensionalBoundingBox bounds)
        {
            for (int i = 0; i < _children.Count; i++)
            {
                if (bounds.Intersects(_children[i].X, _children[i].Y))
                {
                    yield return _children[i].StoredObject;
                }
            }

            for (int i = 0; i < _subtrees.Length; i++)
            {
                if (_subtrees[i] != null && _subtrees[i]._boundingBox.Intersects(bounds))
                {
                    for (var enumerator = _subtrees[i].GetRange(bounds).GetEnumerator(); enumerator.MoveNext();)
                    {
                        yield return enumerator.Current;
                    }
                }
            }
        }

        public T GetNearestNeighbor(float x, float y)
        {
            throw new NotImplementedException();
        }

        private void Subdivide()
        {
            var quarterRadius = _boundingBox.Radius / 2;
            var x = _boundingBox.X;
            var y = _boundingBox.Y;

            var nwBox = new TwoDimensionalBoundingBox(x - quarterRadius, y + quarterRadius, quarterRadius);
            _subtrees[(int)Quadrant.NorthWest] = new Quadtree<T>(nwBox, _maxObjectsPerNode);

            var neBox = new TwoDimensionalBoundingBox(x + quarterRadius, y + quarterRadius, quarterRadius);
            _subtrees[(int)Quadrant.NorthEast] = new Quadtree<T>(neBox, _maxObjectsPerNode);

            var seBox = new TwoDimensionalBoundingBox(x + quarterRadius, y - quarterRadius, quarterRadius);
            _subtrees[(int)Quadrant.SouthEast] = new Quadtree<T>(seBox, _maxObjectsPerNode);

            var swBox = new TwoDimensionalBoundingBox(x - quarterRadius, y - quarterRadius, quarterRadius);
            _subtrees[(int)Quadrant.SouthWest] = new Quadtree<T>(swBox, _maxObjectsPerNode);

            for (int i = 0; i < _children.Count; i++)
            {
                var childX = _children[i].X;
                var childY = _children[i].Y;
                _subtrees[(int)GetQuadrant(childX, childY)].Insert(childX, childY, _children[i].StoredObject);
            }

            _children = new List<Point<T>>();
        }

        private struct Point<K>
        {
            public K StoredObject;
            public float X;
            public float Y;

            public Point(float x, float y, K obj)
            {
                X = x;
                Y = y;
                StoredObject = obj;
            }
        }        
    }
}
