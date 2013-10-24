using System;
using System.Collections.Generic;
using Cirrious.CrossCore;
using Solvberget.Core.Services;
using Solvberget.Core.ViewModels.Base;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.ViewModels
{
    public class MyPageMessagesViewModel : BaseViewModel
    {
        public MyPageMessagesViewModel()
        {
            var service = Mvx.Resolve<IUserInformationService>();
            if (service == null) throw new ArgumentNullException("service");

            Notifications = service.GetUserNotifications("id");
            var tall = 6;
        }

        private List<Notification> _notifications;
        public List<Notification> Notifications
        {
            get { return _notifications; }
            set { _notifications = value; RaisePropertyChanged(() => Notifications); }
        } 
    }
}
