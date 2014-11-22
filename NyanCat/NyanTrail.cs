using System;
using System.Collections.Generic;
using UnityEngine;

namespace NyanCat
{
    public class NyanTrail
    {
        private bool showingTrail = false;
        private Vessel followVessel;
        private NyanSound nyanSound;
        private float lastNyanSquareTime = float.MinValue;
        private List<NyanSquare> nyanSquares = new List<NyanSquare>();

        public NyanTrail(Vessel followVessel)
        {
            this.followVessel = followVessel;
            this.nyanSound = new NyanSound(followVessel);
        }

        public void Update()
        {
            if (!showingTrail && ShouldShowTrail)
            {
                showingTrail = true;
                ShowTrail();
            }
            if (showingTrail && !ShouldShowTrail)
            {
                showingTrail = false;
                HideTrail();
            }
            if (showingTrail)
            {
                UpdateTrail();
            }
        }

        private void ShowTrail()
        {
            Debug.Log("Nyan showing trail for " + followVessel.id + " (" + followVessel.name + ")");
            nyanSound.Start();
        }

        private void HideTrail()
        {
            Debug.Log("Nyan hiding trail for " + followVessel.id + " (" + followVessel.name + ")");
            foreach (NyanSquare nyanSquare in nyanSquares)
            {
                nyanSquare.Destroy();
            }
            nyanSquares.Clear();
            nyanSound.Stop();
        }

        private bool ShouldShowTrail
        {
            get
            {
                bool correctAlt = (followVessel.altitude > 9000f) && (followVessel.altitude < 10000f);
                bool correctSpeed = followVessel.srfSpeed > 500f;
                bool correctVerticalSpeed = followVessel.verticalSpeed < 50f;
                return followVessel.loaded && correctAlt && correctSpeed && correctVerticalSpeed;
            }
        }

        public void UpdateTrail()
        {
            if (showingTrail)
            {
                if ((Time.realtimeSinceStartup - lastNyanSquareTime) > 1f)
                {
                    lastNyanSquareTime = Time.realtimeSinceStartup;
                    ShiftTrail();
                }
                NyanSquare lastSquare = null;
                foreach (NyanSquare nyanSquare in nyanSquares)
                {
                    nyanSquare.Update(lastSquare);
                    lastSquare = nyanSquare;
                }
                nyanSound.Update();
            }
        }

        public void ShiftTrail()
        {
            NyanSquare nyanSquare = new NyanSquare(followVessel, followVessel.latitude, followVessel.longitude, followVessel.altitude);
            nyanSquares.Insert(0, nyanSquare);
            if (nyanSquares.Count > 9)
            {
                nyanSquares[10].Destroy();
                nyanSquares.RemoveAt(10);
            }
            for (int i = 0; i < nyanSquares.Count; i++)
            {
                nyanSquares[i].SetOpacity(i);
            }
        }

        public void Destroy()
        {
            if (showingTrail)
            {
                HideTrail();
            }
        }
    }

}

