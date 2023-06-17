using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class LobbyManger : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public InputField playerNameInputField;
    public GameObject UI_LoginGameobject;


    [Header("Lobby UI")]
    public GameObject UI_LobbyGameobject;
    public GameObject UI_3DGameObject;


    [Header("Connection Status UI")]
    public GameObject UI_ConnectionStatusGameobject;
    public Text connectionStatusText;
    private bool showConnectionStatus = false;

    #region Unity Methord

    private void Start()
    {
        
        UI_3DGameObject.SetActive(false);
        UI_ConnectionStatusGameobject.SetActive(false);
        UI_LobbyGameobject.SetActive(false);

        UI_LoginGameobject.SetActive(true);

    }

    private void Update()
    {
        if (showConnectionStatus)
        { 
            connectionStatusText.text = "Connection Status" + PhotonNetwork.NetworkClientState;
        }
    }

    #endregion

    #region UI CallBack Methords
    public void OnEnterGameButtonClicked()
    {
      
        string playerName;
        playerName = playerNameInputField.text;

        if (!string.IsNullOrEmpty(playerName))  /// try playerName.null != null or " ";
        {
            UI_3DGameObject.SetActive(false);
            UI_LobbyGameobject.SetActive(false);
            UI_LoginGameobject.SetActive(false);
            showConnectionStatus = true;
            UI_ConnectionStatusGameobject.SetActive(true);


            PhotonNetwork.LocalPlayer.NickName = playerName;

            PhotonNetwork.ConnectUsingSettings();

        }
        else
        {
            Debug.Log("Player name is invaled or empty ");
        }
    }

    public void OnPessGoToLoadingSceane()
    {
        // SceneManager.LoadScene("Scene_Loading");
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection");
    }
 
    #endregion

    #region Photon CallBack Methords

    public override void OnConnected() // called when connected to internet ;
    {
        Debug.Log("You are connected to internet ");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + "is connected to photon");
        
        UI_LoginGameobject.SetActive(false);
        UI_ConnectionStatusGameobject.SetActive(false);
        UI_3DGameObject.SetActive(true);
        UI_LobbyGameobject.SetActive(true);
    }

    #endregion
}
