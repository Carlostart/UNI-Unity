using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class jugador : MonoBehaviour
{
    public float velocidad = 5f;
    public GameObject[] proyectiles;
    public int tipoBala = 0;

    private float cooldownprogress;

    private Rigidbody rb;
    private Vector3 posInput;
    private Vector3 velocity;

    private Camera mainCamera;
    private Vector3 pointToLook;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>();

        cooldownprogress = 0;
    }

    // Update is called once per frame

    void Update()
    {
        posInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        velocity = posInput * velocidad;

        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane suelo = new Plane(Vector3.up, Vector3.zero);
        float longRayo;

        if (suelo.Raycast(cameraRay, out longRayo))
        {
            pointToLook = cameraRay.GetPoint(longRayo);

        }

        cooldownprogress += Time.deltaTime;
        if (Input.GetButtonDown("Fire1"))
        {
            proyectil bala = proyectiles[tipoBala].GetComponent<proyectil>();
            if (cooldownprogress > bala.cooldown)
            {
                Instantiate(proyectiles[tipoBala], transform.position, transform.rotation);
                cooldownprogress = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tipoBala = (tipoBala + 1) % proyectiles.Length;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = velocity;
        pointToLook = new Vector3(pointToLook.x, transform.position.y, pointToLook.z);
        transform.LookAt(pointToLook);
        Debug.DrawLine(transform.position, pointToLook, Color.green);

    }
}
