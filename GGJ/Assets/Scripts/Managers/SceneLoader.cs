using System;
using System.Collections;
using Scene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Managers
{
    public class SceneLoader : MonoBehaviour
    {
        private static SceneLoader _instance;
        public static SceneLoader Obj => _instance;
        
        [Header("Helper UI")]
        [SerializeField] private GameObject loadingScreenPrefab;
        
        [Header("Managers")]
        [SerializeField] private GameObject gameManagerPrefab;
        [SerializeField] private GameObject audioManagerPrefab;

        private GameObject _loadingScreen = null;
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(_instance);
        }

        private void Start()
        {
            //Load GameManager and AudioManager
            Instantiate(gameManagerPrefab, transform);
            Instantiate(audioManagerPrefab, transform);
        }

        IEnumerator LoadAsynchronously(SceneData sceneData, bool additive = false)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneData.sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);

            if (_loadingScreen == null)
            {
                _loadingScreen = Instantiate(loadingScreenPrefab, transform);
            }
            
            _loadingScreen.SetActive(true);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                //if (progressBar != null) progressBar.value = progress;
            
                yield return null;
            }

            _loadingScreen.SetActive(false);
        }
    }
}
