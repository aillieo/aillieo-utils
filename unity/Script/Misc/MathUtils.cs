using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils
{
    public static class MathUtils
    {

        public static int LineCircleIntersect(Vector2 pointA, Vector2 pointB, Vector2 center, float radius, List<Vector2> points)
        {
            points.Clear();

            float baX = pointB.x - pointA.x;
            float baY = pointB.y - pointA.y;
            float caX = center.x - pointA.x;
            float caY = center.y - pointA.y;

            float a = baX * baX + baY * baY;
            float b = baX * caX + baY * caY;
            float c = caX * caX + caY * caY - radius * radius;

            float p = b / a;
            float q = c / a;

            float delta = p * p - q;
            if (delta < 0)
            {
                // 无交点
                return 0;
            }

            float deltaSqrt = Mathf.Sqrt(delta);
            float s1 = -p + deltaSqrt;
            float s2 = -p - deltaSqrt;

            // 注意 p1和pointA的距离更近 可以认为p1是进入的点 p2出

            Vector2 p1 = new Vector2(pointA.x - baX * s1, pointA.y - baY * s1);
            points.Add(p1);
            if (delta == 0)
            {
                // 相切
                return 1;
            }

            // 相交
            Vector2 p2 = new Vector2(pointA.x - baX * s2, pointA.y - baY * s2);
            points.Add(p2);
            return 2;
        }



        static void ArchToPoints(Vector2 center, float radius, float inAngle, float outAngle, float step, List<Vector2> points)
        {
            // 确保 in out的角度差值 在±180 以内
            if (outAngle - inAngle > Mathf.PI)
            {
                outAngle -= 2 * Mathf.PI;
            }
            else if (outAngle - inAngle < -Mathf.PI)
            {
                outAngle += 2 * Mathf.PI;
            }

            float deltaAngle = outAngle - inAngle;
            int segCount = Mathf.CeilToInt(Mathf.Abs(deltaAngle) * radius / step);

            if (segCount > 0)
            {
                float angleStep = deltaAngle / segCount;
                for (int i = 0; i < segCount + 1; i++)
                {
                    Vector2 p = new Vector2(
                        center.x + (radius * Mathf.Cos(inAngle + i * angleStep)),
                        center.y + (radius * Mathf.Sin(inAngle + i * angleStep)));
                    points.Add(p);
                }
            }
        }

        public static float PointLineDis(Vector3 p, Vector3 lineP1, Vector3 lineP2)
        {
            return Mathf.Sqrt(PointLineDisSqr(p, lineP1, lineP2));
        }

        public static float PointLineDisSqr(Vector3 p, Vector3 lineP1, Vector3 lineP2)
        {
            Vector3 crs = Vector3.Cross(p - lineP1, p - lineP2);
            return crs.sqrMagnitude / (lineP1 - lineP2).sqrMagnitude;
        }


    }
}

