using System.Collections.Generic;
using UnityEngine;

namespace SamClient.PlayerAssets
{
    public class Player : MonoBehaviour
    {
        public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

        [SerializeField] private ushort id;
        [SerializeField] private string username;

        private void OnDestroy()
        {
            list.Remove(id);
        }
    }
}
