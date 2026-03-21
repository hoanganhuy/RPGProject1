using UnityEngine;
using UnityEngine.UI;

public abstract class LocationInteractableBase
    : MonoBehaviour, ILocationInteractable
{
    Button button;
    GameManager gm;

    public void Init(GameManager gameManager)
    {
        gm = gameManager;
    }

    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (gm != null)
            Interact(gm);
        else
            Debug.LogWarning("GameManager not set on interactable");
    }

    public abstract void Interact(GameManager gm);
}