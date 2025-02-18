using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private int _index;
    private int _hpCount;
    private Sprite _location;
    private List<Sprite> _items = new List<Sprite>();
   
    private CardSetScriptableObject cardSetScriptableObject;

    public int Index { get => _index; set => _index = value; }
    public int HpCount { get => _hpCount; set => _hpCount = value; }
    public List<Sprite> Items { get => _items; set => _items = value; }
    public Sprite Location { get => _location; set => _location = value; }

    public CardSetScriptableObject CardSetScriptableObject 
    {
        get => cardSetScriptableObject; 
        set => cardSetScriptableObject = value; 
    }
}
