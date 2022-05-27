using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float speed = 30.0f;

    public void Save(List<object> serializeableFields)
    {
        serializeableFields.Add(axis);
        serializeableFields.Add(speed);
    }

    public void Restore(List<object> serializeableFields)
    {
        axis = (Vector3)serializeableFields[0];
        speed = (float)serializeableFields[1];
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis, speed*Time.deltaTime);
    }
}
