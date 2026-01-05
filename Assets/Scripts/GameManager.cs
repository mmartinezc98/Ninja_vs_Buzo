using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Monkey", new Vector3(-2.36664629f, -0.29087007f, 0), Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("PinkMan", new Vector3(2.31285286f, -0.327192664f, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
