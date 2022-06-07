using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : MonoBehaviour
{

    [SerializeField]
    private List<TowerObjectScript> allTowerObjects;
    [SerializeField]
    private GameObject scrollMenu;
    [SerializeField]
    private GameHandler gameHandler;

    [SerializeField]
    private Text towerInfoText;

    [SerializeField]
    private Button buttonBuy;

    private GameObject _currentCell;

    private void Start()
    {
        
        foreach (TowerObjectScript tower in allTowerObjects)
        {
            GameObject newTowerUI = Instantiate(tower.prefabInUI, scrollMenu.transform);
            print(newTowerUI);
            Button checkTowerButton = newTowerUI.GetComponentInChildren<Button>();
            checkTowerButton.image.sprite = tower.imgOfTower;
            checkTowerButton.onClick.AddListener(() => TowerInfo(tower));

        }
        buttonBuy.interactable = false;
        //buttonBuy.onClick.AddListener(() => BuyTower(tower.prefabInGame, tower, _currentCell));
    }

    private void TowerInfo(TowerObjectScript tower)
    {
        string info = 
            $"Башня {tower.name}:\n" +
            $"{tower.towerInfo}";

        towerInfoText.text = info;

        print(tower.check);

        if ((tower.check == null || (tower.check != null && tower.check.isBougth)) && !tower.isBougth)
        {
            if (tower.check != null)
                print(tower.check + " " + tower.check.isBougth);
            else
                print(null);
            buttonBuy.interactable = true;
            buttonBuy.onClick.AddListener(() => BuyTower(tower.prefabInGame, tower, _currentCell));
        }
        else
        {
            buttonBuy.interactable = false;
        }
    }

    private void BuyTower(GameObject towerPrefab, TowerObjectScript cost, GameObject cellForTower)
    {
        var finalCost = new Dictionary<string, float>() { 
            { "red", cost.costRed },
            { "blue", cost.costBlue },
            { "yellow", cost.costBrown },
        };
        if (gameHandler.PlayerTryWasteScrap(finalCost))
        {
            cellForTower.GetComponent<TowerCellController>().MakeNewTower(towerPrefab);
            cost.isBougth = true;
        }
        else
        {
            print("не хватает материалов для покупки башни");
        }
        
        print("TOWER BUy!");
    }

    public void GetNewCell(GameObject newCell)
    {
        _currentCell = newCell;
    }

}
