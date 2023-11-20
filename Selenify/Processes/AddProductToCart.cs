using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Selenify.Models.Process;

namespace Selenify.Processes
{
    public class AddProductToCart : Process<AddProductToCart.ProcessState>
    {
        public class ProcessState { }
        public AddProductToCart() : base("Add Product To Cart") { }
        public override void Run() { }
    }
}
