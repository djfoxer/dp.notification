using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace djfoxer.dp.notification.Core.Task
{
    public abstract class MainBackgroundTask : IBackgroundTask
    {
        public abstract void Run(IBackgroundTaskInstance taskInstance);

        
    }
}
