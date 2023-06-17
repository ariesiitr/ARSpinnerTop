using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;


public class SpawnManger : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPostion;

    public GameObject battleArenaGameObject ;
    public enum RaiseEventCode
    { 
    PlayerSpawnEventCode = 0 
    }


    private void Start()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent; 
    }

    #region Photon Callback Methords


    void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code == (byte)RaiseEventCode.PlayerSpawnEventCode)
        {
            object[] data = (object[])photonEvent.CustomData; 
            Vector3 receivedPosition = (Vector3)data[0];
            Quaternion receivedRotation = (Quaternion)data[1];
            int receivedPlayerselectionData = (int)data[3];

            GameObject player = Instantiate(playerPrefabs[receivedPlayerselectionData], receivedPosition+battleArenaGameObject.transform.position , receivedRotation);
            PhotonView _photonView = player.GetComponent<PhotonView>();
            _photonView.ViewID = (int)data[2];
        }
    }
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiPlayerArSpinningTopGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            {
                Debug.Log("Player selection number is " + (int)playerSelectionNumber);
                int randomSpawnPoint = Random.Range(0,spawnPostion.Length -1 );
                Vector3 instantiaePosition = spawnPostion[randomSpawnPoint].position; 
                PhotonNetwork.Instantiate(playerPrefabs[(int)playerSelectionNumber].name , instantiaePosition , Quaternion.identity);
            }
        }
        SpawnPlayer();
    }

    #endregion



    #region PrivateMethord
    private void SpawnPlayer()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiPlayerArSpinningTopGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            {
                Debug.Log("Player selection number is " + (int)playerSelectionNumber);
                int randomSpawnPoint = Random.Range(0, spawnPostion.Length - 1);
                Vector3 instantiaePosition = spawnPostion[randomSpawnPoint].position;

                GameObject playerGameObject = Instantiate(playerPrefabs[(int)playerSelectionNumber] , instantiaePosition ,Quaternion.identity);    

                PhotonView _photonView = playerGameObject.GetComponent<PhotonView>();

                if (PhotonNetwork.AllocateRoomViewID(_photonView))
                {
                    object[] data = new object[]
                    { 
                        playerGameObject.transform.position,battleArenaGameObject.transform.position,playerGameObject.transform.rotation,photonView.ViewID , playerSelectionNumber
                    };

                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                    {
                        Receivers = ReceiverGroup.Others,
                        CachingOption = EventCaching.AddToRoomCache
                    }; 

                    SendOptions sendOption = new SendOptions
                    {

                        Reliability = true
                    };

                    PhotonNetwork.RaiseEvent((byte)RaiseEventCode.PlayerSpawnEventCode,data,raiseEventOptions,sendOption);
                }
                else
                {
                    Debug.Log("Failed to allocate id");
                    Destroy(playerGameObject);
                }
            }
        }
    }
    #endregion
}
