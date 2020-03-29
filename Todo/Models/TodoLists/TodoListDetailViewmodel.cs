using System.Collections.Generic;
using System.Linq;
using Todo.Models.TodoItems;

namespace Todo.Models.TodoLists
{
    public class TodoListDetailViewmodel
    {
        public int TodoListId { get; }
        public string Title { get; }

        public bool HideDone { get; set; }

        private TodoItemOrder _order;

        public TodoItemOrder Order
        {
            get => _order;
            set
            {
                _order = value;
                switch (Order)
                {
                    case TodoItemOrder.Importance:
                        _items = _items.OrderBy(i => i.Importance);
                        break;
                    case TodoItemOrder.Rank:
                        _items = _items.OrderByDescending(i => i.Rank);
                        break;
                    default:
                        _items = _items.OrderBy(i => i.Importance);
                        break;
                }
            }
        }

        private IEnumerable<TodoItemSummaryViewmodel> _items;

        public IEnumerable<TodoItemSummaryViewmodel> Items => _items.Where(i => !i.IsDone || !HideDone);

        public TodoListDetailViewmodel(
            int todoListId,
            string title,
            IEnumerable<TodoItemSummaryViewmodel> items)
        {
            _items = items;
            TodoListId = todoListId;
            Title = title;
            HideDone = false; //hard-coded
            Order = TodoItemOrder.Importance; //hard-coded
        }
    }
}
