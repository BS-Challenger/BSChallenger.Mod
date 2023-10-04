using SiraUtil.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSChallenger.API.Web
{
    public class ErrorResponseModel
    {
        public ErrorResponseModel(IHttpResponse message)
        {
            Message = message;
        }

		public IHttpResponse Message { get; set; }
    }
}
