using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace D
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private LoadingScreen loadingScreen;
        public LoadingScreen LoadingScreen => loadingScreen;
    }
}
