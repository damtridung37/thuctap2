using Firebase;
using UnityEngine;
using System.Threading.Tasks;

namespace D
{
    public class FirebaseManager : Singleton<FirebaseManager>
    {
        private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
        private bool firebaseInitialized = false;
        public bool FirebaseInitialized => firebaseInitialized;

        [Header("Firebase Components")]
        [SerializeField] private RemoteConfig remoteConfig;
        [SerializeField] private RealtimeDatabase realtimeDatabase;

        private PlayerData playerData;
        public PlayerData PlayerData => playerData;
        public void SavePlayerData(PlayerData data)
        {
            playerData = data;
            realtimeDatabase.SaveData(data);
        }

        private async void Start()
        {
            await InitializeFirebase();
        }

        private async Task InitializeFirebase()
        {
            dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase is ready!");

                await remoteConfig.FetchDataAsync();
                playerData = await realtimeDatabase.LoadData();

                await Task.Delay(1000);
                firebaseInitialized = true;
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        }
    }
}
