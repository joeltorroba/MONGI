using UnityEngine;
using System.Collections;

public class ReactiveTreeFall : MonoBehaviour
{
     private Transform targetToRotate;   // opcional (si está vacío, rota este objeto)
    [SerializeField] private float startAtWorldState = -2f;
    [SerializeField] private float fullAtWorldState = -10f;

    [SerializeField, Range(0f, 90f)] private float maxFallAngle = 80f;
    [SerializeField] private Vector3 localFallAxis = Vector3.right;
    [SerializeField] private float smoothSpeed = 3f;

    private WorldStateManager world;
    private Quaternion initialRot;
    private float targetT;
    private float currentT;
    private bool subscribed;

    private void Awake()
    {
        if (targetToRotate == null) targetToRotate = transform;

        initialRot = targetToRotate.localRotation;

        if (localFallAxis.sqrMagnitude < 0.0001f)
            localFallAxis = Vector3.right;
        localFallAxis = localFallAxis.normalized;
    }

    private void OnEnable()
    {
        StartCoroutine(TrySubscribeRoutine());
    }

    private void OnDisable()
    {
        if (world != null)
            world.OnWorldStateChanged -= OnWorldStateChanged;

        subscribed = false;
    }

    private IEnumerator TrySubscribeRoutine()
    {
        for (int i = 0; i < 30; i++)
        {
            TrySubscribe();
            if (subscribed) yield break;
            yield return null;
        }
    }

    private void TrySubscribe()
    {
        if (subscribed) return;

        world = WorldStateManager.Instance;
        if (world == null) world = FindFirstObjectByType<WorldStateManager>();
        if (world == null) return;

        world.OnWorldStateChanged -= OnWorldStateChanged;
        world.OnWorldStateChanged += OnWorldStateChanged;
        subscribed = true;

        OnWorldStateChanged(world.worldState);
    }

    private void OnWorldStateChanged(float ws)
    {
        float t = Mathf.InverseLerp(startAtWorldState, fullAtWorldState, ws);
        targetT = Mathf.Clamp01(t);
    }

    private void Update()
    {
        currentT = Mathf.Lerp(currentT, targetT, Time.deltaTime * smoothSpeed);

        float angle = maxFallAngle * currentT;
        Quaternion rot = initialRot * Quaternion.AngleAxis(angle, localFallAxis);

        targetToRotate.localRotation = rot;
    }
}
