using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storm.TechTask.SharedKernel.Entities;

namespace Storm.TechTask.Core.ProjectAggregate.Events
{
    
    public class NewItemAddedEvent : BaseDomainEvent
    {
        public ToDoItem NewItem { get; set; } = null!;
        public Project RelatedProject { get; set; } = null!;

        public NewItemAddedEvent(Project project,
            ToDoItem newItem)
        {
            RelatedProject = project;
            NewItem = newItem;
        }
    }
    
}
