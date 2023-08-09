using UnityEngine;
using UnityEditor;
using TMPro;

public class CreateInstructions : MonoBehaviour
{
    [SerializeField]
    private TMP_FontAsset font;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Transform[] spawnParent;
    [SerializeField]
    private KEY_TYPE[] keyTypes;

    private string[] keyStrings;
    private KeySetting keySetting;

    private void Awake()
    {
        keySetting = KeySetting.Instance;

        keyStrings = new string[keyTypes.Length];

        keyStrings = GetInstructionStrings();

        for (int i = 0; i < keyStrings.Length; i++)
        {
            GameObject newKeyword = Instantiate(prefab, spawnParent[i]);
            newKeyword.transform.SetParent(spawnParent[i]);
            TMP_Text textComponent = newKeyword.GetComponentInChildren<TMP_Text>();
            textComponent.font = font;
            textComponent.text = keyStrings[i];
            newKeyword.transform.localPosition = Vector3.zero;
        }
    }

    private string[] GetInstructionStrings()
    {
        for (int i = 0; i < keyTypes.Length; i++)
        {
            keyStrings[i] = keySetting.currentKeys[keyTypes[i]].ToString();
        }

        return keyStrings;
    }
}
