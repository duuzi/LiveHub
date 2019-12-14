using Api.LiveHub.Domain.Models;
using System;

namespace Api.LiveHub.Dto
{
    public class ToDoDto
    {
        public long Id { get; set; }
        public ToDoType ToDoType { get; set; }
        public string ToDoName { get; set; }
        public bool Checked { get; set; }
        public ToDoStatus Status { get; set; }
    }
}
