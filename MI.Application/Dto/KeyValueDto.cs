using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class KeyValueDto<K,V>
    {

        public KeyValueDto()
        {
           
        }
        public K Key { get; set; }
        public V Value { get; set; }

    }
}
