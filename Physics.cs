using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Vector3 = OpenToolkit.Mathematics.Vector3;

namespace RedstoneSim
{
    public class Physics
    {
        public static List<Tuple<Vector3, Vector3>> CubePointNorm = new List<Tuple<Vector3, Vector3>>
        {
            //Point on surface, normal
            //front
            Tuple.Create(new Vector3(0, 0, 0.5f), new Vector3(0, 0, 1)),
            //top
            Tuple.Create(new Vector3(0, 0.5f, 0), new Vector3(0, 1, 0)),
            //left
            Tuple.Create(new Vector3(-0.5f, 0, 0), new Vector3(-1, 0, 0)),
            //right
            Tuple.Create(new Vector3(0.5f, 0, 0), new Vector3(1, 0, 0)),
            //back
            Tuple.Create(new Vector3(0, 0, -0.5f), new Vector3(0, 0, -1)),
            //bottom
            Tuple.Create(new Vector3(0, -0.5f, 0), new Vector3(0, -1, 0)),
        };

        public static Tuple<bool, Vector3> RayPlaneIntersect(Vector3 origin, Vector3 ray, Vector3 point, Vector3 norm)
        {
            Vector3 w = point - origin;

            float k = Vector3.Dot(w, norm) / Vector3.Dot(ray, norm);

            Vector3 intersect = origin + k * ray;
            Tuple<bool, Vector3> _return = new Tuple<bool, Vector3>(k >= 0.0f && k <= 1.0f, intersect);
            return _return;
        }

        public static List<Tuple<float, Vector3i>> RayBlockIntersection(Vector3 origin, Vector3 ray)
        {
            int[] sign = new int[]
            {
                (Convert.ToInt32(ray.X <= 0)*2)-1,
                (Convert.ToInt32(ray.Y <= 0)*2)-1,
                (Convert.ToInt32(ray.Z <= 0)*2)-1,
            };
            //Search 5x5x5 area to intersect, order by distance
            //assume ray starts at 0,0,0 +- origin % 1

            Vector3 rayStart = new Vector3(origin.X % 1.0f, origin.Y % 1.0f, origin.Z % 1.0f);
            Vector3 positiveRay = new Vector3(ray.X * sign[0], ray.Y * sign[1], ray.Z * sign[2]);

            List<Tuple<float, Vector3i>> intersections = new List<Tuple<float, Vector3i>>();
            
            
            //Vector3i intersectPosition;
            for (int _y = 0; _y < 6; _y++)
            {
                for (int _z = 0; _z < 6; _z++)
                {
                    for (int _x = 0; _x < 6; _x++)
                    {
                        Tuple<bool, Vector3> _result = new Tuple<bool, Vector3>(false, new Vector3(0, 0, 0));
                        foreach (var tup in CubePointNorm)
                        {
                            _result = RayPlaneIntersect(rayStart, positiveRay, tup.Item1, tup.Item2);

                            if (!_result.Item1)
                            {
                                break;
                            }
                        }
                        //If we looped through all these we have a hit for this block!
                        //Recreate block position in worldorigin() + 
                        Vector3i blockHit = new Vector3i(_x * sign[0] + (int)origin.X , _y * sign[1] + (int)origin.Y, _z * sign[2] + (int)origin.Z);
                        intersections.Add(Tuple.Create((origin - _result.Item2).LengthSquared, new Vector3i(_x, _y, _z)));
                    }
                }
            }
            return intersections;
        }
    }
}
