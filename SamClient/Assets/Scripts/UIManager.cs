using Riptide;
using SamClient.Networking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace SamClient.Utils
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _singleton;
        public static UIManager Singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(UIManager)} instance already exists, destroying object!");
                    Destroy(value);
                }
            }
        }

        [Header("Connection Setup")]
        [SerializeField] private InputField serverAddressInput;
        [SerializeField] private GameObject connectScreen;

        [Header("Active Session View")]
        [SerializeField] GameObject activeSessionScreen = null;
        [SerializeField] ARSession arSession = null;

        private void Awake()
        {
            Singleton = this;
        }
        private void OnEnable()
        {
            connectScreen.SetActive(true);
            activeSessionScreen.SetActive(false);
        }

        public void OnResetArSession()
        {
            Debug.Log("Resetting AR Session");
            arSession.Reset();
        }

        public void ConnectClicked()
        {
            serverAddressInput.interactable = false;
            connectScreen.SetActive(false);
            NetworkManager.Singleton.Connect(serverAddressInput.text.Trim());
        }

        public void BackToMain()
        {
            activeSessionScreen.SetActive(false);
            serverAddressInput.interactable = true;
            connectScreen.SetActive(true);
        }

        #region Messages
        public void SendName()
        {
            // Activate the Active session view if server requests our name; this ensures we
            // are truly connected and the session can be reset if needed
            activeSessionScreen.SetActive(true);

            Message message = Message.Create(MessageSendMode.Reliable, ClientToServerId.PlayerName);
            message.AddString("UserFace");
            NetworkManager.Singleton.Client.Send(message);
        }
        #endregion
    }
}
