using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using weka.classifiers.trees;
using weka.classifiers.evaluation;
using weka.core;
using java.io;
using java.lang;
using java.util;
using weka.classifiers.functions;
using weka.classifiers;
using weka.core.converters;

public class aprendizajeTorreta : MonoBehaviour
{
    weka.classifiers.trees.M5P saberPredecirFuerzaY,conocimiento;
    weka.core.Instances casosEntrenamiento;
    string ESTADO = "Sin conocimiento";
    string acciones;
    bool lanzada = false;
    public GameObject bala;
    GameObject Instanciabala;
    public GameObject Helicoptero;
    float velocidadYInicial, mejorFuerzaY;
    public float valorMaximoFy, pasoFy, Fx_fijo;
    Rigidbody r;

    void OnGUI()
    {
        GUI.Label(new Rect(10, 5, 600, 20), "Estado: " + ESTADO);
        GUI.Label(new Rect(10, 20, 600, 20), acciones);
    }

    void Start()
    {
        if (ESTADO == "Sin conocimiento") {
            Time.timeScale = 15.0f;
            StartCoroutine("Entrenamiento");  //Lanza el proceso de entrenamiento   
        }                                                       
    }
    IEnumerator Entrenamiento()
    {
        casosEntrenamiento = new weka.core.Instances(new java.io.FileReader("Assets/Experiencias.arff"));  //Lee fichero con las variables y experiencias

        if (casosEntrenamiento.numInstances() < 10)
            for (float Fy = 3; Fy <= valorMaximoFy; Fy = Fy + pasoFy)                   //BUCLE de planificación de la fuerza FX durante el entrenamiento
            {
                Instanciabala = Instantiate(bala, transform.position, new Quaternion()) as GameObject;                      //Utiliza la bala física del juego (si no existe la crea)
                Rigidbody rb = Instanciabala.GetComponent<Rigidbody>();               //Crea una bala física
                rb.AddForce(new Vector3(Fx_fijo, Fy, 0), ForceMode.Impulse);                 //y la lanza con esa fuerza Fy  (Fy es siempre 10N)
                yield return new WaitUntil(() => (rb.transform.position.x >= 14.86f)); //Eje X del helicoptero

                Instance casoAaprender = new Instance(casosEntrenamiento.numAttributes());
                acciones = "Generando experiencia con fuerzas Fy= " + Fy + " N   Fx= " + Fx_fijo + " N  impacta en altura = " + rb.transform.position.y + " m";
                casoAaprender.setDataset(casosEntrenamiento);                           //crea un registro de experiencia
                casoAaprender.setValue(0, Fy);                                          //guarda el dato de la fuerza utilizada
                casoAaprender.setValue(1, rb.transform.position.y);                     //anota la altura alcanzada
                casosEntrenamiento.add(casoAaprender);                                  //guarda el registro de experiencia 
                                                                                        //----------------------------------------------------- 
                rb.isKinematic = true; rb.GetComponent<Collider>().isTrigger = true;    //...opcional: paraliza la bala
                Destroy(Instanciabala, 0f);                                           //...opcional: destruye la bala en 1 seg para que ver donde cayó.            
            }                                                                           //FIN bucle de lanzamientos con diferentes de fuerzas
        //APRENDIZADE CONOCIMIENTO:  
        saberPredecirFuerzaY = new M5P();                                               //crea un algoritmo de aprendizaje M5P (árboles de regresión)
        casosEntrenamiento.setClassIndex(0);                                            //la variable a aprender será la fuerza Fy (id=0) dada la distancia
        saberPredecirFuerzaY.buildClassifier(casosEntrenamiento);                       //REALIZA EL APRENDIZAJE DE FX A PARTIR DE LAS EXPERIENCIAS
        SerializationHelper.write("AprendizajeTorreta.modelo", saberPredecirFuerzaY);
        ESTADO = "Con conocimiento";

        File salida = new File("Assets/Finales_Experiencias.arff");
        if (!salida.exists()) System.IO.File.Create(salida.getAbsoluteFile().toString()).Dispose();
        ArffSaver saver = new ArffSaver();
        saver.setInstances(casosEntrenamiento);
        saver.setFile(salida);
        saver.writeBatch();
        Time.timeScale = 1.0f;
    }

    void FixedUpdate()                                                                  //DURANTEL EL JUEGO: Aplica lo aprendido para lanzar a la canasta
    {
        if (ESTADO == "Con conocimiento")
        {
            Classifier saberPredecirFuerzaY = (Classifier) SerializationHelper.read("AprendizajeTorreta.modelo");

            velocidadYInicial = UnityEngine.Random.Range(6.0f, 12.0f);                             //Distancia de la Canasta (... Opcional: generada aleatoriamente)
            Helicoptero = GameObject.CreatePrimitive(PrimitiveType.Cylinder);             // ... opcional: muestra la canasta a la distancia propuesta
            Helicoptero.transform.position = transform.position + new Vector3(15.86f, 0.6f, 0);
            Helicoptero.transform.localScale = new Vector3(0.4f, 1, 0.4f);
            Helicoptero.AddComponent<Rigidbody>();
            Rigidbody rb = Helicoptero.GetComponent<Rigidbody>();
            //rb.useGravity = false;
            if (!lanzada)
            {
                rb.AddForce(new Vector3(0, rb.mass * velocidadYInicial, 0), ForceMode.Impulse);
                lanzada = true;
            }
            acciones = "Helicoptero sube con velocidad: " + velocidadYInicial + " m/s. Masa: " + rb.mass;
            
            casosEntrenamiento = new weka.core.Instances(new java.io.FileReader("Assets/Experiencias.arff"));
            Instance casoPrueba = new Instance(casosEntrenamiento.numAttributes());  //Crea un registro de experiencia durante el juego
            casoPrueba.setDataset(casosEntrenamiento);
            casoPrueba.setValue(1, velocidadYInicial*1f + 0.5*1*1*-9.81f);                               //le pone el dato de la distancia a alcanzar
            mejorFuerzaY = (float)saberPredecirFuerzaY.classifyInstance(casoPrueba);  //predice la fuerza dada la distancia utilizando el algoritmo M5P

            Instanciabala = Instantiate(bala, transform.position, new Quaternion()) as GameObject;                      //Utiliza la bala física del juego (si no existe la crea)
            r = Instanciabala.GetComponent<Rigidbody>();
            r.AddForce(new Vector3(Fx_fijo, mejorFuerzaY, 0), ForceMode.Impulse);          //y por fin la la lanza en el videojuego con la fuerza encontrada
            print("Y que metemos en la tabla " + (velocidadYInicial*1f + 0.5*1*1*-9.81f));
            print("Se lanzó una bala con fuerza Fy=" + mejorFuerzaY + " y Fx= " + Fx_fijo +" N");
            ESTADO = "Jugando";
            acciones = acciones + " Se aplicó Fy= " + mejorFuerzaY.ToString("0.000") + " N  Fy= " + Fx_fijo + " N";
        }
        if (ESTADO == "Jugando")
        {
            //print(Helicoptero.transform.position.y);
            if (r.transform.position.x >= 14.86)                                            //cuando la bala cae por debajo de 0 m
            {                                                                          //escribe la distancia en x alcanzada
                                                                                       // print(" Fuerzas aplicadas Fy=" + mejorFuerzaY.ToString("0.000") + " Fy = 10. ";
                                                                                       // print("La bala lanzada llegó a " + r.transform.position.x + ". El error fue de " + (r.transform.position.x - distanciaObjetivo).ToString("0.000000") + " m");
                acciones = acciones + "  El error: " + (r.transform.position.y - Helicoptero.transform.position.y).ToString("0.000000") + " m";
                print(acciones);
                r.isKinematic = true;
                ESTADO = "Accion Finalizada";
            }
        }
    }


}