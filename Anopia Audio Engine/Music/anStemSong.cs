using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class anStemSong : IanSong
{
    public List<AnopiaSourcerer> SourceHandlers = new List<AnopiaSourcerer>();//need sourcerer??
    public override void FadeOut(float t, Action ondone = null)
    {
        Action<float> ChangeValue = null;
        foreach (AnopiaSourcerer s in SourceHandlers)
        {
            void Fade(float v)
            {
                s.Source.volume = v;
            };
            ChangeValue += Fade;
        }
        StartCoroutine(AnopiaAudioCore.FadeValue(t, 1, 0, ChangeValue, ondone+=()=>Destroy(gameObject) ));
    }
    public override void Play(double startTime)
    {
        foreach (AnopiaSourcerer s in SourceHandlers)
            s.Source.PlayScheduled(startTime);
    }
    public override void StopCue(double stopTime)
    {
        foreach (AnopiaSourcerer s in SourceHandlers)
            s.StopScheduled(stopTime, true);
        transform.DetachChildren();
        Destroy(gameObject);
    }
    public override void FadeIn(float t)
    {
        Action<float> ChangeValue = null;
        foreach (AnopiaSourcerer s in SourceHandlers)
        {
            void Fade(float v)
            {
                s.Source.volume = v;
            };
            ChangeValue += Fade;
        }
        StartCoroutine(AnopiaAudioCore.FadeValue(t, 0, 1, ChangeValue));
    }
    public void Setup(MonoBehaviour host, IanMusicMag mag, AudioMixerGroup[] busses)
    {
        anStemMusicMag Mag = (anStemMusicMag)mag;
        Dictionary<AudioMixerGroup, AudioClip> stems = Mag.GetStems();
        foreach(KeyValuePair<AudioMixerGroup, AudioClip> p in stems)
        {
            AnopiaSourcerer s = AnopiaAudioCore.NewStereoSource(this, p.Key);
            AudioSource a = s.Source;
            a.clip = p.Value;
            a.volume = 1;
            a.loop = true;
            SourceHandlers.Add(s);
        }
    }
    public override void StopImmediate()
    {
        Destroy(gameObject);
    }

    public override void Pause()
    {
        foreach (AnopiaSourcerer s in SourceHandlers)
            s.Source.Pause();
    }
    public override void UnPause()
    {
        foreach (AnopiaSourcerer s in SourceHandlers)
            s.Source.UnPause();
    }
}