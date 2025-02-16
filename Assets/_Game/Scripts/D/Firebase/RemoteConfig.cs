using System;
using System.Threading.Tasks;
using Firebase.RemoteConfig;
using UnityEngine;

namespace D
{
    public class RemoteConfig : MonoBehaviour
    {
        private LastFetchStatus remoteDataState;
        private bool fetchRemoteDone = false;

        [SerializeField] private float timeOutSetFetchTrue = 2f;

        public LastFetchStatus RemoteDataState { get => remoteDataState; set => remoteDataState = value; }
        public bool FetchRemoteDone { get => fetchRemoteDone; set => fetchRemoteDone = value; }

        public void DisplayAllKeys()
        {
            Debug.Log("Current Keys:");
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            foreach (var item in remoteConfig.AllValues)
            {
                Debug.Log($"key: {item.Key}, value: {item.Value.StringValue}");
            }
        }

        public void EnableAutoFetch()
        {
            Debug.Log("Enabling auto-fetch:");
            FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener += ConfigUpdateListenerEventHandler;
        }

        public void DisableAutoFetch()
        {
            Debug.Log("Disabling auto-fetch:");
            FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener -= ConfigUpdateListenerEventHandler;
        }

        private async void ConfigUpdateListenerEventHandler(object sender, ConfigUpdateEventArgs args)
        {
            if (args.Error != RemoteConfigError.None)
            {
                Debug.LogError($"Error occurred while listening: {args.Error}");
                return;
            }

            Debug.Log($"Auto-fetch received a new config. Updated keys: {string.Join(", ", args.UpdatedKeys)}");

            var info = FirebaseRemoteConfig.DefaultInstance.Info;
            await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
            Debug.Log($"Remote data loaded and ready (last fetch time {info.FetchTime}).");
        }

        public async Task FetchDataAsync()
        {
            Debug.Log("Fetching data...");
            Invoke(nameof(SetFetchDone), timeOutSetFetchTrue);

            try
            {
                await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
                await FetchComplete();
            }
            catch (Exception e)
            {
                Debug.LogError($"Fetch encountered an error: {e.Message}");
            }
        }

        private void SetFetchDone()
        {
            FetchRemoteDone = true;
        }

        private async Task FetchComplete()
        {
            var info = FirebaseRemoteConfig.DefaultInstance.Info;

            switch (info.LastFetchStatus)
            {
                case LastFetchStatus.Success:
                    await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
                    Debug.Log($"Remote data loaded and ready (last fetch time {info.FetchTime}).");
                    DisplayAllKeys();
                    RemoteDataState = LastFetchStatus.Success;
                    FetchRemoteDone = true;
                    break;

                case LastFetchStatus.Failure:
                    RemoteDataState = LastFetchStatus.Failure;
                    Debug.LogError($"Fetch failed: {info.LastFetchFailureReason}");
                    break;

                case LastFetchStatus.Pending:
                    RemoteDataState = LastFetchStatus.Pending;
                    Debug.Log("Latest Fetch call still pending.");
                    break;
            }
        }
    }
}
