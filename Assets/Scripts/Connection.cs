using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Photon.Pun;
using Photon.Realtime;

public class Connection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //conexion con los parámetros definidos
        PhotonNetwork.AutomaticallySyncScene = true; //se activa la sincronizacion de escena para el cambio de escena
        
    }

    // Update is called once per frame
   

    //PARA CONECTARSE AL MASTER Y VERIFICARLO
    public void OnConnectedToMaster()
    {
        Debug.Log("Conectando al master");
    }

    //CONECTARSE AL MASTER CON EL BOTON
    public void ConnectButton()
    {
        RoomOptions options = new RoomOptions() { MaxPlayers=4};
        PhotonNetwork.JoinOrCreateRoom("room1", options, TypedLobby.Default);
    }

    //CONEXION A UNA SALA
    public void OnJoinedRoom()
    {
        Debug.Log("Conectado a la sala" + PhotonNetwork.CurrentRoom.Name); //indica a que sala te has conectado
        Debug.Log("Jugadores conectados: " + PhotonNetwork.CurrentRoom.PlayerCount); //indica los jugadores que hay conectados
    }

   

    void Update()
    {
        //PARA PASAR A LA SIGUIENTE ESCENA CUANDO HAY MÁS DE UNA PERSONA
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            //cargamos el siguiente nivel y destruir este 
            PhotonNetwork.LoadLevel(1);
            Destroy(this);

        }

    }
}
