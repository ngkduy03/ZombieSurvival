#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor script for visualize enemy fov. 
/// </summary>
[CustomEditor(typeof(ShotgunComponent))]
public class ShotgunRangeEditor : Editor
{
    private void OnSceneGUI()
    {
        ShotgunComponent shotgun = (ShotgunComponent)target;
        var gunSetting = shotgun.GetType().GetField("gunSetting", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(shotgun) as GunSetting;
        var pelletSpawnPoint = shotgun.GetType().GetField("pelletSpawnPoint", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(shotgun) as Transform;
        if (gunSetting == null || pelletSpawnPoint == null)
            return;

        float radius = gunSetting.Radius;
        float spreadAngle = gunSetting.SpreadAngle;

        Handles.color = Color.white;
        Handles.DrawWireArc(pelletSpawnPoint.position, pelletSpawnPoint.up, pelletSpawnPoint.forward, 360f, radius);

        Handles.color = Color.yellow;
        Vector3 leftDir = Quaternion.Euler(0, -spreadAngle / 2f, 0) * pelletSpawnPoint.forward;
        Vector3 rightDir = Quaternion.Euler(0, spreadAngle / 2f, 0) * pelletSpawnPoint.forward;
        Handles.DrawLine(pelletSpawnPoint.position, pelletSpawnPoint.position + leftDir * radius);
        Handles.DrawLine(pelletSpawnPoint.position, pelletSpawnPoint.position + rightDir * radius);
    }
}
#endif