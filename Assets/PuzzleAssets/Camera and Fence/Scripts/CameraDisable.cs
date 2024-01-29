using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDisable : MonoBehaviour
{
    private bool cameraEnabled = true;
    private ViewconeDetection detectionScript;
    public GameObject swivel;
    private GameObject viewcone;
    private GlobalArrowManager globalArrowManager;

    private Pan panRef;
    private Viewcone viewconeRef;
    private Renderer rendererRef;

    private void Start()
    {
        detectionScript = transform.parent.parent.GetComponent<ViewconeDetection>();
        viewcone = swivel.transform.GetChild(0).gameObject;
        globalArrowManager = FindObjectOfType<GlobalArrowManager>();
        Debug.Log("Start: Camera detection, panning, and viewcone are enabled.");

        panRef = swivel.GetComponent<Pan>();
        viewconeRef = viewcone.GetComponent<Viewcone>();
        rendererRef = viewcone.GetComponent<Renderer>();
        viewconeRef.Rebuild();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if the arrow hit the camera and it was electrifies
        if (this.gameObject == globalArrowManager.latestObjectHit 
            && globalArrowManager.latestArrowShot.GetComponent<ArrowManager>().electricEnabled)
            DisableCameraDetection();
    }

    public void EnableCameraDetection()
    {
        cameraEnabled = true;
        detectionScript.enabled = panRef.enabled = viewconeRef.enabled = rendererRef.enabled = cameraEnabled;
        viewconeRef.Rebuild();
        Debug.Log("Camera enabled!");
    }

    public void DisableCameraDetection()
    {
        cameraEnabled = false;
        detectionScript.enabled = panRef.enabled = viewconeRef.enabled = rendererRef.enabled = cameraEnabled;
        Debug.Log("Camera disabled!");
    }
}
