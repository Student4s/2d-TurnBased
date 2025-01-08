using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    public Color outlineColor = Color.red;
    public float outlineSize = 0.1f;

    private GameObject outlineObject;
    private SpriteRenderer outlineRenderer;
    void Start()
    {
        // ������� ������ ��� �������
        outlineObject = new GameObject("Outline");
        outlineObject.transform.SetParent(transform); // ������ ��� ��������
        outlineObject.transform.localPosition = Vector3.zero; // ����������
        outlineObject.transform.localScale = Vector3.one + Vector3.one * outlineSize; // ����������� �������

        // ��������� SpriteRenderer ��� �������
        outlineRenderer = outlineObject.AddComponent<SpriteRenderer>();
        outlineRenderer.sprite = GetComponent<SpriteRenderer>().sprite; // ����� ������ ���������
        outlineRenderer.color = outlineColor; // ������������� ����
        outlineRenderer.sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID; // �������������� ���� ���������
        outlineRenderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1; // ������������� ���� ��������� �������

        outlineObject.SetActive(false); // ������ ���������� ��������
    }

    void OnMouseEnter()
    {
        outlineObject.SetActive(true); // �������� ������ ��� ���������
    }

    void OnMouseExit()
    {
        outlineObject.SetActive(false); // ��������� ������ ��� ������
    }
}
