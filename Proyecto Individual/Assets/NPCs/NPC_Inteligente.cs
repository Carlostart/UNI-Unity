using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Inteligente : MonoBehaviour
{
    public int objetivoActual;
    public NavMeshAgent miAgente;
    public GameObject jugador;
    public Vector3[] objetivos;
    public LayerMask jugadorLayer;
    public LayerMask npcLayer;
    public LayerMask obstaculoLayer;
    public GameObject[] proyectiles;

    public float rangoVision;
    public float rangoAtaque;
    public bool vistoJugador;
    public bool ataqueJugador;
    private bool vistoVictima;
    private Transform npcVisto;
    private float ultimoTiro;
    private int idxBala;

    [Range(0, 360)]
    public float angulo;
    private bool cargado;

    void Start()
    {
        jugador = GameObject.Find("Jugador");
        if (miAgente == null)
        {
            miAgente = GetComponent<NavMeshAgent>();
        }
        idxBala = UnityEngine.Random.Range(0, proyectiles.Length);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        vistoJugador = buscaObjetivo(jugadorLayer);
        vistoVictima = buscaObjetivo(npcLayer);

        Debug.DrawLine(transform.position, transform.position + transform.forward * rangoVision, Color.black);

        if (!vistoJugador && !(vistoVictima && !cargado)) explorarLaberinto();
        if (vistoJugador) sigueJugador();
        if (vistoJugador && ataqueJugador) atacaJugador();
        if (vistoVictima && !cargado && !vistoJugador) sigueNPC();
    }

    private bool buscaObjetivo(LayerMask objetivo)
    {
        Collider[] objetosVistos = Physics.OverlapSphere(transform.position, rangoVision, objetivo);

        if (objetosVistos.Length != 0)
        {
            Transform visto = objetosVistos[0].transform;
            Vector3 direccionObjetivo = (visto.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, direccionObjetivo) < angulo / 2)
            {
                float distanciaObjetivo = Vector3.Distance(transform.position, visto.position);

                if (Physics.Raycast(transform.position, direccionObjetivo, distanciaObjetivo, obstaculoLayer))
                    return false;
                else
                {
                    if (visto.CompareTag("Amigo"))
                    {
                        if (visto.parent.GetComponent<NPC_amigo>().cogido)
                            return false;
                        else
                        {
                            npcVisto = visto.parent;
                        }
                    }
                    return true;
                }
            }
            else
                return false;
        }
        else
            return false;
    }


    void explorarLaberinto()
    {
        // Debug.Log("Explora");
        if (miAgente.remainingDistance <= miAgente.stoppingDistance)
        {
            // Debug.Log(miAgente.remainingDistance);
            // Debug.Log(objetivoActual);
            objetivoActual++;
            if (objetivoActual >= objetivos.Length) objetivoActual = 0;
            miAgente.destination = objetivos[objetivoActual];
            miAgente.speed = 2;

            // Debug.Log(miAgente.destination);
        }

    }
    private void sigueJugador()
    {
        // Debug.Log("Sigue Jugador");
        transform.LookAt(jugador.transform.position);
        miAgente.destination = jugador.transform.position;
        miAgente.stoppingDistance = 2;
        miAgente.speed = 3.5f;
        ataqueJugador = Vector3.Distance(transform.position, jugador.transform.position) < rangoAtaque;
        // ataqueJugador =Physics.Raycast(transform.position, transform.forward, rangoAtaque, jugadorLayer);

    }
    private void atacaJugador()
    {
        // Debug.Log("Ataca Jugador");
        transform.LookAt(jugador.transform.position);
        if (!cargado)
        {
            proyectil bala = proyectiles[idxBala].GetComponent<proyectil>();
            if (Time.time > ultimoTiro + bala.cooldown)
            {
                Instantiate(bala, transform.position + transform.forward / 2, transform.rotation);
                ultimoTiro = Time.time;
            }
        }
        else
        {
            //intención de soltar el objeto
            npcVisto.transform.SetParent(null); //el objeto deja de tener padre
            npcVisto.transform.position = transform.position - new Vector3(0, -.4f, 0);
            npcVisto.GetComponent<NPC_amigo>().cogido = false;
            npcVisto.GetComponent<NPC_amigo>().agro = true;
            npcVisto = null;
            cargado = false;
        }
    }

    private void sigueNPC()
    {
        // Debug.Log("Sigue NPC");
        miAgente.destination = npcVisto.position;
        miAgente.stoppingDistance = 0;
    }

    private void OnTriggerEnter(Collider other)
    { //un Collider con Trigger toca un objeto
        if (vistoVictima && (other.gameObject.CompareTag("Amigo")) && !other.transform.parent.GetComponent<NPC_amigo>().cogido && !cargado)
        {
            other.gameObject.transform.parent.SetParent(gameObject.transform); //El objeto pasa a ser hijo
            other.gameObject.transform.parent.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            npcVisto = other.transform.parent;
            npcVisto.GetComponent<NPC_amigo>().cogido = true;
            cargado = true;
        }
    }
    private void OnCollisionEnter(Collision colider)
    {
        if (colider.collider.gameObject.CompareTag("Player") || colider.collider.gameObject.CompareTag("Bala"))
        {
            Destroy(gameObject);
        }
    }
}

