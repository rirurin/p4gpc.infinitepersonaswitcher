using Reloaded.Memory.Sources;
using System;
using System.Collections.Generic;
using System.Text;
using Reloaded.Memory.Sigscan;
using System.Diagnostics;
using System.Runtime.InteropServices;
using p4gpc.infinitepersonaswitcher.Configuration;
using Reloaded.Hooks.Definitions;

namespace p4gpc.infinitepersonaswitcher
{
    class Switcher
    {
        private IMemory _memory;
        private int _baseAddress;
        private Utils _utils;

        public Switcher(Utils utils, IMemory memory)
        {
            _utils = utils;
            _memory = memory;
            using var thisProcess = Process.GetCurrentProcess();
            _baseAddress = thisProcess.MainModule.BaseAddress.ToInt32();
            using var scanner = new Scanner(thisProcess, thisProcess.MainModule);
            int switcherAddress = scanner.CompiledFindPattern("F7 46 1C 00 04 00 00 74 AF").Offset + _baseAddress + 7;
            _memory.SafeWrite((IntPtr)switcherAddress, 0xAF70);
        }
    }
}
