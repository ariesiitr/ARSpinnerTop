using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacement : MonoBehaviour
{
    ARRaycastManager m_ARRaycastManager;
    static List<ARRaycastHit> raycast_Hits = new List<ARRaycastHit>();//sorted by distance

    public Camera ARCamara;

    public GameObject BattleArenaGameObject;

    private void Awake()
    {
            m_ARRaycastManager = GetComponent<ARRaycastManager>();
    }


    void Start()
    {
        
    }

    
    void Update()
    {
        Vector3 centerOfScreen = new Vector3 (Screen.width / 2, Screen.height / 2);
        Ray ray = ARCamara.ScreenPointToRay (centerOfScreen);


        if (m_ARRaycastManager.Raycast(ray, raycast_Hits, TrackableType.PlaneWithinPolygon)) ;
        {
            Pose hitPose = raycast_Hits[0].pose;
            Vector3 positionToPlaced = hitPose.position;
            BattleArenaGameObject.transform.position = positionToPlaced;
        }
     }
}
