using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    public RoomInfo info;
    public void SetUp(RoomInfo _info)
    {
        info = _info;
        text.text = _info.Name;
    }

    public void updateItem()
    {
        if (info.RemovedFromList)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnClick()
    {
        LobbyManager.Instance.JoinRoom(info);
    }
}
