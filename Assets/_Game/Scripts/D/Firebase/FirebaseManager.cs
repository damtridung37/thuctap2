using Firebase;
using UnityEngine;
using System.Threading.Tasks;

namespace D
{
    public class FirebaseManager : Singleton<FirebaseManager>
    {
        private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
        private bool firebaseInitialized = false;

        [Header("Firebase Components")]
        [SerializeField] private RemoteConfig remoteConfig;
        [SerializeField] private RealtimeDatabase realtimeDatabase;

        private async void Start()
        {
            await InitializeFirebase();
        }

        private async Task InitializeFirebase()
        {
            dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus == DependencyStatus.Available)
            {
                firebaseInitialized = true;
                Debug.Log("Firebase is ready!");

                await remoteConfig.FetchDataAsync();
                await realtimeDatabase.LoadData();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        }
    }
}
