using UnityEngine;
using TMPro;

public class CreateIntroPuzzleKeyword : MonoBehaviour
{
    public TMP_FontAsset font;
    public string[] words;
    public GameObject prefab;
    public Transform[] spawnParent;

    private void Awake()
    {
        string[] selectedStrings = GetRandomStrings(words, 4);

        for(int i=0; i<selectedStrings.Length; i++)
        {
            GameObject newKeyword = Instantiate(prefab, spawnParent[i]);
            newKeyword.transform.SetParent(spawnParent[i]);
            TMP_Text textComponent = newKeyword.GetComponentInChildren<TMP_Text>();
            textComponent.font = font;
            textComponent.text = selectedStrings[i];
            newKeyword.transform.localPosition = Vector3.zero;
        }
    }

    private string[] GetRandomStrings(string[] sourceArray, int count)
    {
        string[] randomStrings = new string[count];

        int[] randomIndices = new int[count];

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, sourceArray.Length);

            while (ContainsIndex(randomIndices, randomIndex))
            {
                randomIndex = Random.Range(0, sourceArray.Length);
            }

            randomIndices[i] = randomIndex;
        }

        for (int i = 0; i < count; i++)
        {
            randomStrings[i] = sourceArray[randomIndices[i]];
        }

        return randomStrings;
    }

    private bool ContainsIndex(int[] array, int index)
    {
        for(int i=0; i<array.Length; i++)
        {
            if(array[i]==index)
            {
                return true;
            }
        }

        return false;
    }
}
