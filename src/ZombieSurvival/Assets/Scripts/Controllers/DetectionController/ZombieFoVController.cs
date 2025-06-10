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

    public ZombieFoVController(
        Transform transform,
        float spreadAngle,
        float radius,
        LayerMask playerMask)
    {
        this.transform = transform;
        this.spreadAngle = spreadAngle;
        this.radius = radius;
        this.playerMask = playerMask;
    }

    public ZombieFoVController(
        Transform transform,
        ZombieSetting zombieSetting,
        LayerMask playerMask)
    {
        this.transform = transform;
        spreadAngle = zombieSetting.SpreadAngle;
        radius = zombieSetting.Radius;
        this.playerMask = playerMask;
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
        if (Physics.OverlapSphereNonAlloc(transform.position, radius, playerCheck, playerMask, QueryTriggerInteraction.Collide) > 0)
        {
            if (playerCheck[0].TryGetComponent<PlayerComponent>(out var playerComponent))
            {
                return playerCheck[0].transform;
            }
        }

        return null;
    }
}
