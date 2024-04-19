using _Project.Scripts;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyFOV))]
public class FieldOfViewEditor : Editor
{
    private GameObject playerRef;

    private void Awake()
    {
        playerRef = ReferenceManager.BlakeHeroCharacter?.gameObject;
    }

    private void OnSceneGUI()
    {
        EnemyFOV fov = (EnemyFOV)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.Radius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.Angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.Angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.Radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.Radius);

        if (fov.CanSeePlayer && playerRef != null)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, playerRef.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
