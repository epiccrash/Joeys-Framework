using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        [SerializeField]
        private bool dontDestroyOnLoad = true;

        private static T _instance;
        public static T Instance => _instance;

        public virtual void Awake()
        {
            if (_instance && _instance != this) Destroy(this.gameObject);
            else
            {
                _instance = this as T;
                if (dontDestroyOnLoad) DontDestroyOnLoad(this.gameObject);
            }
        }
    }
}
