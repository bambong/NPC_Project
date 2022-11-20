using UnityEngine;

public abstract class Singleton<T> where T : new()
{
    public static T Instance 
    { 
        get 
        {
            if(instance  == null) 
            {
                Generate();
            }
            return instance;
        }
    }

    private static T instance;
    private static void Generate()
    {
        if(instance != null) 
        {
            Debug.LogError($"{typeof(T).Name} Singleton Instance 가 하나 이상 생성되었습니다.");
            return;
        }

        instance = new T();
    }
}
