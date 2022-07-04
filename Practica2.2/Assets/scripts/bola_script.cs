using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bola_script : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cuboEnemigo;

    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 p1 = transform.position;
        Vector3 p2 = cuboEnemigo.transform.position;
        if(Vector3.Distance(p1,p2) < 2)
            Debug.Log("enemigo cerca!");
    }
}
