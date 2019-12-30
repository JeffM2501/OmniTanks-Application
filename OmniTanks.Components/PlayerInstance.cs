using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniTanks.Components
{
    public class PlayerInstance : TankInstance
    {
        public string UserID = string.Empty;
        public string PlayerName = string.Empty;

        public string Avatar = string.Empty;
        public float RespawnTime = 0;
    }
}
