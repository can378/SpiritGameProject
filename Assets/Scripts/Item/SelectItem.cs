using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class SelectItem : MonoBehaviour, Interactable
{
    [HideInInspector] public ItemInstance itemInstance { get; protected set; }
    public string GetInteractText()
    {
        return "�ݱ�";
    }

    public void Interact()
    {
        Player.instance.GainSelectItem(this);
    }

    #region Sort CompareFunc
    static int compareClass(SelectItem a, SelectItem b)
    {
        if (a.itemInstance.itemData.selectItemType < b.itemInstance.itemData.selectItemType)
        {
            return -1;
        }
        else if (a.itemInstance.itemData.selectItemType > b.itemInstance.itemData.selectItemType)
        {
            return 1;
        }
        return 0;
    }

    static int compareRating(SelectItem a, SelectItem b)
    {
        if (a.itemInstance.itemData.selectItemRating < b.itemInstance.itemData.selectItemRating)
        {
            return -1;
        }
        else if (a.itemInstance.itemData.selectItemRating > b.itemInstance.itemData.selectItemRating)
        {
            return 1;
        }
        return 0;
    }

    static int compareID(SelectItem a, SelectItem b)
    {
        return a.itemInstance.itemData.selectItemID < b.itemInstance.itemData.selectItemID ? -1 : 1;
    }
    
    // �Ŀ� ���ݺ� ���� �߰�?

    #endregion Sort ComapreFunc

    #region Sort
    public static int ClassSort(SelectItem a, SelectItem b)
    {
        // ����
        if(compareClass(a, b) != 0)
            return compareClass(a,b);
        // ������ ���ٸ� �������
        if (compareRating(a, b) != 0)
            return compareRating(a, b);
        // ��޵� ���ٸ� ID��
        return compareID(a, b);
    }

    public static int RatingSort(SelectItem a, SelectItem b)
    {
        // ���
        if (compareRating(a, b) != 0)
            return compareRating(a, b);
        // ����� ���ٸ� ��������
        if (compareClass(a, b) != 0)
            return compareClass(a, b);
        // ������ ���ٸ� ID��
        return compareID(a, b);
    }
    #endregion

}

