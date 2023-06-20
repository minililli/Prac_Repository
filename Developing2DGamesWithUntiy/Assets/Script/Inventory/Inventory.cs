using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;
    public const int numSlots = 5;
    Image[] itemImages = new Image[numSlots];
    Item[] items = new Item[numSlots];
    GameObject[] slots = new GameObject[numSlots];

    public void Start()
    {
        CreateSlots();
    }

    public void CreateSlots()
    {
        if (slotPrefab != null)
        {
            for(int i=0; i< numSlots; i++)
            {
                GameObject newSlot = Instantiate(slotPrefab);
                newSlot.name = "ItemSlot_" + i;
                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);
                slots[i] = newSlot;
                itemImages[i] = newSlot.transform.GetChild(1).GetComponent<Image>(); //슬롯의 자식 오브젝트 중 인덱스 1에 해당하는 자식 오브젝트는 ItemImage.
            }
        }
    }
    /// <summary>
    /// 실제로 인벤토리에 아이템을 추가하는 함수
    /// </summary>
    /// <param name="itemToAdd"> 추가할 아이템</param>
    /// <returns></returns>
    public bool AddItem(Item itemToAdd)
    {
        for(int i=0; i< items.Length; i++)
        {
            if (items[i] != null && items[i].itemType == itemToAdd.itemType && itemToAdd.stackable ==true)
            {
                items[i].quantity += 1;

                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                Text quantityText = slotScript.qtyText;

                quantityText.enabled = true;
                quantityText.text = items[i].quantity.ToString();

                return true;
            }

            if (items[i] == null)
            {
                items[i] = Instantiate(itemToAdd);
                items[i].quantity = 1;

                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;

                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                Text quantityText = slotScript.qtyText;
                quantityText.enabled = true;
                quantityText.text = items[i].quantity.ToString();
                return true;
            }
        }
        return false;
    }
}
