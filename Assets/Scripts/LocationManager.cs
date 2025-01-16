using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    [Header("Locations")]
    [SerializeField] private Transform[] locations;
    [SerializeField] private GameObject[] locationUI;
    private int currentLocationIndex = 0;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float transitionDuration = 1.0f; // Duration of the camera transition

    private void Start()
    {
        foreach (GameObject location in locationUI)
        {
            location.SetActive(false);
        }
        if (locations.Length > 0)
        {
            MoveToLocation(currentLocationIndex);
        }
    }

    public void MoveToNextLocation()
    {
        currentLocationIndex++;
        if (currentLocationIndex >= locations.Length)
        {
            currentLocationIndex = 0;
        }
        MoveToLocation(currentLocationIndex);
    }

    public void MoveToPreviousLocation()
    {
        currentLocationIndex--;
        if (currentLocationIndex < 0)
        {
            currentLocationIndex = locations.Length - 1;
        }
        MoveToLocation(currentLocationIndex);
    }

    public void MoveToLocation(int index)
    {
        locationUI[currentLocationIndex].SetActive(false);
        currentLocationIndex = index;
        locationUI[currentLocationIndex].SetActive(true);
        Vector3 targetLocation = locations[index].position;
        StartCoroutine(SmoothTransition(mainCamera.transform.position, targetLocation));
    }

    private IEnumerator SmoothTransition(Vector3 start, Vector3 end)
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            t = t * t * (3f - 2f * t); // SmoothStep function
            mainCamera.transform.position = Vector3.Lerp(start, end, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = end;
    }
}
