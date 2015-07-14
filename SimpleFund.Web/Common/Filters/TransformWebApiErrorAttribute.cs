using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using Autofac.Integration.WebApi;
using log4net;
using SimpleFund.Domain;

namespace SimpleFund.Web.Common.Filters
{
    public class TransformWebApiErrorAttribute : ExceptionFilterAttribute, IAutofacExceptionFilter
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransformWebApiErrorAttribute));

        public override void OnException(HttpActionExecutedContext context)
        {
            var excepton = context.Exception as BusinessException;
            if (excepton == null)
            {
                Logger.Error(context.Exception);
                excepton = new BusinessException("System error.");
            }

            var error = new HttpError(excepton.Message);
            error["Level"] = excepton.Level.ToString().ToLower();

            var responseMessage = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            throw new HttpResponseException(responseMessage);
        }
    }
}