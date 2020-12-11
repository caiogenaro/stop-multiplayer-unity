using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.IO;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    public static SpawnManager Instance;
    private bool[] isSlotOccuped;
    

    public Transform[] slots;
    public GameObject[] slotsButtons;
    public GameObject questionMenuButton;
    public GameObject questionMenu;
    public int slotsActual;
    public int ready = 0;
    public bool startQuestionTrigger = true;

    public void Awake()
    {
        if(SpawnManager.Instance == null)
        {
            SpawnManager.Instance = this;
        }
        
    }
    public void Start()
    {
        isSlotOccuped = new bool[PhotonNetwork.PlayerList.Length];
        ChangeSizeSlots();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void ChangeSizeSlots()
    {

        slotsActual = PhotonNetwork.PlayerList.Length;

        for (int i = 0; i < slotsActual; i++)
        {
            slotsButtons[i].SetActive(true);
            isSlotOccuped[i] = false;

        }

    }

    void TakeSlot(int slotIndex)
    {
        if(isSlotOccuped[slotIndex] == false)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), SpawnManager.Instance.slots[slotIndex].position, SpawnManager.Instance.slots[slotIndex].rotation);    
        } 
        else
        {
            Debug.Log("Slot Occuped");
        }      
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Slot", slotIndex } });
        isSlotOccuped[slotIndex] = true;
        PlayerInfo.PI.SetName();        
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertisThatChanged)
    {

        VerifySlot();
        VerifyStartQuestion();

    }


    void VerifySlot()
    {
        slotsButtons[(int)PhotonNetwork.CurrentRoom.CustomProperties["Slot"]].SetActive(false);
        ready++;
        if(ready == slotsActual && PhotonNetwork.LocalPlayer.IsMasterClient == true)
        {
            questionMenuButton.SetActive(PhotonNetwork.IsMasterClient);
        }
        
    }
    public void StartQuestion(bool startIndex)
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "StartQuestion", startIndex } });
        
    }
    public void VerifyStartQuestion()
    {
        
        if(startQuestionTrigger == (bool)PhotonNetwork.CurrentRoom.CustomProperties["StartQuestion"])
        {
            Player[] players = PhotonNetwork.PlayerList;
            OpenQuestionMenu();

        }
    }

    void OpenQuestionMenu()
    {
        questionMenu.SetActive(true);
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "StartQuestion", false } });
    }



}
