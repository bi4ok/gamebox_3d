using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedPackCellContoller : MonoBehaviour
{
    [SerializeField]
    private GameObject medpackBuyCanvas;
    [SerializeField]
    private GameObject bonusHandler;
    [SerializeField]
    private Light lightOnBild;
    [SerializeField]
    private Color colorOnMouseEnter;
    [SerializeField]
    private Color originalColor;
    [SerializeField]
    private float rangeBetweenPacks;

    private MedPackObject _medPack=null;
    private List<GameObject> _currentMedpacks=new List<GameObject>();


    private void Start()
    {
        medpackBuyCanvas.SetActive(false);
    }

    private void OnMouseDown()
    {
        medpackBuyCanvas.SetActive(true);
        medpackBuyCanvas.GetComponent<MedPackController>().GetNewCell(gameObject);
        
    }

    private void OnMouseEnter()
    {
        lightOnBild.color = colorOnMouseEnter;
    }

    private void OnMouseExit()
    {
        lightOnBild.color = originalColor;
    }

    public void ChooseNewMedpack(MedPackObject medpackScriptObject)
    {
        _medPack = medpackScriptObject;
        OnWaveMedPackSpawn();

    }

    public void OnWaveMedPackSpawn()
    {
        print("MEDPACK START SPAWNING");
        if (_medPack == null)
            return;

        if (_currentMedpacks.Count > 0)
        {
            foreach (var medpack in _currentMedpacks)
            {
                Destroy(medpack);
            }
        }
        print("CHECK complete, SPAWN: ");
        int start = -1;

        for (int i=0; i<_medPack.countOfMedpacks; i++)
        {
            var medPackObject = Instantiate(_medPack.prefabInGame, transform.position + Vector3.right * start * rangeBetweenPacks, transform.rotation, gameObject.transform);
            start += 1;
            var objScript = medPackObject.GetComponent<Bonus>();
            objScript.aliveBonusTimer = 999;
            objScript.bonusHandler = bonusHandler;
            _currentMedpacks.Add(medPackObject);
        }
        print("ALL right, medppacks here");

    }

}
