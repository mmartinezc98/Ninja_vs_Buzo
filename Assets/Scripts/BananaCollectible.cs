using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaCollectible : MonoBehaviour
{
    //public int scoreValue = 10;
    public float bobAmplitude = 0.15f; // Amplitud de la animación de flotación
    public float bobSpeed = 2f;        // Velocidad de la flotación

    private Vector3 startPosition;

    void Start()
    {
        // Guardamos la posición inicial para el efecto de flotación
        startPosition = transform.position;
    }

    void Update()
    {
        // Efecto de flotación: movimiento sinusoidal en el eje Y
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobamos que quien nos toca tiene el tag "Player"
        // (asigna ese tag a los prefabs Frog y VirtualGuy)
        if (other.CompareTag("Player"))
        {
            // Solo el MasterClient destruye para evitar conflictos en red
            if (PhotonNetwork.IsMasterClient)
            {
               // Debug.Log("Recogido! Puntos: " + scoreValue);
                // Destruye el objeto en TODOS los clientes
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
