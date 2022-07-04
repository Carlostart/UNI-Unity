using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coche : MonoBehaviour
{

    public int contador;
    public List<GameObject> monedas;
    float distCola;
    float z;

    // Start is called before the first frame update
    void Start()
    {
        monedas = new List<GameObject>();
        contador = 0;
        distCola = -1.2f;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "moneda")
        {
            if(contador > 2){
                SoltarMoneda();
            }

            other.gameObject.transform.SetParent(gameObject.transform);

            z = -3 + distCola*(contador);
            other.gameObject.transform.position = gameObject.transform.position + transform.forward * z;
            if (other.gameObject.GetComponent<Rigidbody>() != null)
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            monedas.Insert(contador,other.gameObject);
            contador++;

        }
    }

    void SoltarMoneda(){
        GameObject m = monedas[contador-1];
        m.transform.SetParent(GameObject.Find("Monedas").transform);
        m.transform.position -= transform.forward * 1;
        monedas.Remove(m);
        contador--;
    }

    void SoltarMonedas(){
        foreach(GameObject m in monedas){
            m.transform.SetParent(GameObject.Find("Monedas").transform);
            m.GetComponent<Rigidbody>().isKinematic = false;
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10);
        foreach(Collider hit in colliders){
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb != null){
                rb.AddExplosionForce(1000,transform.position - transform.forward * 4.2f,5000);
            }
        }
        monedas.RemoveRange(0,contador);
        contador = 0;
    }

    void OnCollisionEnter(Collision col){
        if (col.gameObject.tag == "Bala"){
            int tipoBala = col.gameObject.GetComponent<proyectil>().tipo;
            if(tipoBala == 0){
                SoltarMonedas();
                // Debug.Log("0");
            }else if(tipoBala == 1 && contador > 0){
                SoltarMoneda();
                // Debug.Log("1");
            }else if(tipoBala == 2){
                GetComponent<Rigidbody>().AddExplosionForce(500000,col.transform.position,5000);
                SoltarMonedas();
                // Debug.Log("2");

            }
        }
    }
}
