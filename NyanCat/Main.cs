using System;
using System.Collections.Generic;
using UnityEngine;

namespace NyanCat
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class Main : MonoBehaviour
    {
        private Dictionary<Vessel, NyanTrail> nyanVessels = new Dictionary<Vessel, NyanTrail>();

        public void Awake()
        {
            GameEvents.onVesselCreate.Add(OnVesselCreate);
            GameEvents.onVesselDestroy.Add(OnVesselDestroy);
        }

        public void OnVesselCreate(Vessel createdVessel)
        {
            if (!nyanVessels.ContainsKey(createdVessel))
            {
                nyanVessels.Add(createdVessel, new NyanTrail(createdVessel));
            }
        }

        public void OnVesselDestroy(Vessel destroyedVessel)
        {
            if (nyanVessels.ContainsKey(destroyedVessel))
            {
                nyanVessels[destroyedVessel].Destroy();
                nyanVessels.Remove(destroyedVessel);
            }
        }

        public void Update()
        {
            if (!FlightGlobals.ready)
            {
                return;
            }
            foreach (KeyValuePair<Vessel, NyanTrail> kvp in nyanVessels)
            {
                kvp.Value.Update();
            }
        }

        public void OnDestroy()
        {
            GameEvents.onVesselCreate.Remove(OnVesselCreate);
            GameEvents.onVesselDestroy.Remove(OnVesselDestroy);
        }
    }
}

