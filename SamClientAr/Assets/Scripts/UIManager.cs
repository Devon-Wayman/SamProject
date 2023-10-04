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
            tempServerAddress = serverAddressInput.text.Trim();
            //if (!ValidAddressEntry(tempServerAddress))
            //{
            //    return;
            //}
            connectScreen.SetActive(false);
            NetworkManager.Instance.Connect(tempServerAddress);
        }

        //private bool ValidAddressEntry(string checkAddress)
        //{
        //    if (checkAddress.Length == 0)
        //    {
        //        Debug.Log("IP address length is 0. Invalid entry");
        //        return false;
        //    }

        //    IPAddress ip;
        //    bool ValidateIP = IPAddress.TryParse(checkAddress, out ip);
        //    if (ValidateIP)
        //    {
        //        Debug.Log("Valid IP entered");
        //        return true;
        //    }

        //    return false;
        //}

        public void BackToMain()
        {
            connectScreen.SetActive(true);
        }
    }
}
