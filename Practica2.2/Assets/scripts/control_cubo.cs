using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control_cubo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(h,0,0);
    }
}
