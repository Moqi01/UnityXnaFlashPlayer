using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Microsoft.Xna.Framework.Audio
{
    public class SoundEffect : IDisposable
    {
        public UnityEngine.AudioClip Clip { get; set; }
        private object syncObject = new object();
        private bool disposed;
        private static AudioSource audioSettings;
        public void Play()
        {
            GameObject gameObject = new GameObject("SoundEffectAudioClip");
            audioSettings = gameObject.AddComponent<AudioSource>();
            audioSettings.clip = Clip;
            audioSettings.Play();
            audioSettings.volume = MasterVolume;
            gameObject.AddComponent<AudioSourceController>();
            // TODO
        }

        public bool Play(float volume, float pitch, float pan)
        {
            SoundEffectInstance pooledInstance = this.GetPooledInstance(false);
            if (pooledInstance == null)
            {
                return false;
            }
            pooledInstance.Volume = volume;
            pooledInstance.Pitch = pitch;
            pooledInstance.Pan = pan;
            pooledInstance.Play();
            return true;
        }

        internal SoundEffectInstance GetPooledInstance(bool forXAct)
        {
            if (!SoundEffectInstancePool.SoundsAvailable)
            {
                return null;
            }
            SoundEffectInstance instance = SoundEffectInstancePool.GetInstance(forXAct);
            instance._effect = this;
           // this.PlatformSetupInstance(instance);
            return instance;
        }
        public SoundEffect(byte[] buffer, int offset, int count, int sampleRate, AudioChannels channels, int loopStart, int loopLength)
        {
            this._duration = SoundEffect.GetSampleDuration(count, sampleRate, channels);
        }

        public SoundEffect() { }

        public static TimeSpan GetSampleDuration(int sizeInBytes, int sampleRate, AudioChannels channels)
        {
            if (sizeInBytes < 0)
            {
                Debug.Log("Buffer size cannot be negative. sizeInBytes");
            }
            if (sampleRate < 8000 || sampleRate > 48000)
            {
                Debug.Log("sampleRate");
            }
            if (channels != AudioChannels.Mono && channels != AudioChannels.Stereo)
            {
                Debug.Log("channels");
            }
            if (sizeInBytes == 0)
            {
                return TimeSpan.Zero;
            }
            float num = (float)sizeInBytes / ((float)(sampleRate * (int)channels) * 16f / 8f);
            return TimeSpan.FromSeconds((double)num);
        }

        public static SoundEffect FromStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            return new SoundEffect(stream);
        }
        private readonly TimeSpan _duration;
        private SoundEffect(Stream stream)
        {
            this.PlatformLoadAudioStream(stream, out this._duration);
        }

        private void PlatformLoadAudioStream(Stream s, out TimeSpan duration)
        {
            //SoundStream soundStream = new SoundStream(s);
            //if (soundStream.Format.Encoding != WaveFormatEncoding.Pcm)
            //{
            //    throw new ArgumentException("Ensure that the specified stream contains valid PCM mono or stereo wave data.");
            //}
            //DataStream dataStream = soundStream.ToDataStream();
            //int num = (int)(dataStream.Length / (long)(soundStream.Format.Channels * soundStream.Format.BitsPerSample / 8));
            //this.CreateBuffers(soundStream.Format, dataStream, 0, num);
            //duration = TimeSpan.FromSeconds((double)((float)num / (float)soundStream.Format.SampleRate));
            duration = TimeSpan.FromSeconds((double)(0));
            Debug.Log("PlatformLoadAudioStream");
        }

        internal List<WeakReference> Instances = new List<WeakReference>();
        internal IALBuffer INTERNAL_buffer;
        public TimeSpan Duration
        {
            get
            {
                if (this.INTERNAL_buffer != null)
                    return this.INTERNAL_buffer.Duration;
                else
                    return new TimeSpan();
            }
        }

        public static float MasterVolume
        {
            get
            {
                return AudioDevice.MasterVolume;
            }
            set
            {
                AudioDevice.MasterVolume = value;
                if(audioSettings!=null)
                {
                    audioSettings.volume = value;
                }
            }
        }

        public string Name
        {

            get
            {
                if (Clip != null)
                    return this.Clip.name;
                else
                    return string.Empty;
            }

            set
            {
                if (Clip != null)
                    this.Clip.name = value;
            }
        }

        public void Dispose()
        {
            Debug.Log("Dispose");
            foreach (WeakReference weakReference in this.Instances.ToArray())
            {
                object target = weakReference.Target;
                if (target != null)
                {
                    (target as IDisposable).Dispose();
                }
            }
            this.Instances.Clear();
            this.Dispose(true);
        }
        private void Dispose(bool disposing)
        {
            lock (this.syncObject)
            {
                if (!this.IsDisposed)
                {
                    this.disposed = true;
                }
            }
        }

        public SoundEffectInstance CreateInstance()
        {
            SoundEffectInstance soundEffectInstance = new SoundEffectInstance();
           // this.PlatformSetupInstance(soundEffectInstance);
            soundEffectInstance._isPooled = false;
            soundEffectInstance._effect = this;
            return soundEffectInstance;
        }

        public static SoundEffectInstance SoundEffectInstance;

        public static SoundEffectInstance CreateStaticInstance()
        {
            if (SoundEffectInstance == null)
            {
                SoundEffectInstance = new SoundEffectInstance();
                SoundEffectInstance._isPooled = false;
                return SoundEffectInstance;
            }
            else
            {
                return SoundEffectInstance;
            }
        }

        public bool IsDisposed
        {
            get
            {
                return this.disposed;
            }
        }
    }

    public enum AudioChannels
    {
        // Token: 0x0400017A RID: 378
        Mono = 1,
        // Token: 0x0400017B RID: 379
        Stereo
    }

    public class SoundEffectInstance : IDisposable
    {
        private uint voiceHandle = uint.MaxValue;
        internal bool _isDynamic;
        // Token: 0x04000186 RID: 390
        internal bool _isPooled = true;
        // Token: 0x0400021A RID: 538
        private object voiceHandleLock = new object();
        private float _pan;

        // Token: 0x0400018B RID: 395
        private float _volume;

        // Token: 0x0400018C RID: 396
        private float _pitch;
        private SoundEffect INTERNAL_parentEffect;
        void IDisposable.Dispose()
        {
            Debug.Log("Dispose");
            if (!this.IsDisposed)
            {
                this.Stop(true);
                if (this.INTERNAL_parentEffect != null)
                {
                    this.INTERNAL_parentEffect.Instances.Remove(this.selfReference);
                    this.selfReference = null;
                }
                this.IsDisposed = true;
            }
        }
        internal bool _isXAct;
        public float Pitch
        {
            get
            {
                return this._pitch;
            }
            set
            {
                if (!this._isXAct && (value < -1f || value > 1f))
                {
                    //Debug.Log();
                }
                this._pitch = value;
                // this.PlatformSetPitch(value);
               
            }
        }

        public virtual void Pause()
        {
            this.PlatformPause();
        }
        private bool _paused;
        private void PlatformPause()
        {
            //if (this._voice != null)
            //{
            //    this._voice.Stop();
            //}
            INTERNAL_alSource.Stop();
            this._paused = true;
        }

        private SoundState PlatformGetState()
        {
            //if (this._voice == null || this._voice.State.BuffersQueued == 0)
            //{
            //    return SoundState.Stopped;
            //}
            if (this._paused)
            {
                return SoundState.Paused;
            }
            return SoundState.Playing;
        }

        public void Stop(bool immediate = false)
        {
            if (this.INTERNAL_alSource != null)
            {
                INTERNAL_alSource.Stop();
                UnityEngine.Object.Destroy(INTERNAL_alSource);
                this.INTERNAL_alSource = null;
            }
        }

        public uint VoiceHandle
        {
            get
            {
                return this.voiceHandle;
            }
            set
            {
                this.voiceHandle = value;
            }
        }

        // Token: 0x1700004C RID: 76
        // (get) Token: 0x060003E9 RID: 1001 RVA: 0x0001A8F8 File Offset: 0x00019CF8
        public object VoiceHandleLock
        {
            get
            {
                return this.voiceHandleLock;
            }
        }

        // Token: 0x0400018D RID: 397
        private static float[] _defaultChannelAzimuths;
        static SoundEffectInstance()
        {
            float[] defaultChannelAzimuths = new float[2];
            SoundEffectInstance._defaultChannelAzimuths = defaultChannelAzimuths;
            SoundEffectInstance._outputMatrix = new float[16];
        }

        private static readonly float[] _outputMatrix;
        internal static float[] CalculateOutputMatrix(float pan, float scale, int inputChannels)
        {
            float[] outputMatrix = SoundEffectInstance._outputMatrix;
            Array.Clear(outputMatrix, 0, outputMatrix.Length);
            if (inputChannels == 1)
            {
                outputMatrix[0] = ((pan > 0f) ? ((1f - pan) * scale) : scale);
                outputMatrix[1] = ((pan < 0f) ? ((1f + pan) * scale) : scale);
            }
            else if (inputChannels == 2)
            {
                if (pan <= 0f)
                {
                    outputMatrix[0] = (1f + pan * 0.5f) * scale;
                    outputMatrix[1] = -pan * 0.5f * scale;
                    outputMatrix[2] = 0f;
                    outputMatrix[3] = (1f + pan) * scale;
                }
                else
                {
                    outputMatrix[0] = (1f - pan) * scale;
                    outputMatrix[1] = 0f;
                    outputMatrix[2] = pan * 0.5f * scale;
                    outputMatrix[3] = (1f - pan * 0.5f) * scale;
                }
            }
            return outputMatrix;
        }

        // Token: 0x060003EA RID: 1002 RVA: 0x0001A90C File Offset: 0x00019D0C
        public SoundEffectInstance(SoundEffect parentEffect, bool fireAndForget = false)
        {
            if (parentEffect.IsDisposed)
            {
                Debug.Log("SoundEffectInstance");
                throw new ObjectDisposedException(typeof(SoundEffect).Name, "????");
            }
            this.INTERNAL_parentEffect = parentEffect;
            this.Volume = 1f;
            if (this.INTERNAL_parentEffect != null)
            {
                this.selfReference = new WeakReference(this);
                this.INTERNAL_parentEffect.Instances.Add(this.selfReference);
            }
            this.isDynamic = false;
            this.isFireAndForget = fireAndForget;
        }

        internal SoundEffect _effect;
        // Token: 0x060003EB RID: 1003 RVA: 0x0001A9A4 File Offset: 0x00019DA4
        private void PlatformDispose(bool disposing)
        {
            if (disposing)
            {
                //if (this._reverb != null)
                //{
                //    this._reverb.Dispose();
                //}
                //if (this._voice != null)
                //{
                //    this._voice.DestroyVoice();
                //    this._voice.Dispose();
                //}
            }
           // this._voice = null;
            this._effect = null;
          //  this._reverb = null;
        }

        public void Dispose()
        {
            this.Dispose(true);
           // GC.SuppressFinalize(this);
        }

        public SoundEffectInstance()
        {
            this._pan = 0f;
            this._volume = 1f;
            this._pitch = 0f;
        }

        private bool _isDisposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!this._isDisposed)
            {
                this.PlatformDispose(disposing);
                this._isDisposed = true;
            }
        }

        private WeakReference selfReference;

        private bool isFireAndForget;

        public UnityEngine.AudioClip Clip { get; set; }

        private AudioSource INTERNAL_alSource;
        public virtual void Play()
        {
            
            GameObject gameObject = new GameObject("SoundEffectAudioClip");

            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = Clip;
            audioSource.Play();
            audioSource.volume = Volume;
            INTERNAL_alSource = audioSource;
            gameObject.AddComponent<AudioSourceController>();
        }


        // Token: 0x040000BF RID: 191
        internal bool INTERNAL_isXACTSource;
        // Token: 0x040000B8 RID: 184
        private float INTERNAL_volume = 1f;
        public float Volume
        {
            get
            {
                return this.INTERNAL_volume;
            }
            set
            {
                if (!this.INTERNAL_isXACTSource)
                {
                    value = MathHelper.Clamp(value, 0f, 1f);
                }
                this.INTERNAL_volume = value;
                if (this.INTERNAL_alSource != null)
                {
                    INTERNAL_alSource.volume = Volume;
                    // AudioDevice.ALDevice.SetSourceVolume(this.INTERNAL_alSource, value);
                }
            }
        }
        // Token: 0x040000B5 RID: 181
        private bool INTERNAL_looped;
        public virtual bool IsLooped
        {
            get
            {
                return this.INTERNAL_looped;
            }
            set
            {
                this.INTERNAL_looped = value;
                if (this.INTERNAL_alSource != null)
                {
                    INTERNAL_alSource.volume = Volume;
                    // AudioDevice.ALDevice.SetSourceVolume(this.INTERNAL_alSource, value);
                }
            }
        }

        public float Pan
        {
            get
            {
                return this._pan;
            }
            set
            {
                if (value < -1f || value > 1f)
                {
                    Debug.Log("Pan");
                }
                this._pan = value;
                //this.PlatformSetPan(value);
            }
        }

        private bool isDisposed;
        public bool IsDisposed
        {

            get
            {
                return this.isDisposed;
            }

            protected set
            {
                this.isDisposed = value;
            }
        }

        internal bool isDynamic;
        public SoundState State
        {
            get
            {
                if (this.INTERNAL_alSource == null)
                {
                    return SoundState.Stopped;
                }

                //SoundState sourceState = AudioDevice.ALDevice.GetSourceState(this.INTERNAL_alSource);
                SoundState sourceState = INTERNAL_alSource.isPlaying ? SoundState.Playing : SoundState.Stopped;
                if (sourceState == SoundState.Stopped && this.isDynamic)
                {
                    return SoundState.Playing;
                }
                return sourceState;
            }
        }
    }
    // Token: 0x02000030 RID: 48
    public enum SoundState
    {
        // Token: 0x040000E2 RID: 226
        Playing,
        // Token: 0x040000E3 RID: 227
        Paused,
        // Token: 0x040000E4 RID: 228
        Stopped
    }

    internal interface IALBuffer
    {
        // Token: 0x1700013C RID: 316
        // (get) Token: 0x060005BB RID: 1467
        TimeSpan Duration { get; }

        // Token: 0x1700013D RID: 317
        // (get) Token: 0x060005BC RID: 1468
        int Channels { get; }

        // Token: 0x1700013E RID: 318
        // (get) Token: 0x060005BD RID: 1469
        int SampleRate { get; }
    }

    public sealed class NoAudioHardwareException : ExternalException
    {
        // Token: 0x06000114 RID: 276 RVA: 0x0000764F File Offset: 0x0000584F
        public NoAudioHardwareException()
        {
        }

        // Token: 0x06000115 RID: 277 RVA: 0x00007657 File Offset: 0x00005857
        public NoAudioHardwareException(string message) : base(message)
        {
        }

        // Token: 0x06000116 RID: 278 RVA: 0x00007660 File Offset: 0x00005860
        public NoAudioHardwareException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
    internal static class AudioDevice
    {
        public static float MasterVolume
        {
            get
            {
                return AudioDevice.INTERNAL_masterVolume;
            }
            set
            {
                AudioDevice.INTERNAL_masterVolume = value;
                //if (AudioDevice.ALDevice != null)
                //{
                //    AudioDevice.ALDevice.SetMasterVolume(value);
                //}
            }
        }
        private static float INTERNAL_masterVolume = 1f;
        private static float INTERNAL_dopplerScale =1f;

        public static float DopplerScale
        {
            get
            {
                return AudioDevice.INTERNAL_dopplerScale;
            }
            set
            {
                AudioDevice.INTERNAL_dopplerScale = value;
                //if (AudioDevice.ALDevice != null)
                //{
                //    AudioDevice.ALDevice.SetDopplerScale(value);
                //}
            }
        }
    }

    internal static class SoundEffectInstancePool
    {
        // Token: 0x060005C0 RID: 1472 RVA: 0x000226BC File Offset: 0x000208BC
        static SoundEffectInstancePool()
        {
            int capacity = 1024;
            SoundEffectInstancePool._playingInstances = new List<SoundEffectInstance>(capacity);
            SoundEffectInstancePool._pooledInstances = new List<SoundEffectInstance>(capacity);
        }

        // Token: 0x17000136 RID: 310
        // (get) Token: 0x060005C1 RID: 1473 RVA: 0x000084B7 File Offset: 0x000066B7
        internal static bool SoundsAvailable
        {
            get
            {
                return SoundEffectInstancePool._playingInstances.Count < int.MaxValue;
            }
        }

        // Token: 0x060005C2 RID: 1474 RVA: 0x000084CA File Offset: 0x000066CA
        internal static void Add(SoundEffectInstance inst)
        {
            if (inst._isPooled)
            {
                SoundEffectInstancePool._pooledInstances.Add(inst);
                inst._effect = null;
            }
            SoundEffectInstancePool._playingInstances.Remove(inst);
        }

        // Token: 0x060005C3 RID: 1475 RVA: 0x000084F2 File Offset: 0x000066F2
        internal static void Remove(SoundEffectInstance inst)
        {
            SoundEffectInstancePool._playingInstances.Add(inst);
        }

        // Token: 0x060005C4 RID: 1476 RVA: 0x000226E8 File Offset: 0x000208E8
        internal static SoundEffectInstance GetInstance(bool forXAct)
        {
            UnityEngine.Debug.Log("SoundEffectInstance GetInstance");
            int count = SoundEffectInstancePool._pooledInstances.Count;
            SoundEffectInstance soundEffectInstance;
            if (count > 0)
            {
                soundEffectInstance = SoundEffectInstancePool._pooledInstances[count - 1];
                SoundEffectInstancePool._pooledInstances.RemoveAt(count - 1);
                soundEffectInstance._isPooled = true;
                soundEffectInstance._isXAct = forXAct;
                soundEffectInstance.Volume = 1f;
                soundEffectInstance.Pan = 0f;
                soundEffectInstance.Pitch = 0f;
                soundEffectInstance.IsLooped = false;
               // soundEffectInstance.PlatformSetReverbMix(0f);
               // soundEffectInstance.PlatformClearFilter();
            }
            else
            {
                soundEffectInstance = new SoundEffectInstance();
                soundEffectInstance._isPooled = true;
                soundEffectInstance._isXAct = forXAct;
            }
            return soundEffectInstance;
        }

        // Token: 0x060005C5 RID: 1477 RVA: 0x00022780 File Offset: 0x00020980
        internal static void Update()
        {
            int i = 0;
            while (i < SoundEffectInstancePool._playingInstances.Count)
            {
                SoundEffectInstance soundEffectInstance = SoundEffectInstancePool._playingInstances[i];
                if (soundEffectInstance.IsDisposed || soundEffectInstance.State == SoundState.Stopped || (soundEffectInstance._effect == null && !soundEffectInstance._isDynamic))
                {
                    SoundEffectInstancePool.Add(soundEffectInstance);
                }
                else
                {
                    i++;
                }
            }
        }

        // Token: 0x060005C6 RID: 1478 RVA: 0x000227DC File Offset: 0x000209DC
        internal static void StopPooledInstances(SoundEffect effect)
        {
            int i = 0;
            while (i < SoundEffectInstancePool._playingInstances.Count)
            {
                SoundEffectInstance soundEffectInstance = SoundEffectInstancePool._playingInstances[i];
                if (soundEffectInstance._effect == effect)
                {
                    soundEffectInstance.Stop(true);
                    SoundEffectInstancePool.Add(soundEffectInstance);
                }
                else
                {
                    i++;
                }
            }
        }

        // Token: 0x060005C7 RID: 1479 RVA: 0x00022824 File Offset: 0x00020A24
        internal static void UpdateMasterVolume()
        {
            foreach (SoundEffectInstance soundEffectInstance in SoundEffectInstancePool._playingInstances)
            {
                if (!soundEffectInstance._isXAct)
                {
                    soundEffectInstance.Volume = soundEffectInstance.Volume;
                }
            }
        }

        // Token: 0x040001B3 RID: 435
        private static readonly List<SoundEffectInstance> _playingInstances;

        // Token: 0x040001B4 RID: 436
        private static readonly List<SoundEffectInstance> _pooledInstances;
    }
}
