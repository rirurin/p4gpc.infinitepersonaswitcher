using Reloaded.Memory.Sources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Reloaded.Memory.Sources;
using Reloaded.Hooks.Definitions;

namespace p4gpc.infinitepersonaswitcher
{
    class SwitchStart
    {
        private IReloadedHooks _hooks;

        private Utils _utils;
        private int _baseAddress;
        private IMemory _memory;
        private Switcher _switcher;

        public SwitchStart(Utils utils)
        {
            _memory = new Memory();
            _utils = utils;
            using var thisProcess = Process.GetCurrentProcess();
            _baseAddress = thisProcess.MainModule.BaseAddress.ToInt32();
            _utils.Log("Initialising Switcher");
            _switcher = new Switcher(_utils, _memory);
        }
    }
}
