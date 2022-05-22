using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float respawnKD;
    private PlayerController playerController;
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();

        print(playerController.CheckStats("health"));
    }

    public IEnumerator Respawn()
    {   //�������� ������
        player.SetActive(false);
        yield return new WaitForSeconds(respawnKD);
        player.transform.position = transform.position;
        playerController.TakeHeal(playerController.CheckStats("health"));
        playerController.alive = true;
        player.SetActive(true);
       
    }
    public void StartRespawn()
    {
        StartCoroutine(Respawn());
    }
}
