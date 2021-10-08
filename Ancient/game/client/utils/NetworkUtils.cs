using ancient.game.client.network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.utils
{
    public class NetworkUtils
    {
        public static bool TryConnect(string ipAddress)
        {
            string[] splitedAdress = ipAddress.Split(':');

            if (splitedAdress.Length > 2 || splitedAdress.Length == 0)
                return false;

            string ip = splitedAdress[0];
            int port = 15050;

            if (splitedAdress.Length == 2)
            {
                bool parsed = int.TryParse(splitedAdress[1], out port);

                if (!parsed)
                    return false;
            }

            return NetClient.instance.TryConnect(ip, port);
        }
    }
}
