using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    [SerializeField]
    public Image popupPanel; // Assign the popup Panel in the Inspector

    public void ShowPopup()
    {
        popupPanel.enabled = true;
        Debug.Log("SET ACTIVE");
    }

    public void HidePopup()
    {
        popupPanel.enabled = false; 
    }
}