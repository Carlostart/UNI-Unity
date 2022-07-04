using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{

    public float ratio = 10f;
    public GameObject[] generaciones;

    private float espera = 0;
    // Start is called before the first frame update
    void Start()
    {
        espera = ratio;
    }

    // Update is called once per frame
    void Update()
    {
        espera += Time.deltaTime;

        if (Input.GetKeyDown("5"))
        {

        }
        if (espera > ratio)
        {
            SpawnAleatorio();
            espera = 0;
        }
    }

    void SpawnAleatorio()
    {
        Instantiate(generaciones[Random.Range(0, generaciones.Length)], new Vector3(0, 0.2f, 15f), Quaternion.identity, transform);
    }
}
