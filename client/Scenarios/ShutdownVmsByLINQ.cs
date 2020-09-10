﻿using azure_proto_compute;
using azure_proto_core;
using System;
using System.Linq;

namespace client
{
    class ShutdownVmsByLINQ : Scenario
    {
        public override void Execute()
        {
            var createMultipleVms = new CreateMultipleVms(Context);
            createMultipleVms.Execute();

            var resourceGroup = new ArmClient().ResourceGroup(Context.SubscriptionId, Context.RgName);

            resourceGroup.ListVirtualMachines().Select(vm =>
            {
                var parts = vm.Id.Name.Split('-');
                var n = Convert.ToInt32(parts[parts.Length - 2]);
                return (vm, n);
            })
                .Where(tuple => tuple.n % 2 == 0)
                .ToList()
                .ForEach(tuple =>
                {
                    Console.WriteLine($"Stopping {tuple.vm.Id.Name}");
                    tuple.vm.Stop();
                    Console.WriteLine($"Starting {tuple.vm.Id.Name}");
                    tuple.vm.Start();
                });
        }
    }
}
