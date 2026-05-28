using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class Actor_DataSaverLoader : MonoBehaviour
{
    [Header("Objects To Save")]
    public List<GameObject> targetObjects = new List<GameObject>();

    [Header("Player To Save")]
    public GameObject playerObject; // Character_PC_Quest 드래그

    private string GetSavePath()
{
    return Path.Combine(Application.persistentDataPath, "HW23_worldData.json");
}

    public void Act_SaveData()
    {
        WorldData data = new WorldData();

        // 플레이어 저장
        if (playerObject != null)
        {
            data.objects.Add(new TransformData
            {
                objectName = "__PLAYER__",
                position = playerObject.transform.position,
                rotation = playerObject.transform.rotation
            });
        }

        // 오브젝트 저장
        foreach (GameObject obj in targetObjects)
        {
            if (obj == null) continue;
            data.objects.Add(new TransformData
            {
                objectName = obj.name,
                position = obj.transform.position,
                rotation = obj.transform.rotation
            });
        }

        File.WriteAllText(GetSavePath(), JsonUtility.ToJson(data, true));
        Debug.Log("저장 완료: " + GetSavePath());
    }

    public void Act_LoadData()
    {
        string path = GetSavePath();
        if (!File.Exists(path))
        {
            Debug.Log("세이브 파일 없음 - 기본 위치 유지");
            return;
        }

        WorldData data = JsonUtility.FromJson<WorldData>(File.ReadAllText(path));
        if (data == null) return;

        var map = new Dictionary<string, TransformData>();
        foreach (var t in data.objects)
            if (!map.ContainsKey(t.objectName)) map[t.objectName] = t;

        // 플레이어 복원
        if (playerObject != null && map.TryGetValue("__PLAYER__", out TransformData pd))
        {
            playerObject.transform.position = pd.position;
            playerObject.transform.rotation = pd.rotation;

            // CharacterController가 있으면 비활성화 후 이동
            var cc = playerObject.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            playerObject.transform.position = pd.position;
            if (cc != null) cc.enabled = true;
        }

        // 오브젝트 복원
        foreach (GameObject obj in targetObjects)
        {
            if (obj == null) continue;
            if (map.TryGetValue(obj.name, out TransformData td))
            {
                obj.transform.position = td.position;
                obj.transform.rotation = td.rotation;

                if (obj.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }

        Debug.Log("로드 완료!");
    }

    public void Act_ResetAndClearData()
    {
        string path = GetSavePath();
        if (File.Exists(path)) File.Delete(path);
        Debug.Log("데이터 초기화 완료");
    }

    // 자동 저장 (앱 종료/정지 시)
    private void OnApplicationPause(bool pause) { if (pause) Act_SaveData(); }
    private void OnApplicationQuit() { Act_SaveData(); }
}