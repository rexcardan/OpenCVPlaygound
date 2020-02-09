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
using System.Windows.Input;
using System.Reflection;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using ActionsModule.Attributes;
using Newtonsoft.Json.Linq;
using MenuModule.Events;

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
            _ea.GetEvent<OpenImageEvent>().Subscribe((ia) =>
            {
                _ea.GetEvent<OperateOnImageEvent>().Publish(this.CurrentActions);
            });

            var availableActions = typeof(ImageAction).Assembly
                                                      .GetTypes()
                                                      .Where(t => typeof(ImageAction).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                                                      .Select(Activator.CreateInstance)
                                                      .Cast<ImageAction>();

            AvailableActions = new ObservableCollection<ImageAction>(availableActions);
            CurrentActions = new ObservableCollection<ImageAction>();

            AddCommand = new DelegateCommand(() =>
            {
                if (SelectedAction != null)
                {
                    var type = SelectedAction.GetType();
                    this.AddAction(type);
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

            ExportCommand = new DelegateCommand(this.OnExport);
            ImportCommand = new DelegateCommand(this.OnImport);
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

        public ICommand ExportCommand { get; }
        public ICommand ImportCommand { get; }
        
        private void OnExport()
        {
            var data = this.CurrentActions.Select(action => new { Type = action.GetType().FullName, Options = this.GetActionProperties(action) });
            var sfd = new SaveFileDialog();
            sfd.Filter = "Action stack|*.asj";
            if (sfd.ShowDialog() == true)
            {
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(sfd.FileName, json);
            }
        }

        private void OnImport()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Action stack|*.asj";
            if (ofd.ShowDialog() == true)
            {
                var json = File.ReadAllText(ofd.FileName);
                dynamic data = JArray.Parse(json);

                this.CurrentActions.Clear();
                foreach (var actionData in data)
                {
                    var actionType = Type.GetType((string)actionData.Type);
                    this.AddAction(actionType, action => this.RestoreProperties(action, actionData.Options));
                }
            }
        }

        private void RestoreProperties(ImageAction action, JObject actionData)
        {
            foreach (var prop in this.GetImportExportProperties(action))
            {
                if (actionData.ContainsKey(prop.Name))
                {
                    prop.SetValue(action, Convert.ChangeType(actionData[prop.Name], prop.PropertyType));
                }
            }
        }

        private Dictionary<string, object> GetActionProperties(ImageAction action)
        {
            return this.GetImportExportProperties(action)
                       .Select(p => new { p.Name, Value = p.GetValue(action) })
                       .ToDictionary(i => i.Name, i => i.Value);
        }

        private IEnumerable<PropertyInfo> GetImportExportProperties(ImageAction action)
        {
            return action.GetType()
                         .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                         .Where(prop => prop.GetCustomAttribute<ImportExportAttribute>() != null);
        }

        private void AddAction(Type type, Action<ImageAction> initializer = null)
        {
            var action = (ImageAction)Activator.CreateInstance(type);
            action.EventAggregator = _ea;
            action.IsEditMode = true;
            action.HasChanged = true;
            initializer?.Invoke(action);
            CurrentActions.Add(action);
            _ea.GetEvent<OperateOnImageEvent>().Publish(this.CurrentActions);
        }
    }
}
