using UnityEngine;

public class StateManager : MonoBehaviour
{
    private StateGame _state = StateGame.Menu;
    public StateGame State { get => _state; set => _state = value; }
}

public enum StateGame
{
    Menu = 0,
    Play = 1,
    MenuToSetting = 2,
    PlayToSetting = 3,
    LoadToMenu = 4,
    LoadToPlay = 5,
    LoadDialoguePlay = 6,
    LoadDialogueMenu = 7,
    Map = 8,
    Dead = 9,
    Win = 10,
    LanguageMenu = 11,
    LanguagePlay = 12,
    DialogToMenu = 13,
    DialogEsc = 14
}
