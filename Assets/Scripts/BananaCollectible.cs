using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaCollectible : MonoBehaviour
{
    // ID único que se asigna desde el spawner al instanciar
    [HideInInspector] public int collectibleID = -1;

    public float bobAmplitude = 0.15f;
    public float bobSpeed = 2f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Solo el MasterClient destruye para evitar que los dos jugadores
        // intenten destruir el mismo objeto a la vez
        if (PhotonNetwork.IsMasterClient)
        {
            // PhotonNetwork.Destroy destruye el objeto en TODOS los clientes
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
