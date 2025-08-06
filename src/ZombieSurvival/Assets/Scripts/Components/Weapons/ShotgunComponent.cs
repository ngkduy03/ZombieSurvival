using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// ShotgunComponent is a component that manages the shotgun weapon in the game.
/// It handles the firing mechanism, pellet spawning, and ammo management.
/// </summary>
public class ShotgunComponent : SceneComponent<ShotgunController>
{
    [SerializeField]
    [Expandable]
    private GunSetting gunSetting;

    [SerializeField]
    private Transform pelletSpawnPoint;

    [SerializeField]
    private AudioSource audioSource;

    private ShotgunController controller;

    protected override ShotgunController CreateControllerImpl()
    {
        controller = new ShotgunController(transform, gunSetting, pelletSpawnPoint, audioSource);
        return controller;
    }

}
