using UnityEngine;

namespace AillieoUtils {

	public static class VectorExt
	{
		
        public static Vector3 SetX(this Vector3 v3, float x)
		{
			return new Vector3(x, v3.y, v3.z);
		}
        public static Vector3 SetY(this Vector3 v3, float y)
		{
            return new Vector3(v3.x, y, v3.z);
		}
        public static Vector3 SetZ(this Vector3 v3, float z)
		{
            return new Vector3(v3.x, v3.y, z);
		}
       
        public static Vector2 SetX(this Vector2 v2, float x)
        {
            return new Vector2(x, v2.y);
        }
        public static Vector3 SetY(this Vector2 v2, float y)
        {
            return new Vector3(v2.x, y);
        }

	}
}
