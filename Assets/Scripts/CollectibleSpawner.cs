using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Referencias")]
    public Tilemap tilemap; // Arrastra aquí el Tilemap de la escena

    [Header("Configuración de Spawn")]
    [Range(0f, 1f)]
    public float spawnProbability = 0.15f; // Probabilidad por tile (0 a 1)
    public float heightOffset = 0.6f;      // Altura sobre el tile
    public int maxCollectibles = 20;       // Máximo de objetos simultáneos

    [Header("Respawn")]
    public float respawnInterval = 15f;    // Segundos entre oleadas de respawn

    private int spawnedCount = 0;

    void Start()
    {// Si estamos conectados a Photon, solo el master hace el spawn
     // Si estamos en local (sin red), lo ejecutamos siempre
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            SpawnCollectibles();
            InvokeRepeating("RespawnCollectibles", respawnInterval, respawnInterval);
        }
    }

    void SpawnCollectibles()
    {
        // Obtenemos los límites del Tilemap (el rectángulo que contiene todos los tiles)
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int tilePos in bounds.allPositionsWithin)
        {
            // ¿Existe un tile aquí?
            if (!tilemap.HasTile(tilePos)) continue;

            // Posición justo encima del tile
            Vector3Int abovePos = tilePos + Vector3Int.up;

            // Si el tile de arriba también está ocupado, no cabe un objeto
            if (tilemap.HasTile(abovePos)) continue;

            // Lanzamos los dados: ¿spawneamos en este tile?
            if (Random.value > spawnProbability) continue;

            // Límite máximo de objetos
            if (spawnedCount >= maxCollectibles) break;

            // Convertimos posición de tile a posición en el mundo
            Vector3 worldPos = tilemap.CellToWorld(abovePos);
            worldPos.y += heightOffset;

            // Verificamos que no haya ya algo en esa posición exacta
            Collider2D overlap = Physics2D.OverlapCircle(worldPos, 0.2f);
            if (overlap != null) continue;

            // Instanciamos el coleccionable de forma sincronizada en red
            // El prefab "Collectible" debe estar en Assets/Resources/
            PhotonNetwork.Instantiate("Collectible", worldPos, Quaternion.identity);
            spawnedCount++;
        }
    }

    void RespawnCollectibles()
    {
        // Actualizamos el contador con los objetos reales que quedan en escena
        spawnedCount = Object.FindAnyObjectByType<BananaCollectible>().Length;

        // Solo spawneamos si hay menos del máximo
        if (spawnedCount < maxCollectibles)
            SpawnCollectibles();
    }

}
