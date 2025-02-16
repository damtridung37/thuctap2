using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace D
{
    public static class SaveManager<T> where T : class, new()
    {
        private static readonly string SaveKey = typeof(T).FullName;
    
        public static void Save(T data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }
    
        public static T Load(bool debug = false)
        {
            if (PlayerPrefs.HasKey(SaveKey))
            {
                string json = PlayerPrefs.GetString(SaveKey);
                if (debug)
                {
                    Debug.Log($"{SaveKey}: {json}");
                }
                return JsonUtility.FromJson<T>(json);
            }
            T t = new T();
            Save(t);
            return t;
        }
    
        public static void Delete()
        {
            if (PlayerPrefs.HasKey(SaveKey))
            {
                PlayerPrefs.DeleteKey(SaveKey);
                PlayerPrefs.Save();
            }
        }
    }
}
