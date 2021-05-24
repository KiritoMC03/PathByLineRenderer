using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Path
{
    [Serializable]
    public class Path
    {
        [HideInInspector]
        [SerializeField] private List<Vector2> points;

        [SerializeField] private bool isClosed;

        public Vector2 this[int i] => points[i];
        public int NumPoints => points.Count;
        public int NumSegments => points.Count / 3;

        public Path(Vector2 centre)
        {
            points = new List<Vector2>()
            {
                centre + Vector2.left,
                centre + (Vector2.left + Vector2.up) * 0.5f,
                centre + (Vector2.right + Vector2.down) * 0.5f,
                centre + Vector2.right
            };
        }
        
        public void AddSegment(Vector2 anchorPosition)
        {
            points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
            points.Add((points[points.Count - 1] + anchorPosition) * 0.5f);
            points.Add(anchorPosition);
        }

        public void DeleteSemgent(int anchorIndex)
        {
            if (NumSegments <= 2 || isClosed && NumSegments <= 1)
            {
                return;
            }
            
            if (anchorIndex == 0)
            {
                if (isClosed)
                {
                    points[points.Count-1] = points[2];
                }
                points.RemoveRange(0,3);
            }
            else if (anchorIndex == points.Count - 1 && !isClosed)
            {
                points.RemoveRange(anchorIndex-2,3);
            }
            else
            {
                points.RemoveRange(anchorIndex-1, 3);
            }
        }

        public Vector2[] GetPointsInSegment(int i)
        {
            return new Vector2[]
            {
                points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[LoopIndex(i * 3 + 3)]
            };
        }

        public void MovePoint(int i, Vector2 position)
        {
            Vector2 deltaMove = position - points[i];
            points[i] = position;

            if (i % 3 == 0)
            {
                if (i + 1 < points.Count || isClosed)
                {
                    points[LoopIndex(i + 1)] += deltaMove;
                }

                if (i - 1 >= 0 || isClosed)
                {
                    points[LoopIndex(i - 1)] += deltaMove;
                }
            }
            else
            {
                bool nextPointIsAnchor = (i + 1) % 3 == 0;
                int correspondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
                int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;

                if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count || isClosed)
                {
                    float distance = (points[LoopIndex(anchorIndex)] - points[LoopIndex(correspondingControlIndex)]).magnitude;
                    Vector2 dir = (points[LoopIndex(anchorIndex)] - position).normalized;
                    points[LoopIndex(correspondingControlIndex)] = points[LoopIndex(anchorIndex)] + dir * distance;
                }
            }
        }
        
        public void ToggleClosed()
        {
            isClosed = !isClosed;

            if (isClosed)
            {
                points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
                points.Add(points[0] * 2 - points[1]);
            }
            else
            {
                points.RemoveRange(points.Count - 2, 2);
            }
        }

        private int LoopIndex(int i)
        {
            return (i + points.Count) % points.Count;
        }
    }
}