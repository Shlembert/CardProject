using Cysharp.Threading.Tasks;
using UnityEngine;

public class ESCManager : MonoBehaviour
{
    [SerializeField] private StateManager stateManager;
    [SerializeField] private GameController gameController;
    [SerializeField] private LanguageController languageController;
    [SerializeField] private DataSlotsManager dataSlotsManager;

    public void ButtonChangeState(int index)
    {
        if (index == 11)
        {
            if (stateManager.State == StateGame.MenuToSetting) stateManager.State = StateGame.LanguageMenu;
            else stateManager.State = StateGame.LanguagePlay;
        }
        else if(index == 4)
        {
            if (stateManager.State == StateGame.Menu) stateManager.State = StateGame.LoadToMenu;
            else stateManager.State = StateGame.LoadToPlay;
        }
        else stateManager.State = (StateGame)index;
        
        Debug.Log($"Set State Button: {stateManager.State}");
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) SwitchingState();
    }

    private void SwitchingState()
    {
        Debug.Log($"Current State: {stateManager.State}");
        switch (stateManager.State)
        {
            case StateGame.Menu:
                gameController.ShowMenuESC();
                stateManager.State = StateGame.DialogEsc;
                break;
            case StateGame.Play:
                gameController.ShowPlayESC();
                stateManager.State = StateGame.DialogToMenu;
                break;
            case StateGame.MenuToSetting:
                gameController.GoToMenu();
                stateManager.State = StateGame.Menu;
                break;
            case StateGame.PlayToSetting:
                gameController.HidePause();
                stateManager.State = StateGame.Play;
                break;
            case StateGame.LoadToMenu:
                gameController.HideLoad();
                stateManager.State = StateGame.Menu;
                break;
            case StateGame.LoadToPlay:
                gameController.HideLoad();
                stateManager.State = StateGame.PlayToSetting;
                break;
            case StateGame.LoadDialogueMenu:
                dataSlotsManager.HideDialog();
                stateManager.State = StateGame.MenuToSetting;
                break;
            case StateGame.LoadDialoguePlay:
                dataSlotsManager.HideDialog();
                stateManager.State = StateGame.LoadToPlay;
                break;
            case StateGame.Map:
                gameController.HideMap().Forget();
                stateManager.State = StateGame.Play;
                break;
            case StateGame.Dead:
                gameController.GoToMenu();
                stateManager.State = StateGame.Menu;
                break;
            case StateGame.Win:
                gameController.GoToMenu();
                stateManager.State = StateGame.Menu;
                break;
            case StateGame.LanguageMenu:
                languageController.HidePanel();
                stateManager.State = StateGame.MenuToSetting;
                break;
            case StateGame.LanguagePlay:
                languageController.HidePanel();
                stateManager.State = StateGame.PlayToSetting;
                break;
            case StateGame.DialogToMenu:
                gameController.HidePlayESC();
                stateManager.State = StateGame.Play;
                break;
            case StateGame.DialogEsc:
                gameController.HideMenuESC();
                stateManager.State = StateGame.Menu;
                break;
            default:
                break;
        }

        Debug.Log($"Set Switch State: {stateManager.State}");
    }
}
