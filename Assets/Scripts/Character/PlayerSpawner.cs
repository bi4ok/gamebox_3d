using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] 
    private GameObject player;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera playerCam;
    [SerializeField]
    private GameObject dieTarget;
    [SerializeField]
    private GameObject hell;
    [SerializeField]
    private GameObject respawnTarget;

    private PlayerController playerController;
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        print(playerController);
    }

    public IEnumerator Respawn(float timeToSpawn)
    {   //Анимация смерти
        //player.SetActive(false);
        dieTarget.transform.position = player.transform.position;
        playerCam.m_LookAt = dieTarget.transform;
        playerCam.m_Follow = dieTarget.transform;
        player.transform.position = hell.transform.position;
        yield return new WaitForSeconds(timeToSpawn);
        player.transform.position = respawnTarget.transform.position;
        playerController.TakeHeal(playerController.CheckStats("health"));
        playerController.alive = true;
        playerCam.m_LookAt = player.transform;
        playerCam.m_Follow = player.transform;
        //player.SetActive(true);

    }
    public void StartRespawn(float timeToSpawn)
    {
        StartCoroutine(Respawn(timeToSpawn));
    }
}
