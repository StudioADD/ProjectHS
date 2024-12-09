using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static Define;

public enum ESoundType
{
    BGM = 0,
    SFX = 1,
    MASTER = 2,
}

public class SoundMgr
{
    AudioSource[] audioSources = new AudioSource[(int)ESoundType.MASTER]; // BGM, SFX
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>(); // 키 : 파일경로

    readonly float FADETIME = 1f;

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            UnityEngine.Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(ESoundType)); // BGM, SFX
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            audioSources[(int)ESoundType.BGM].loop = true; // BGM은 반복 무한 재생
        }
    }

    public void Clear()
    {

    }

    public void PlayBgm(EBgmSoundType bgmType)
    {
        if (coFadeOutInBGM != null)
            CoroutineHelper.StopCoroutine(coFadeOutInBGM);

        // BGM 사운드 구현부는 코루틴 안에서 수행
        coFadeOutInBGM = CoroutineHelper.StartCoroutine(CoFadeOutInBGM(bgmType));
    }

    public void StopBgm()
    {
        if (coStopBGM != null)
            CoroutineHelper.StopCoroutine(coStopBGM);

        coStopBGM = CoroutineHelper.StartCoroutine(CoStopBGM());
    }

    public void PlaySfx(ESfxSoundType sfxType)
    {
        string path = $"{LoadPath.SOUND_SFX_PATH}/{sfxType}";
        AudioClip audioClip = GetOrAddAudioClip(path);

        if (audioClip == null)
            return;

        audioSources[(int)ESoundType.SFX].PlayOneShot(audioClip);
    }

    public void ChangeBGMSpeed(float speed, float time)
    {
        if (coChangeBGMSpeed != null)
            CoroutineHelper.StopCoroutine(coChangeBGMSpeed);

        CoroutineHelper.StartCoroutine(CoChangeBGMSpeed(speed, time));
    }

    Coroutine coChangeBGMSpeed = null;
    private IEnumerator CoChangeBGMSpeed(float pitch, float time)
    {
        float currPitch = 1f;
        audioSources[(int)ESoundType.BGM].pitch = currPitch;

        float currTime = 0;
        while (currTime < FADETIME)
        {
            currTime += Time.deltaTime / FADETIME;
            currPitch = Mathf.Lerp(currPitch, pitch, currTime);
            audioSources[(int)ESoundType.BGM].pitch = currPitch;
        }
        audioSources[(int)ESoundType.BGM].pitch = pitch;

        yield return new WaitForSeconds(time - FADETIME * 2);

        currTime = 0;
        while(currTime < FADETIME)
        {
            currTime += Time.deltaTime / FADETIME;
            currPitch = Mathf.Lerp(currPitch, 1f, currTime);
            audioSources[(int)ESoundType.BGM].pitch = currPitch;
        }
        audioSources[(int)ESoundType.BGM].pitch = 1f;
        coChangeBGMSpeed = null;
    }

    private void SetBgmAudioSource(float currVolume)
    {
        audioSources[(int)ESoundType.BGM].volume = currVolume;
    }

    private AudioClip GetOrAddAudioClip(string path)
    {
        AudioClip audioClip = null;
        if (audioClips.TryGetValue(path, out audioClip) == false)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
            audioClips.Add(path, audioClip);
        }

        if (audioClip == null)
        {
            Debug.Log($"AudioClip Error! : {path}");
        }

        return audioClip;
    }

    Coroutine coStopBGM = null;
    private IEnumerator CoStopBGM()
    {
        yield return new WaitUntil(() => coFadeOutInBGM == null);

        float time = 0f;
        float currVolume = audioSources[(int)ESoundType.BGM].volume;

        if (audioSources[(int)ESoundType.BGM].isPlaying) // 실행 중일 경우
        {
            float volume = currVolume;
            // FadeOut
            while (volume > 0f)
            {
                time += Time.deltaTime / FADETIME;
                volume = Mathf.Lerp(currVolume, 0, time);
                SetBgmAudioSource(volume);

                yield return null;
            }
        }

        currVolume = 0.0f;
        SetBgmAudioSource(currVolume);
        audioSources[(int)ESoundType.BGM].Stop();

        coStopBGM = null;
    }

    Coroutine coFadeOutInBGM = null;
    private IEnumerator CoFadeOutInBGM(EBgmSoundType bgmType)
    {
        yield return new WaitUntil(() => coStopBGM == null);

        float time = 0f;
        float _currBgmVolume = 1f;
        float currVolume = audioSources[(int)ESoundType.BGM].volume;

        string path = $"{LoadPath.SOUND_BGM_PATH}/{bgmType}";

        AudioClip audioClip = GetOrAddAudioClip(path);

        if (audioSources[(int)ESoundType.BGM].isPlaying)
        {
            float volume = currVolume;
            // FadeOut
            while (volume > 0)
            {
                time += Time.deltaTime / FADETIME;
                volume = Mathf.Lerp(currVolume, 0, time);
                SetBgmAudioSource(volume);

                yield return null;
            }
        }

        time = 0f;
        currVolume = 0.0f;
        SetBgmAudioSource(0.0f);

        if (audioClip != null)
            audioSources[(int)ESoundType.BGM].clip = audioClip;

        if (bgmType != EBgmSoundType.None)
        {
            audioSources[(int)ESoundType.BGM].Play();

            // FadeIn
            while (currVolume < _currBgmVolume)
            {
                time += Time.deltaTime / FADETIME;
                currVolume = Mathf.Lerp(0, _currBgmVolume, time);
                SetBgmAudioSource(currVolume);

                yield return null;
            }

            currVolume = _currBgmVolume;
            SetBgmAudioSource(currVolume);
        }
        else // Bgm이 None인 경우
        {
            currVolume = 0.0f;
            SetBgmAudioSource(currVolume);
            audioSources[(int)ESoundType.BGM].Stop();
        }

        coFadeOutInBGM = null;
    }
}
