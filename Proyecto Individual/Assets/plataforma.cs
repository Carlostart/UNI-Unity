using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class plataforma : MonoBehaviour
{

    public float velCinta = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in transform){
            if(child.position.z <- 30)
                child.position += new Vector3(0, 0, 90);

            child.position += new Vector3 (0, 0, -velCinta * Time.deltaTime);
        }
    }

    void OnTriggerStay(Collider collision){
        if(collision.transform.CompareTag("Movimiento") || collision.transform.CompareTag("Muro")){
            // Debug.Log(collision.name);
            collision.transform.parent = transform;
            // collision.transform.position += new Vector3(0, 0, -velCinta * 2 * Time.deltaTime);
            // collision.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -velCinta);
        }
    }
}
