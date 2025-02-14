using System;
using UnityEngine;

namespace Avatye.Pointhome.Util
{
    public static class PointHomeUtil
    {
        private const string NAME = "PointHomeUtil";
        private static readonly Tuple<float, float> DEFAULT_BUTTON_POSITION = new(10f, 10f);

        public delegate void OnSuccessDelegate();
        public delegate void OnFailureDelegate(string error);

        private static AndroidJavaObject pointHomeSlider;

        private enum OpenType
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
        /// <param name="appID">아바티에서 제공한 앱ID</param>
        /// <param name="appSecret">아바티에서 제공한 앱Secret</param>
        /// <param name="appSecret">아바티에서 제공한 앱Secret</param>        
        /// <param name="onSuccess">초기화 성공 콜백</param>        
        /// <param name="onFailure">초기화 실패 콜백</param>            
        public static void Init(string appID, string appSecret, OnSuccessDelegate onSuccess, OnFailureDelegate onFailure)
        {
            LogTracer.I($"{NAME} -> Init [ appID: {appID}, appSecret, {appSecret} ]");

            using AndroidJavaClass unityPlayer = new("com.unity3d.player.UnityPlayer");
            using AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using AndroidJavaClass pointHomeSDK = new("com.avatye.pointhome.PointHomeSDK");
            IInitListener listener = new(onSuccess, onFailure);

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
        public class IInitListener : AndroidJavaProxy
        {
            private OnSuccessDelegate _onInitSuccess;
            private OnFailureDelegate _onInitFailure;
            public IInitListener(OnSuccessDelegate onSuccess, OnFailureDelegate onFailure) : base("com.avatye.pointhome.PointHomeSDK$InitializationListener")
            {

            }
            public void onSuccess()
            {
                LogTracer.I("PointHomeSDK: Initialization Success!");
                _onInitSuccess?.Invoke();
            }

            // 실패 콜백
            public void onFailure(AndroidJavaObject error)
            {
                string errorMessage = error.Call<string>("toString");
                LogTracer.E($"PointHomeSDK: Initialization onFailure {errorMessage}");
                _onInitFailure?.Invoke(errorMessage);
            }


        }


        /** 포인트홈 빌더생성  */
        /// <param name="userID">앱사에서 사용하고 있는 유저 키(고유키로만) </param>
        /// <param name="sliderHeight">포인트홈 열리는 슬라이더 형태</param>        
        public static void MakePointHomeBuilder(string userID, OpenSliderHeight sliderHeight, OnSuccessDelegate onSuccess, OnFailureDelegate onFailure)
        {
            LogTracer.I($"{NAME} -> MakePointHomeBuilder [ userID: {userID}, sliderHeight: {((int)sliderHeight)} ]");
            using AndroidJavaClass unityPlayer = new("com.unity3d.player.UnityPlayer");
            using AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using AndroidJavaObject pointHomeService = new("com.avatye.pointhome.builder.PointHomeService");

            string reqData = $"{{ \"openType\": \"{OpenType.Bottom.ToString().ToLower()}\", \"userID\": \"{userID}\", \"sliderHeight\": \"{(int)sliderHeight}\" }}";
            LogTracer.I($"{NAME} -> MakePointHomeBuilder ->   {reqData}  ");

            pointHomeService.CallStatic(
              "pointHomeUnity",
              currentActivity,
              reqData,
              new PointHomeBuilderCallback(onSuccess, onFailure)
          );
        }


        /** 포인트홈 플로팅 버튼 빌더생성  */
        /// <param name="userID"> 앱사에서 사용하고 있는 유저 키(고유키로만) </param>
        /// <param name="sliderHeight">포인트홈 열리는 슬라이더 형태</param>        
        /// <param name="buttonPosition">플로팅 버튼 위치값(x,y) </param>        
        public static void MakePointHomeFloatingBuilder(string userID, OpenSliderHeight sliderHeight, OnSuccessDelegate onSuccess, OnFailureDelegate onFailure, Tuple<float, float> buttonPosition = null)
        {
            LogTracer.I($"{NAME} -> MakePointHomeBuilder [ userID: {userID}, sliderHeight: {((int)sliderHeight)}, buttonPositionX: {buttonPosition.Item1}, buttonPositionY: {buttonPosition.Item2} ]");
            using AndroidJavaClass unityPlayer = new("com.unity3d.player.UnityPlayer");
            using AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using AndroidJavaObject pointHomeService = new("com.avatye.pointhome.builder.PointHomeService");
            float posX = buttonPosition?.Item1 ?? DEFAULT_BUTTON_POSITION.Item1;
            float posY = buttonPosition?.Item2 ?? DEFAULT_BUTTON_POSITION.Item2;

            string reqData = $"{{ \"openType\": \"{OpenType.Float.ToString().ToLower()}\", \"userID\": \"{userID}\", \"sliderHeight\": \"{(int)sliderHeight}\", \"buttonPositionX\": \"{posX}\", \"buttonPositionY\": \"{posY}\" }}";
            LogTracer.I($"{NAME} -> MakePointHomeBuilder ->   {reqData}  ");

            pointHomeService.CallStatic(
              "pointHomeUnity",
              currentActivity,
              reqData,
              new PointHomeBuilderCallback(onSuccess, onFailure)
          );
        }


        public class PointHomeBuilderCallback : AndroidJavaProxy
        {
            private OnSuccessDelegate _onBuildSuccess;
            private OnFailureDelegate _onBuildFailure;
            public PointHomeBuilderCallback(OnSuccessDelegate onSuccess, OnFailureDelegate onFailure)
                : base("com.avatye.pointhome.builder.IBuilderCallback")
            {
                _onBuildSuccess = onSuccess;
                _onBuildFailure = onFailure;
            }

            public void onBuildCompleted(AndroidJavaObject builder)
            {
                LogTracer.I($"{NAME} -> onBuildCompleted -> PointHome 초기화 성공!");
                pointHomeSlider = builder;
                _onBuildSuccess.Invoke();
            }

            public void onBuildFailed(AndroidJavaObject error)
            {
                string errorMessage = error.Call<string>("toString");
                LogTracer.E($"{NAME} -> onBuildFailed -> PointHome 초기화 실패: {errorMessage}");
                _onBuildFailure.Invoke(errorMessage);
            }
        }


        /** 포인트홈 실행 */
        public static void Open()
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
