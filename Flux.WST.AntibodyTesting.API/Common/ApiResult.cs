using System;
using System.Collections.Generic;

namespace Flux.WST.AntibodyTesting.API.Common
{
    class ApiResult<T>
    {
        public T Result { get; set; }
        public Exception Error { get; set; }
    }

    public class ApiResultList<T>
    {
        public List<T> Result { get; set; }
        public Exception Error { get; set; }
    }
}
