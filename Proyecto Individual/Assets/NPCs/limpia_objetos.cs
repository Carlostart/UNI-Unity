using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class limpia_objetos : MonoBehaviour
{
    // Start is called before the first frame update
    public float lifetime = 30f;

    private float tiempo = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        tiempo += Time.deltaTime;

        if(tiempo>lifetime || transform.position.y < -20){

            Destroy(gameObject);
            // Debug.Log("Destruido");
        }

    }
}
