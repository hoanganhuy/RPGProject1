using TMPro;
using UnityEngine;

public class LocationUI : MonoBehaviour
{
    public TextMeshProUGUI locationText;

    public void UpdateLocation(string locationName)
    {
        locationText.text = locationName;
    }
}