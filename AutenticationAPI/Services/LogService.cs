using AutenticationAPI.Contexts;
using AutenticationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutenticationAPI.Services
{
    public class LogService : ILogService
    {

        #region Variables

        private readonly ApplicationDbContext _context;

        #endregion

        #region Ctro

        public LogService(ApplicationDbContext context)
        {
            this._context = context;
        }


        #endregion

        #region Methods

        public async Task LogMessage(string _message)
        {
            Log log = new Log() { message = _message };
            _context.Log.Add(log);
            await _context.SaveChangesAsync();
        }
        #endregion



    }
}
