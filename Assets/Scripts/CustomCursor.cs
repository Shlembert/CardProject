using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private List<Sprite> cursorSprites; // ���� �������� ��� �������
    [SerializeField] private Vector2 hotspot = Vector2.zero; // ����� ������� ������� (�� ��������� �����)
    [SerializeField] private Texture2D defaultCursorTexture; // �������� ��� ������ �� ��������� ������, ���� ����������

    private SpriteRenderer _cursorRenderer;

    private void Start()
    {
        // �������� ��������� ������
        Cursor.visible = false;

        // ��������� ������ ��� ����������� ���������� �������
        GameObject cursorObject = new GameObject("CustomCursor");
        cursorObject.transform.SetParent(transform);

        _cursorRenderer = cursorObject.AddComponent<SpriteRenderer>();
        _cursorRenderer.sortingOrder = 100; // ������������� ������� ���������, ����� ������ ��� ������ ������
        ChangeCursorSprite(0);
    }

    private void Update()
    {
        // ������ �� ���������� ���� � ��������� ������� ���������� �������
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0; // ������ Z � 0, ����� ������ ��� �� ������ UI
        _cursorRenderer.transform.position = cursorPosition;
    }

    // ����� ��� ����� ������� ������� �� �������
    public void ChangeCursorSprite(int index)
    {
        if (index >= 0 && index < cursorSprites.Count)
        {
            _cursorRenderer.sprite = cursorSprites[index];
        }
        else
        {
            Debug.LogWarning("Index out of range of cursorSprites list.");
        }
    }

    // ����� ��� �������� ���������� �������
    public void ResetToDefaultCursor()
    {
        Cursor.visible = true;
        _cursorRenderer.sprite = null;
    }
}
