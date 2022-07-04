using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jugador : MonoBehaviour
{
    private float tiempo = 0;
    private bool colisona = false;

    private void OnCollisionEnter(Collision colision){
        if (colision.gameObject.name == "Cube"){
            Debug.Log("colision!");
            colisona = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if(colisona){
            float velocidadY = 0.5f;
            rb.velocity = new Vector3(0, velocidadY, 0);

            tiempo += Time.deltaTime;
            if(tiempo>=3)
                gameObject.SetActive(false);
        }
        
        float velocidadX = 1; //1 metro por segundo
        if (Input.GetKey (KeyCode.RightArrow)) { //si se presiona la flecha derecha
            // float desplazamientoX = velocidadX * Time.deltaTime;
            // transform.position = transform.position + new Vector3(desplazamientoX, 0, 0);
            rb.velocity = new Vector3(velocidadX, 0, 0);
        }
        if (Input.GetKey (KeyCode.LeftArrow)) { //si se presiona la flecha izquierda
            // float desplazamientoX = velocidadX * Time.deltaTime;
            // transform.position = transform.position + new Vector3(-desplazamientoX, 0, 0);
            rb.velocity = new Vector3(-velocidadX, 0, 0);
        } 
    }
}
