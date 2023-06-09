using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacementManagerPhase3 : MonoBehaviour
{
    ARRaycastManager m_ARRaycastManager;
    static List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    public Camera aRCamera;
    public GameObject box1GameObject;
    public GameObject box2GameObject;
    public GameObject destiny1GameObject;
    public GameObject destiny2GameObject;
    public Vector3 destination1Offset;
    public Vector3 destination2Offset;
    public Vector3 boxOffset1;
    public Vector3 boxOffset2;
    private GameManagerPhase3 gameManager;

    private void Awake()
    {
        m_ARRaycastManager = GetComponent<ARRaycastManager>();
        gameManager = FindObjectOfType<GameManagerPhase3>();

        DestinationBoxPhase3Script[] destinationBoxScripts = FindObjectsOfType<DestinationBoxPhase3Script>();
        foreach (DestinationBoxPhase3Script destinationBoxScript in destinationBoxScripts)
        {
            gameManager.destinationBoxes.Add(destinationBoxScript);
        }
    }

    void Update()
    {
        Vector3 centerOfScreen = new Vector3(Screen.width / 2, Screen.height / 2);
        Ray ray = aRCamera.ScreenPointToRay(centerOfScreen);

        if (m_ARRaycastManager.Raycast(ray, raycastHits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = raycastHits[0].pose;
            Vector3 positionToBePlaced = hitPose.position;
            box1GameObject.transform.position = positionToBePlaced + boxOffset1;
            box2GameObject.transform.position = positionToBePlaced + boxOffset2;
            destiny1GameObject.transform.position = positionToBePlaced + destination1Offset;
            destiny2GameObject.transform.position = positionToBePlaced + destination2Offset;
        }
    }
}
