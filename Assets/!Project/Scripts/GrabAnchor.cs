using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAnchor : MonoBehaviour
{
    public Transform baseObject; // Later replace with "PrefabBase" system

    public void SetPositionRotation(Vector3 position, Quaternion rotation)
    {
        // Rotation first
        Quaternion rotationDiff = rotation * Quaternion.Inverse(transform.rotation);
        baseObject.rotation = rotationDiff * baseObject.rotation;

        // Position last (offset based on new position of GrabAnchor)
        Vector3 positionDiff = position - transform.position;
        baseObject.position += positionDiff;
    }
}
