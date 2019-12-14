using Api.LiveHub.Domain.Models;

namespace Api.LiveHub.ViewModel
{
    public class ToDoViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public ToDoStatus Status { get; set; }
        public bool Checked { get; set; }
        public int? Order { get; set; }
        public ToDoType ToDoType { get; set; }
    }
}
