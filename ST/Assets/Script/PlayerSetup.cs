using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class PlayerSetup : MonoBehaviourPun
{
    public TextMeshProUGUI playerNameText;

    private void Start()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<MovementController>().enabled= true ;
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(true);
       
        }
        else
        {
            transform.GetComponent<MovementController>().enabled = false;
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(false);
        }
        setPlayerName();
    }

    void setPlayerName()
    {

        if (playerNameText != null)
        {
            if (photonView.IsMine)
            {
                playerNameText.text = "YOU";
                playerNameText.color = Color.red;
            }
            else
            {
                playerNameText.text = photonView.Owner.NickName;
            }
        }

    }
}
