using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
public class ARPlaneDetectionAndPlacementController : MonoBehaviour
{

    ARPlaneManager m_ARPlaneManager;
    ARPlacement m_ARPlacement;
    // Start is called before the first frame update

    public GameObject placeButton;
    public GameObject adjustButton;
    public GameObject serchForGameButton;
    public TextMeshProUGUI informUIPanal_Text;
    public GameObject scaleSlider;
    private void Awake()
    {
        m_ARPlacement = GetComponent<ARPlacement>();
        m_ARPlaneManager = GetComponent<ARPlaneManager>();  
    }
    void Start()
    {
        placeButton.SetActive(true);
        adjustButton.SetActive(false); 
        serchForGameButton.SetActive(false);
        scaleSlider.SetActive(true);
        informUIPanal_Text.text = "Move phone to detect planes and place the Battle Arena";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableARPlacementAndPlaceDetection()
    {
        m_ARPlaneManager.enabled = false;
        m_ARPlacement.enabled = false;

        SetAllPlaneActiveteOrDeactivate(false);
        placeButton.SetActive(false);
        adjustButton.SetActive(true);
        serchForGameButton.SetActive(true);
        informUIPanal_Text.text = "Great! You placed the Arena. Now , serch for game to Battle!  ";
        scaleSlider.SetActive(false);
    }


    public void EnableARPlacementAndPlaceDetection()
    {
        m_ARPlaneManager.enabled = true;
        m_ARPlacement.enabled = true;

        SetAllPlaneActiveteOrDeactivate(true);
        placeButton.SetActive(true);
        adjustButton.SetActive(false);
        serchForGameButton.SetActive(false);
        informUIPanal_Text.text = "Move phone to detect planes and place the Battle Arena";
        scaleSlider.SetActive(false);
    }
    private void SetAllPlaneActiveteOrDeactivate(bool value)
    {
        foreach (var plane in m_ARPlaneManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }
    }
}
