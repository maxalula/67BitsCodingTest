using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectAndStackObjects : MonoBehaviour
{
    [SerializeField] private float verticalSpacing = 1;
    [SerializeField] private float springStrength = 50;
    [SerializeField] private float damping = 8;

    [SerializeField] private List<Transform> stackObjects = new List<Transform>();
    private List<Vector3> targetPositions = new List<Vector3>();
    private List<Vector3> targetVelocities = new List<Vector3>();
    [SerializeField] private Transform firstPosition;
    private void Update()
    {
        Vector3 lastTargetPos = firstPosition.position;
        for (int i = 0; i < stackObjects.Count; i++)
        {
            Vector3 targetPos = lastTargetPos + Vector3.up * verticalSpacing;
            Vector3 displacement = targetPos - targetPositions[i];
            Vector3 springForce = displacement * springStrength;
            Vector3 dampingForce = -targetVelocities[i] * damping;
            Vector3 force = springForce + dampingForce;

            targetVelocities[i] += force * Time.deltaTime;
            targetPositions[i] += targetVelocities[i] * Time.deltaTime;
            stackObjects[i].position = targetPositions[i];

            lastTargetPos = targetPositions[i];
        }
    }

    public void AddObject(GameObject stackObject)
    {
        stackObjects.Add(stackObject.transform);
        stackObject.transform.position = transform.position + Vector3.up * verticalSpacing * (stackObjects.Count);
        targetPositions.Add(transform.position + Vector3.up * verticalSpacing * (stackObjects.Count));
        targetVelocities.Add(Vector3.zero);
    }
    public void RemoveObjects()
    {
        if (stackObjects.Count == 0)
            return;

        foreach (Transform t in stackObjects)
        {
            CurrencyManager.Instance.AddCash(10);
            Destroy(t.gameObject);
        }

        stackObjects.Clear();
        targetPositions.Clear();
        targetVelocities.Clear();
    }
    public int CurrentStackSize()
    {
        return stackObjects.Count;
    }
}

/*
* [SerializeField] private float followSpeed = 5;
* for (int i = 0; i < stackObjects.Count; i++)
{
Vector3 desiredPosition = lastTargetPos + Vector3.up * verticalSpacing;
targetPositions[i] = Vector3.Lerp(targetPositions[i], desiredPosition, Time.deltaTime * followSpeed);
stackObjects[i].position = targetPositions[i];
lastTargetPos = targetPositions[i];
}*/