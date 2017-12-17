using System.Collections.Generic;

namespace drugi.Models.TodoModels
{
    public class IndexViewModel
    {
         private readonly List<TodoViewModel> _list;

         public IndexViewModel(List<TodoViewModel> list)
         {
             _list = list;
         }

         public List<TodoViewModel> GetList()
         {
             return _list;
         }
         
    }
}

