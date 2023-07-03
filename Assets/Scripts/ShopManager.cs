using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _shopPanel;
    [SerializeField]
    private GameObject _inventoryPanel;
    [SerializeField]
    private GameObject _inventoryButtonPrefab;
    [SerializeField]
    private Transform _inventoryButtonContainer;
    [SerializeField]
    private GameObject _shopButtonA;
    [SerializeField]
    private GameObject _shopButtonB;
    [SerializeField]
    private Text _moneyText;
    [SerializeField]
    private Text _inventoryText;
    [SerializeField]
    private int _money = 1000;

    private Dictionary<string, int> _itemPrices = new Dictionary<string, int>()
    {
        {"�₭����", 100},
        {"�ǂ�����", 200},
        {"��������", 300},
        {"�邬", 500},
        {"��낢", 800}
    };

    private List<string> inventroryItems = new List<string>();

    private List<GameObject> inventoryButtons = new List<GameObject>();

    void Start()
    {
        _moneyText.text = $"������:{_money}";
        _inventoryPanel.SetActive(false);
        _shopPanel.SetActive(false);
        UpdateInventoryText();
    }

    public void EnterShopA()
    {
        _shopPanel.SetActive(true);
        _shopButtonB.SetActive(false);
        DisplayShopItems(new string[] { "�₭����", "�ǂ�����", "��������" });
        //_inventoryPanel.SetActive(true);
       
    }

    public void EnterShopB()
    {
        _shopPanel.SetActive(false);
        _shopButtonA.SetActive(false);
        DisplayShopItems(new string[] { "�₭����", "�ǂ�����", "��������" });
        // _inventoryPanel.SetActive(true);
    }

    public void BuyItem(string itemName)
    {
        int itemCost;
        if(_itemPrices.TryGetValue(itemName,out itemCost))
        {
            if(_money >= itemCost)
            {
                _money -= itemCost;
                _moneyText.text = $"������:{_money}";

                inventroryItems.Add(itemName);
                UpdateInventoryText() ;
            }
            else
            {
                Debug.Log("������������܂���");
            }
        }
    }

    public void UseItem(string itemName)
    {
        _inventoryPanel.SetActive(true);
        if (inventroryItems.Contains(itemName))
        {
            if(itemName == "�₭����" ||  itemName == "�ǂ�����"�@|| itemName == "��������")
            {
                inventroryItems.Remove(itemName);
                UpdateInventoryText();
                Debug.Log(itemName + "���g�p���܂���");
            }
            else if (itemName == "�邬" || itemName == "��낢")
            {
                Debug.Log("����������Ȃ�����");
            }
        }
        else
        {
            Debug.Log("�������Ă��܂���");
        }
    }

    private void DisplayShopItems(string[] items)
    {
        _inventoryPanel.SetActive(false);
        _shopPanel.SetActive(true);

        //GridLayoutGroup���擾���ă��C�A�E�g�ݒ��ύX
        GridLayoutGroup gridLayout = _shopPanel.GetComponentInChildren<GridLayoutGroup>();
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayout.constraintCount = 1;

        //�A�C�e���{�^���̐����ƕ\���̏���
        foreach(string item in items) 
        {
            //�A�C�e���{�^���̃v���n�u�����[�h���ăC���X�^���X��
            GameObject buttonObject = Instantiate(Resources.Load<GameObject>("ItemButton"));

            //�A�C�e���{�^���̐e��ShopPanel�ɐݒ肵�A�\���ʒu�𒲐�
            buttonObject.transform.SetParent(_shopPanel.transform, false);

            //�{�^�����̃e�L�X�g�R���|�[�l���g���擾���A�\������A�C�e������ݒ�
            Text buttonText = buttonObject.GetComponentInChildren<Text>();
            buttonText.text = item;

            // �{�^���̃N���b�N�C�x���g�ɑΉ�����w��������ǉ�
            Button button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => BuyItem(item));
        }
    }

    private void UpdateInventoryButtons()
    {
        //���݂̃C���x���g���{�^�����폜
        foreach(GameObject button in inventoryButtons)
        {
            Destroy(button);
        }
        inventoryButtons.Clear();

        //�C���x���g���A�C�e�����ƂɃ{�^���𐶐�
        foreach(string item in inventroryItems)
        {
            GameObject buttonObject = Instantiate(_inventoryButtonPrefab);
            buttonObject.transform.SetParent(_inventoryButtonContainer,false);

            Text buttonText = buttonObject.GetComponentInChildren<Text>();
            buttonText.text = item;

            Button button = buttonObject.GetComponent<Button>();
            string itemName = item;
            button.onClick.AddListener(() => UseItem(itemName));

            inventoryButtons.Add(buttonObject);
        }
    }

    public void OpenInventory()
    {
        _inventoryPanel.SetActive(true);
        UpdateInventoryButtons();
    }

    public void CloseInventory()
    {
        _inventoryPanel.SetActive(false); 
        UpdateInventoryButtons();
    }

    private void UpdateInventoryText()
    {
        
        _inventoryText.text = "�����i:" + string.Join(",", inventroryItems.ToArray());
    }
}
