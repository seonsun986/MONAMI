using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SSH_Player : MonoBehaviourPun
{
    public int id;              // UI¿ë ID
    public string weaponName;
    // Start is called before the first frame update
    public virtual void Start()
    {
        id = photonView.ViewID / 1000;
    }

    
}
