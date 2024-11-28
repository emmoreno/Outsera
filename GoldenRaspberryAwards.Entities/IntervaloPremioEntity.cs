using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenRaspberryAwards.Entities
{
    public class IntervaloPremioEntity
    {
       
        public IntervaloPremioEntity()
        {
            Max = new List<ProdutorEntity>();
            Min = new List<ProdutorEntity>();
        }
        public List<ProdutorEntity> Max { get; set; }
        public List<ProdutorEntity> Min { get; set; }

    }
}
