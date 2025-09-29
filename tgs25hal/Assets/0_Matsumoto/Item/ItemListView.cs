using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ItemListView : MonoBehaviour
{
    [SerializeField] private ItemInventory m_Inventory;
    [SerializeField] private TextMeshProUGUI m_InventoryViewText;
    [SerializeField] private UnityEngine.UI.Button m_Button;
    [SerializeField] private string[] m_string;
    [SerializeField] private TurnManager m_TurnManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DispItemInventory();
    }

   public void DispItemInventory()
    {
        ItemName item = m_Inventory.GetItemInventory();
        m_InventoryViewText.text= m_string[(int)item];
        m_Button.interactable = false;

        // 操作時間であればアイテム使用可能
        if (!m_TurnManager.GetPlayTimeTurn()) return;
        m_Button.interactable = true;

    }

    public void OnClick()
    {
        m_Inventory.UseItem();
    }
}
