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
            PhotonNetwork.Instantiate("Rana", new Vector3(-2.33250046f, -0.327192903f, 0), Quaternion.identity);
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
