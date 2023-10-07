using Riptide;
using SamClient.Networking;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField] private InputField serverAddressInput;
        [SerializeField] private GameObject connectScreen;

        private void Awake()
        {
            Singleton = this;
        }

        public void ConnectClicked()
        {
            serverAddressInput.interactable = false;
            connectScreen.SetActive(false);
            NetworkManager.Singleton.Connect(serverAddressInput.text.Trim());
        }

        public void BackToMain()
        {
            serverAddressInput.interactable = true;
            connectScreen.SetActive(true);
        }

        #region Messages
        public void SendName()
        {
            Message message = Message.Create(MessageSendMode.Reliable, ClientToServerId.PlayerName);
            message.AddString("UserFace");
            NetworkManager.Singleton.Client.Send(message);
        }
        #endregion
    }
}
