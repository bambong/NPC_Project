using UnityEngine;
using TMPro;

public class CreateInstructions : MonoBehaviour
{
    [SerializeField]
    private TMP_FontAsset font;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Transform[] spawnParent;

    private string[] keyStrings = new string[6];
    private KeySetting keySetting;

    private void Awake()
    {
        keySetting = KeySetting.Instance;

        keyStrings = new string[6];

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
        KeyCode upKey = keySetting.currentKeys[KEY_TYPE.UP_KEY];
        KeyCode leftKey = keySetting.currentKeys[KEY_TYPE.LEFT_KEY];
        KeyCode downKey = keySetting.currentKeys[KEY_TYPE.DOWN_KEY];
        KeyCode rightKey = keySetting.currentKeys[KEY_TYPE.RIGHT_KEY];
        KeyCode interactionKey = keySetting.currentKeys[KEY_TYPE.INTERACTION_KEY];
        KeyCode runKey = keySetting.currentKeys[KEY_TYPE.RUN_KEY];

        keyStrings[0] = upKey.ToString();
        keyStrings[1] = leftKey.ToString();
        keyStrings[2] = downKey.ToString();
        keyStrings[3] = rightKey.ToString();
        keyStrings[4] = interactionKey.ToString();
        keyStrings[5] = runKey.ToString();

        return keyStrings;
    }
}
