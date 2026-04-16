using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Murcielago_Seguir_Behaviour : StateMachineBehaviour
{
        private float velocidadMovimiento;
    private Vector3 puntoinicial;
    private Murcielago murcielago;
    private PhotonView photonView;
    [SerializeField]private float tiempoBase = 2f; // Tiempo base para el ataque

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        murcielago = animator.gameObject.GetComponent<Murcielago>();
        photonView = animator.gameObject.GetComponent<PhotonView>();
        puntoinicial = murcielago.puntoinicial;
        velocidadMovimiento = 3; // Velocidad a la que persigue al jugador
    }
 
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Solo el maestro controla el movimiento del murciélago
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
 
        // Movemos el murciélago hacia el jugador
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, murcielago.jugador.position, velocidadMovimiento * Time.deltaTime);
        
        // Giramos el murciélago para que mire hacia el jugador
        murcielago.Girar(murcielago.jugador.position);
 
        tiempoBase -= Time.deltaTime;
        // Si llegamos al jugador, activamos el trigger
        if (Vector2.Distance(animator.transform.position, murcielago.jugador.position) < 0.5f)
        {
            animator.SetTrigger("Volver");
        }
    }
 
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
