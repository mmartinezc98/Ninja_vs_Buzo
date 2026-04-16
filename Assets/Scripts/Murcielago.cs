using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;  
public class Murcielago : MonoBehaviourPun
{
     
    [SerializeField] public Transform jugador; // Referencia al jugador
    [SerializeField] public Vector3 puntoinicial; // Punto inicial del murciélago
    
    private SpriteRenderer spriteRenderer;
    private Animator animator;
 
    void Start()
    {
        // Guardamos la posición inicial del murciélago
        puntoinicial = transform.position;
        
        // Obtenemos los componentes necesarios
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        // Buscamos al jugador en la escena (cualquiera de los dos)
       
        GameObject[] jugadores = GameObject.FindGameObjectsWithTag("Player");
        if (jugadores.Length > 0)
        {
            // Podemos elegir cualquier jugador, pero para la IA es mejor elegir el más cercano
            jugador = jugadores[0].transform;
        }
    }
 
    void Update()
    {
        
 
        // Si no tenemos referencia al jugador, intentamos encontrarlo
        if (jugador == null)
        {
            GameObject[] jugadores = GameObject.FindGameObjectsWithTag("Player");
            if (jugadores.Length > 0)
            {
                jugador = jugadores[0].transform;
            }
            return;
        }
    }
 
    // Función para girar el murciélago hacia un objetivo
    public void Girar(Vector3 objetivo)
    {
        if (objetivo.x < transform.position.x)
        {
            spriteRenderer.flipX = true; // Miramos a la izquierda
        }
        else
        {
            spriteRenderer.flipX = false; // Miramos a la derecha
        }
    }
}
