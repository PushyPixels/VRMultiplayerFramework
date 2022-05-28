using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCastPhysics : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float radius;
    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float velocityMagnitude = rigidbody.velocity.magnitude;

        RaycastHit hit;
        if(rigidbody.velocity.magnitude > 0.01)
        {
            if(Physics.SphereCast(rigidbody.position,radius,rigidbody.velocity, out hit, rigidbody.velocity.magnitude+(radius*2.0f),layerMask))
            {
                //transform.position = hit.point;
                rigidbody.velocity = Vector3.Reflect(rigidbody.velocity,hit.normal);
            }
        }
    }
}
