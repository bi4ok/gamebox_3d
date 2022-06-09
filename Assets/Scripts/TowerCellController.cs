using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCellController : MonoBehaviour
{
    [SerializeField]
    private GameObject towerBuyCanvas;
    [SerializeField]
    private Light lightOnBild;
    [SerializeField]
    private Color colorOnMouseEnter;
    [SerializeField]
    private Color originalColor;

    private GameObject _currentTower;

    private void Start()
    {
        towerBuyCanvas.SetActive(false);
    }

    private void OnMouseDown()
    {
        towerBuyCanvas.SetActive(true);
        towerBuyCanvas.GetComponent<TowerController>().GetNewCell(gameObject);
    }

    private void OnMouseEnter()
    {
        lightOnBild.color = colorOnMouseEnter;
    }

    private void OnMouseExit()
    {
        lightOnBild.color = originalColor;
    }

    public void MakeNewTower(GameObject towerPrefab)
    {
        if (_currentTower != null)
        {
            Destroy(_currentTower);
        }
        _currentTower = Instantiate(towerPrefab, transform.position, transform.rotation, gameObject.transform);
        
    }
}
