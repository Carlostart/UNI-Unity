using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_velocidad : MonoBehaviour
{
 public float velocidad;

    public bool muerto;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       if(!muerto)
            rb.velocity = new Vector3(0, 0, -velocidad);
    }
    
    private void OnCollisionEnter(Collision colider) {
        if(colider.collider.gameObject.CompareTag("Player") || colider.collider.gameObject.CompareTag("Bala") || colider.collider.gameObject.CompareTag("Muro")){
            muerto = true;
        }
    }
}
