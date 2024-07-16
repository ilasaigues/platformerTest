using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public static readonly int MAINMENU_SCENEID = 0;
    public static readonly int GAMEPLAY_SCENEID = 1;

    [SerializeField]
    private GameObject _curtain1;
    [SerializeField]
    private GameObject _curtain2;
    [SerializeField]
    private GameObject _curtain3;

    public async void LoadSceneAsync(int sceneID)
    {
        await ShowLoadingScreen(true);
        await SceneManager.LoadSceneAsync(sceneID);
        await UniTask.WaitForSeconds(1); // fake loading lol
        await ShowLoadingScreen(false);
    }

    public async UniTask ShowLoadingScreen(bool show)
    {
        if (show)
        {
            _curtain1.SetActive(true);
            _curtain2.SetActive(true);
            _curtain3.SetActive(true);
        }
        bool doneTransitioning = false;
        LeanTween.rotate(_curtain1, show ? Vector3.zero : new Vector3(0, 0, 90), 1).setEase(show ? LeanTweenType.easeOutCubic : LeanTweenType.easeInCubic);
        LeanTween.rotate(_curtain2, show ? Vector3.zero : new Vector3(0, 0, 90), 1).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.rotate(_curtain3, show ? Vector3.zero : new Vector3(0, 0, 90), 1).setEase(show ? LeanTweenType.easeInCubic : LeanTweenType.easeOutCubic).setOnComplete(
            () =>
            {
                doneTransitioning = true;
                if (!show)
                {
                    _curtain1.SetActive(false);
                    _curtain2.SetActive(false);
                    _curtain3.SetActive(false);
                }
            }
        );
        await UniTask.WaitUntil(() => doneTransitioning);
    }
}
