using ActionsModule.Actions;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionsModule.Events
{
    public class ImageActionChangedEvent : PubSubEvent<ImageAction>
    {
    }
}
