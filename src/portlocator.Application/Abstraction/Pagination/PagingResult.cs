using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Abstraction.Pagination
{
    public class PagingResult<T>
    {
        public int Count { get; set; }
        public int CurrentPage { get; set; }
        public List<T> Data { get; set; }
    }
}
