using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    public GameObject[] ArPrefabs;

    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();

    void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable() => trackedImages.trackedImagesChanged += OnTrackedImagesChanged;

    void OnDisable() => trackedImages.trackedImagesChanged -= OnTrackedImagesChanged;

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Handle added tracked images
        foreach (var trackedImage in eventArgs.added)
        {
            if (IsValidTrackedImage(trackedImage))
            {
                SpawnOrUpdateObject(trackedImage);
            }
        }

        // Handle updated tracked images
        foreach (var trackedImage in eventArgs.updated)
        {
            if (IsValidTrackedImage(trackedImage))
            {
                SpawnOrUpdateObject(trackedImage);
            }
        }

        // Handle removed tracked images
        foreach (var trackedImage in eventArgs.removed)
        {
            if (trackedImage.referenceImage != null &&
                spawnedObjects.TryGetValue(trackedImage.referenceImage.name, out var trackedObject))
            {
                Destroy(trackedObject);
                spawnedObjects.Remove(trackedImage.referenceImage.name);
            }
        }
    }

    private void SpawnOrUpdateObject(ARTrackedImage trackedImage)
    {
        if (!spawnedObjects.TryGetValue(trackedImage.referenceImage.name, out var existingObject))
        {
            // Spawn a new object if one doesn't exist
            foreach (var arPrefab in ArPrefabs)
            {
                if (arPrefab.name == trackedImage.referenceImage.name)
                {
                    var newPrefab = Instantiate(arPrefab, trackedImage.transform);
                    spawnedObjects[trackedImage.referenceImage.name] = newPrefab;
                }
            }
        }
        else
        {
            // Update the existing object
            existingObject.SetActive(trackedImage.trackingState == TrackingState.Tracking);
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                existingObject.transform.position = trackedImage.transform.position;
                existingObject.transform.rotation = trackedImage.transform.rotation;
            }
        }
    }

    private bool IsValidTrackedImage(ARTrackedImage trackedImage)
    {
        // Check if the tracked image and its reference image are valid
        return trackedImage != null && trackedImage.referenceImage != null && !string.IsNullOrEmpty(trackedImage.referenceImage.name);
    }
}
