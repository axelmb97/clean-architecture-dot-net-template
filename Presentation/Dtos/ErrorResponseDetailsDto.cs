using Newtonsoft.Json;

namespace Presentation.Dtos
{
    public class ErrorResponseDetailsDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
