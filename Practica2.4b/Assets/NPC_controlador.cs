using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPC_controlador : MonoBehaviour{
    public NavMeshAgent miAgente;
    public GameObject[] puntosCamino;
    public int objetivoActual;

    void Start () {
        if (miAgente==null) miAgente = GetComponent<NavMeshAgent>();
        miAgente.SetDestination (puntosCamino[0].transform.position);
    }

    void Update(){
        if (miAgente.remainingDistance <= miAgente.stoppingDistance){
            objetivoActual++;
            if (objetivoActual >= puntosCamino.Length) objetivoActual = 0;
            miAgente.destination = puntosCamino[objetivoActual].transform.position;
        }
    }
}