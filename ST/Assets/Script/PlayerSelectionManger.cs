using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerSelectionManger : MonoBehaviour
{

    public Transform Ptransform;
    public int playerSelectionNumber;  
    public GameObject[] spinner;

    [Header("UI")]
    public TextMeshProUGUI playerModelType_Text;
    public Button UI_nextButton;
    public Button UI_previousButton;
    public GameObject UI_Selection;
    public GameObject UI_AfterSelection;
    public GameObject b1;
    public GameObject b2;


    #region Unity Function
    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            UI_Selection.SetActive(true);
            UI_AfterSelection.SetActive(false);
            b1.SetActive(true);
            b2.SetActive(true);
            playerSelectionNumber = 0;
        }
        else
        {
            UI_Selection.SetActive(false);
            UI_AfterSelection.SetActive(true);
            b1.SetActive(false);
            b2.SetActive(false);
            playerSelectionNumber = 0;
        }
        
    }


    #endregion

    #region UI Callback Methord
    public void NextPlayer()
    {
        if (playerSelectionNumber == 3)
        { 
            playerSelectionNumber = 0;
        }
        else
        { 
            playerSelectionNumber++; 
        }
        Debug.Log(playerSelectionNumber);
        StartCoroutine(Rotate(Vector3.up, Ptransform, 90, 1f));
        UI_nextButton.enabled = false;
        UI_previousButton.enabled = false;

        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            playerModelType_Text.text = "ATTACKER";
        }
        else
        {
            playerModelType_Text.text = "DFENDER";
        }
    }

    public void PreviousPlayer()
    {
        if (playerSelectionNumber == 0)
        {
            playerSelectionNumber = 3;
        }
        else
        {
            playerSelectionNumber--;
        }
        Debug.Log(playerSelectionNumber);
        StartCoroutine(Rotate(Vector3.up,  Ptransform, -90, 1f));
        UI_nextButton.enabled = false;
        UI_previousButton.enabled = false;
        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            playerModelType_Text.text = "ATTACKER";
        }
        else
        {
            playerModelType_Text.text = "DFENDER";
        }

    }

    public void OnSelectedButtonClicked()
    {
        UI_Selection.SetActive(false);
        UI_AfterSelection.SetActive(true);
        b1.SetActive(false);
        b2.SetActive(false);
        ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable { {MultiPlayerArSpinningTopGame.PLAYER_SELECTION_NUMBER , playerSelectionNumber}  };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
    }

    public void OnReselectButtonClicked()
    {
        UI_Selection.SetActive(true);
        UI_AfterSelection.SetActive(false);
        b1.SetActive(true);
        b2.SetActive(true);
    }

    public void OnBattleButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }

    public void OnBackButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }
    #endregion end 

    #region Private Methords

    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1.0f)
    {
        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transformToRotate.rotation = finalRotation;
        UI_nextButton.enabled = true;
        UI_previousButton.enabled = true;

    }

    #endregion
}
 