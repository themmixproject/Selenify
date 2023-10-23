using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Models {
    internal class AutomationProcess {
        private Action InitialMethod;

        public AutomationProcess(Action initialMethod) {
            InitialMethod = initialMethod;
        }

        public void Run() {
            InitialMethod();
        }
    }
}
