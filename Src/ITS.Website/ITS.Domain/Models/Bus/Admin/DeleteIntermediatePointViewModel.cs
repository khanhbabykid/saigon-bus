using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITS.Domain.Models.Bus.Admin
{
    public class DeleteIntermediatePointViewModel
    {
        public Guid IntermediatePointID { get; set; }
        public Guid MovementID { get; set; }
    }
}
