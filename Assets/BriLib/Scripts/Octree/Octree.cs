using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BriLib
{
    public class Octree<T>
    {
        public enum Octant
        {
            TopLeftBack = 0,
            TopLeftFront = 1,
            BottomLeftBack = 2,
            BottomLeftFront = 3,
            TopRightBack = 4,
            TopRightFront = 5,
            BottomRightBack = 6,
            BottomRightFront = 7,
        }

        private int _maxObjectsPerNode;
        private ThreeDimensionalBoundingBox _bounds;
        private List<Point<T>> _children;
        private Octree<T>[] _subtrees;

        public Octree(ThreeDimensionalBoundingBox bounds, int maxObjsPerNode)
        {
            _maxObjectsPerNode = maxObjsPerNode;
            _bounds = bounds;
            _children = new List<Point<T>>();
            _subtrees = new Octree<T>[8];
        }

        public Octree(float centerX, float centerY, float centerZ, float radius, int maxObjs)
            : this(new ThreeDimensionalBoundingBox(centerX, centerY, centerZ, radius), maxObjs) { }

        public Octant GetOctant(float x, float y, float z)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(float x, float y, float z, T obj)
        {
            if (_children.Count >= _maxObjectsPerNode)
            {
                Subdivide();
                _subtrees[(int)GetOctant(x, y, z)].Insert(x, y, z, obj);
            }
            else
            {
                _children.Add(new Point<T>(x, y, z, obj));
            }
        }

        public IEnumerable<T> GetRange(float x, float y, float z, float radius)
        {
            return GetRange(new ThreeDimensionalBoundingBox(x, y, z, radius));
        }

        public IEnumerable<T> GetRange(ThreeDimensionalBoundingBox bounds)
        {
            for (int i = 0; i < _children.Count; i++)
            {
                if (bounds.Intersects(_children[i].X, _children[i].Y, _children[i].Z))
                {
                    yield return _children[i].StoredObject;
                }
            }

            for (int i = 0; i < _subtrees.Length; i++)
            {
                if (_subtrees[i] == null || !_subtrees[i]._bounds.Intersects(bounds)) continue;
                for (var enumerator = _subtrees[i].GetRange(bounds).GetEnumerator(); enumerator.MoveNext();)
                {
                    yield return enumerator.Current;
                }
            }
        }

        public T GetNearestNeighbor(float x, float y, float z)
        {
            throw new System.NotImplementedException();
        }

        private void Subdivide()
        {
            throw new System.NotImplementedException();
        }
        
        private struct Point<K>
        {
            public float X;
            public float Y;
            public float Z;
            public K StoredObject;

            public Point(float x, float y, float z, K obj)
            {
                X = x;
                Y = y;
                Z = z;
                StoredObject = obj;
            }
        }
    }
}
