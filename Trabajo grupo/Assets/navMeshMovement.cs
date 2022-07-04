using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navMeshMovement : MonoBehaviour
{
    public NavMeshAgent miAgente;
    public GameObject[] puntosCamino;
    private int objetivoActual = 0;
    // Start is called before the first frame update
    void Start()
    {
        miAgente = GetComponent<NavMeshAgent>();
        miAgente.SetDestination(puntosCamino[0].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (miAgente.remainingDistance <= miAgente.stoppingDistance){
            objetivoActual++;
            if (objetivoActual >= puntosCamino.Length) objetivoActual = 0;
            miAgente.destination = puntosCamino[objetivoActual].transform.position;
        }
    }
}
