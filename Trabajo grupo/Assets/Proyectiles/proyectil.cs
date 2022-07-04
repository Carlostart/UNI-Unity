using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proyectil : MonoBehaviour
{
    public float fuerza;
    public int tipo;
    public float cooldown;

    private Rigidbody rb;
    private float tiempo = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * fuerza, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        tiempo += Time.deltaTime;
        if (tiempo > 3)
            Destroy(gameObject);
        }

    void OnCollisionEnter(Collision collider)
    {
        // if (tipo != 1 && collider.transform.CompareTag("Muro"))
        // {
        //     Destroy(gameObject);
        // }
    }
}
