using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform holdPanel, mapPanel, mainMenuPanel, winPanel, deadPanel, pausePanel, loadPanel;
    [SerializeField] private Image bg;
    [SerializeField] private HPController hPController;
    [SerializeField] private SoundController soundController;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private MapController mapController;
    [SerializeField] private HPController hpController;
    [SerializeField] private GameObject createNewSaveButton;
    [SerializeField] private float duration, posUp;

    private Image _imageHold;
    private Color _normalColorHold;
    private Vector3 _positionUp;
    private bool _isGameOver = false;
    private bool _isMineMenu = true;

    public bool IsGameOver { get => _isGameOver; set => _isGameOver = value; }

    private void Start()
    {
        _imageHold = holdPanel.GetComponent<Image>();
        _normalColorHold = _imageHold.color;
        _positionUp = new Vector3(0, posUp, 0);
        holdPanel.position = _positionUp;
    }

    public async UniTask ShowHold()
    {
        var taskCompletionSource = new UniTaskCompletionSource();
        holdPanel.position = Vector3.zero;

        _imageHold.DOFade(0, duration).From().OnComplete(() =>
        {
            taskCompletionSource.TrySetResult();
        });

        await taskCompletionSource.Task;
    }

    public async UniTask HideHold()
    {
        var taskCompletionSource = new UniTaskCompletionSource();

        _imageHold.DOFade(0, duration).OnComplete(() =>
        {
            holdPanel.position = _positionUp;
            _imageHold.color = _normalColorHold;
            taskCompletionSource.TrySetResult();
        });

        await taskCompletionSource.Task;
    }

    public async void ShowMenu()
    {
        _isMineMenu = true;
        soundController.PlayMenuMusic();
        await ShowHold();
        mainMenuPanel.position = Vector3.zero;
        await HideHold();
    }

    public async void HideMenu()
    {
        _isMineMenu = false;
        soundController.PlayGameMusic();
        await ShowHold();

        mainMenuPanel.position = _positionUp;
        await HideHold();
    }

    public async void ShowLoad()
    {
        createNewSaveButton.SetActive(!_isMineMenu);
        await ShowHold();
        pausePanel.position = _positionUp;
        loadPanel.position = Vector3.zero;
        await HideHold();
    }

    public async void HideLoad()
    {
        await ShowHold();

        loadPanel.position = _positionUp;
        pausePanel.position = Vector3.zero;
        await HideHold();
    }

    public async void ShowPause()
    {
        soundController.PlayMenuMusic();
        await ShowHold();
        pausePanel.position = Vector3.zero;
        await HideHold();
    }

    public async void HidePause()
    {
        soundController.PlayGameMusic();
        await ShowHold();
        pausePanel.position = _positionUp;
        await HideHold();
    }

    public async void ShowMap()
    {
        soundController.PlayMapMusic();
        await ShowHold();
        mapPanel.position = Vector3.zero;

        await HideHold();
    }

    public async UniTask HideMap()
    {
        soundController.PlayGameMusic();
        await ShowHold();
        mapPanel.position = _positionUp;
        mainMenuPanel.position = _positionUp;

        await HideHold();
        await UniTask.Yield();
    }


    public async UniTask ShowDead()
    {
        Debug.Log("Show Dead");
        await ShowHold();
        deadPanel.position = Vector3.zero;
        await HideHold();
        await UniTask.Yield();
    }

    public async UniTask HideDead()
    {
        _isGameOver = false;

        await ShowHold();
        deadPanel.position = _positionUp;
        await HideHold();
        await UniTask.Yield();
    }
    public async UniTask ShowWin()
    {
        _isGameOver = true;
        await ShowHold();
        winPanel.position = Vector3.zero;
        await HideHold();
    }

    public async UniTask HideWin()
    {
        _isGameOver = false;
        await ShowHold();
        winPanel.position = _positionUp;
        await HideHold();
        await UniTask.Yield();
    }
    public async void GoToMenu()
    {
        _isMineMenu = true;
        await ShowHold();
        inventoryController.ClearInventory();
        hpController.SetHP(3);
        mapController.DeactivateCheckPoint(0);
        pausePanel.position = _positionUp;
        loadPanel.position = _positionUp;
        deadPanel.position = _positionUp;
        winPanel.position = _positionUp;
        mainMenuPanel.position = Vector3.zero;
        await HideHold();
    }

    public async void GoToMap()
    {
        await ShowHold();
        deadPanel.position = _positionUp;
        mapPanel.position = Vector3.zero;
        await HideHold();
    }

    public async UniTask ChangeBG(Sprite sprite)
    {
        await ShowHold();

        bg.sprite = sprite;
     
        await HideHold();
    }

    public void ClearForScreenshot()
    {
        loadPanel.position = _positionUp;
    }

    public void CompleteScreenshot()
    {
        loadPanel.position = Vector3.zero;
    }

    public void ExitGame()=> Application.Quit();
    public void HideMapButton() => HideMap().Forget();
}
