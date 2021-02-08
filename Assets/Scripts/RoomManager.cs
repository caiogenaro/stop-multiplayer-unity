using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;
    [SerializeField] TMP_Text roomName;
    [SerializeField] public TMP_Text[] answer;
    [SerializeField] GameObject nextQuestion;  
    [SerializeField] GameObject answerMenu;
    [SerializeField] GameObject[] scoresPlayer;
    [SerializeField] GameObject[] buttonCorrect;
    [SerializeField] GameObject[] buttonIncorrect;
    [SerializeField] GameObject[] answerPlayer;    
    [SerializeField] GameObject[] buttonAnswerQuestion;    


    public bool asnwersReady = true;
    public bool scoreReady = false;
    public bool letterReady = true;
    
    public TMP_Text[] answerPlayerShow;
    public TMP_Text[] answerPlayerShow1;
    public TMP_Text[] scorePlayerShow;    
    public int questionTrigger = 0;
    public int answerTrigger = 0;    


    public TextMeshProUGUI text;
    public GameObject randomizeButton;


    public void Awake()
    {
        if (Instance == null)
        {            
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

        roomName.text = PhotonNetwork.CurrentRoom.CustomProperties["RoomName"].ToString();


    }

    public void AsnwerQuestion(int answerIndex)
    {
        if(answerIndex == 0)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { {"Pais", answer[answerIndex].text } });
            questionTrigger = answerIndex;
            buttonAnswerQuestion[answerIndex].SetActive(false);
        }
        if(answerIndex == 1)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { {"Nome", answer[answerIndex].text } });
            questionTrigger = answerIndex;
            buttonAnswerQuestion[answerIndex].SetActive(false);
        }
        if(answerIndex == 2)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { {"Cor", answer[answerIndex].text } });
            questionTrigger = answerIndex;
            buttonAnswerQuestion[answerIndex].SetActive(false);
        }
        if(answerIndex == 3)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { {"Filme", answer[answerIndex].text } });
            questionTrigger = answerIndex;
            buttonAnswerQuestion[answerIndex].SetActive(false);
        }
        if(answerIndex == 4)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { {"PCH", answer[answerIndex].text } });
            questionTrigger = answerIndex;
            buttonAnswerQuestion[answerIndex].SetActive(false);
        }
        if(answerIndex == 5)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { {"SOGRA", answer[answerIndex].text } });
            questionTrigger = answerIndex;
            buttonAnswerQuestion[answerIndex].SetActive(false);
        }
        
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        AtualizeAnswers();
        AtualizeScores();
    }


    public void AtualizeAnswers()
    {
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
            {            
                answerPlayerShow[i].text = players[i].CustomProperties["Pais"].ToString();
            }

        /*if(answerTrigger == (int)PhotonNetwork.CurrentRoom.CustomProperties["Answers"])
        {
            for (int i = 0; i < players.Length; i++)
            {            
                answerPlayerShow1[i].text = players[i].CustomProperties["Nome"].ToString();
                buttonCorrect[i].SetActive(true);
                buttonIncorrect[i].SetActive(true);
            }
        }
        /*if((int)PhotonNetwork.CurrentRoom.CustomProperties["Answers"] == 2)
        {
            for (int i = 0; i < players.Length; i++)
            {            
                answerPlayerShow[i].text = players[i].CustomProperties["Cor"].ToString();
                buttonCorrect[i].SetActive(true);
                buttonIncorrect[i].SetActive(true);
            }
        }
        if((int)PhotonNetwork.CurrentRoom.CustomProperties["Answers"] == 3)
        {
            for (int i = 0; i < players.Length; i++)
            {            
                answerPlayerShow[i].text = players[i].CustomProperties["Filme"].ToString();
                buttonCorrect[i].SetActive(true);
                buttonIncorrect[i].SetActive(true);
            }
        }
        if((int)PhotonNetwork.CurrentRoom.CustomProperties["Answers"] == 4)
        {
            for (int i = 0; i < players.Length; i++)
            {            
                answerPlayerShow[i].text = players[i].CustomProperties["PCH"].ToString();
                buttonCorrect[i].SetActive(true);
                buttonIncorrect[i].SetActive(true);
            }
        }
        if((int)PhotonNetwork.CurrentRoom.CustomProperties["Answers"] == 5)
        {
            for (int i = 0; i < players.Length; i++)
            {            
                answerPlayerShow[i].text = players[i].CustomProperties["SOGRA"].ToString();
                buttonCorrect[i].SetActive(true);
                buttonIncorrect[i].SetActive(true);
            }
        }*/


        text.text = "Letter" + PhotonNetwork.CurrentRoom.CustomProperties["NewLetter"];
        Debug.Log("AQUI" + PhotonNetwork.CurrentRoom.CustomProperties["NewLetter"]);




        nextQuestion.SetActive(PhotonNetwork.IsMasterClient);
        if(asnwersReady == (bool)PhotonNetwork.CurrentRoom.CustomProperties["Answers"])
        {
            answerMenu.SetActive(true);
            SpawnManager.Instance.questionMenu.SetActive(false);
            asnwersReady = false;
            scoreReady = true;

            for (int i = 0; i < players.Length; i++)
            {
                scoresPlayer[i].SetActive(true);
                buttonCorrect[i].SetActive(true);
                buttonIncorrect[i].SetActive(true);
                answerPlayer[i].SetActive(true);

            }
        }

    }
    public void AtualizeScores()
    {
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            scorePlayerShow[i].text = PhotonNetwork.CurrentRoom.CustomProperties["Score"+i].ToString();
        }

    }


    public void StopQuestion(bool slotIndex)
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Answers", slotIndex } });
    }
    public void NextAnswer(int answerIndex)
    {
        answerTrigger++;
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "AnswersTrigger", answerTrigger } });

    }

    public void ConfirmButton(int confirmIndex)
    {
        int score = 0;
        score++;
        Hashtable RoomCustomProps = new Hashtable();        
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Score"+confirmIndex, score++ } });
        buttonCorrect[confirmIndex].SetActive(false);
        buttonIncorrect[confirmIndex].SetActive(false);

    }

    public void IncorrectButton(int confirmIndex)
    {
        buttonCorrect[confirmIndex].SetActive(false);
        buttonIncorrect[confirmIndex].SetActive(false);
    }

    public void CorrectButton(int confirmIndex)
    {
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
            {            
                answerPlayerShow[i].text = players[i].CustomProperties["Pais"].ToString();                
            }
    }


    




    public void PickRandomLetter(int stringLenght)
    {
        int _stringLenght = stringLenght - 1;
        string randomString = "";
        string[] characters = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "y", "z" };
        for (int i = 0; i <= 0; i++)
        {
            randomString = characters[Random.Range(0, characters.Length)];           
        }
        randomizeButton.SetActive(false);
        StartCoroutine(StartQuestionSpawn(2.0f));
        Debug.Log("Randomizou" + ": " + randomString);
        text.text = "Letter is : " + randomString;
        Hashtable RoomCustomProps = new Hashtable();
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "NewLetter", randomString } });
        
    }

    private IEnumerator StartQuestionSpawn(float waitTime)
    {
        
        yield return new WaitForSeconds(waitTime);
        SpawnManager.Instance.StartQuestion(true);
    }
}