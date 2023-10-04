using UnityEngine;
using UnityEngine.UI;

namespace Riptide.Demos.DedicatedClient
{
    public class UIManager : DevSingleton<UIManager>
    {
        [SerializeField] private InputField serverAddressInput;
        [SerializeField] private GameObject connectScreen;

        string tempServerAddress = string.Empty;

        public void ConnectClicked()
        {
            tempServerAddress = serverAddressInput.text;
            connectScreen.SetActive(false);
            NetworkManager.Instance.Connect(tempServerAddress);
        }

        public void BackToMain()
        {
            connectScreen.SetActive(true);
        }

        #region Messages
        public void SendName()
        {
            Message message = Message.Create(MessageSendMode.Reliable, ClientToServerId.PlayerName);
            message.AddString("ClientArFace");
            NetworkManager.Instance.Client.Send(message);
        }
        #endregion
    }
}
