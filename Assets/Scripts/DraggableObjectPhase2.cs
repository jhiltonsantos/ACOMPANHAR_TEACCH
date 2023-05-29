using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using System.Collections.Generic;

public class DraggableObjectPhase2 : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public bool isBeingDragged;
    private Rigidbody rb;
    private DestinationBoxPhase2Script destinationBoxPhase2Script;

    private void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        rb = GetComponent<Rigidbody>();
        destinationBoxPhase2Script = FindObjectOfType<DestinationBoxPhase2Script>();
    }

    private void Update()
    {
        if (isBeingDragged && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                MoveObject(touch);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                StopDragging();
            }
        }
    }

    private void MoveObject(Touch touch)
    {
        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            transform.position = hitPose.position;
        }
    }

    private void OnMouseDown()
    {
        if (!isBeingDragged)
        {
            StartDragging();
        }
    }

    private void StartDragging()
    {
        isBeingDragged = true;
        rb.isKinematic = true;
    }

    private void StopDragging()
    {
        isBeingDragged = false;
        rb.isKinematic = false;

        if (destinationBoxPhase2Script != null)
        {
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        Camera mainCamera = Camera.main;
        Vector3 cameraCenter = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, mainCamera.nearClipPlane));
        transform.position = cameraCenter;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DestinoCaixa") || other.CompareTag("DestinoCaixaPhase2"))
        {
            GameManagerPhase2 gameManager = FindObjectOfType<GameManagerPhase2>();
            if (gameManager != null)
            {
                if (!gameManager.IsObjectAlreadyActivated(gameObject))
                {
                    gameManager.ObjectReachedDestination();
                    gameObject.SetActive(false);
                }
                else
                {
                    ResetPosition();
                }
            }
        }
        else
        {
            ResetPosition();
        }
    }
}
