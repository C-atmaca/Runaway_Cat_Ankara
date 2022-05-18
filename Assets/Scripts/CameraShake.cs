using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private PlayerMovement player;

    [SerializeField] private float duration;
    [SerializeField] private float strength;
    [SerializeField] private float shakeTimer;
    private float startingStrength;
    private float durationTotal;

    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        player = FindObjectOfType<PlayerMovement>();
        player.onPlayerFallDamage += ShakeCamera;
    }

    private void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMulti = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMulti.m_AmplitudeGain = strength;
        startingStrength = strength;
        durationTotal = duration;
        shakeTimer = duration;

    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMulti = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMulti.m_AmplitudeGain = Mathf.Lerp(startingStrength, 0, duration / durationTotal);
                
            }
        }

    }
}
