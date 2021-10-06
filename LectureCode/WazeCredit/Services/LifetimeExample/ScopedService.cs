using System;

namespace WazeCredit.Services.LifetimeExample
{
    public class ScopedService
    {
        private readonly Guid _guid;

        public ScopedService()
        {
            this._guid = Guid.NewGuid();
        }

        public string GetGuid() => this._guid.ToString();
    }
}
