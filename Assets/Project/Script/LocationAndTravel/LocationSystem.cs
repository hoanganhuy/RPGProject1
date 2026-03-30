using UnityEngine;
using UnityEngine.UI;

public class LocationSystem : MonoBehaviour
{
    public Transform locationMapHolder;
    public Image backgroundImage;
    public UIDataSystem uiData;

    GameObject currentLocationMapObj;
    LocationData currentLocation;

    public void EnterLocation(LocationData loc, GameManager gm)
    {
        currentLocation = loc;

        //backgroundImage.sprite = loc.locationBackground;
        uiData.UpdateLocation(loc.displayName);

        if (currentLocationMapObj != null)
            Destroy(currentLocationMapObj);

        currentLocationMapObj =
            Instantiate(loc.locationMapPrefab, locationMapHolder);

        // ⭐ Inject GM vào tất cả interactable
        var interactables =
            currentLocationMapObj.GetComponentsInChildren<LocationInteractableBase>();

        foreach (var i in interactables)
            i.Init(gm);

        OpenAndCLoseMapLocation(false);
    }

    public void ToggleLocationMap()
    {
        bool active = locationMapHolder.gameObject.activeSelf;
        locationMapHolder.gameObject.SetActive(!active);
    }
    public void OpenAndCLoseMapLocation(bool b)
    {
        locationMapHolder.gameObject.SetActive(b);
    }    
}