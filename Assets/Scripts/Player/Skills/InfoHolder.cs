using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoHolder : MonoBehaviour
{
    public string info;
    public Sprite image;

    public delegate void SendInfos(string info, Sprite image);
    public static event SendInfos Send;

    public void SendInfo()
    {
        Send(info, image);
    }
}
