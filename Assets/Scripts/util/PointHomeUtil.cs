using System;
using UnityEngine;

namespace Avatye.Pointhome.Util
{
    internal static class PointHomeUtil
    {
        private const string NAME = "PointHomeUtil";

        public delegate void OnSuccessDelegate();
        public delegate void OnFailureDelegate(string error);

        public static event OnSuccessDelegate D_OnSuccess;
        public static event OnFailureDelegate D_OnFailure;

        private static AndroidJavaObject pointHomeSlider;

        public enum OpenType
        {
            Bottom,
            Float
        }

        public enum OpenSliderHeight
        {
            Default = 0,
            Full = 1
        }




        /** 포인트홈 초기화 */
        internal static void Init(string appID, string appSecret, OnSuccessDelegate onSuccess, OnFailureDelegate onFailure)
        {
            LogTracer.I($"{NAME} -> Init [ appID: {appID}, appSecret, {appSecret} ]");
            D_OnSuccess = onSuccess;
            D_OnFailure = onFailure;

            using AndroidJavaClass unityPlayer = new("com.unity3d.player.UnityPlayer");
            using AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using AndroidJavaClass pointHomeSDK = new("com.avatye.pointhome.PointHomeSDK");
            IInitListener listener = new();

            pointHomeSDK.CallStatic(
                "initializer",
                activity.Call<AndroidJavaObject>("getApplication"),
                appID,
                appSecret,
                listener
            );
            // 로그 추적 사용 설정
            pointHomeSDK.CallStatic<AndroidJavaObject>("setLogTrace", true);
            // 개발 모드 설정
            pointHomeSDK.CallStatic<AndroidJavaObject>("setDevelopMode", false);
            // 외부 토큰 사용 여부 추가
            pointHomeSDK.CallStatic<AndroidJavaObject>("useExternalToken", false);

        }
        private class IInitListener : AndroidJavaProxy
        {
            public IInitListener() : base("com.avatye.pointhome.PointHomeSDK$InitializationListener") { }
            public void onSuccess()
            {
                LogTracer.I("PointHomeSDK: Initialization Success!");
                D_OnSuccess?.Invoke();
            }

            // 실패 콜백
            public void onFailure(AndroidJavaObject error)
            {
                string errorMessage = error.Call<string>("toString");
                LogTracer.E($"PointHomeSDK: Initialization onFailure {errorMessage}");
                D_OnFailure?.Invoke(errorMessage);
            }

            public static implicit operator AndroidJavaObject(IInitListener v)
            {
                throw new NotImplementedException();
            }
        }


        /** 포인트홈 빌더생성 */
        internal static void MakePointHomeBuilder(OpenType openType, string userID, OpenSliderHeight sliderHeight, (float, float) buttonPosition)
        {
            LogTracer.I($"{NAME} -> MakePointHomeBuilder [ openType: {openType.ToString().ToLower()}, userID: {userID}, sliderHeight: {((int)sliderHeight)}, buttonPositionX: {buttonPosition.Item1}, buttonPositionY: {buttonPosition.Item2} ]");
            using AndroidJavaClass unityPlayer = new("com.unity3d.player.UnityPlayer");
            using AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using AndroidJavaObject pointHomeService = new("com.avatye.pointhome.builder.PointHomeService");


            float posX = buttonPosition.Item1;
            float posY = buttonPosition.Item2;

            string reqData = $"{{ \"openType\": \"{openType.ToString().ToLower()}\", \"userID\": \"{userID}\", \"sliderHeight\": \"{(int)sliderHeight}\", \"buttonPositionX\": \"{posX}\", \"buttonPositionY\": \"{posY}\" }}";
            LogTracer.I($"{NAME} -> MakePointHomeBuilder ->   {reqData}  ");

            pointHomeService.CallStatic(
              "pointHomeUnity",
              currentActivity,
              reqData,
              new PointHomeBuilderCallback()
          );


            // pointHomeService.CallStatic(
            //     "pointHomeUnity",
            //     currentActivity,
            //     openType.ToString().ToLower(),              // bottom: 포인트홈 view 형태 // float : 포인트홈 플로팅 버튼형태
            //     userID,                                     // 유저ID ? 채널링 : 일반
            //     (int)sliderHeight,                          // 0 : Default크기, 1: Full 크기
            //     posXObj,                                    // 플로팅 버튼 x 좌표 
            //     posYObj,                                    // 플로팅 버튼 y 좌표                
            //     new PointHomeBuilderCallback()
            // );


            // pointHomeService.CallStatic(
            //     "pointHomeBuilder",
            //     currentActivity,
            //     userID,                
            //     new PointHomeBuilderCallback()
            // );
        }


        internal static void MakePointHomeFloatBuilder(string userID)
        {
            LogTracer.I($"{NAME} -> MakeBuilder [ userID: {userID}]");
            using AndroidJavaClass unityPlayer = new("com.unity3d.player.UnityPlayer");
            using AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using AndroidJavaObject pointHomeService = new("com.avatye.pointhome.builder.PointHomeService");
            pointHomeService.CallStatic(
                "pointHomeFloatBuilder",
                currentActivity,
                null, // 플로팅버튼의 초기 위치설정
                userID,
                new PointHomeBuilderCallback()
            );
        }

        private class PointHomeBuilderCallback : AndroidJavaProxy
        {
            public PointHomeBuilderCallback()
                : base("com.avatye.pointhome.builder.IBuilderCallback") { }

            public void onBuildCompleted(AndroidJavaObject builder)
            {
                LogTracer.I($"{NAME} -> onBuildCompleted -> PointHome 초기화 성공!");
                pointHomeSlider = builder;
                Open();
            }

            public void onBuildFailed(AndroidJavaObject error)
            {
                string errorMessage = error.Call<string>("toString");
                LogTracer.E($"{NAME} -> onBuildFailed -> PointHome 초기화 실패: {errorMessage}");
            }
        }

        /** 포인트홈 실행 */
        private static void Open()
        {
            LogTracer.I($"{NAME} -> Open ");
            if (pointHomeSlider != null)
            {
                LogTracer.I($"{NAME} -> Pointhome Open");
                pointHomeSlider.Call("dashboardOpen");
            }
            else
            {
                LogTracer.E($"{NAME} -> Pointhome OpenFailed");
            }
        }

    }
}
