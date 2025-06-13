using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DissolverObject : MonoBehaviour
{
    private const float MaxDissolveValue = 1f;
    private const float MinDissolveValue = 0f;
    private static readonly int dissolveAmountID = Shader.PropertyToID("_DissolveAmount");

    [SerializeField]
    private List<SkinnedMeshRenderer> meshRenderers;

    [SerializeField]
    private float dissolveDuration = 1f;

    private float dissolveValue;

    /// <summary>
    /// The dissolve value.
    /// </summary>
    public float DissolveValue
    {
        get => dissolveValue;
        set
        {
            dissolveValue = value;
            propertyBlock.SetFloat(dissolveAmountID, value);

            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.SetPropertyBlock(propertyBlock);
            }
        }
    }

    private MaterialPropertyBlock propertyBlock;

    private void Awake()
    {
        propertyBlock = new();
        DissolveValue = MinDissolveValue;
    }

    /// <summary>
    /// Starts the dissolve effect, transitioning from the current dissolve value to the maximum dissolve value.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async UniTask DissolveAsync(CancellationToken cancellationToken)
    {
        await DOTween
            .To(() => DissolveValue, (value) => DissolveValue = value, MaxDissolveValue, dissolveDuration)
            .ToUniTask(cancellationToken: cancellationToken);
    }
}
