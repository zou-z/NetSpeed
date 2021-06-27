using System;
using System.Windows.Input;

namespace NetSpeed.Util
{
    internal class RelayCommand : NotifyBase, ICommand
    {
        private readonly Action _execute;

        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute) : this(execute, null) { }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = new Action(execute);

            if (canExecute != null)
            {
                _canExecute = new Func<bool>(canExecute);
            }
        }

        private EventHandler _requerySuggestedLocal;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                {
                    EventHandler handler2;
                    EventHandler canExecuteChanged = _requerySuggestedLocal;

                    do
                    {
                        handler2 = canExecuteChanged;
                        EventHandler handler3 = (EventHandler)Delegate.Combine(handler2, value);
                        canExecuteChanged = System.Threading.Interlocked.CompareExchange(ref _requerySuggestedLocal, handler3, handler2);
                    }
                    while (canExecuteChanged != handler2);

                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (_canExecute != null)
                {
                    EventHandler handler2;
                    EventHandler canExecuteChanged = _requerySuggestedLocal;

                    do
                    {
                        handler2 = canExecuteChanged;
                        EventHandler handler3 = (EventHandler)Delegate.Remove(handler2, value);
                        canExecuteChanged = System.Threading.Interlocked.CompareExchange(ref _requerySuggestedLocal, handler3, handler2);
                    }
                    while (canExecuteChanged != handler2);

                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute.Invoke();
        }

        public virtual void Execute(object parameter)
        {
            if (CanExecute(parameter) && _execute != null)
            {
                _execute.Invoke();
            }
        }
    }

    internal class RelayCommand<T> : NotifyBase, ICommand
    {
        private readonly Action<T> _execute;

        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute) : this(execute, null) { }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = new Action<T>(execute);

            if (canExecute != null)
            {
                _canExecute = new Func<T, bool>(canExecute);
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            if (parameter == null)
            {
                if (typeof(T).IsValueType)
                {
                    return _canExecute.Invoke(default);
                }
                if (parameter is T t)
                {
                    return _canExecute.Invoke(t);
                }
            }
            return false;
        }

        public virtual void Execute(object parameter)
        {
            object val = parameter;

            if (parameter != null && parameter.GetType() != typeof(T))
            {
                if (parameter is IConvertible)
                {
                    val = Convert.ChangeType(parameter, typeof(T), null);
                }
            }

            if (CanExecute(val) && _execute != null)
            {
                if (val == null)
                {

                    if (typeof(T).IsValueType)
                    {
                        _execute.Invoke(default);
                    }
                    else
                    {
                        _execute.Invoke((T)val);
                    }
                }
                else
                {
                    _execute.Invoke((T)val);
                }
            }
        }
    }
}
