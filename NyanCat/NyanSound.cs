using System;
using UnityEngine;

namespace NyanCat
{
    public class NyanSound
    {
        private GameObject gameObject;
        private Vessel followVessel;
        public bool soundDelayComplete
        {
            get;
            private set;
        }
        private float soundStartTime;

        public NyanSound(Vessel followVessel)
        {
            this.followVessel = followVessel;
            soundDelayComplete = false;
        }

        public void Start()
        {
            AudioClip nyanSound = GameDatabase.Instance.GetAudioClip("NyanCat/nyan");
            if (nyanSound != null)
            {
                if (gameObject == null)
                {
                    gameObject = new GameObject();
                    AudioSource source = gameObject.AddComponent<AudioSource>();
                    source.clip = nyanSound;
                    source.Play();
                    soundStartTime = Time.realtimeSinceStartup;
                }
            }
        }

        public void Stop()
        {
            if (gameObject != null)
            {
                AudioSource source = gameObject.GetComponent<AudioSource>();
                source.Stop();
                GameObject.Destroy(gameObject);
                gameObject = null;
            }
            soundDelayComplete = false;
        }

        public void Update()
        {
            if (gameObject != null)
            {
                gameObject.transform.position = followVessel.transform.position;
                if (!soundDelayComplete && ((Time.realtimeSinceStartup - soundStartTime) > 3.5f))
                {
                    soundDelayComplete = true;
                }
            }
        }
    }
}

