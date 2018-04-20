using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Polly.DownloadService
{
    public partial class PollyScheduleCheckService : ServiceBase
    {
        public PollyScheduleCheckService() => InitializeComponent();

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
