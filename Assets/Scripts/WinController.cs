using DG.Tweening;
using I2.Loc;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinController : MonoBehaviour
{
    [SerializeField] private ContentCard contentCard;
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject nextButton, previousButton;
    [SerializeField] private Image bG, nextBG, previousBG;
    [SerializeField] private Localize message_key, nextMessage_key, previousMessage_key;

    private Vector3 _nextPosition, _previousPosition;
    private List<WinContent> _contentList;
    private int _currentIndex = 0;

    public void ResetIndex() {  _currentIndex = 0; }

    public void SetWinContent(List<WinContent> contentList)
    {
        _nextPosition = new Vector3(nextBG.transform.position.x, 0, 0);
        _previousPosition = new Vector3(previousBG.transform.position.x, 0, 0);

        _contentList = new List<WinContent>();
        _contentList = contentList;
        if (_contentList.Count > 0) SetCurrentContent();
    }

    private void SetCurrentContent()
    {
        CheckViewButtons();
        bG.sprite = _contentList[_currentIndex].Sprite;
        message_key.Term = _contentList[_currentIndex].Message;
    }

    private void SetNextContent()
    {
        if(_currentIndex >= _contentList.Count -1) return;

        nextBG.sprite = _contentList[_currentIndex + 1].Sprite;
        nextMessage_key.Term = _contentList[_currentIndex + 1].Message;
    }

    private void SetPreviousContent()
    {
        if(_currentIndex == 0) return;

        previousBG.sprite = _contentList[_currentIndex - 1].Sprite; ;
        previousMessage_key.Term = _contentList[_currentIndex - 1].Message;
    }

    private void CheckViewButtons()
    {
         nextButton.SetActive(_currentIndex < _contentList.Count - 1);
        previousButton.SetActive(_currentIndex > 0);
    }

    public void NextScreen()
    {
        SetNextContent();
        _currentIndex++;

        nextButton.SetActive(false);
        previousButton.SetActive(false);

        nextBG.transform.DOMoveX(0, 0.9f).SetEase(Ease.OutBack, 0.5f).OnComplete(() =>
        {
            SetPreviousContent();
            SetCurrentContent();
            CheckViewButtons();
            nextBG.transform.position = _nextPosition;
            SetNextContent();
        });
    }

    public void PrevScreen()
    {
        _currentIndex--;

        nextButton.SetActive(false);
        previousButton.SetActive(false);

        previousBG.transform.DOMoveX(0, 0.9f).SetEase(Ease.OutBack, 0.5f).OnComplete(() =>
        {
            SetNextContent();
            SetCurrentContent();
            CheckViewButtons();
            previousBG.transform.position = _previousPosition;
            SetPreviousContent();
        });
    }
}


[Serializable]
public class WinContent
{
    public Sprite Sprite;
    public string Message;
}

