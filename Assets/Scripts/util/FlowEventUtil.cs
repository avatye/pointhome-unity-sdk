using UnityEngine.Events;

namespace Avatye.Pointhome.Util
{
    internal class FlowEventUtil
    {

        private static UnityEvent<EventData> OnEventHandler = new();
        // Event-Send
        internal static void Post(string key, string message)
        {
            OnEventHandler?.Invoke(new EventData(key, message));
        }

        // Event-Object-Send
        internal static void Post(string key, object obj)
        {
            OnEventHandler?.Invoke(new EventData(key, obj));
        }

        // Event-Register
        internal static void RegisterEvent(IEventObserver observer)
        {
            OnEventHandler.AddListener(observer.onEventObserved);
        }

        // Event-UnRegister
        internal static void UnRegisterEvent(IEventObserver observer)
        {
            OnEventHandler.RemoveListener(observer.onEventObserved);
        }

        // Event-Observer
        internal interface IEventObserver
        {
            void onEventObserved(EventData eventData);
        }



        internal struct EventData
        {
            internal string key;
            internal object message;


            internal EventData(string key, object message)
            {
                this.key = key;
                this.message = message;
            }


            public override string ToString()
            {
                return $"Key: {key}, Message: {message}";
            }
        }


        // Event Key
        internal const string WEB_MESSAGE_INITIALIZE = "web-message:initialize";
        internal const string WEB_MESSAGE_OPEN_ADVERTISE_INTERSTITIAL = "web-message:open-advertise-interstitial";
        internal const string WEB_MESSAGE_OPEN_ADVERTISE_INTERSTITIAL_REWARD = "web-message:open-advertise-interstitialReward";
        internal const string WEB_MESSAGE_OPEN_ADVERTISE_NATIVE = "web-message:open-advertise-native";
        internal const string WEB_MESSAGE_OPEN_ADVERTISE_BANNER = "web-message:open-advertise-banner";
        internal const string WEB_MESSAGE_OPEN_BROWSER_EXTERNAL = "web-message:open-browser-external";
        internal const string WEB_MESSAGE_OPEN_BROWSER_INTERNAL = "web-message:open-browser-internal";
        internal const string WEB_MESSAGE_OPEN_SETTING = "web-message:open-setting";
        internal const string WEB_MESSAGE_AUTH_TOKEN_REQUEST = "web-message:auth-token-request";
        internal const string WEB_MESSAGE_AUTH_TOKEN_EXPIRE = "web-message:auth-token-expire";
        internal const string WEB_MESSAGE_AUTH_SIGN_IN = "web-message:auth-signin";
        internal const string WEB_MESSAGE_AUTH_SIGN_UP = "web-message:auth-signup";
        internal const string WEB_MESSAGE_AUTH_WITHDRAW = "web-message:auth-withdraw";
        internal const string WEB_MESSAGE_CLOSE_MAIN = "web-message:close-main";
        internal const string WEB_MESSAGE_CLOSE_ADVERTISE_NATIVE = "web-message:close-advertise-native";
        internal const string WEB_MESSAGE_CLOSE_ADVERTISE_BANNER = "web-message:close-advertise-banner";
        internal const string WEB_MESSAGE_REQUEST_ADVERTISE_NATIVE = "web-message:request-advertise-native";
        internal const string WEB_MESSAGE_REQUEST_ADID = "web-message:request-adid";
        internal const string WEB_MESSAGE_REQUEST_STORAGE_GET = "web-message:request-storage-get";
        internal const string WEB_MESSAGE_REQUEST_STORAGE_GET_STRING = "web-message:request-storage-get-string";
        internal const string WEB_MESSAGE_REQUEST_STORAGE_SET = "web-message:request-storage-set";
        internal const string WEB_MESSAGE_REQUEST_STORAGE_DELETE = "web-message:request-storage-delete";
        internal const string WEB_MESSAGE_REQUEST_EXCHANGE_CASH = "web-message:request-exchange-cash";
        internal const string WEB_MESSAGE_REQUEST_ACCEPT_USER = "web-message:request-accept-user";
        internal const string WEB_MESSAGE_EVENT_WIDGET = "web-message:event-widget";

    }
}
