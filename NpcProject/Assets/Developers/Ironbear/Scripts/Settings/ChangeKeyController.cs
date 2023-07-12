using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeKeyController : MonoBehaviour
{
    private KeyMappingController keyMapping;

    int key = -1;


    private void Start()
    {
        keyMapping = new KeyMappingController();

    }
    private void OnGUI()
    {
        Event keyEvent = Event.current;

        if(keyEvent.isKey)
        {
            keyMapping.keyMap[KEY_TYPE.DEBUGMOD_KEY] = keyEvent.keyCode;
            key = -1;
        }
    }

    public void ChangeKeys(KEY_TYPE keyType, KeyCode newKeyCode)
    {
        keyMapping.ChangeKey(keyType, newKeyCode);
    }
}
