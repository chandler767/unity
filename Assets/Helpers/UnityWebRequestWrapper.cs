using System;
using UnityEngine;
using UnityEngine.Networking;

namespace PubNubAPI
{
    internal class UnityWebRequestWrapper
    {  
        internal UnityWebRequest CurrentUnityWebRequest;
        internal string URL;
        internal int Timeout;
        internal int Pause;
        internal PNCurrentRequestType CurrentRequestType;
        internal bool IsComplete;
        //public Type typeParameterType;
        internal RequestState CurrentRequestState;
        internal bool RunTimer = false;
        internal float Timer = 0;
        internal bool RunPauseTimer = false;
        internal float PauseTimer = 0;
        internal bool UsePost = false;
        internal string PostData = "";

        internal UnityWebRequestWrapper(string url, int timeout, int pause, PNCurrentRequestType crt, RequestState requestState, bool usePost, string postData){
            this.CurrentUnityWebRequest = null;
            this.URL = url;
            this.Timeout = timeout;
            this.Pause = pause;
            this.CurrentRequestType = crt;
            this.CurrentRequestState = requestState;
            this.IsComplete = false;
            this.UsePost = usePost;
            this.Timer = timeout;
            this.PostData = postData;

            //CurrentUnityWebRequest.timeout = timeout;
        }
    }
}