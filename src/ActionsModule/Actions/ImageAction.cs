using OpenCvSharp;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Prism.Events;
using ActionsModule.Events;

namespace ActionsModule.Actions
{
    public abstract class ImageAction : BindableBase
    {
        public Func<Mat, Mat> Action
        {
            get; set;
        }

        private bool hasChanged = false;
        public bool HasChanged
        {
            get { return hasChanged; }
            set
            {
                if (base.SetProperty(ref hasChanged, value) && value)
                {
                    EventAggregator.GetEvent<ImageActionChangedEvent>().Publish(this);
                }
            }
        }

        private bool isEditMode = false;
        public bool IsEditMode
        {
            get { return isEditMode; }
            set
            {
                base.SetProperty(ref isEditMode, value);
            }
        }

        private bool isEnabled = true;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                this.SetProperty(ref isEnabled, value);
            }
        }
        
        public string Name { get; protected set; }
        public IEventAggregator EventAggregator { get; internal set; }
        
        protected void SetPropertyNoChange<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            base.SetProperty(ref storage, value, propertyName);
        }

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            HasChanged = base.SetProperty(ref storage, value, propertyName);
            return HasChanged;
        }

        private bool hasError = false;
        public bool HasError
        {
            get { return hasError; }
            set { base.SetProperty(ref hasError, value); }
        }

        private string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { base.SetProperty(ref errorMessage, value); }
        }

    }
}
