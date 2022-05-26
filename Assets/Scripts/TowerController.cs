using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : MonoBehaviour
{
    [System.Serializable]
    public class TowerToBuy
    {
        public string name;
        public GameObject prefabTowerInGame;
        public GameObject prefabTowerInUI;
        public Sprite imgOfTower;
        public int cost;

    }

    [SerializeField]
    private List<TowerToBuy> allTowerObjects;
    [SerializeField]
    private GameObject scrollMenu;
    [SerializeField]
    private GameHandler gameHandler;

    private GameObject _currentCell;

    private void Start()
    {
        foreach(TowerToBuy tower in allTowerObjects)
        {
            GameObject newTowerUI = Instantiate(tower.prefabTowerInUI, scrollMenu.transform);
            Button buyTowerButton = newTowerUI.GetComponentInChildren<Button>();
            buyTowerButton.onClick.AddListener(() => BuyTower(tower.prefabTowerInGame, tower.cost, _currentCell));
        }
    }

    private void BuyTower(GameObject towerPrefab, float cost, GameObject cellForTower)
    {
        if (gameHandler.PlayerTryWasteScrap("red", cost))
        {
            cellForTower.GetComponent<TowerCellController>().MakeNewTower(towerPrefab);
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
