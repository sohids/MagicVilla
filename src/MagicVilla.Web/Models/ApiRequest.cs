using static MagicVilla.Utility.StaticDetails;

namespace MagicVilla.Web.Models
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.Get;
        public string Url { get; set; }
        public object Data { get; set; }
    }
}
