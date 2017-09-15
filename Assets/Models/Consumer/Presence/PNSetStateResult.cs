using System;
using System.Collections.Generic;

namespace PubNubAPI
{
    public class PNSetStateResult: PNResult
    {
        public int TotalChannels  { get; set;}
        public int TotalOccupancy  { get; set;}
        //public Dictionary<string, PNHereNowChannelData> Channels { get; set;}
        public PNSetStateResult ()
        {

        }
    }

    /*public class PNHereNowChannelData {
        public string ChannelName;
        public int Occupancy;
        public List<PNHereNowOccupantData> Occupants;
    }

    public class PNHereNowOccupantData {
        public string UUID;
        public object State;

    }*/

}