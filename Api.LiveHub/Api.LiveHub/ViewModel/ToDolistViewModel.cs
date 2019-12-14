using System.Collections.Generic;

namespace Api.LiveHub.ViewModel
{
    public class ToDolistViewModel : BaseViewModel
    {
        public List<ToDoViewModel> doList { get; set; }
        public List<ToDoViewModel> thingsList { get; set; }
        public List<ToDoViewModel> trainingList { get; set; }
    }
}
