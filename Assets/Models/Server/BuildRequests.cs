﻿using System;
using System.Text;
using System.Collections.Generic;

namespace PubNubAPI
{

    public class BuildRequests
    {

        #region "Build Request State"

        /*internal static RequestState<T> BuildRequestState<T>(List<ChannelEntity> channelEntities, ResponseType responseType, 
            bool reconnect, long id, bool timeout, long timetoken, Type typeParam, string uuid,
            Action<T> userCallback, Action<PubnubClientError> errorCallback
        ){
            RequestState<T> requestState = new RequestState<T> ();
            requestState.ChannelEntities = channelEntities;
            requestState.RespType = responseType;
            requestState.Reconnect = reconnect;
            requestState.SuccessCallback = userCallback;
            requestState.ErrorCallback = errorCallback;
            requestState.ID = id;
            requestState.Timeout = timeout;
            requestState.Timetoken = timetoken;
            requestState.TypeParameterType = typeParam;
            requestState.UUID = uuid;
            return requestState;
        }

        internal static RequestState<T> BuildRequestState<T>(List<ChannelEntity> channelEntities, ResponseType responseType, 
            bool reconnect, long id, bool timeout, long timetoken, Type typeParam
        ){
            return BuildRequestState<T> (channelEntities, responseType, reconnect, id, timeout, timetoken,
                typeParam, String.Empty, null, null);
        }

        internal static RequestState<T> BuildRequestState<T>(Action<T> userCallback, Action<PubnubClientError> errorCallback, ResponseType responseType, 
            bool reconnect, long id, bool timeout, long timetoken, Type typeParam, string uuid
        ){
            return BuildRequestState<T> (null, responseType, reconnect, id, timeout, timetoken,
                typeParam, uuid, userCallback, errorCallback);
        }*/

        #endregion

        #region "Build Requests"
        internal static Uri BuildRegisterDevicePushRequest(string channel, PNPushType pushType, string pushToken,  string uuid, bool ssl, string origin, string authenticationKey,string subscribeKey, string pnSdkVersion)
        {
            StringBuilder parameterBuilder = new StringBuilder();

            parameterBuilder.AppendFormat("?add={0}", Utility.EncodeUricomponent(channel, PNOperationType.PNAddPushNotificationsOnChannelsOperation, true, false));
            parameterBuilder.AppendFormat("&type={0}", pushType.ToString().ToLower());

            // Build URL
            List<string> url = new List<string>();
            url.Add("v1");
            url.Add("push");
            url.Add("sub-key");
            url.Add(subscribeKey);
            url.Add("devices");
            url.Add(pushToken);

            return BuildRestApiRequest<Uri>(url, PNOperationType.PNAddPushNotificationsOnChannelsOperation, uuid, ssl, origin, 0, authenticationKey, parameterBuilder.ToString(), pnSdkVersion);
        }

        internal static Uri BuildRemoveChannelPushRequest(string channel, PNPushType pushType, string pushToken,  string uuid, bool ssl, string origin, string authenticationKey,string subscribeKey, string pnSdkVersion)
        {
            StringBuilder parameterBuilder = new StringBuilder();

            parameterBuilder.AppendFormat("?remove={0}", Utility.EncodeUricomponent(channel, PNOperationType.PNRemoveChannelsFromGroupOperation, true, false));
            parameterBuilder.AppendFormat("&type={0}", pushType.ToString().ToLower());

            // Build URL
            List<string> url = new List<string>();
            url.Add("v1");
            url.Add("push");
            url.Add("sub-key");
            url.Add(subscribeKey);
            url.Add("devices");
            url.Add(pushToken);

            return BuildRestApiRequest<Uri>(url, PNOperationType.PNRemoveChannelsFromGroupOperation, uuid, ssl, origin, 0, authenticationKey, parameterBuilder.ToString(), pnSdkVersion);
        }

        internal static Uri BuildRemoveAllDevicePushRequest(PNPushType pushType, string pushToken,  string uuid, bool ssl, string origin, string authenticationKey,string subscribeKey, string pnSdkVersion)
        {
            StringBuilder parameterBuilder = new StringBuilder();

            parameterBuilder.AppendFormat("?type={0}", pushType.ToString().ToLower());

            // Build URL
            List<string> url = new List<string>();
            url.Add("v1");
            url.Add("push");
            url.Add("sub-key");
            url.Add(subscribeKey);
            url.Add("devices");
            url.Add(pushToken);
            url.Add("remove");

            return BuildRestApiRequest<Uri>(url, PNOperationType.PNRemoveChannelsFromGroupOperation, uuid, ssl, origin, 0, authenticationKey, parameterBuilder.ToString(), pnSdkVersion);
        }

        internal static Uri BuildGetChannelsPushRequest(PNPushType pushType, string pushToken, string uuid, bool ssl, string origin, string authenticationKey, string subscribeKey, string pnSdkVersion)
        {
            StringBuilder parameterBuilder = new StringBuilder();

            parameterBuilder.AppendFormat("?type={0}", pushType.ToString().ToLower());

            // Build URL
            List<string> url = new List<string>();
            url.Add("v1");
            url.Add("push");
            url.Add("sub-key");
            url.Add(subscribeKey);
            url.Add("devices");
            url.Add(pushToken);

            return BuildRestApiRequest<Uri>(url, PNOperationType.PNPushNotificationEnabledChannelsOperation, uuid, ssl, origin, 0, authenticationKey, parameterBuilder.ToString(), pnSdkVersion);
        }

        internal static Uri BuildUnregisterDevicePushRequest(PNPushType pushType, string pushToken, string uuid, bool ssl, string origin, string authenticationKey, string subscribeKey, string pnSdkVersion)
        {
            StringBuilder parameterBuilder = new StringBuilder();

            parameterBuilder.AppendFormat("?type={0}", pushType.ToString().ToLower());

            // Build URL
            List<string> url = new List<string>();
            url.Add("v1");
            url.Add("push");
            url.Add("sub-key");
            url.Add(subscribeKey);
            url.Add("devices");
            url.Add (pushToken);
            url.Add("remove");

            return BuildRestApiRequest<Uri>(url, PNOperationType.PNRemoveAllPushNotificationsOperation, uuid, ssl, origin, 0, authenticationKey, parameterBuilder.ToString(), pnSdkVersion);
        }

        internal static Uri BuildPublishRequest (string channel, string message, bool storeInHistory, string metadata, uint messageCounter, int ttl, string uuid, bool ssl, string origin, string authenticationKey, string publishKey, string subscribeKey, string cipherKey, string secretKey, string pnSdkVersion)
        {
            StringBuilder parameterBuilder = new StringBuilder ();
            parameterBuilder.AppendFormat ("&seqn={0}", messageCounter.ToString ());
            parameterBuilder.Append ((storeInHistory) ? "" : "&store=0");
            if (ttl >= 0) {
                parameterBuilder.AppendFormat ("&ttl={0}", ttl.ToString());
            }

            if (!string.IsNullOrEmpty (metadata) || metadata.Equals("\"\"")) {
                parameterBuilder.AppendFormat ("&meta={0}", Utility.EncodeUricomponent (metadata, PNOperationType.PNPublishOperation, false, false));
            }

            // Generate String to Sign
            string signature = "0";
            if (!string.IsNullOrEmpty(secretKey) && (secretKey.Length > 0)) {
                StringBuilder stringToSign = new StringBuilder ();
                stringToSign
                    .Append (publishKey)
                    .Append ('/')
                    .Append (subscribeKey)
                    .Append ('/')
                    .Append (secretKey)
                    .Append ('/')
                    .Append (channel)
                    .Append ('/')
                    .Append (message); // 1

                // Sign Message
                signature = Utility.Md5 (stringToSign.ToString ());
            }

            // Build URL
            List<string> url = new List<string> ();
            url.Add ("publish");
            url.Add (publishKey);
            url.Add (subscribeKey);
            url.Add (signature);
            url.Add (channel);
            url.Add ("0");
            url.Add (message);

            return BuildRestApiRequest<Uri> (url, PNOperationType.PNPublishOperation, uuid, ssl, origin, 0, authenticationKey, parameterBuilder.ToString (), pnSdkVersion);
        }

        internal static Uri BuildFetchRequest (string[] channels, long start, long end, uint count, bool reverse, bool includeToken, string uuid, bool ssl, string origin, string authenticationKey, string subscribeKey, string pnSdkVersion)
        {
            StringBuilder parameterBuilder = new StringBuilder ();

            parameterBuilder.AppendFormat ("?count={0}", count);
            if (includeToken) {
                parameterBuilder.AppendFormat ("&include_token={0}", includeToken.ToString ().ToLower ());
            }
            if (reverse) {
                parameterBuilder.AppendFormat ("&reverse={0}", reverse.ToString ().ToLower ());
            }
            if (start != -1) {
                parameterBuilder.AppendFormat ("&start={0}", start.ToString ().ToLower ());
            }
            if (end != -1) {
                parameterBuilder.AppendFormat ("&end={0}", end.ToString ().ToLower ());
            }

            parameterBuilder.AppendFormat ("&uuid={0}", Utility.EncodeUricomponent (uuid, PNOperationType.PNFetchMessagesOperation, false, false));

            List<string> url = new List<string> ();

            url.Add ("v3");
            url.Add ("history");
            url.Add ("sub-key");
            url.Add (subscribeKey);
            url.Add ("channel");
            url.Add (Utility.EncodeUricomponent(string.Join(",", channels), PNOperationType.PNFetchMessagesOperation, true, false));

            return BuildRestApiRequest<Uri> (url, PNOperationType.PNFetchMessagesOperation, uuid, ssl, origin, 0, authenticationKey, parameterBuilder.ToString(), pnSdkVersion);
        }

        internal static Uri BuildHistoryRequest (string channel, long start, long end, uint count, bool reverse, bool includeToken, string uuid, bool ssl, string origin, string authenticationKey, string subscribeKey, string pnSdkVersion)
        {
            StringBuilder parameterBuilder = new StringBuilder ();

            parameterBuilder.AppendFormat ("?count={0}", count);
            if (includeToken) {
                parameterBuilder.AppendFormat ("&include_token={0}", includeToken.ToString ().ToLower ());
            }
            if (reverse) {
                parameterBuilder.AppendFormat ("&reverse={0}", reverse.ToString ().ToLower ());
            }
            if (start != -1) {
                parameterBuilder.AppendFormat ("&start={0}", start.ToString ().ToLower ());
            }
            if (end != -1) {
                parameterBuilder.AppendFormat ("&end={0}", end.ToString ().ToLower ());
            }
            parameterBuilder.AppendFormat ("&uuid={0}", Utility.EncodeUricomponent (uuid, PNOperationType.PNHistoryOperation, false, false));

            List<string> url = new List<string> ();

            url.Add ("v2");
            url.Add ("history");
            url.Add ("sub-key");
            url.Add (subscribeKey);
            url.Add ("channel");
            url.Add (channel);

            return BuildRestApiRequest<Uri> (url, PNOperationType.PNHistoryOperation, uuid, ssl, origin, 0, authenticationKey, parameterBuilder.ToString(), pnSdkVersion);
        }

        internal static Uri BuildHereNowRequest (string channel, string channelGroups, bool showUUIDList, bool includeUserState, string uuid, 
            bool ssl, string origin, string authenticationKey, string subscribeKey, string pnSdkVersion)
        {
            int disableUUID = (showUUIDList) ? 0 : 1;
            int userState = (includeUserState) ? 1 : 0;
            StringBuilder parameterBuilder = new StringBuilder ();
            parameterBuilder.AppendFormat ("?disable_uuids={0}&state={1}", disableUUID, userState);
            if (!string.IsNullOrEmpty(channelGroups))
            {
                parameterBuilder.AppendFormat("&channel-group={0}",  Utility.EncodeUricomponent(channelGroups, PNOperationType.PNHereNowOperation, true, false));
            }

            List<string> url = new List<string> ();

            url.Add ("v2");
            url.Add ("presence");
            url.Add ("sub_key");
            url.Add (subscribeKey);
            if(!string.IsNullOrEmpty(channel))
            {
                url.Add ("channel");
                url.Add (channel);
            } else if (string.IsNullOrEmpty(channel) && (!string.IsNullOrEmpty(channelGroups))){
                url.Add ("channel");
                url.Add (",");
            }

            return BuildRestApiRequest<Uri> (url, PNOperationType.PNHereNowOperation, uuid, ssl, origin, 0, authenticationKey, parameterBuilder.ToString(), pnSdkVersion);
        }


        internal static Uri BuildWhereNowRequest (string uuid, string sessionUUID, bool ssl, string origin, string authenticationKey, string subscribeKey, string pnSdkVersion)
        {
            List<string> url = new List<string> ();

            url.Add ("v2");
            url.Add ("presence");
            url.Add ("sub_key");
            url.Add (subscribeKey);
            url.Add ("uuid");
            url.Add (uuid);

            return BuildRestApiRequest<Uri> (url, PNOperationType.PNWhereNowOperation, sessionUUID, ssl, origin, 0, authenticationKey, "", pnSdkVersion);
        }

        internal static Uri BuildTimeRequest (string uuid, bool ssl, string origin, string pnSdkVersion)
        {
            List<string> url = new List<string> ();

            url.Add ("time");
            url.Add ("0");

            return BuildRestApiRequest<Uri> (url, PNOperationType.PNTimeOperation, uuid, ssl, origin, 0, "", "", pnSdkVersion);
        }

        internal static Uri BuildSetStateRequest (string channel, string channelGroup, string jsonUserState, string uuid,  string sessionUUID, bool ssl, string origin, string authenticationKey, string subscribeKey, string pnSdkVersion)
        {
            StringBuilder paramBuilder = new StringBuilder ();
            paramBuilder.AppendFormat ("?state={0}", Utility.EncodeUricomponent (jsonUserState, PNOperationType.PNSetStateOperation, false, false));
            if (!string.IsNullOrEmpty(channelGroup) && channelGroup.Trim().Length > 0)
            {
                paramBuilder.AppendFormat("&channel-group={0}", Utility.EncodeUricomponent(channelGroup, PNOperationType.PNSetStateOperation, true, false));
            }

            List<string> url = new List<string> ();

            url.Add ("v2");
            url.Add ("presence");
            url.Add ("sub_key");
            url.Add (subscribeKey);
            url.Add ("channel");
            url.Add (string.IsNullOrEmpty(channel) ? "," : channel);
            url.Add ("uuid");
            url.Add (uuid);
            url.Add ("data");

            return BuildRestApiRequest<Uri> (url, PNOperationType.PNSetStateOperation, sessionUUID, ssl, origin, 0, authenticationKey, paramBuilder.ToString (), pnSdkVersion);
        }

        internal static Uri BuildGetStateRequest (string channel, string channelGroup, string uuid, string sessionUUID, bool ssl, string origin, string authenticationKey, string subscribeKey, string pnSdkVersion)
        {
            string parameters = "";
            if (!string.IsNullOrEmpty(channelGroup) && channelGroup.Trim().Length > 0)
            {
                parameters = string.Format("&channel-group={0}", Utility.EncodeUricomponent(channelGroup, PNOperationType.PNGetStateOperation, true, false));
            }

            List<string> url = new List<string> ();

            url.Add ("v2");
            url.Add ("presence");
            url.Add ("sub_key");
            url.Add (subscribeKey);
            url.Add ("channel");
            url.Add (string.IsNullOrEmpty(channel) ? "," : channel);
            url.Add ("uuid");
            url.Add (uuid);

            return BuildRestApiRequest<Uri> (url, PNOperationType.PNGetStateOperation, sessionUUID, ssl, origin, 0, authenticationKey, parameters, pnSdkVersion);
        }

        internal static Uri BuildPresenceHeartbeatRequest (string channels, string channelGroups, string channelsJsonState, string uuid, bool ssl, string origin, string authenticationKey, string subscribeKey, string pnSdkVersion)
        {
            StringBuilder presenceParamBuilder = new StringBuilder ();
            if (channelsJsonState != "{}" && channelsJsonState != "") {
                presenceParamBuilder.AppendFormat("&state={0}", Utility.EncodeUricomponent (channelsJsonState, PNOperationType.PNPresenceHeartbeatOperation, false, false));
            }
            if (channelGroups != null && channelGroups.Length > 0)
            {
                presenceParamBuilder.AppendFormat("&channel-group={0}", Utility.EncodeUricomponent(channelGroups, PNOperationType.PNPresenceHeartbeatOperation, true, false));
            }

            List<string> url = new List<string> ();

            url.Add ("v2");
            url.Add ("presence");
            url.Add ("sub_key");
            url.Add (subscribeKey);
            url.Add ("channel");
            url.Add (string.IsNullOrEmpty(channels) ? "," : channels);
            url.Add ("heartbeat");

            return BuildRestApiRequest<Uri> (url, PNOperationType.PNPresenceHeartbeatOperation, uuid, ssl, origin, 0, authenticationKey, presenceParamBuilder.ToString(), pnSdkVersion);
        }

        internal static Uri BuildLeaveRequest (string channels, string channelGroups, string compiledUserState, string uuid, bool ssl, string origin, string authenticationKey, string subscribeKey, string pnSdkVersion)
        {
            StringBuilder unsubscribeParamBuilder = new StringBuilder ();
            if(!string.IsNullOrEmpty(compiledUserState)){
                unsubscribeParamBuilder.AppendFormat("&state={0}", Utility.EncodeUricomponent(compiledUserState, PNOperationType.PNLeaveOperation, false, false));
            }
            if (channelGroups != null && channelGroups.Length > 0)
            {
                unsubscribeParamBuilder.AppendFormat("&channel-group={0}",  Utility.EncodeUricomponent(channelGroups, PNOperationType.PNLeaveOperation, true, false));
            }

            List<string> url = new List<string> ();

            url.Add ("v2");
            url.Add ("presence");
            url.Add ("sub_key");
            url.Add (subscribeKey);
            url.Add ("channel");
            url.Add (string.IsNullOrEmpty(channels) ? "," : channels);
            url.Add ("leave");

            return BuildRestApiRequest<Uri> (url, PNOperationType.PNLeaveOperation, uuid, ssl, origin, 0, authenticationKey, unsubscribeParamBuilder.ToString(), pnSdkVersion);
        }

        internal static Uri BuildSubscribeRequest (string channels, string channelGroups, string timetoken, string channelsJsonState, string uuid, string region, string filterExpr, bool ssl, string origin, string authenticationKey, string subscribeKey, int presenceHeartbeat, string pnsdkVersion)
        {
            StringBuilder subscribeParamBuilder = new StringBuilder ();
            subscribeParamBuilder.AppendFormat ("&tt={0}", timetoken);

            if (!string.IsNullOrEmpty (filterExpr)) {
                subscribeParamBuilder.AppendFormat ("&filter-expr=({0})",  Utility.EncodeUricomponent(filterExpr, PNOperationType.PNSubscribeOperation, false, false));
            }

            if (!string.IsNullOrEmpty (region)) {
                subscribeParamBuilder.AppendFormat ("&tr={0}", Utility.EncodeUricomponent(region, PNOperationType.PNSubscribeOperation, false, false));
            }

            if (channelsJsonState != "{}" && channelsJsonState != "") {
                subscribeParamBuilder.AppendFormat ("&state={0}", Utility.EncodeUricomponent (channelsJsonState, PNOperationType.PNSubscribeOperation, false, false));
            }

            if (!string.IsNullOrEmpty(channelGroups))
            {
                subscribeParamBuilder.AppendFormat ("&channel-group={0}", Utility.EncodeUricomponent (channelGroups, PNOperationType.PNSubscribeOperation, true, false));
            }                   

            List<string> url = new List<string> ();
            url.Add ("v2");
            url.Add ("subscribe");
            url.Add (subscribeKey);
            url.Add (string.IsNullOrEmpty(channels) ? "," : channels);
            url.Add ("0");

            return BuildRestApiRequest<Uri> (
                url, 
                PNOperationType.PNSubscribeOperation, 
                uuid, 
                ssl, 
                origin, 
                presenceHeartbeat, 
                authenticationKey, 
                subscribeParamBuilder.ToString (),
                pnsdkVersion
            );
        }

        internal static Uri BuildAddChannelsToChannelGroupRequest(string[] channels, string nameSpace, string groupName, string uuid, bool ssl, string origin, string authenticationKey, string subscribeKey, string pnsdkVersion)
        {
            string parameters = string.Format("?add={0}", Utility.EncodeUricomponent(string.Join(",", channels), PNOperationType.PNAddChannelsToGroupOperation, true, false));

            // Build URL
            List<string> url = new List<string>();
            url.Add("v1");
            url.Add("channel-registration");
            url.Add("sub-key");
            url.Add(subscribeKey);
            List<string> ns = Utility.CheckAndAddNameSpace (nameSpace);
            if (ns != null) {
                url.AddRange (ns);    
            }

            url.Add("channel-group");
            url.Add(groupName);

            return BuildRestApiRequest<Uri>(url, PNOperationType.PNAddChannelsToGroupOperation, uuid, ssl, origin, 0, authenticationKey, parameters, pnsdkVersion);
        }

        internal static Uri BuildRemoveChannelsFromChannelGroupRequest(string[] channels, string nameSpace, string groupName, string uuid, bool ssl, string origin, string authenticationKey, string subscribeKey, string pnsdkVersion)
        {
            bool channelsAvailable = false;
            string parameters = "";
            if (channels != null && channels.Length > 0) {
                parameters = string.Format ("?remove={0}", Utility.EncodeUricomponent(string.Join(",", channels), PNOperationType.PNAddChannelsToGroupOperation, true, false));
                channelsAvailable = true;
            }

            // Build URL
            List<string> url = new List<string>();
            url.Add("v1");
            url.Add("channel-registration");
            url.Add("sub-key");
            url.Add(subscribeKey);
            List<string> ns = Utility.CheckAndAddNameSpace (nameSpace);
            if (ns != null) {
                url.AddRange (ns);    
            }
            url.Add("channel-group");
            url.Add(groupName);

            PNOperationType respType = PNOperationType.PNAddChannelsToGroupOperation;
            if (!channelsAvailable) {
                url.Add ("remove");
                respType = PNOperationType.PNRemoveGroupOperation;
            }

            return BuildRestApiRequest<Uri> (url, respType, uuid, ssl, origin, 0, authenticationKey, parameters, pnsdkVersion);
        }

        internal static Uri BuildGetChannelsForChannelGroupRequest(string nameSpace, string groupName, bool limitToChannelGroupScopeOnly, string uuid, bool ssl, string origin, string authenticationKey, string subscribeKey, string pnsdkVersion)
        {
            bool groupNameAvailable = false;
            bool nameSpaceAvailable = false;

            // Build URL
            List<string> url = new List<string>();
            url.Add("v1");
            url.Add("channel-registration");
            url.Add("sub-key");
            url.Add(subscribeKey);
            List<string> ns = Utility.CheckAndAddNameSpace (nameSpace);
            if (ns != null) {
                nameSpaceAvailable = true;
                url.AddRange (ns);    
            }

            if (limitToChannelGroupScopeOnly)
            {
                url.Add("channel-group");
            }
            else
            {
                if (!string.IsNullOrEmpty(groupName) && groupName.Trim().Length > 0)
                {
                    groupNameAvailable = true;
                    url.Add("channel-group");
                    url.Add(groupName);
                }

                if (!nameSpaceAvailable && !groupNameAvailable)
                {
                    url.Add("namespace");
                }
                else if (nameSpaceAvailable && !groupNameAvailable)
                {
                    url.Add("channel-group");
                }
            }

            return BuildRestApiRequest<Uri>(url, PNOperationType.PNChannelsForGroupOperation, uuid, ssl, origin, 0, authenticationKey, "", pnsdkVersion);
        }

        static StringBuilder AddSSLAndEncodeURL<T>(List<string> urlComponents, PNOperationType type, bool ssl, string origin, StringBuilder url)
        {
            // Add http or https based on SSL flag
            if (ssl)
            {
                url.Append("https://");
            }
            else
            {
                url.Append("http://");
            }
            // Add Origin To The Request
            url.Append(origin);
            // Generate URL with UTF-8 Encoding
            for (int componentIndex = 0; componentIndex < urlComponents.Count; componentIndex++)
            {
                url.Append("/");
                if (type == PNOperationType.PNPublishOperation && componentIndex == urlComponents.Count - 1)
                {
                    url.Append(Utility.EncodeUricomponent(urlComponents[componentIndex].ToString(), type, false, false));
                }
                else
                {
                    url.Append(Utility.EncodeUricomponent(urlComponents[componentIndex].ToString(), type, true, false));
                }
            }
            return url;
        }

        private static StringBuilder AppendAuthKeyToURL(StringBuilder url, string authenticationKey, PNOperationType type){
            if (!string.IsNullOrEmpty (authenticationKey)) {
                url.AppendFormat ("&auth={0}", Utility.EncodeUricomponent (authenticationKey, type, false, false));
            }
            return url;
        }

        private static StringBuilder AppendUUIDToURL(StringBuilder url, string uuid, bool firstInQS){
            if (firstInQS)
            {
                url.AppendFormat("?uuid={0}", uuid);
            }
            else
            {
                url.AppendFormat("&uuid={0}", uuid);
            }
            return url;
        }

        private static StringBuilder AppendPresenceHeartbeatToURL(StringBuilder url, int pubnubPresenceHeartbeatInSeconds){
            if (pubnubPresenceHeartbeatInSeconds != 0) {
                url.AppendFormat ("&heartbeat={0}", pubnubPresenceHeartbeatInSeconds);
            }
            return url;
        }

        private static StringBuilder AppendPNSDKVersionToURL(StringBuilder url, string pnsdkVersion, PNOperationType type){
            url.AppendFormat ("&pnsdk={0}", Utility.EncodeUricomponent (pnsdkVersion, type, false, true));
            return url;
        }
        
        private static Uri BuildRestApiRequest<T> (List<string> urlComponents, PNOperationType type, string uuid, bool ssl, string origin, int pubnubPresenceHeartbeatInSeconds, string authenticationKey, string parameters, string pnsdkVersion)
        {
            StringBuilder url = new StringBuilder ();
            uuid = Utility.EncodeUricomponent (uuid, type, false, false);

            url = AddSSLAndEncodeURL<T>(urlComponents, type, ssl, origin, url);

            switch (type) {
            case PNOperationType.PNLeaveOperation:
            case PNOperationType.PNSubscribeOperation:
            case PNOperationType.PNPresenceOperation:

                url = AppendUUIDToURL(url, uuid, true);
                url.Append(parameters);
                url = AppendAuthKeyToURL(url, authenticationKey, type);

                url = AppendPresenceHeartbeatToURL(url, pubnubPresenceHeartbeatInSeconds);
                url = AppendPNSDKVersionToURL(url, pnsdkVersion, type);
                break;

            case PNOperationType.PNPresenceHeartbeatOperation:

                url = AppendUUIDToURL(url, uuid, true);
                url.Append (parameters);
                url = AppendAuthKeyToURL(url, authenticationKey, type);
                url = AppendPNSDKVersionToURL(url, pnsdkVersion, type);
                break;

            case PNOperationType.PNSetStateOperation:

                url.Append (parameters);
                url = AppendUUIDToURL(url, uuid, false);
                url = AppendAuthKeyToURL(url, authenticationKey, type);
                url = AppendPNSDKVersionToURL(url, pnsdkVersion, type);
                break;

            case PNOperationType.PNGetStateOperation:

                url = AppendUUIDToURL(url, uuid, true);
                url.Append (parameters);
                url = AppendAuthKeyToURL(url, authenticationKey, type);
                url = AppendPNSDKVersionToURL(url, pnsdkVersion, type);
                break;
            case PNOperationType.PNHereNowOperation:

                url.Append (parameters);
                url = AppendUUIDToURL(url, uuid, false);
                url = AppendAuthKeyToURL(url, authenticationKey, type);
                url = AppendPNSDKVersionToURL(url, pnsdkVersion, type);
                break;

            case PNOperationType.PNWhereNowOperation:

                url = AppendUUIDToURL(url, uuid, true);
                url = AppendAuthKeyToURL(url, authenticationKey, type);
                url = AppendPNSDKVersionToURL(url, pnsdkVersion, type);
                break;

            case PNOperationType.PNPublishOperation:
            
                url = AppendUUIDToURL(url, uuid, true);
                url.Append (parameters);
                url = AppendAuthKeyToURL(url, authenticationKey, type);
                url = AppendPNSDKVersionToURL(url, pnsdkVersion, type);

                break;
            case PNOperationType.PNPushNotificationEnabledChannelsOperation:
            case PNOperationType.PNAddPushNotificationsOnChannelsOperation:
            case PNOperationType.PNRemoveAllPushNotificationsOperation:
            case PNOperationType.PNRemovePushNotificationsFromChannelsOperation:
            case PNOperationType.PNAddChannelsToGroupOperation:
            case PNOperationType.PNRemoveChannelsFromGroupOperation:
            
                url.Append (parameters);
                url = AppendUUIDToURL (url, uuid, false);
                url = AppendAuthKeyToURL (url, authenticationKey, type);
                url = AppendPNSDKVersionToURL (url, pnsdkVersion, type);

                break;
            
            case PNOperationType.PNChannelGroupsOperation:
            case PNOperationType.PNRemoveGroupOperation:
            case PNOperationType.PNChannelsForGroupOperation:
                url.Append (parameters);
                url = AppendUUIDToURL(url, uuid, true);
                url = AppendAuthKeyToURL(url, authenticationKey, type);
                url = AppendPNSDKVersionToURL(url, pnsdkVersion, type);
                break;
            case PNOperationType.PNHistoryOperation:
            case PNOperationType.PNFetchMessagesOperation:
                url.Append (parameters);
                url = AppendAuthKeyToURL(url, authenticationKey, type);
                url = AppendPNSDKVersionToURL(url, pnsdkVersion, type);
                break;
            default:
                url = AppendUUIDToURL(url, uuid, true);
                url = AppendPNSDKVersionToURL(url, pnsdkVersion, type);
                break;
            }

            Uri requestUri = new Uri (url.ToString ());

            return requestUri;

        }
        #endregion
    }
}
