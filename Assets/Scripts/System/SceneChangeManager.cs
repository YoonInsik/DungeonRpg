using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : PersistentSingleton<SceneChangeManager>
{
    [SerializeField] private CanvasGroup fadeImg;
    [SerializeField] private float fadeDuration;

    protected override void Awake()
    {
        base.Awake();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(int index)
    {
        fadeImg.DOFade(1, fadeDuration)
            .OnStart(() => fadeImg.blocksRaycasts = true)
            .OnComplete(() => SceneManager.LoadScene(index));

    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트에서 제거*
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fadeImg.DOFade(0, fadeDuration)
        .OnComplete(() => {
            fadeImg.blocksRaycasts = false;
        });
    }
}
