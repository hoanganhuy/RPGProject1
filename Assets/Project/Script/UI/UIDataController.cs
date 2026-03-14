using TMPro;
using UnityEngine;

public class UIDataController : MonoBehaviour
{
    public TextMeshProUGUI locationText;

    public void UpdateLocation(string name)
    {
        locationText.text = name;
    }
}