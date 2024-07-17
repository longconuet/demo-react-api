using DemoReactAPI.Enums;

namespace DemoReactAPI.Dtos
{
    public class ResponseDto
    {
        public ResponseStatusEnum Status { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }

    public class ResponseDto<T>
    {
        public ResponseStatusEnum Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
