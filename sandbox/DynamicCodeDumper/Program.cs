﻿using MessagePack;
using MessagePack.Resolvers;
using SharedData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DynamicCodeDumper
{
    public class Program
    {
        static void Main(string[] args)
        {
            DynamicObjectResolver.Instance.GetFormatter<ArrayOptimizeClass>();
            DynamicObjectResolver.Instance.GetFormatter<Empty1>();
            DynamicObjectResolver.Instance.GetFormatter<Empty2>();
            DynamicObjectResolver.Instance.GetFormatter<NonEmpty1>();
            DynamicObjectResolver.Instance.GetFormatter<NonEmpty2>();
            DynamicObjectResolver.Instance.GetFormatter<FirstSimpleData>();
            DynamicObjectResolver.Instance.GetFormatter<Version0>();
            DynamicObjectResolver.Instance.GetFormatter<Version1>();
            DynamicObjectResolver.Instance.GetFormatter<Version2>();
            DynamicObjectResolver.Instance.GetFormatter<SimpleIntKeyData>();
            DynamicObjectResolver.Instance.GetFormatter<SimlpeStringKeyData>();
            DynamicObjectResolver.Instance.GetFormatter<Callback1>();
            DynamicObjectResolver.Instance.GetFormatter<Callback1_2>();
            DynamicObjectResolver.Instance.GetFormatter<Callback2>();
            DynamicObjectResolver.Instance.GetFormatter<Callback2_2>();

            DynamicUnionResolver.Instance.GetFormatter<IHogeMoge>();
            DynamicUnionResolver.Instance.GetFormatter<IUnionChecker>();
            DynamicUnionResolver.Instance.GetFormatter<IUnionChecker2>();

            DynamicUnionResolver.Instance.GetFormatter<RootUnionType>();

            DynamicEnumResolver.Instance.GetFormatter<IntEnum>();
            DynamicEnumResolver.Instance.GetFormatter<ShortEnum>();

            DynamicContractlessObjectResolver.Instance.GetFormatter<ContractlessConstructorCheck>();
            DynamicContractlessObjectResolver.Instance.GetFormatter<Contractless2>();

            DynamicContractlessObjectResolver.Instance.GetFormatter<IEntity>();

            var a1 = DynamicObjectResolver.Instance.Save();
            var a2 = DynamicUnionResolver.Instance.Save();
            var a3 = DynamicEnumResolver.Instance.Save();
            var a4 = DynamicContractlessObjectResolver.Instance.Save();

            Verify(a1, a2, a3, a4);
        }

        static void Verify(params AssemblyBuilder[] builders)
        {
            var path = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\PEVerify.exe";

            foreach (var targetDll in builders)
            {
                var psi = new ProcessStartInfo(path, targetDll.GetName().Name + ".dll")
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };

                var p = Process.Start(psi);
                var data = p.StandardOutput.ReadToEnd();
                Console.WriteLine(data);
            }
        }
    }

    [Union(0, typeof(HogeMoge1))]
    [Union(1, typeof(HogeMoge2))]
    public interface IHogeMoge
    {
    }

    public class HogeMoge1 : IHogeMoge
    {
    }

    public class HogeMoge2 : IHogeMoge
    {
    }


    public class EmptyContractless
    {

    }


    public class Contractless2
    {
        public int MyProperty { get; set; }
        public string MyProperty2 { get; set; }
    }


    public interface IEntity
    {
        string Name { get; }
    }

    public class Event : IEntity
    {
        public Event(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public class Holder
    {
        public Holder(IEntity entity)
        {
            Entity = entity;
        }

        public IEntity Entity { get; }
    }
}
