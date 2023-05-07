using Core.Application.Interfaces.Services.System;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastruture.Shared.Services
{
    public class ApplicationLogger : IApplicationLogger
    {
        public void Information(string message) => Log.Information(message);

        public void Warning(string message) => Log.Warning(message);

        public void Debug(string message) => Log.Debug(message);

        public void Error(string message) => Log.Error(message);

    }
}
