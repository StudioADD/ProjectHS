using System;
using System.Collections;
using System.Collections.Generic;
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

    Coroutine fadeOutInBGMCoroutine;
    Coroutine bgmStopCoroutine;

    readonly float fadeSoundTime = 0.75f;

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
        if (fadeOutInBGMCoroutine != null)
            CoroutineHelper.StopCoroutine(fadeOutInBGMCoroutine);

        // BGM 사운드 구현부는 코루틴 안에서 수행
        fadeOutInBGMCoroutine = CoroutineHelper.StartCoroutine(IFadeOutIn_BGM(bgmType));
    }

    public void StopBgm()
    {
        if (bgmStopCoroutine != null)
            CoroutineHelper.StopCoroutine(bgmStopCoroutine);

        bgmStopCoroutine = CoroutineHelper.StartCoroutine(IStop_BGM());
    }

    public void PlaySfx(ESfxSoundType sfxType)
    {
        string path = $"{LoadPath.SOUND_SFX_PATH}/{sfxType}";
        AudioClip audioClip = GetOrAddAudioClip(path);

        if (audioClip == null)
            return;

        audioSources[(int)ESoundType.SFX].PlayOneShot(audioClip);
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

    private IEnumerator IStop_BGM()
    {
        yield return new WaitUntil(() => fadeOutInBGMCoroutine == null);

        float time = 0f;
        float currVolume = audioSources[(int)ESoundType.BGM].volume;

        if (audioSources[(int)ESoundType.BGM].isPlaying) // 실행 중일 경우
        {
            float volume = currVolume;
            // FadeOut
            while (volume > 0f)
            {
                time += Time.deltaTime / fadeSoundTime;
                volume = Mathf.Lerp(currVolume, 0, time);
                SetBgmAudioSource(volume);

                yield return null;
            }
        }

        currVolume = 0.0f;
        SetBgmAudioSource(currVolume);
        audioSources[(int)ESoundType.BGM].Stop();

        bgmStopCoroutine = null;
    }

    private IEnumerator IFadeOutIn_BGM(EBgmSoundType bgmType)
    {
        yield return new WaitUntil(() => bgmStopCoroutine == null);

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
                time += Time.deltaTime / fadeSoundTime;
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
                time += Time.deltaTime / fadeSoundTime;
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

        fadeOutInBGMCoroutine = null;
    }
}
