using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPC_agresivo : MonoBehaviour
{
    public NavMeshAgent miAgente;
    public GameObject objetivo;
    public float velocidad = 2f;

    void Start () {
        if (miAgente==null) {
            miAgente = GetComponent<NavMeshAgent>();
            miAgente.SetDestination(objetivo.transform.position);
        }
        objetivo = GameObject.Find("Jugador");
    }

    void Update(){
        if (miAgente.enabled){
            miAgente.destination = objetivo.transform.position;
        }
    }

    private void OnCollisionEnter(Collision colider) {
        if(colider.collider.gameObject.CompareTag("Player") || colider.collider.gameObject.CompareTag("Bala") ){
            miAgente.enabled = false;
        }
    }
}
