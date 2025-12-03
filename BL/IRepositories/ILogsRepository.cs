using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IRepositories
{
    public interface ILogsRepository
    {
        public void Log(int level, string message);
        IEnumerable<Log> GetLogs(int n);
        int GetCount();
    }
}
