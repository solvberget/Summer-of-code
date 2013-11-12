using System.Collections.Generic;
using System.Linq;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class MyPageMessagesViewModel : BaseViewModel
    {
        private readonly IUserService _service;
        private readonly IUserAuthenticationDataService _userAuthenticationService;

        public MyPageMessagesViewModel(IUserService service, IUserAuthenticationDataService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
            _service = service;
            Load();
        }

        private List<NotificationDto> _notifications;
        public List<NotificationDto> Notifications
        {
            get { return _notifications; }
            set { _notifications = value; RaisePropertyChanged(() => Notifications); }
        }

        public async void Load()
        {
            var user = await _service.GetUserInformation(_userAuthenticationService.GetUserId());

            Notifications = user.Notifications == null ? new List<NotificationDto>() : user.Notifications.ToList();

            if (Notifications.Count == 0)
            {
                Notifications = new List<NotificationDto>
                {
                    new NotificationDto
                    {
                        Title = "Du har ingen meldinger",
                        Content = "Du får beskjed når lån forfaller, når noe er klart til henting, og når du får et gebyr"
                    }
                };
            }
            IsLoading = false;
        }
    }
}
