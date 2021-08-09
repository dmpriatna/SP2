using System.Net;

namespace SP2.Models
{
    public class BaseResponse<T>
    {
        public HttpStatusCode Status { get; set; }
        private string _message;
        public string Message
        {
            get
            {
                return Status == HttpStatusCode.OK ? "Success" : _message;
            }
            set
            {
                _message = value;
            }
        }
        public T Data { get; set; }
    }
}