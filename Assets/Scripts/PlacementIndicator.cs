using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementIndicator : MonoBehaviour
{
    private ARRaycastManager managerRay;
    private GameObject indicatorObject;

    private void Start()
    {
        managerRay = FindObjectOfType<ARRaycastManager>();
        indicatorObject = transform.GetChild(0).gameObject;

        indicatorObject.SetActive(false);
    }

    private void Update()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        //raycast from center of the screen
        managerRay.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        //if raycast hits
        if (hits.Count > 0)
        {
            //update indicator's transform
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;

            if (!indicatorObject.activeInHierarchy)
            {
                indicatorObject.SetActive(true);
            }
        }
    }
}
