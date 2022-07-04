using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cocheMov : MonoBehaviour
{
  
    public GameObject esfera;
    private Transform target;
    private float k;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        target = esfera.transform;
        k = 2000;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, target.position) < 10){
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            float fuerzaPersecucion = (Vector3.Distance(transform.position, target.position)-5) * k;
            rb.AddForce(transform.forward * fuerzaPersecucion);
        }
    }
}
