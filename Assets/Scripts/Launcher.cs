using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;
    [SerializeField] TMP_InputField playerNameInputField;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField roomCountInputField;
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_Text roomCountText;
    [SerializeField] Transform roomListContent; 
    [SerializeField] Transform playerListContent; 
    [SerializeField] GameObject roomListPrefab; 
    [SerializeField] GameObject playerListPrefab; 
    [SerializeField] GameObject startGameButton;

    public int roomCount;
    

    void Awake()
    {
        Instance = this;  
    }

    void Start()
    {
        Debug.Log("Connecting to Master");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.AutomaticallySyncScene = true;
        MenuManager.Instance.OpenMenu("title");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.NickName = playerName.text;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined on the lobby.");
        PhotonNetwork.NickName = playerName.text;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Pilot()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = playerName.text;
    }
    public void CreateRoom()
    {
        roomCountText.text = roomCount.ToString();
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        int randonRoomName = Random.Range(0, 10000);

        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomCount };
        PhotonNetwork.CreateRoom(roomNameInputField.text, roomOps);
        MenuManager.Instance.OpenMenu("loading");        
        Player[] playersCount = PhotonNetwork.PlayerList;        
        startGameButton.SetActive(!PhotonNetwork.LocalPlayer.IsMasterClient);        
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }        
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed Must have a room with same name, or you didn't picked slots Error Name: " + message;
        MenuManager.Instance.OpenMenu("error");
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }
    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("find room");

    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

}
