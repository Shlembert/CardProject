using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private GameObject newButton;
    [SerializeField] private Transform content;

}

[Serializable]
public class GameData
{
    public int Health;
    public float MusicVolume;
    public float SoundVolume;
    public bool MusicMute;
    public bool SoundMute;
    public string Name;
    public string DataTime;
    public Sprite Screenshot;
    public Sprite Location;
    public List<Sprite> Items;
    public List<GameObject> Checkpoints;
    public List<GameObject> SaveButtons;
    public CardSetScriptableObject CardSetScriptableObject;
}
