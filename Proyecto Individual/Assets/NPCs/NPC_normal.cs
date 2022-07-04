using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_normal : MonoBehaviour
{
    public float velocidad;

    // private bool muerto;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame


    private void OnCollisionEnter(Collision colider) {
        if(colider.collider.gameObject.CompareTag("Player") || colider.collider.gameObject.CompareTag("Bala")){
            // Debug.Log("Muerto");
            // muerto = true;
            rb.constraints = RigidbodyConstraints.None;            
            // rb.useGravity = false;
        }
    }
}
