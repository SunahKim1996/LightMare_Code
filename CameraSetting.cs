using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerCam;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(SetPlayerCam());
    }

    IEnumerator SetPlayerCam()
    {
        while (Player.instnace == null)
            yield return null;

        playerCam.Follow = Player.instnace.transform;
        GameManager.instance.isReadyCamSet = true;
    }
}
