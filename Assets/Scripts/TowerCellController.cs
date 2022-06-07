using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCellController : MonoBehaviour
{
    [SerializeField]
    private GameObject towerBuyCanvas;
    [SerializeField]
    private SpriteRenderer spriteHandler;

    private Color originalColor;
    private GameObject _currentTower;

    private void Start()
    {
        originalColor = spriteHandler.color;
        towerBuyCanvas.SetActive(false);
    }

    private void OnMouseDown()
    {
        towerBuyCanvas.SetActive(true);
        towerBuyCanvas.GetComponent<TowerController>().GetNewCell(gameObject);
    }

    private void OnMouseEnter()
    {
        spriteHandler.color = Color.yellow;
    }

    private void OnMouseExit()
    {
        spriteHandler.color = originalColor;
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
