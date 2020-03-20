using ActionsModule.Actions;
using ActionsModule.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionsModule.Design
{
    public class ActionsViewModel
    {
        public ActionsViewModel()
        {
            Categories = new ObservableCollection<ActionCategory>();

            CurrentActions = new ObservableCollection<ImageAction>();
            CurrentActions.Add(new ThresholdAction());
        }

        public ObservableCollection<ActionCategory> Categories { get; set; }
        public ObservableCollection<ImageAction> CurrentActions { get; private set; }

        public ImageAction SeletedAction { get; set; }
    }
}
