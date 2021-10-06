using System;

namespace WazeCredit.Services.LifetimeExample
{
    public class SingletonService
    {
        private readonly Guid _guid;

        public SingletonService()
        {
            this._guid = Guid.NewGuid();
        }

        public string GetGuid() => this._guid.ToString();
    }
}
