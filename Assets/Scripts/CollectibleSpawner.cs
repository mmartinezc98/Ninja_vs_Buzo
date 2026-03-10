using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollectibleSpawner : MonoBehaviourPunCallbacks
{
    [Header("Prefab")]
    [SerializeField]public GameObject collectiblePrefab;

    [Header("Configuración de Spawn")]
    [Range(0f, 1f)]
    public float spawnProbability = 0.8f;
    public int maxCollectibles = 20;
    public float separacionEntreObjetos = 1f;

    [Header("Respawn")]
    public float respawnInterval = 15f;

    private List<Vector3> spawnPoints = new List<Vector3>();

    void Start()
    {
        CalcularPuntosDeSpawn();

        // Solo el MasterClient spawneará objetos via Photon
        // así se crean en todos los clientes automáticamente
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnCollectibles();
            InvokeRepeating("RespawnCollectibles", respawnInterval, respawnInterval);
        }
    }

    void CalcularPuntosDeSpawn()
    {
        GameObject[] zonas = GameObject.FindGameObjectsWithTag("SpawnZone");

        if (zonas.Length == 0)
        {
            Debug.LogWarning("No se encontraron zonas con el tag 'SpawnZone'");
            return;
        }

        foreach (GameObject zona in zonas)
        {
            BoxCollider2D box = zona.GetComponent<BoxCollider2D>();
            if (box == null) continue;

            Bounds bounds = box.bounds;
            float x = bounds.min.x;
            while (x <= bounds.max.x)
            {
                float y = bounds.min.y;
                while (y <= bounds.max.y)
                {
                    spawnPoints.Add(new Vector3(x, y, 0));
                    y += separacionEntreObjetos;
                }
                x += separacionEntreObjetos;
            }
        }

        Debug.Log("Puntos de spawn calculados: " + spawnPoints.Count);
    }

    void SpawnCollectibles()
    {
        if (collectiblePrefab == null)
        {
            Debug.LogError("¡Asigna el prefab en el Inspector!");
            return;
        }

        // Obtenemos las bananas ya existentes en escena
        BananaCollectible[] bananasActuales = FindObjectsByType<BananaCollectible>(FindObjectsSortMode.None);
        int yaExisten = bananasActuales.Length;

        if (yaExisten >= maxCollectibles)
        {
            Debug.Log("Ya hay suficientes bananas: " + yaExisten);
            return;
        }

        int cuantasFaltan = maxCollectibles - yaExisten;

        // Filtramos puntos ocupados
        List<Vector3> puntosLibres = new List<Vector3>();
        foreach (Vector3 punto in spawnPoints)
        {
            bool ocupado = false;
            foreach (BananaCollectible banana in bananasActuales)
            {
                if (banana == null) continue;
                if (Vector3.Distance(banana.transform.position, punto) < 0.5f)
                {
                    ocupado = true;
                    break;
                }
            }
            if (!ocupado) puntosLibres.Add(punto);
        }

        // Mezclamos aleatoriamente
        for (int i = 0; i < puntosLibres.Count; i++)
        {
            int j = Random.Range(i, puntosLibres.Count);
            Vector3 temp = puntosLibres[i];
            puntosLibres[i] = puntosLibres[j];
            puntosLibres[j] = temp;
        }

        int spawneados = 0;
        foreach (Vector3 punto in puntosLibres)
        {
            if (spawneados >= cuantasFaltan) break;
            if (Random.value > spawnProbability) continue;

            // PhotonNetwork.Instantiate crea el objeto en TODOS los clientes
            PhotonNetwork.Instantiate(
                collectiblePrefab.name,
                punto,
                Quaternion.identity
            );
            spawneados++;
        }

        Debug.Log("Bananas existentes: " + yaExisten + " | Nuevas: " + spawneados);
    }

    void RespawnCollectibles()
    {
        // Solo el master gestiona el respawn
        if (!PhotonNetwork.IsMasterClient) return;

        int actuales = FindObjectsByType<BananaCollectible>(FindObjectsSortMode.None).Length;
        Debug.Log("Bananas actuales: " + actuales);

        if (actuales < maxCollectibles)
            SpawnCollectibles();
    }

    // Si el MasterClient se va, el nuevo master retoma el control del spawn
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Nuevo MasterClient, retomando spawn.");
            SpawnCollectibles();
            InvokeRepeating("RespawnCollectibles", respawnInterval, respawnInterval);
        }
    }

    void OnDrawGizmos()
    {
        GameObject[] zonas = GameObject.FindGameObjectsWithTag("SpawnZone");
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        foreach (GameObject zona in zonas)
        {
            BoxCollider2D box = zona.GetComponent<BoxCollider2D>();
            if (box != null)
                Gizmos.DrawCube(box.bounds.center, box.bounds.size);
        }

        Gizmos.color = Color.yellow;
        foreach (Vector3 punto in spawnPoints)
            Gizmos.DrawSphere(punto, 0.1f);
    }

}
