using System;
using System.Collections.Generic;
using System.Text;

using Urho;

namespace Server
{
    internal class ServerApp : Application
    {
        public ServerApp(ApplicationOptions options = null) : base(options)
        {
        }

        protected override void Start()
        {
            base.Start();
         
            Update += App_Update;
        }

        private void App_Update(UpdateEventArgs obj)
        {
        }

        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);
        }
    }
}
