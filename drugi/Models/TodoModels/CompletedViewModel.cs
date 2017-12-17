using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drugi.Models.TodoModels
{
    public class CompletedViewModel
    {
        private readonly List<TodoViewModel> _list;

        public CompletedViewModel(List<TodoViewModel> list)
        {
            _list = list;
        }

        public List<TodoViewModel> GetList()
        {
            return _list;
        }
    }
}
