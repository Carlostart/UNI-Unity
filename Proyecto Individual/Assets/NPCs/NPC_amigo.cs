using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_amigo : MonoBehaviour
{
    // Start is called before the first frame update

    public bool agro;
    public bool cogido;
    public float amplitudLevitacion;
    public float velocidadLevitacion;
    public float velocidadPataleoPacifico;
    public float amplitudPataleoPacifico;
    public float velocidadPataleoAgro;
    public float amplitudPataleoAgro;
    private Transform[] patas = new Transform[4];
    private bool unaVez;
    public NavMeshAgent miAgente;
    private GameObject jugador;
    void Start()
    {
        int i = 0;
        foreach (Transform objeto in transform)
        {
            if (objeto.CompareTag("Eje"))
            {
                patas[i] = objeto;
                i++;
            }
        }

        miAgente = GetComponent<NavMeshAgent>();
        jugador = GameObject.Find("Jugador");
    }

    // Update is called once per frame
    void Update()
    {
        if (agro) comportamientoAgro();
        else if (!cogido) comportamientoPacifico();

    }

    private void comportamientoPacifico()
    {
        transform.position += new Vector3(0, Time.deltaTime * Mathf.Sin(Time.time * velocidadLevitacion) * amplitudLevitacion, 0);
        for (int i = 0; i < 4; i++)
        {
            patas[i].rotation = new Quaternion(patas[i].rotation.x, patas[i].rotation.y, Mathf.Sin(i + Time.time * velocidadPataleoPacifico) * amplitudPataleoPacifico, 1);
        }
    }

    private void comportamientoAgro()
    {
        if (!unaVez)
        {
            transform.rotation = new Quaternion(180f, 0, 0, 1);
            Renderer[] children;
            children = GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in children)
            {
                if (rend.CompareTag("Untagged"))
                    rend.material.color = Color.red;
            }
            miAgente.enabled = true;
            unaVez = true;
        }
        for (int i = 0; i < 4; i++)
        {
            patas[i].rotation = new Quaternion(patas[i].rotation.x, patas[i].rotation.y, 180 + Mathf.Sin(i + Time.time * velocidadPataleoAgro) * amplitudPataleoAgro, 1);
        }
        miAgente.destination = jugador.transform.position;
    }
}
