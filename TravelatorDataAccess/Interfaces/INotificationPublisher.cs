using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelatorDataAccess.Interfaces
{
    public interface INotificationPublisher
    {
        void PublishNotification(Guid userId, string message, string queue);
    }
}
