using System;
using System.Collections.Concurrent;
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
        private ObservableCollection<MyTaskSpecification> _taskSpecifications = new ObservableCollection<MyTaskSpecification>();
        private ConcurrentBag<Task> _tasks = new ConcurrentBag<Task>();

        public ICommand CommandRunTaskInMainThread { get; private set; }
        public ICommand CommandRunEachTaskInSeperateThread { get; private set; }

        /// <summary>
        /// Do not use time consuming action in the thread associated with the target window. 
        /// </summary>
        public MainViewModel()
        {
            _taskSpecifications = new ObservableCollection<MyTaskSpecification>();
            _taskSpecifications.Add(new MyTaskSpecification(new CancellationTokenSource()) { TaskId = 0, ThreadId = 0, Progress = 0 });

            CommandRunTaskInMainThread = new DelegateCommand(() =>
            {
                MyTaskSpecification task = new MyTaskSpecification(new CancellationTokenSource());
                task.TaskId = Task.CurrentId.GetValueOrDefault();
                task.ThreadId = Thread.CurrentThread.ManagedThreadId;
                _taskSpecifications.Add(task);
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
                    var cancellationTokenSource = new CancellationTokenSource();
                    CancellationToken cancellationToken = cancellationTokenSource.Token;

                    Task task = Task.Factory.StartNew(() =>
                    {
                        // Were we already canceled?
                        cancellationToken.ThrowIfCancellationRequested();

                        MyTaskSpecification taskSpecification = new MyTaskSpecification(cancellationTokenSource);
                        taskSpecification.TaskId = Task.CurrentId.Value;
                        taskSpecification.ThreadId = Thread.CurrentThread.ManagedThreadId;
                        System.Windows.Application.Current.Dispatcher.Invoke(() => TaskSpecifications.Add(taskSpecification));

                        for (int stage = 0; stage < 10000; stage++)
                        {
                            // Poll on this property if you have to do
                            // other cleanup before throwing.
                            if (cancellationToken.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                // cancellationToken.ThrowIfCancellationRequested();
                                break;
                            }

                            taskSpecification.Progress = stage;
                            // todo your own code
                            Random random = new Random();
                            Thread.Sleep(random.Next(50));
                        }
                    }, cancellationToken);

                    /*
                    // Just continue on this thread, or Wait/WaitAll with try-catch:
                    try
                    {
                        Task.WaitAll(Tasks.ToArray());
                    }
                    catch (AggregateException e)
                    {
                        foreach (var v in e.InnerExceptions)
                            Console.WriteLine(e.Message + " " + v.Message);
                    }
                    finally
                    {
                        cancellationTokenSource.Dispose();
                    }
                    */ 
                }
            });
        }

        public ObservableCollection<MyTaskSpecification> TaskSpecifications
        {
            get { return _taskSpecifications; }
        }

        public ConcurrentBag<Task> Tasks
        {
            get { return _tasks; }
        }
    }
}
