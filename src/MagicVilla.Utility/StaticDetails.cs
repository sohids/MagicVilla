using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicVilla.Utility
{
    public static class StaticDetails
    {
        public enum ApiType
        {
            Get,
            Post,
            Put,
            Delete
        }

        public static string SessionToken = "JWTToken";
    }
}
