using System;
using Avatye.Pointhome.Util;
using UnityEngine;

namespace Avatye.Pointhome.Public
{
    public class MainSceneObj : MonoBehaviour
    {
        private const string NAME = "MainSceneObject";

        private void Start()
        {
            Debug.Log($"{NAME} -> Start");
            LogTracer.SetLogEnabled(true);
            InitPointHome();
        }

        public void OnButtonClick()
        {
            LogTracer.I($"{NAME} -> onButtonClick");
            OpenPointHome();
        }

        private void InitPointHome()
        {
            LogTracer.I($"{NAME} -> InitPointHome");
            // Init
            /** 캐시버튼 */
            // a0a6481585e04929aae8676437406cb8
            // 17293bec4d454bdf
            /** 캐시버튼 채널링 */
            // 4105ffef52734d8c83d0ae3078124c3f
            // 2179976d2a7648e3
            PointHomeUtil.Init(
                appID: "a0a6481585e04929aae8676437406cb8",
                appSecret: "17293bec4d454bdf",
                onSuccess: () =>
                {
                    LogTracer.I($"{NAME} -> InitPointHome -> Init::onSuccess");
                },
                onFailure: (error) =>
                {
                    LogTracer.E($"{NAME} -> InitPointHome -> Init.onFailure {error}");
                }
            );
        }
        private void OpenPointHome()
        {
            LogTracer.I($"{NAME} -> OpenPointHome()");
                        
            // PointHomeUtil.MakePointHomeBuilder(
            //     userID: null,
            //     sliderHeight: PointHomeUtil.OpenSliderHeight.Full,
            //     onSuccess: () =>
            //     {
            //         LogTracer.I($"{NAME} -> OpenPointHome -> MakePointHomeBuilder::onSuccess");
            //     },
            //     onFailure: (error) =>
            //     {
            //         LogTracer.E($"{NAME} -> OpenPointHome -> MakePointHomeBuilder::onFailure {error}");
            //     }
            // );

            PointHomeUtil.MakePointHomeFloatingBuilder(
                userID: null,
                sliderHeight: PointHomeUtil.OpenSliderHeight.Full,
                buttonPosition: new Tuple<float, float>(10f, 10f),
                 onSuccess: () =>
                {
                    LogTracer.I($"{NAME} -> OpenPointHome -> MakePointHomeFloatingBuilder::onSuccess");
                },
                onFailure: (error) =>
                {
                    LogTracer.E($"{NAME} -> OpenPointHome -> MakePointHomeFloatingBuilder::onFailure {error}");
                }
            );

        }
    }

}