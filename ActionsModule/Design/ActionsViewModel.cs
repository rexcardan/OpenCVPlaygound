using ActionsModule.Structures;
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
            AvailableActions = new ObservableCollection<ImageAction>();
            AvailableActions.Add(new ThresholdAction());
            SeletedAction = AvailableActions.First();

            CurrentActions = new ObservableCollection<ImageAction>();
            CurrentActions.Add(new ThresholdAction());
        }

        public ObservableCollection<ImageAction> AvailableActions { get; private set; }
        public ObservableCollection<ImageAction> CurrentActions { get; private set; }

        public ImageAction SeletedAction { get; set; }
    }
}
