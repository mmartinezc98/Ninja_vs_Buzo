using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Photon.Pun;
using Photon.Realtime;

public class Connection : MonoBehaviourPunCallbacks
{
    private bool sceneLoadStarted = false; // Para evitar que se cargue la escena múltiples veces

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //conexion con los parámetros definidos
        PhotonNetwork.AutomaticallySyncScene = true; //se activa la sincronizacion de escena para el cambio de escena

    }

    // Update is called once per frame


    //PARA CONECTARSE AL MASTER Y VERIFICARLO
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectando al master");
    }

    //CONECTARSE AL MASTER CON EL BOTON
    public void ConnectButton()
    {
        RoomOptions options = new RoomOptions() { MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom("room1", options, TypedLobby.Default);
    }

    //CONEXION A UNA SALA
    public override void OnJoinedRoom()
    {
        Debug.Log("Conectado a la sala: " + PhotonNetwork.CurrentRoom.Name); //indica a que sala te has conectado
        Debug.Log("Jugadores conectados: " + PhotonNetwork.CurrentRoom.PlayerCount); //indica los jugadores que hay conectados
    }

    // Se ejecuta cuando un jugador entra a la sala
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Nuevo jugador conectado: " + newPlayer.NickName);
        Debug.Log("Jugadores totales: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }



    void Update()
    {
        //PARA PASAR A LA SIGUIENTE ESCENA CUANDO HAY 2 O MÁS PERSONAS
        if (!sceneLoadStarted && PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            sceneLoadStarted = true; //evitamos que se llame múltiples veces
            Debug.Log("Cargando escena del juego...");
            PhotonNetwork.LoadLevel(1);

        }

    }
}
