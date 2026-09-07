using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetector : MonoBehaviour
{
    [Header("Límite de Caída")]
    public float alturaMinima = -20f; // Si la Y del carro baja de esto, se considera caída al vacío

    [Header("Punto de Reaparición Inicial")]
    public Vector3 ultimaPosicionCheckpoint;
    public Quaternion ultimaRotacionCheckpoint;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        
        // Guardamos una posición por defecto (la de inicio) al arrancar
        ultimaPosicionCheckpoint = transform.position;
        ultimaRotacionCheckpoint = transform.rotation;
    }

    void Update()
    {
        // Si el carro cae por debajo de la altura límite establecida
        if (transform.position.y < alturaMinima)
        {
            RespawnearCarro();
        }
    }

    public void RespawnearCarro()
    {
        // 1. Apagamos momentáneamente las físicas para que el carro no salga disparado al moverlo
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        // 2. Devolvemos el carro a las coordenadas del último checkpoint guardado
        transform.position = ultimaPosicionCheckpoint;
        transform.rotation = ultimaRotacionCheckpoint;

        // Opcional: Elevarlo un par de metros en Y para que no aparezca metido dentro del suelo
        transform.position += new Vector3(0, 2f, 0);
    }
}
