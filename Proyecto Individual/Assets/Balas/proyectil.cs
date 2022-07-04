using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proyectil : MonoBehaviour
{
    public float velocidad;
    public int tipo;
    public float cooldown;

    private Rigidbody rb;
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
        if (tiempo > 3 || transform.position.z > 8)
            Destroy(gameObject);

        rb.velocity = transform.forward * velocidad;
    }

    void OnCollisionEnter(Collision collider)
    {
        if (tipo != 1 && collider.transform.CompareTag("Muro"))
        {
            Destroy(gameObject);
        }
    }
}
