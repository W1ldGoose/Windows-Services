using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Windows_Services
{
    class ViewModel : INotifyPropertyChanged
    {
        private ServiceControllerInfo selectedService;

        public ObservableCollection<ServiceControllerInfo> Services
        {
            get;
            set;
        }

        private BaseCommand startServiceCommand;
        private BaseCommand stopServiceCommand;


        public ViewModel()
        {
            Services = new ObservableCollection<ServiceControllerInfo>();
            foreach (var sc in ServiceController.GetServices())
            {
                Services.Add(new ServiceControllerInfo(sc));
            }
        }
        public ServiceControllerInfo SelectedService
        {
            get => selectedService;
            set
            {
                selectedService = value;
                OnPropertyChanged("SelectedService");
            }
        }

        public BaseCommand StartServiceCommand
        {
            get
            {
                return startServiceCommand ??
                    (startServiceCommand = new BaseCommand(obj =>
                    {
                        var selectStartService = obj as ServiceControllerInfo;
                        selectStartService.StartService();
                    },
                    (canFlag) => SelectedService.CanStart));

            }
        }
        public BaseCommand StopServiceCommand
        {
            get
            {
                return stopServiceCommand ??
                    (stopServiceCommand = new BaseCommand(obj =>
                    {
                        var selectStopService = obj as ServiceControllerInfo;
                        if (selectStopService != null)
                        {
                            selectStopService.StopService();
                        }
                    },
                    (canFlag) => SelectedService.CanStop));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
