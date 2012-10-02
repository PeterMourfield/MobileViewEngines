using System.IO;
using System.Web.Hosting;

namespace MobileViewEngines.MVC3
{
    sealed class UserAgentWorkerRequest : SimpleWorkerRequest
    {
        private readonly string userAgent;

        public UserAgentWorkerRequest(string userAgent)
            : base(string.Empty, string.Empty, new StringWriter())
        {
            this.userAgent = userAgent;
        }

        public override string GetKnownRequestHeader(int index)
        {
            return (index != 0x27) ? null : this.userAgent;
        }
    }
}
