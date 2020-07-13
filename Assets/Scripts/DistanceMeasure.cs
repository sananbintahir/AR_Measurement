using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DistanceMeasure : MonoBehaviour
{

    [SerializeField] private Text distanceText;

    public GameObject spawnObject;
    public Material mat;

    private PlacementIndicator placementIndicator;
    private Vector3 initialPosition, finalPosition;
    private bool objectPlaced;
    private float distance;
    private LineRenderer lineRend, line;
    private int numLines;

    // Start is called before the first frame update
    void Start()
    {
        objectPlaced = false;
        numLines = 0;
        lineRend = GetComponent<LineRenderer>();
        placementIndicator = FindObjectOfType<PlacementIndicator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if the touch is recent, and the first marker has not been placed
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && !objectPlaced)
        {
            //make a marker at the location and store the position
            lineRend.positionCount = 2;
            GameObject obj = Instantiate(spawnObject, placementIndicator.transform.position, placementIndicator.transform.rotation);
            initialPosition = obj.transform.position;
            objectPlaced = true;
        }
        //if the touch is recent and the first marker has already been placed
        else if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && objectPlaced)
        {
            numLines++;
            //stop rendering the intermediate line
            lineRend.positionCount = 0;
            //mark the second location and store location
            GameObject obj = Instantiate(spawnObject, placementIndicator.transform.position, placementIndicator.transform.rotation);
            finalPosition = obj.transform.position;
            objectPlaced = false;
            //create a line between the two markers
            CreatLine();
            //make the canvas active and update the text with the distance between the markers
            obj.transform.Find("Canvas").gameObject.SetActive(true);
            obj.transform.Find("Canvas/Text").GetComponent<Text>().text = Vector3.Distance(finalPosition, initialPosition).ToString("F2") + " m";
        }
        //update the distance and the intermediate line position
        if (objectPlaced)
        {
            distance = Mathf.Round(Vector3.Distance(placementIndicator.transform.position, initialPosition) * 100.0f) * 0.01f;
            distanceText.text = distance.ToString();
            lineRend.SetPosition(0, initialPosition);
            lineRend.SetPosition(1, placementIndicator.transform.position);
        }

        void CreatLine()
        {
            //Create a new line
            line = new GameObject("Line" + numLines).AddComponent<LineRenderer>();
            line.material = mat;
            line.startWidth = 0.01f;
            line.useWorldSpace = false;
            line.endWidth = 0.01f;
            line.SetPosition(0, initialPosition);
            line.SetPosition(1, finalPosition);
        }
    }
}
