using UnityEngine;

public static class BlakeAnimatorHelper
{
    static public float CalculateDirection(Vector3 inVelocity, Transform inTransform)
    {
        if (inVelocity != Vector3.zero)
        {
            Vector3 NormalizedVel = inVelocity.normalized;

            float ForwardCosAngle = Vector3.Dot(inTransform.forward, NormalizedVel);
            float ForwardDeltaDegree = Mathf.Acos(ForwardCosAngle) * Mathf.Rad2Deg;

            float RightCosAngle = Vector3.Dot(inTransform.right, NormalizedVel);
            if (RightCosAngle < 0)
            {
                ForwardDeltaDegree *= -1;
            }
            if(float.IsNaN(ForwardDeltaDegree)) { return 0f; }
            return ForwardDeltaDegree;
        }

        return 0f;
    }
}
