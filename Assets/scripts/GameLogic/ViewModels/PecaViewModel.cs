using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.GameLogic.ViewModels
{
    public class PecaViewModel : MonoBehaviour
    {
        public Peca peca;

        public Text ValorA;
        public Text ValorB;

        public void StartViewModel()
        {
            if (peca != null)
            {
                ValorA.text = peca.ValorA.ToString();
                ValorB.text = peca.ValorB.ToString();
            }
        }

        public void SendPeca()
        {
            NetworkMessage message = new NetworkMessage(DataEvents.PlayerMove, peca);

            GlobalConfigInfo.nodeClient.SendMessage(message, GlobalConfigInfo.CurrentAdversary);
        }
    }
}
