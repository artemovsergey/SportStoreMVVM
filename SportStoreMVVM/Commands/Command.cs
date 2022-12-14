using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SportStoreMVVM.Commands
{
    public abstract class Command : ICommand
    {

        /*
         CanExecuteChanged уведомляет любые источники команд
         (такие как Button или CheckBox), привязанные к этому ICommand,
         об изменении значения, возвращаемого CanExecute.
         Источники команд заботятся об этом, потому что обычно им необходимо
         соответствующим образом обновлять свой статус (например, кнопка отключится, если CanExecute() вернет false).
         */

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // возможно ли выполнить команду?
        public abstract bool CanExecute(object parameter);

        // логика команды
        public abstract void Execute(object parameter);

    }
}
