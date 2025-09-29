using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviourPun
{
    ItemName m_Item;    // 所持中のアイテム

    GameObject m_ItemObj;
    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.tag != ("Player"))
        {
            this.enabled = false;
        }
        m_Item= ItemName.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            UseItem();
        }
    }

    void PickItem(ItemName _item)
    {
        m_Item = _item;
    }

    // アイテム使用
    public void UseItem()
    {
        switch (m_Item)
        {
            case ItemName.None:
                break;

            case ItemName.SpeedUp:  // スピードアップ使用
                UseSpeedUp();
                break;

            case ItemName.PowerUp:  // パワーアップ使用
                UsePowerUp();
                break;
        }
    }

    private void UseSpeedUp()
    { 
    
    }

    private void UsePowerUp()
    { 
    
    }

    public ItemName GetItemInventory()
    {
        return m_Item;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            PickItem(other.gameObject.GetComponent<Item>().GetItemName());
        }
    }

}
