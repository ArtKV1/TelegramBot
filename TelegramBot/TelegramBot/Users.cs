using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    public static class Users
    {
        public static Dictionary<long, List<int>> MessageToDelete = new Dictionary<long, List<int>>();
    }
}
