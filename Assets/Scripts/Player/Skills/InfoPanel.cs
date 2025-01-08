using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public Text infoText;
    public Image infoImage;

    private void OnEnable()
    {
        InfoHolder.Send += ChangeInfo;
    }
    private void OnDisable()
    {
        InfoHolder.Send -= ChangeInfo;
    }
    public void ChangeInfo(string info, Sprite image)
    {
        infoText.text = info;
        infoImage.sprite = image;
    }
}
