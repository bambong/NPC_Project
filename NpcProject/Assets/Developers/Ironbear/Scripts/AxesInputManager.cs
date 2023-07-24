using UnityEngine;

public class AxesInputManager : MonoBehaviour
{
    private static AxesInputManager instance;
    public static AxesInputManager Instance
    {
        get
        {
            if(instance==null)
            {
                instance = new GameObject("AxesInputManager").AddComponent<AxesInputManager>();
            }
            return instance;
        }
    }

    private float horizontalValue = 0f;
    private float verticalValue = 0f;

    public float GetHorizontal()
    {
        return horizontalValue;
    }

    public float GetVertical()
    {
        return verticalValue;
    }

    private void Update()
    {
        horizontalValue = Input.GetKey(KeySetting.GetKeyCode(KEY_TYPE.RIGHT_KEY)) ? 1f : (Input.GetKey(KeySetting.GetKeyCode(KEY_TYPE.LEFT_KEY)) ? -1f : 0f);
        verticalValue = Input.GetKey(KeySetting.GetKeyCode(KEY_TYPE.UP_KEY)) ? 1f : (Input.GetKey(KeySetting.GetKeyCode(KEY_TYPE.DOWN_KEY)) ? -1f : 0f);
    }
}
