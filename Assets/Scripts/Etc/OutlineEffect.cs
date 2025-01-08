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
        // Создаем объект для контура
        outlineObject = new GameObject("Outline");
        outlineObject.transform.SetParent(transform); // Делаем его дочерним
        outlineObject.transform.localPosition = Vector3.zero; // Центрируем
        outlineObject.transform.localScale = Vector3.one + Vector3.one * outlineSize; // Увеличиваем масштаб

        // Добавляем SpriteRenderer для контура
        outlineRenderer = outlineObject.AddComponent<SpriteRenderer>();
        outlineRenderer.sprite = GetComponent<SpriteRenderer>().sprite; // Берем спрайт оригинала
        outlineRenderer.color = outlineColor; // Устанавливаем цвет
        outlineRenderer.sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID; // Синхронизируем слой отрисовки
        outlineRenderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1; // Устанавливаем ниже основного спрайта

        outlineObject.SetActive(false); // Контур изначально отключен
    }

    void OnMouseEnter()
    {
        outlineObject.SetActive(true); // Включаем контур при наведении
    }

    void OnMouseExit()
    {
        outlineObject.SetActive(false); // Выключаем контур при выходе
    }
}
