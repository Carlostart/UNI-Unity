using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Torreta : MonoBehaviour
{
    public LayerMask cocheLayer;
    public float rangoVision;
    public int angulo;
    public bool vistoCoche;
    public GameObject coche;

    public GameObject[] proyectiles;
    public int idxBala;
    private float ultimoTiro;

    private Quaternion rotInicial;
    public float velRotacion;
    public float amplitudRotacion;

    void Start()
    {
        coche = GameObject.Find("Coche");
        rotInicial = transform.rotation;

    }

    void FixedUpdate()
    {
        vistoCoche = buscaObjetivo(cocheLayer);
        Debug.DrawLine(transform.position, transform.position + transform.forward * rangoVision, Color.black);
        
        if (vistoCoche) atacaCoche();
        else movimientoBusqueda();
    }

    private void movimientoBusqueda()
    {
        transform.rotation = rotInicial*(new Quaternion(0f, Mathf.Sin(Time.time * velRotacion) * amplitudRotacion, 0, 1));
    }

    private bool buscaObjetivo(LayerMask objetivo)
    {
        Collider[] objetosVistos = Physics.OverlapSphere(transform.position, rangoVision, objetivo);

        if (objetosVistos.Length != 0)
        {
            Transform visto = objetosVistos[0].transform;
            Vector3 direccionObjetivo = (visto.position - transform.position).normalized;

            bool detectado = Vector3.Angle(transform.forward, direccionObjetivo) < angulo / 2;
            bool esCoche = visto.CompareTag("Coche");
            // Debug.Log(detectado + "    " +  esCoche);
            if (detectado && esCoche)
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    private void atacaCoche()
    {
        // Debug.Log("Ataca Jugador");
        transform.LookAt(coche.transform.position);

        proyectil bala = proyectiles[idxBala].GetComponent<proyectil>();
        if (Time.time > ultimoTiro + bala.cooldown)
        {
            Instantiate(bala, transform.position, transform.rotation);
            ultimoTiro = Time.time;
        }
       
    }
}
