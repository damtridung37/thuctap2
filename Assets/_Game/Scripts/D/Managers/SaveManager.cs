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
    
        public static T Load()
        {
            if (PlayerPrefs.HasKey(SaveKey))
            {
                string json = PlayerPrefs.GetString(SaveKey);
                return JsonUtility.FromJson<T>(json);
            }
            return new T();
        }
    
        public static void Delete()
        {
            PlayerPrefs.DeleteKey(SaveKey);
            PlayerPrefs.Save();
        }
    }
}
