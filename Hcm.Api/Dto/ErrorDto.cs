using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Text;

namespace Hcm.Api.Dto
{
    public class ErrorDto
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public bool Unhandled { get; set; }

        public ErrorDto()
        {

        }

        public ErrorDto(ModelStateDictionary modelState)
        {
            var stringBuilder = new StringBuilder();
            foreach(var pair in modelState)
            {
                var errors = string.Join(";", pair.Value.Errors.SelectMany(e => e.ErrorMessage).ToArray());
                stringBuilder.AppendLine($"{pair.Key}: {errors}");
            }

            Message = stringBuilder.ToString();
            Error = true;
            Unhandled = false;
        }
    }
}
