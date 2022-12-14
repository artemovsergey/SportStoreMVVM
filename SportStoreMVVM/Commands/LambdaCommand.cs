using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportStoreMVVM.Commands
{
    public class LambdaCommand : Command
    {

        // если поля помечены readonly, то они будут работать быстрее
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        // в конструкторе надо получить два делегата Action и Func
        public LambdaCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            _execute = Execute;
            _canExecute = CanExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute.Invoke(parameter);
        }
        public override void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
