using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using TMPro;
using Photon.Realtime;
public class PlayerInfo : MonoBehaviour
{
    [SerializeField]  bool hasControl;
    public static PlayerInfo PI;
    public PhotonView PV;
    public GameObject myAvatar;
    public TMP_Text myName;
    public TMP_Text[] otherName;
    static Color myColor;
    static MeshFilter myMaterial;
    MeshRenderer myAvatarSprite;
    public GameObject[] slot;    

    void Start()
    {   
        if(hasControl)
        {
            PI = this;
        }     
        if (slot == null)
        {
            slot = GameObject.FindGameObjectsWithTag("Slot0");
        }
        
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            PI = this;
            myName.text = PV.Owner.NickName;            
        }
 		else
        {
            Player[] players = PhotonNetwork.PlayerListOthers;        
            
            for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                
                myName.text = players[i].NickName;
            }
        }
        myAvatarSprite = myAvatar.GetComponent<MeshRenderer>();
        if (myColor == Color.clear)
        {
            myColor = Color.white;
        }
        myAvatarSprite.material.color = myColor;
        myMaterial = myAvatar.GetComponent<MeshFilter>();        


    }
    public void SetColor(Color newColor)
    {
        Debug.Log("Changed");
        myColor = newColor;  
        if (myAvatarSprite != null)
        {
            myAvatarSprite.material.color = myColor;
        }      

    }

    public void SetGender(Mesh newMaterial)
    {
        myMaterial.mesh = newMaterial;  
    }
    public void Spawn(int spawnIndex)
    {
        
        if (PV.IsMine)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), SpawnManager.Instance.slots[spawnIndex].position, Quaternion.identity);
        }

    }

    public void SetName()
    {
        myName.text = PV.Owner.NickName;
    }
}
