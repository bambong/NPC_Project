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
            Debug.LogError($"{typeof(T).Name} Singleton Instance �� �ϳ� �̻� �����Ǿ����ϴ�.");
            return;
        }

        instance = new T();
    }
}
