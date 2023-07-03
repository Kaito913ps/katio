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
        {"やくそう", 100},
        {"どくけし", 200},
        {"せいすい", 300},
        {"つるぎ", 500},
        {"よろい", 800}
    };

    private List<string> inventroryItems = new List<string>();

    private List<GameObject> inventoryButtons = new List<GameObject>();

    void Start()
    {
        _moneyText.text = $"所持金:{_money}";
        _inventoryPanel.SetActive(false);
        _shopPanel.SetActive(false);
        UpdateInventoryText();
    }

    public void EnterShopA()
    {
        _shopPanel.SetActive(true);
        _shopButtonB.SetActive(false);
        DisplayShopItems(new string[] { "やくそう", "どくけし", "せいすい" });
        //_inventoryPanel.SetActive(true);
       
    }

    public void EnterShopB()
    {
        _shopPanel.SetActive(false);
        _shopButtonA.SetActive(false);
        DisplayShopItems(new string[] { "やくそう", "どくけし", "せいすい" });
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
                _moneyText.text = $"所持金:{_money}";

                inventroryItems.Add(itemName);
                UpdateInventoryText() ;
            }
            else
            {
                Debug.Log("所持金が足りません");
            }
        }
    }

    public void UseItem(string itemName)
    {
        _inventoryPanel.SetActive(true);
        if (inventroryItems.Contains(itemName))
        {
            if(itemName == "やくそう" ||  itemName == "どくけし"　|| itemName == "せいすい")
            {
                inventroryItems.Remove(itemName);
                UpdateInventoryText();
                Debug.Log(itemName + "を使用しました");
            }
            else if (itemName == "つるぎ" || itemName == "よろい")
            {
                Debug.Log("何もおこらなかった");
            }
        }
        else
        {
            Debug.Log("所持していません");
        }
    }

    private void DisplayShopItems(string[] items)
    {
        _inventoryPanel.SetActive(false);
        _shopPanel.SetActive(true);

        //GridLayoutGroupを取得してレイアウト設定を変更
        GridLayoutGroup gridLayout = _shopPanel.GetComponentInChildren<GridLayoutGroup>();
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayout.constraintCount = 1;

        //アイテムボタンの生成と表示の処理
        foreach(string item in items) 
        {
            //アイテムボタンのプレハブをロードしてインスタンス化
            GameObject buttonObject = Instantiate(Resources.Load<GameObject>("ItemButton"));

            //アイテムボタンの親をShopPanelに設定し、表示位置を調整
            buttonObject.transform.SetParent(_shopPanel.transform, false);

            //ボタン内のテキストコンポーネントを取得し、表示するアイテム名を設定
            Text buttonText = buttonObject.GetComponentInChildren<Text>();
            buttonText.text = item;

            // ボタンのクリックイベントに対応する購入処理を追加
            Button button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => BuyItem(item));
        }
    }

    private void UpdateInventoryButtons()
    {
        //現在のインベントリボタンを削除
        foreach(GameObject button in inventoryButtons)
        {
            Destroy(button);
        }
        inventoryButtons.Clear();

        //インベントリアイテムごとにボタンを生成
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
        
        _inventoryText.text = "所持品:" + string.Join(",", inventroryItems.ToArray());
    }
}
