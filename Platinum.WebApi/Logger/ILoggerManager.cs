using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platinum.WebApi.Logger
{
  public interface ILoggerManager
    {
        void Information(string message);
        void Warning(string message);
        void Debug(string message);
        void Error(string message);
    }
}
