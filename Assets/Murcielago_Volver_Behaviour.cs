using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Murcielago_Volver_Behaviour : StateMachineBehaviour    
{
        private float velocidadMovimiento;
    private Vector3 puntoinicial;
    private Murcielago murcielago;
    private PhotonView photonView;
 
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        murcielago = animator.gameObject.GetComponent<Murcielago>();
        photonView = animator.gameObject.GetComponent<PhotonView>();
        puntoinicial = murcielago.puntoinicial;
        velocidadMovimiento = 3; // Velocidad a la que regresa
    }
 
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Solo el maestro controla el movimiento del murciélago
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
 
        // Movemos el murciélago hacia su posición inicial
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, puntoinicial, velocidadMovimiento * Time.deltaTime);
 
        // Giramos el murciélago para que mire hacia donde va
        murcielago.Girar(puntoinicial);
 
        // Si llegamos a la posición inicial, activamos el trigger "Llego"
        if (Vector2.Distance(animator.transform.position, puntoinicial) < 0.1f)
        {
            animator.SetTrigger("Llego");
        }
    }
 
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
