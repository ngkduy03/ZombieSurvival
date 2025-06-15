using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ZombieFoVController class implements the IDetectionController interface for zombie field of view detection.
/// </summary>
public class ZombieFoVController : ControllerBase, IDetectionController
{
    private Transform transform;
    private float spreadAngle;
    private float radius;
    private LayerMask playerMask;
    private Collider[] playerCheck = new Collider[1];
    private Vector3 centerPos => transform.position + Vector3.up * 1f;

    public ZombieFoVController(
        Transform transform,
        ZombieSetting zombieSetting)
    {
        this.transform = transform;
        spreadAngle = zombieSetting.SpreadAngle;
        radius = zombieSetting.Radius;
        playerMask = zombieSetting.PlayerMask;
    }

    /// <inheritdoc />
    public bool CheckInRange()
    {
        var player = GetPlayerComponent();

        if (player == null)
        {
            return false;
        }

        if (player.playerController.GetAttackStatus())
        {
            return true;
        }

        // Check if the player is within the spread angle of the zombiePos
        var ToPlayerDirection = player.transform.position - transform.position;
        ToPlayerDirection.y = 0;
        if (Vector3.Angle(transform.forward, ToPlayerDirection) < spreadAngle / 2f)
        {
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public Transform GetTransform()
    {
        return transform;
    }

    /// <inheritdoc />
    public PlayerComponent GetPlayerComponent()
    {
        if (Physics.OverlapSphereNonAlloc(centerPos, radius, playerCheck, playerMask, QueryTriggerInteraction.Collide) > 0)
        {
            if (playerCheck[0].TryGetComponent<PlayerComponent>(out var playerComponent))
            {
                return playerComponent;
            }
        }

        return null;
    }

    protected override void Dispose(bool isDispose)
    {
        radius = 0f;
        spreadAngle = 0f;
    }
}
