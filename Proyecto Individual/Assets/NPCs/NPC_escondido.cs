using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_escondido : MonoBehaviour
{

    public float velocidadAsomar;
    public float distAsomar;

    private Rigidbody rb;
    private bool muerto = false;
    private float tiempo = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        tiempo += Time.deltaTime;
        if(!muerto)
            rb.velocity = new Vector3(Mathf.Sin(tiempo * velocidadAsomar) *  distAsomar, 0, 0);
    }

    private void OnCollisionEnter(Collision colider) {
        if(colider.collider.gameObject.CompareTag("Player") || colider.collider.gameObject.CompareTag("Bala")){
            muerto = true;
        }
    }
}
