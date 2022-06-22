using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedPackController : MonoBehaviour
{

    [SerializeField]
    private List<MedPackObject> allMedpackObjects;
    [SerializeField]
    private GameObject scrollMenu;
    [SerializeField]
    private GameHandler gameHandler;

    [SerializeField]
    private Text medpackInfoText;

    [SerializeField]
    private Button buttonBuy;

    private GameObject _currentCell;

    private void Start()
    {

        foreach (MedPackObject medpack in allMedpackObjects)
        {
            GameObject newTowerUI = Instantiate(medpack.prefabInUI, scrollMenu.transform);
            Button checkTowerButton = newTowerUI.GetComponentInChildren<Button>();
            checkTowerButton.image.sprite = medpack.imgOfMedPack;
            checkTowerButton.onClick.AddListener(() => TowerInfo(medpack));

        }
        buttonBuy.interactable = false;
    }

    private void TowerInfo(MedPackObject medpack)
    {
        string info =
            $"Самобранка {medpack.name}:\n" +
            $"{medpack.medpackInfo}\n" +
            $"Стоимость:\n" +
            $"Горящие ошмётки: {medpack.costRed}\n" +
            $"Мокрые ошмётки: {medpack.costBlue}\n" +
            $"Твёрдые ошмётки: {medpack.costBrown}";

        medpackInfoText.text = info;


        if ((medpack.check == null || (medpack.check != null && medpack.check.isBougth)) && !medpack.isBougth)
        {
            buttonBuy.interactable = true;
            buttonBuy.onClick.AddListener(() => BuyMedpack(medpack.prefabInGame, medpack, _currentCell));
        }
        else
        {
            buttonBuy.interactable = false;
        }
    }

    private void BuyMedpack(GameObject medpackPrefab, MedPackObject medPack, GameObject cellForMedPack)
    {
        var finalCost = new Dictionary<string, float>() {
            { "red", medPack.costRed },
            { "blue", medPack.costBlue },
            { "yellow", medPack.costBrown },
        };
        if (gameHandler.PlayerTryWasteScrap(finalCost))
        {
            cellForMedPack.GetComponent<MedPackCellContoller>().ChooseNewMedpack(medPack);
            medPack.isBougth = true;
            print("медпак куплен");
        }
        else
        {
            print("не хватает материалов для покупки аптечек");
        }

    }

    public void GetNewCell(GameObject newCell)
    {
        _currentCell = newCell;
    }
}
