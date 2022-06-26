using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerCellController : MonoBehaviour
{
    [SerializeField]
    private GameObject towerBuyCanvas;
    [SerializeField]
    private GameHandler gameHandler;
    [SerializeField]
    private Light lightOnBild;
    [SerializeField]
    private Color colorOnMouseEnter;
    [SerializeField]
    private Color originalColor;
    [SerializeField]
    private AudioManager audioManager;

    private GameObject _currentTower;
    public bool ismouseactive = false;
    private void Start()
    {
        towerBuyCanvas.SetActive(false);
    }

    private bool IsClickAble()
    {
        return !gameHandler.gameStateFight && !EventSystem.current.IsPointerOverGameObject();
    }

    private void OnMouseDown()
    {
        if (IsClickAble())
        {
            towerBuyCanvas.SetActive(true);
            towerBuyCanvas.GetComponent<TowerController>().GetNewCell(gameObject);
        }
            
    }

    private void OnMouseEnter()
    {
        if (IsClickAble())
        {
            ismouseactive = true;
            lightOnBild.color = colorOnMouseEnter;
        }
    }

    private void OnMouseExit()
    {
        if (IsClickAble())
        {
            ismouseactive = false;
            lightOnBild.color = originalColor;
        }
    }

    public void MakeNewTower(GameObject towerPrefab)
    {
        if (_currentTower != null)
        {
            Destroy(_currentTower);
        }
        _currentTower = Instantiate(towerPrefab, transform.position, transform.rotation, gameObject.transform);
        TowerAttackControl towerAttackScript = _currentTower.GetComponent<TowerAttackControl>();
        towerAttackScript.audioManager = audioManager;
        towerAttackScript.OnCreate();
        
    }
}
