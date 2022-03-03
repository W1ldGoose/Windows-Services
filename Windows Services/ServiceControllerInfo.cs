using System;
using System.ComponentModel;
using System.Management;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Windows;

namespace Windows_Services
{
    class ServiceControllerInfo : INotifyPropertyChanged
    {
        private readonly ServiceController controller;

        public ServiceControllerInfo(ServiceController controller)
        {
            this.controller = controller;
        }

        public ServiceController Controller
        {
            get => controller;
        }

        public string ServiceName
        {
            get => controller.ServiceName;
        }

        public string DisplayName
        {
            get => controller.DisplayName;
        }

        public string ServiceStatus
        {
            get => controller.Status.ToString();
        }
        public string Account
        {
            get
            {
                ManagementObject wmiService = new ManagementObject("Win32_Service.Name='" + this.ServiceName + "'");
                wmiService.Get();
                string account;
                try
                {
                    account = wmiService["startname"].ToString();
                }
                catch
                {
                    account = "";
                }
                return account;
            }
        }
        public bool CanStart
        {
            get => controller.Status == ServiceControllerStatus.Stopped;
        }
        public bool CanStop
        {
            get => controller.Status == ServiceControllerStatus.Running;
        }
        public void StartService()
        {
            try
            {
                controller.Start();
                controller.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(3));
                OnPropertyChanged("ServiceStatus");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }    
        }
        public void StopService()
        {
            try
            {
                controller.Stop();
                controller.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(3));
                OnPropertyChanged("ServiceStatus");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
