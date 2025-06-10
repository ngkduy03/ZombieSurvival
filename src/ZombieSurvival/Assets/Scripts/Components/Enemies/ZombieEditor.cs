using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor script for visualizing zombie field of view (FOV).
/// </summary>
[CustomEditor(typeof(ZombieComponent))]
public class ZombieEditor : Editor
{
    private void OnSceneGUI()
    {
        ZombieComponent zombie = (ZombieComponent)target;
        var zombieSetting = zombie.GetType().GetField("zombieSetting", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(zombie) as ZombieSetting;

        if (zombieSetting == null)
            return;

        float radius = zombieSetting.Radius;
        float spreadAngle = zombieSetting.SpreadAngle;
        Transform zombieTransform = zombie.transform;
        var zombieCenterPosition = zombieTransform.position + Vector3.up * 1f;

        // Draw the radius as a wire arc
        Handles.color = Color.white;
        Handles.DrawWireArc(zombieCenterPosition, zombieTransform.up, zombieTransform.forward, 360f, radius);

        // Draw the field of view lines based on the spread angle
        Handles.color = Color.yellow;
        Vector3 leftDir = Quaternion.Euler(0, -spreadAngle / 2f, 0) * zombieTransform.forward;
        Vector3 rightDir = Quaternion.Euler(0, spreadAngle / 2f, 0) * zombieTransform.forward;
        Handles.DrawLine(zombieCenterPosition, zombieCenterPosition + leftDir * radius);
        Handles.DrawLine(zombieCenterPosition, zombieCenterPosition + rightDir * radius);
    }
}
