using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WPFThreadingModel.DataTypes;

namespace WPFThreadingModel.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        private ObservableCollection<MyTaskSpecification> _tasks = new ObservableCollection<MyTaskSpecification>();

        public ICommand CommandRunTaskInMainThread { get; private set; }
        public ICommand CommandRunEachTaskInSeperateThread { get; private set; }

        /// <summary>
        /// Do not use time consuming action in the thread associated with the target window. 
        /// </summary>
        public MainViewModel()
        {
            _tasks = new ObservableCollection<MyTaskSpecification>();
            _tasks.Add(new MyTaskSpecification() { TaskId = 0, ThreadId = 0, Progress = 0 });

            CommandRunTaskInMainThread = new DelegateCommand(() =>
            {
                MyTaskSpecification task = new MyTaskSpecification();
                task.TaskId = Task.CurrentId.GetValueOrDefault();
                task.ThreadId = Thread.CurrentThread.ManagedThreadId;
                _tasks.Add(task);
                for (int stage = 0; stage < 10000; stage++)
                {
                    if ((stage + 1) % 1000 == 0)
                        task.Progress = stage;
                    Random random = new Random();
                    Thread.Sleep(random.Next(24));
                }
            });

            CommandRunEachTaskInSeperateThread = new DelegateCommand(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Task t = Task.Factory.StartNew(() =>
                    {
                        MyTaskSpecification task = new MyTaskSpecification();
                        task.TaskId = Task.CurrentId.Value;
                        task.ThreadId = Thread.CurrentThread.ManagedThreadId;
                        System.Windows.Application.Current.Dispatcher.Invoke(() => Tasks.Add(task));

                        for (int stage = 0; stage < 10000; stage++)
                        {
                            task.Progress = stage;
                            // todo your own code
                            Random random = new Random();
                            Thread.Sleep(random.Next(50));
                        }
                    });
                }
            });
        }

        public ObservableCollection<MyTaskSpecification> Tasks
        {
            get { return _tasks; }
        }
    }
}
