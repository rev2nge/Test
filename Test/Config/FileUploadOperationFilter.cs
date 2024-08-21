using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Test.Config
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParameters = context.ApiDescription.ParameterDescriptions
            .Where(p => p.ModelMetadata?.ContainerType == typeof(IFormFile) ||
                        p.ModelMetadata?.ContainerType == typeof(IEnumerable<IFormFile>))
            .ToList();

            if (fileParameters.Any())
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = fileParameters.ToDictionary(p => p.Name, p => new OpenApiSchema
                            {
                                Type = "string",
                                Format = "binary"
                            }),
                            Required = fileParameters.Select(p => p.Name).ToHashSet()
                        }
                    }
                }
                };
            }
        }
    }
}