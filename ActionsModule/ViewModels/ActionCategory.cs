using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionsModule.Actions;

namespace ActionsModule.ViewModels
{
    public class ActionCategory
    {
        public ActionCategory(string name, IEnumerable<ImageAction> actions)
        {
            this.Name = name;
            this.Actions = actions.ToArray();
        }

        public string Name { get; }

        public ImageAction[] Actions { get; }
    }
}
