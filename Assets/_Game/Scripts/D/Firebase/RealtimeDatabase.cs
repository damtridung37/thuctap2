using Firebase.Database;
using System.Threading.Tasks;
using UnityEngine;
namespace D
{
    public class RealtimeDatabase : MonoBehaviour
    {
        DatabaseReference dbRef;
        private string uuid;

        private void Awake()
        {
            dbRef = FirebaseDatabase.DefaultInstance.RootReference;
            uuid = SystemInfo.deviceUniqueIdentifier;
        }

        public void SaveData(PlayerData data)
        {
            string json = JsonUtility.ToJson(data);
            dbRef.Child("users").Child(uuid).SetRawJsonValueAsync(json);
        }

        public async Task<PlayerData> LoadData()
        {
            var serverData = dbRef.Child("users").Child(uuid).GetValueAsync();

            // Wait asynchronously until data is retrieved
            await serverData;

            if (serverData.Exception != null)
            {
                Debug.LogError($"Failed to load data: {serverData.Exception}");
                return null;
            }

            DataSnapshot snapshot = serverData.Result;
            string jsonData = snapshot.GetRawJsonValue();

            if (!string.IsNullOrEmpty(jsonData))
            {
                Debug.Log("Server data found");
                Debug.Log(jsonData);
                return JsonUtility.FromJson<PlayerData>(jsonData);
            }
            else
            {
                Debug.Log("No data found, creating new entry...");
                PlayerData temp = new PlayerData();
                SaveData(temp);
                return temp;
            }
        }
    }
}
