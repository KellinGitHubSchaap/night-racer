//______________________________________________//
//___________Realistic Engine Sounds____________//
//______________________________________________//
//_______Copyright © 2019 Yugel Mobile__________//
//______________________________________________//
//_________ http://mobile.yugel.net/ ___________//
//______________________________________________//
//________ http://fb.com/yugelmobile/ __________//
//______________________________________________//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TurboCharger : MonoBehaviour {

    RealisticEngineSound res;
    private float clipsValue;
    // master volume setting
    [Range(0.1f, 1.0f)]
    public float masterVolume = 1f;
    // audio mixer
    public AudioMixerGroup audioMixer;
    private AudioMixerGroup _audioMixer;
    // turbo loop
    public AudioClip turboLoopClip; // ssssssSSSS
    public AudioClip maxRpmLoopClip; // played at max rpm
    public AnimationCurve chargerVolCurve;
    public AnimationCurve chargerPitchCurve;
    // long shoot settings
    [Range(0.4f, 1.0f)]
    public float longShotTreshold = 0.8f; // after this % of current rpm, long shots are played when gas pedal is released
    // one shoot ssshutututus
    public AudioClip[] longShotClips; // SssHuuTuTuTututu
    public AudioClip[] shortShotClips; // SssHuuTu
    public AnimationCurve oneShotVolCurve;
    public AnimationCurve oneShotPitchCurve;
    //
    private AudioSource turboLoop;
    private AudioSource oneShot;
    private AudioSource maxTurboLoop;

    private int oneShotController = 0;
    private WaitForSeconds _playtime;

    void Start ()
    {
        res = gameObject.transform.parent.GetComponent<RealisticEngineSound>();
        if (audioMixer != null) // user is using a seperate audio mixer for this prefab
        {
            _audioMixer = audioMixer;
        }
        else
        {
            if (res.audioMixer != null) // use engine sound's audio mixer for this prefab
            {
                _audioMixer = res.audioMixer;
                audioMixer = _audioMixer;
            }
        }
        _playtime = new WaitForSeconds(0.15f);
        // create audio
        oneShot = gameObject.AddComponent<AudioSource>();
        oneShot.rolloffMode = res.audioRolloffMode;
        oneShot.dopplerLevel = res.dopplerLevel;
        oneShot.spatialBlend = res.spatialBlend;
        oneShot.minDistance = res.minDistance;
        oneShot.maxDistance = res.maxDistance;
        oneShot.loop = false;
        if (_audioMixer != null)
            oneShot.outputAudioMixerGroup = _audioMixer;
        oneShot.Stop();
    }
    void Update ()
    {
		clipsValue = res.engineCurrentRPM / res.maxRPMLimit; // calculate % percentage of rpm
        if (res.isCameraNear)
        {
            // if gas pedal on play turbo loop sound
            if (res.gasPedalPressing)
            {
                oneShotController = 1; // prepare for one shoot
                if (res.maxRPMVolCurve.Evaluate(clipsValue) < 0.5f)
                {
                    if (turboLoop == null)
                        CreateTurboLoop();
                    turboLoop.volume = chargerVolCurve.Evaluate(clipsValue) * masterVolume * res.gasPedalValue;
                    turboLoop.pitch = chargerPitchCurve.Evaluate(clipsValue);
                    if (maxTurboLoop != null)
                        Destroy(maxTurboLoop);
                }
                else // play max rpm turbo loop
                {
                    if (res.useRPMLimit)
                    {
                        if (maxTurboLoop == null)
                            CreateMaxTurboLoop();
                        else
                        {
                            maxTurboLoop.volume = chargerVolCurve.Evaluate(clipsValue) * masterVolume;
                            maxTurboLoop.pitch = chargerPitchCurve.Evaluate(clipsValue);
                        }
                        if (turboLoop != null)
                            Destroy(turboLoop);
                    }
                }
            }
            else// if gas released play one shoot
            {
                // destroy turbo loops
                if (turboLoop != null)
                    Destroy(turboLoop);
                if (maxTurboLoop != null)
                    Destroy(maxTurboLoop);
                // play one shot
                if (oneShotController == 1)
                {
                    CreateOneShot();
                    oneShotController = 0; // one shot is played, do not play more
                }
            }
        }
        else
        {
            DestroyAll();
        }
	}
    private void OnDisable() // destroy audio sources if disabled
    {
        DestroyAll();
    }
    private void OnEnable() // recreate audio sources if reEnabled
    {
        StartCoroutine(WaitForStart());
    }
    private void DestroyAll()
    {
        if (turboLoop != null)
            Destroy(turboLoop);
        if (oneShot != null)
            Destroy(oneShot);
        if (maxTurboLoop != null)
            Destroy(maxTurboLoop);
    }
    IEnumerator WaitForStart()
    {
        while (true)
        {
            yield return _playtime; // this is needed to avoid duplicate audio sources at scene start
            if (oneShot == null)
                Start();
            break;
        }
    }
    // create audio sources
    void CreateTurboLoop()
    {
        if (turboLoopClip != null)
        {
            turboLoop = gameObject.AddComponent<AudioSource>();
            turboLoop.rolloffMode = res.audioRolloffMode;
            turboLoop.dopplerLevel = res.dopplerLevel;
            turboLoop.volume = chargerVolCurve.Evaluate(clipsValue) * masterVolume;
            turboLoop.pitch = chargerPitchCurve.Evaluate(clipsValue);
            turboLoop.minDistance = res.minDistance;
            turboLoop.maxDistance = res.maxDistance;
            turboLoop.spatialBlend = res.spatialBlend;
            turboLoop.loop = true;
            if (_audioMixer != null)
                turboLoop.outputAudioMixerGroup = _audioMixer;
            turboLoop.clip = turboLoopClip;
            turboLoop.Play();
        }
    }
    void CreateOneShot()
    {
        if (oneShot != null)
        {
            oneShot.volume = oneShotVolCurve.Evaluate(clipsValue) * masterVolume;
            oneShot.pitch = oneShotPitchCurve.Evaluate(clipsValue) * Random.Range(0.85f, 1.2f);
            if (clipsValue > longShotTreshold)
            {
                oneShot.clip = longShotClips[Random.Range(0, longShotClips.Length)];
            }
            else
            {
                oneShot.clip = shortShotClips[Random.Range(0, shortShotClips.Length)];
            }
            oneShot.Play();
        }
    }
    void CreateMaxTurboLoop()
    {
        if (maxRpmLoopClip != null)
        {
            maxTurboLoop = gameObject.AddComponent<AudioSource>();
            maxTurboLoop.rolloffMode = res.audioRolloffMode;
            maxTurboLoop.dopplerLevel = res.dopplerLevel;
            maxTurboLoop.volume = chargerVolCurve.Evaluate(clipsValue) * masterVolume;
            maxTurboLoop.pitch = chargerPitchCurve.Evaluate(clipsValue);
            maxTurboLoop.minDistance = res.minDistance;
            maxTurboLoop.maxDistance = res.maxDistance;
            maxTurboLoop.spatialBlend = res.spatialBlend;
            maxTurboLoop.loop = true;
            if (_audioMixer != null)
                maxTurboLoop.outputAudioMixerGroup = _audioMixer;
            maxTurboLoop.clip = maxRpmLoopClip;
            maxTurboLoop.Play();
        }
    }
}
