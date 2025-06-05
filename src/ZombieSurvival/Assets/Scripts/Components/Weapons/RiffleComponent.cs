using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RiffleComponent is a component that manages the riffle weapon in the game.
/// It handles the firing mechanism, bullet spawning, and ammo management.
/// </summary>
public class RiffleComponent : SceneComponent<RiffleController>
{
    [SerializeField]
    private GunSetting gunSetting;
    [SerializeField]
    private Transform bulletSpawnPoint;

    [SerializeField]
    private BulletComponent bulletInstance;

    [SerializeField]
    private Transform poolHolder;

    private RiffleController controller;

    protected override RiffleController CreateControllerImpl()
    {
        controller = new RiffleController(gunSetting, bulletSpawnPoint, bulletInstance);
        return controller;
    }
}
