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
        public Config _config { get; set; }
        public int instantSwitch1, instantSwitch2, instantSwitchOriginal1, instantSwitchOriginal2;

        public Switcher(Utils utils, IMemory memory, Config configuration)
        {
            _utils = utils;
            _memory = memory;
            _config = configuration;
            using var thisProcess = Process.GetCurrentProcess();
            _baseAddress = thisProcess.MainModule.BaseAddress.ToInt32();
            using var scanner = new Scanner(thisProcess, thisProcess.MainModule);
            // scan for infinite switching address
            int switcherAddress = scanner.CompiledFindPattern("F7 46 1C 00 04 00 00 74 AF").Offset + _baseAddress + 7;
            // scan for points to skip persona switching animation
            instantSwitch1 = scanner.CompiledFindPattern("0F B7 7B 78 BA 0C 00 00 00 8B 73 38").Offset + _baseAddress;
            instantSwitch2 = scanner.CompiledFindPattern("A1 80 16 AF 00 8B 53 38 6A 00 6A 00").Offset + _baseAddress;
            // read and save original memory values for instant switch toggle
            _memory.SafeRead((IntPtr)instantSwitch1, out instantSwitchOriginal1);
            _memory.SafeRead((IntPtr)instantSwitch2, out instantSwitchOriginal2);
            // rewrite memory values
            _memory.SafeWrite((IntPtr)switcherAddress, 0xAF70);
        }
        public void ToggleInstantSwitcher()
        {
            try
            {
                _utils.Log($"Toggling Instant Persona switch");
                if (_config.InstantSwitch)
                {
                    _memory.SafeWrite((IntPtr)instantSwitch1, 0x900000011FE9);
                    _memory.SafeWrite((IntPtr)instantSwitch2, 0x000000AAE9);
                } else
                {
                    _memory.SafeWrite((IntPtr)instantSwitch1, 0x0CBA787BB70F);
                    _memory.SafeWrite((IntPtr)instantSwitch2, instantSwitchOriginal2);
                }
            } catch (Exception e)
            {
                _utils.LogError($"Unable to toggle instant persona switcher", e);
            }
        }
    }
}
