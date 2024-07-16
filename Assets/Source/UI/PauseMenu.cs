using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;
using Cysharp.Threading.Tasks;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
    public ScriptableTimeContext PlayerTimeContext;
    public float TotalPauseTime = 1;
    public LeanTweenType EaseCurve;
    bool _paused = false;

    [Inject]
    private SceneController _sceneController;

    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private TextMeshProUGUI _titleText;

    [SerializeField]
    private GameObject _restartGameButton;
    [SerializeField]
    private GameObject _returnToGameButton;

    bool _gameOver = false;

    void Start()
    {
        transform.localPosition = Vector2.down * 1080;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public async void OnGameOver(bool victory)
    {
        _gameOver = true;
        await UniTask.WaitForSeconds(victory ? 1 : 3);
        TogglePause();
        _titleText.text = victory ? "Victory!" : "Defeat";
        _restartGameButton.SetActive(true);
        _returnToGameButton.SetActive(false);
        ShowMenu(true);

    }

    private void TogglePause()
    {
        _restartGameButton.SetActive(false);
        _returnToGameButton.SetActive(true);
        _titleText.text = "Paused";
        _paused = !_paused;
        LeanTween.cancel(gameObject);
        ShowMenu(_paused);
        SetGameSpeed(_paused);
    }

    private void ShowMenu(bool show)
    {
        _canvasGroup.interactable = false;
        LeanTween.moveLocal(gameObject, show ? Vector2.zero : Vector2.down * 1080, TotalPauseTime)
        .setEase(EaseCurve)
        .setOnComplete(() => _canvasGroup.interactable = show);
    }

    private void SetGameSpeed(bool paused)
    {
        LeanTween.value(gameObject, PlayerTimeContext.Value, paused ? 0 : 1, TotalPauseTime)
        .setEase(EaseCurve)
        .setOnUpdate((float value) =>
        {
            PlayerTimeContext.SetDeltaTimeMultiplier(value);
        });
    }

    public void ReturnToMenu()
    {
        _canvasGroup.interactable = false;
        _sceneController.LoadSceneAsync(SceneController.MAINMENU_SCENEID);
    }

    public void Restart()
    {
        _canvasGroup.interactable = false;
        _sceneController.LoadSceneAsync(SceneController.GAMEPLAY_SCENEID);
    }

    public void Continue()
    {
        TogglePause();
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
}
