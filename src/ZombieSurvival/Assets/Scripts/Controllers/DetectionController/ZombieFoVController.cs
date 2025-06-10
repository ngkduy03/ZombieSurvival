using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        var playerTransform = GetPlayerTransform();

        if (playerTransform == null)
        {
            return false;
        }

        // Check if the player is within the spread angle of the zombiePos
        var ToPlayerDirection = playerTransform.position - transform.position;
        ToPlayerDirection.y = 0;
        if (Vector3.Angle(transform.forward, ToPlayerDirection) < spreadAngle / 2f)
        {
            return true;
        }
        return false;
    }

    /// <inheritdoc />
    public Transform GetPlayerTransform()
    {
        if (Physics.OverlapSphereNonAlloc(centerPos, radius, playerCheck, playerMask, QueryTriggerInteraction.Collide) > 0)
        {
            if (playerCheck[0].TryGetComponent<PlayerComponent>(out var playerComponent))
            {
                return playerComponent.transform;
            }
        }

        return null;
    }
}
