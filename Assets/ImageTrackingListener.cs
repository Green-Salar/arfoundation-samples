using UnityEngine;
using UnityEngine.XR.ARFoundation;
public class ImageTrackingListener : MonoBehaviour
{
    public ARTrackedImageManager arTrackedImageManager;

    void OnEnable()
    {
        arTrackedImageManager.trackablesChanged.AddListener(OnChanged);
    }

    void OnDisable()
    {
        arTrackedImageManager.trackablesChanged.RemoveListener(OnChanged);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         
    }
    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args  )
    {
        foreach(var newImage in args.added)
        {
            Debug.Log($"Image detected: {newImage.referenceImage.name}");
        }
        foreach(var updatedImage in args.updated)
        {
            Debug.Log($"Image updated: {updatedImage.referenceImage.name}");
        }

        foreach(var removedImage in args.removed)
        {
            Debug.Log($"Image removed: ");
        }
    }

    void ListAllImages()
    {
        Debug.Log(
            $"There are {arTrackedImageManager.trackables.count} images being tracked.");

        foreach (var trackedImage in arTrackedImageManager.trackables)
        {
            Debug.Log($"Image: {trackedImage.referenceImage.name} is at " +
                      $"{trackedImage.transform.position}");
        }
    }
}
