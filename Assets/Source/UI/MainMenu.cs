using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    [Inject]
    private SceneController _sceneController;

    [SerializeField]
    private Image _selector;

    public void StartGame()
    {
        _sceneController.LoadSceneAsync(SceneController.GAMEPLAY_SCENEID);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
}
