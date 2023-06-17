using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class SpninningTopGameManger :  MonoBehaviourPunCallbacks
{
    [Header("UI")]
    public GameObject UI_InformedPanelGameObject;
    public TextMeshProUGUI UI_InformText;
    public GameObject serchForGameButtonGameObject;

    public GameObject adjust_button;
    public GameObject raycastCenterImage;
     
    #region Unity Methord
    // Start is called before the first frame update
    void Start()
    {
        UI_InformedPanelGameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion
    #region UI CallMethods

    public void JoinRandomRoom()
    {
        UI_InformText.text = "Serch for avaiable rooms ...... ";
        PhotonNetwork.JoinRandomRoom();
        serchForGameButtonGameObject.SetActive(false);
    }

    public void OnQuitMacthButtonClicked()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneLoader.Instance.LoadScene("Scene_Lobby");
        }
    }
    #endregion

    #region PHOTON Callback Methord
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        UI_InformText.text = "No Room Found";
       
        CreateAndJoinRoom();
    }
    public override void OnJoinedRoom()
    {

        adjust_button.SetActive(false);
        raycastCenterImage.SetActive(false);
        Debug.Log(PhotonNetwork.NickName + "joined to " + PhotonNetwork.CurrentRoom.Name );
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            UI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name + " waiting for others to join";
        }
        else
        {
            UI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(DeactiveAfterTime(2.0f, UI_InformedPanelGameObject));

        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UI_InformText.text = newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + PhotonNetwork.CurrentRoom.PlayerCount;
        StartCoroutine(DeactiveAfterTime( 2.0f , UI_InformedPanelGameObject)); 
    }

    public override void OnLeftRoom()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }
    #endregion

    #region PrivateMethord

    void CreateAndJoinRoom()
    {
        string RandomRoomName = "Room" + Random.Range(0, 100000);
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(RandomRoomName , roomOption );
    }

    IEnumerator DeactiveAfterTime(float _seceonds , GameObject _gameObject)
    {
        yield return new WaitForSeconds(_seceonds);

        _gameObject.SetActive(false);


    }
    #endregion

}
