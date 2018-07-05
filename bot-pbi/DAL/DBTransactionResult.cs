using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bot_pbi.DAL
{
    public class DBTransactionResult
    {
        const string EXMSG = "An unexpected exception was generated, see DBTransactionResult.Ex field";

        public bool Success { get; set; } = true;
        public Exception Ex { get; set; }
        public string Message { get; set; }

        public void SetException(Exception ex)
        {
            Message = EXMSG;
            Ex = ex;
            Success = false;
        }
    }
}