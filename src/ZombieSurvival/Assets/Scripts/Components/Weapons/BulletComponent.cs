using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// BulletComponent is a component that manages the behavior of a bullet in the game.
/// It handles the bullet's movement, expiration, and deactivation.
/// </summary>
public class BulletComponent : MonoBehaviour
{
    [SerializeField]
    private float speed;

    /// <summary>
    /// If the bullet is deactivated or not.
    /// </summary>
    public bool IsDeactivated { get; set; }

    /// <summary>
    /// If the bullet is deactivated or not.
    /// </summary>
    [field: SerializeField]
    public float Damage { get; set; }

    private CancellationTokenSource cts = new();
    private const int ExpiredTime = 5;

    private void OnEnable()
    {
        ExpireBullet(cts.Token).Forget();
    }

    private void Update()
    {
        ApplySpeedMovement();
    }

    private async UniTask ExpireBullet(CancellationToken cancellationToken)
    {
        await UniTask.WaitForSeconds(ExpiredTime, cancellationToken: cancellationToken);
        DestroyBullet();
    }

    private void ApplySpeedMovement()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    /// <summary>
    /// Destroy the bullet.
    /// </summary>
    public void DestroyBullet()
    {
        cts.Cancel();
        cts.Dispose();
        cts = new CancellationTokenSource();
        gameObject.SetActive(false);
        IsDeactivated = true;
    }
}
