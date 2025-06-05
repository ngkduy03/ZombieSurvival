using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject for storing gun settings.
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Weapon/GunSetting", order = 1)]
public class GunSetting : ScriptableObject
{
    [field: SerializeField]
    public int TotalAmmo { get; private set; }

    [field: SerializeField]
    public int MagazineSize { get; private set; }

    [field: SerializeField]
    public float ReloadTime { get; private set; }

    [field: SerializeField]
    public float FireRate { get; private set; }

    [field: SerializeField]
    public BulletComponent BulletPrefab { get; private set; }
}
