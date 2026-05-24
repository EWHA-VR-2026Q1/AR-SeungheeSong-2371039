using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ButtonTextChanger : MonoBehaviour
{
    public TextMeshProUGUI targetText;
    private int clickCount = 0;

    public void OnButtonClick()
    {
        clickCount++;
        targetText.text = "Clicked! " + clickCount + "times";
    }
    public GameObject sphere;

void Update()
{
    if (Keyboard.current.enterKey.wasPressedThisFrame)
    {
        if (sphere != null)
            sphere.SetActive(!sphere.activeSelf);
    }
}
}
