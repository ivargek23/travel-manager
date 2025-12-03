using AutoMapper;
using BL.IRepositories;
using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class LogsRepository : ILogsRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        public LogsRepository(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public int GetCount()
        {
            return _context.Logs.Count();
        }

        public IEnumerable<Log> GetLogs(int n)
        {
            var logs = _context.Logs.OrderByDescending(x => x.Id).Take(n).ToList();

            var mappedLogs = _mapper.Map<IEnumerable<Log>>(logs);

            return mappedLogs;
        }

        public void Log(int level, string message)
        {
            Log log = new Log
            {
                LogLevel = level,
                Message = message,
                Timestamp = DateTime.Now
            };

            _context.Logs.Add(log);
            _context.SaveChanges();
        }
    }
}
