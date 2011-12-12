using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITS.Domain.Models.Bus.Admin
{
    public class DeleteBusMovementViewModel
    {
        public Guid BusMovementID { get; set; }
        public Guid RouteID { get; set; }
    }
}
