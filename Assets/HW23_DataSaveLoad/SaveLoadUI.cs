using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class SaveLoadUI : MonoBehaviour
{
    private void Start()
    {
        // Canvas 생성
        GameObject canvasGO = new GameObject("SaveLoadCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        CreateButton(canvasGO, "Save", new Vector2(0, -300), () =>
        {
            Data_Controller.Instance?.SaveBeforeLeaveScene();
        });

        CreateButton(canvasGO, "Load", new Vector2(0, -380), () =>
        {
            Data_Controller.Instance?.DataActor?.Act_LoadData();
        });

        CreateButton(canvasGO, "Reset", new Vector2(0, -460), () =>
        {
            Data_Controller.Instance?.DataActor?.Act_ResetAndClearData();
        });
    }

    private void CreateButton(GameObject parent, string label, Vector2 anchoredPos, System.Action onClick)
    {
        GameObject btnGO = new GameObject(label + "Button");
        btnGO.transform.SetParent(parent.transform, false);

        Image img = btnGO.AddComponent<Image>();
        img.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

        Button btn = btnGO.AddComponent<Button>();
        btn.onClick.AddListener(() => onClick());

        RectTransform rt = btnGO.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(160, 60);
        rt.anchorMin = rt.anchorMax = new Vector2(1, 1); // 우측 상단
        rt.pivot = new Vector2(1, 1);
        rt.anchoredPosition = anchoredPos;

        // 텍스트
        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(btnGO.transform, false);
        TextMeshProUGUI tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = 24;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;

        RectTransform trt = textGO.GetComponent<RectTransform>();
        trt.anchorMin = Vector2.zero;
        trt.anchorMax = Vector2.one;
        trt.sizeDelta = Vector2.zero;
    }
    private void Update()
{
    if (Keyboard.current.f5Key.wasPressedThisFrame)
        Data_Controller.Instance?.SaveBeforeLeaveScene();

    if (Keyboard.current.f9Key.wasPressedThisFrame)
        Data_Controller.Instance?.DataActor?.Act_LoadData();
}
}