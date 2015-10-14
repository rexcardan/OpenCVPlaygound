using ActionsModule.Events;
using ActionsModule.Actions;
using OpenCvSharp;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionsModule.ViewModels
{
    public class ActionsViewModel : BindableBase
    {
        private readonly IEventAggregator _ea;

        public ActionsViewModel(IEventAggregator ea)
        {
            _ea = ea;

            _ea.GetEvent<ImageActionChangedEvent>().Subscribe((ia) =>
            {
                _ea.GetEvent<OperateOnImageEvent>().Publish(this.CurrentActions);
            });

            AvailableActions = new ObservableCollection<ImageAction>();
            AvailableActions.Add(new ColorCVTAction());
            AvailableActions.Add(new ContrastAction());
            AvailableActions.Add(new FlipAction());
            AvailableActions.Add(new ThresholdAction());
            AvailableActions.Add(new AdaptiveThresholdAction());
            AvailableActions.Add(new BlurAction());
            AvailableActions.Add(new CannyAction());
            AvailableActions.Add(new ErodeAction());
            AvailableActions.Add(new DilateAction());
            AvailableActions.Add(new FindAndDrawContours());

            CurrentActions = new ObservableCollection<ImageAction>();

            AddCommand = new DelegateCommand(() =>
            {
                if (SelectedAction != null)
                {
                    var type = SelectedAction.GetType();
                    var dup = (ImageAction)Activator.CreateInstance(type);
                    dup.EventAggregator = _ea;
                    dup.IsEditMode = true;
                    dup.HasChanged = true;
                    CurrentActions.Add(dup);
                    _ea.GetEvent<OperateOnImageEvent>().Publish(this.CurrentActions);
                }
            });

            DeleteCommand = new DelegateCommand<ImageAction>((id) =>
            {
                CurrentActions.Remove(id);
                _ea.GetEvent<OperateOnImageEvent>().Publish(this.CurrentActions);
            });

            MoveUpCommand = new DelegateCommand<ImageAction>((id) =>
            {
                var idx = CurrentActions.IndexOf(id);
                if (idx > 0)
                {
                    CurrentActions.Move(idx, idx - 1);
                }
                _ea.GetEvent<OperateOnImageEvent>().Publish(this.CurrentActions);
            });

            MoveDownCommand = new DelegateCommand<ImageAction>((id) =>
            {
                var idx = CurrentActions.IndexOf(id);
                if (idx < CurrentActions.Count - 1)
                {
                    CurrentActions.Move(idx, idx + 1);
                }
                _ea.GetEvent<OperateOnImageEvent>().Publish(this.CurrentActions);
            });

            EditCommand = new DelegateCommand<ImageAction>((id) =>
            {
                id.IsEditMode = !id.IsEditMode;
            });
        }

        public ObservableCollection<ImageAction> CurrentActions { get; set; }
        public ObservableCollection<ImageAction> AvailableActions { get; set; }
        public ImageAction SelectedAction { get; set; }

        public DelegateCommand<ImageAction> DeleteCommand { get; set; }
        public DelegateCommand<ImageAction> EditCommand { get; set; }
        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand<ImageAction> ApplyCommand { get; private set; }
        public DelegateCommand<ImageAction> MoveUpCommand { get; private set; }
        public DelegateCommand<ImageAction> MoveDownCommand { get; private set; }
    }
}
