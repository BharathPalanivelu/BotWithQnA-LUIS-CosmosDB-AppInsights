using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bot_pbi.DAL
{
    public interface IDBManager : IDisposable
    {
        DBTransactionResult AddData(object info);
    }
}