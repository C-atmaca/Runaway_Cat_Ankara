using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AssetManager : MonoBehaviour
{
    [Header("Level Triggers")]
    [SerializeField] private LevelEndTrigger level1EndTrigger;
    [SerializeField] private LevelEndTrigger level2EndTrigger;
    [SerializeField] private LevelEndTrigger level3EndTrigger;


    [Header("Assets")]
    [SerializeField] private GameObject dangerChaseAsset;
    [SerializeField] private GameObject level1Assets;
    [SerializeField] private GameObject level2Assets;
    [SerializeField] private GameObject level3Assets;
    [SerializeField] private GameObject level4Assets;

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera playerFollowCamera;
    [SerializeField] private GameObject[] level2Delete;
    [SerializeField] private CinemachineVirtualCamera level2Camera;
    [SerializeField] private GameObject[] level4Delete;
    [SerializeField] private CinemachineVirtualCamera level4Camera;

    [Header("Audio")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip dogSounds;

    void Start()
    {
        level1EndTrigger.level1Done += LoadLevel2Assets;
        level2EndTrigger.level2Done += LoadLevel3Assets;
        level3EndTrigger.level3Done += LoadLevel4Assets;

        level2Delete[0].SetActive(false);
        level4Delete[0].SetActive(false);
    }

    private void LoadLevel2Assets()
    {
        StartCoroutine(FadeTrackStart(music));
        StartCoroutine(FadeTrackStart(dogSounds));
        LoadChaseLevelAssets();
        level2Assets.SetActive(true);
        level2Camera.Priority = 10;
        playerFollowCamera.Priority = 1;
        level2Delete[0].SetActive(true);
        StartCoroutine(UnloadLevel(level1Assets));
    }

    private void LoadLevel3Assets()
    {
        StartCoroutine(FadeTrack(music));
        StartCoroutine(FadeTrack(dogSounds));
        UnLoadChaseLevelAssets();
        level3Assets.SetActive(true);
        playerFollowCamera.Priority = 10;
        level2Camera.Priority = 1;
        StartCoroutine(UnloadLevel(level2Assets));
        StartCoroutine(DeleteGarbage(level2Delete));
    }

    private void LoadLevel4Assets()
    {
        StartCoroutine(FadeTrackStart(music));
        StartCoroutine(FadeTrackStart(dogSounds));
        LoadChaseLevelAssets();
        level4Assets.SetActive(true);
        playerFollowCamera.Priority = 1;
        level4Camera.Priority = 10;
        level4Delete[0].SetActive(true);
        StartCoroutine(UnloadLevel(level3Assets));
    }

    private void LoadChaseLevelAssets()
    {
        dangerChaseAsset.SetActive(true);
    }

    private void UnLoadChaseLevelAssets()
    {
        dangerChaseAsset.SetActive(false);
    }


    IEnumerator UnloadLevel(GameObject level)
    {
        yield return new WaitForSecondsRealtime(5f);

        level.SetActive(false);
    }

    IEnumerator DeleteGarbage(GameObject[] deleteList)
    {
        yield return new WaitForSecondsRealtime(2f);

        foreach (GameObject item in deleteList)
        {
            Destroy(item);
        }
    }

    private IEnumerator FadeTrackStart(AudioClip clip)
    {
        float timeToFade = 0.5f;
        float timeElapsed = 0f;

        source.PlayOneShot(clip);

        while (timeElapsed < timeToFade)
        {
            source.volume = Mathf.Lerp(0, 0.5f, timeElapsed / timeToFade*5);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        
    }

    private IEnumerator FadeTrack(AudioClip clip)
    {
        float timeToFade = 0.25f;
        float timeElapsed = 0f;
        while (timeElapsed < timeToFade)
        {
            source.volume = Mathf.Lerp(0.5f, 0, timeElapsed / timeToFade*3);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        source.Stop();
    }
}
