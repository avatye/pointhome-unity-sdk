using System;
using UnityEngine;

namespace Avatye.Pointhome.Util
{
    internal static class AndroidUtil
    {
        private const string NAME = "AndroidUtil";
        private const string ERROR_USER_ID = "-1-1-1-1-1-1-1-1-1-1-1-1-1-1";

        internal static string GetAndroidID()
        {
            string androidID = string.Empty;
            try
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    using (AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver"))
                    using (AndroidJavaClass settingsSecure = new AndroidJavaClass("android.provider.Settings$Secure"))
                    {
                        androidID = settingsSecure.CallStatic<string>("getString", contentResolver, "android_id");
                        LogTracer.I($"{NAME} -> GetAndroidID -> androidID: {androidID}");
                    }
                }
                else
                {
                    androidID = ERROR_USER_ID;
                    LogTracer.E("This method only works on Android platform.");
                }

            }
            catch (Exception e)
            {
                LogTracer.E($"{NAME} -> GetAndroidID error [ {e.Message} ] ");
            }
            return androidID;
        }



        internal static string GetDeviceAdvertisingID()
        {
#if UNITY_ANDROID
            string deviceADID = string.Empty;
            bool isLimitAdTracking = false;
            try
            {
                using AndroidJavaClass unityPlayer = new("com.unity3d.player.UnityPlayer");
                using AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                using AndroidJavaClass advertisingIdClient = new("com.google.android.gms.ads.identifier.AdvertisingIdClient");
                using AndroidJavaObject adInfo = advertisingIdClient.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", currentActivity);
                if (adInfo != null)
                {
                    deviceADID = adInfo.Call<string>("getId");
                    isLimitAdTracking = adInfo.Call<bool>("isLimitAdTrackingEnabled");
                    LogTracer.I($"{NAME} -> GetDeviceAdvertisingID -> deviceADID: {deviceADID}, isLimitAdTracking: {isLimitAdTracking}");
                }
                else
                {
                    LogTracer.W($"{NAME} -> GetDeviceAdvertisingID -> adInfo is null!");
                }
            }
            catch (Exception e)
            {
                LogTracer.E($"{NAME} -> GetDeviceAdvertisingID -> Error: {e.Message}");
            }
            return deviceADID;
#else
    return string.Empty; 
#endif
        }
    }
}
