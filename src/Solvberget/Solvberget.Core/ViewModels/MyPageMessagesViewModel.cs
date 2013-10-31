using System;
using System.Collections.Generic;
using Solvberget.Core.DTOs.Deprecated.DTO;
using Solvberget.Core.Services;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class MyPageMessagesViewModel : BaseViewModel
    {
        public MyPageMessagesViewModel(IUserService service)
        {
            if (service == null) throw new ArgumentNullException("service");

            //Notifications = service.GetUserNotifications("id");
        }

        private List<Notification> _notifications;
        public List<Notification> Notifications
        {
            get { return _notifications; }
            set { _notifications = value; RaisePropertyChanged(() => Notifications); }
        } 
    }
}
