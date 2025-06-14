using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject for storing zombie settings.
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Enemy/Zombie", order = 2)]
public class ZombieSetting : ScriptableObject
{
    [field: SerializeField, Header("FoV")]
    public float SpreadAngle { get; private set; }

    [field: SerializeField]
    public float Radius { get; private set; }

    [field: SerializeField, Header("Attack")]
    public float Damage { get; private set; }

    [field: SerializeField]
    public float MaxHealth { get; private set; }

    [field: SerializeField]
    public LayerMask PlayerMask { get; private set; }

    [field: SerializeField, Header("Movement")]
    public float WalkSpeed { get; private set; }

    [field: SerializeField]
    public float WalkAcceleration { get; private set; }

    [field: SerializeField]
    public float WalkAnimationSpeed { get; private set; }

    [field: SerializeField]
    public float WalkAnimationAcceleration { get; private set; }

    [field: SerializeField]
    public float StopDistance { get; private set; }

    [field: SerializeField]
    public AudioClip AttackClip { get; private set; }

    [field: SerializeField]
    public AudioClip DieClip { get; private set; }
}
