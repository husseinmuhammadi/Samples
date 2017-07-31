using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFThreadingModel.ViewModel;


namespace WPFThreadingModel.DataTypes
{
    class MyTaskSpecification : ViewModelBase
    {
        private int _taskId;
        private int _threadId;
        private int _progress;

        public ICommand CommandStopTask { get; private set; }

        public MyTaskSpecification()
        {
            CommandStopTask = new DelegateCommand(() => { });
        }

        public int TaskId
        {
            get { return _taskId; }
            set
            {
                _taskId = value;
                NotifyPropertyChanged("TaskId");
            }
        }

        public int ThreadId
        {
            get { return _threadId; }
            set
            {
                _threadId = value;
                NotifyPropertyChanged("ThreadId");
            }
        }

        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                NotifyPropertyChanged("Progress");
                NotifyPropertyChanged("Percent");
            }
        }

        public int Percent
        {
            get { return (_progress + 1) / 100; }
        }
    }
}
