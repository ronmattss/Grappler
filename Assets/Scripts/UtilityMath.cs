namespace DefaultNamespace

{
    using System;
    using UnityEngine;
    public class UtilityMath
    {
        
        // create Singleton Class
        private  UtilityMath _instance;
        public  UtilityMath Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UtilityMath();
                }
                return _instance;
            }
        }
        
        // angle Between 2 Vector3
        public float AngleBetweenVector3(Vector3 v1, Vector3 v2)
        {
            return Mathf.Acos(Vector3.Dot(v1.normalized, v2.normalized)) * Mathf.Rad2Deg;
        }
        
        // angle Between 2 Vector2
        public float AngleBetweenVector2(Vector2 v1, Vector2 v2)
        {
            return Mathf.Acos(Vector2.Dot(v1.normalized, v2.normalized)) * Mathf.Rad2Deg;
        }
        
        // distance between 2 Vector3
        public float DistanceBetweenVector3(Vector3 v1, Vector3 v2)
        {
            return Vector3.Distance(v1, v2);
        }
        
        // distance between 2 Vector2
        public float DistanceBetweenVector2(Vector2 v1, Vector2 v2)
        {
            return Vector2.Distance(v1, v2);
        }
        
        // Sets the position from the maximum distance
        Vector3 SetPositionViaDistance(Vector3 point1, Vector3 point2, float distance)
        {
            return point1 + (point2 - point1).normalized * distance;
        }



        

        
    }
}