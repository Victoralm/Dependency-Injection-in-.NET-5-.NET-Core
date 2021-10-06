using System;

namespace WazeCredit.Services.LifetimeExample
{
    public class TransientService
    {
        private readonly Guid _guid;

        public TransientService()
        {
            this._guid = Guid.NewGuid();
        }

        public string GetGuid() => this._guid.ToString();
    }
}
