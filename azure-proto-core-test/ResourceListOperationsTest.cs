using azure_proto_core;
using NUnit.Framework;
using System.Reflection;
using Azure.Identity;
using System;
using Azure.ResourceManager.Resources.Models;
using System.Collections.Generic;

namespace azure_proto_core_test
{
    public class ResourceListOperationsTest
    {
        [TestCase]
        public void TestArmResonseArmResource()
        {
            var testDic = new Dictionary<string, string> { { "tag1", "value1" } };
            String loc = "Japan East";
            var asArmOp = (ArmResourceOperations)TestListActivater<ArmResourceOperations, ArmResource>(testDic, loc);
            Assert.IsTrue(loc == asArmOp.DefaultLocation);
        }
        private static object TestListActivater<TOperation, TResource>(Dictionary<string, string> tags = null, string location = "East US")
        {
            var testMethod = typeof(azure_proto_core.ResourceListOperations).GetMethod("CreateResourceConverter", BindingFlags.Static | BindingFlags.NonPublic);
            var asGeneric = testMethod.MakeGenericMethod(new System.Type[] { typeof(TOperation), typeof(TResource) });
            var context = new ArmClientContext(new Uri("https://management.azure.com"), new DefaultAzureCredential());
            var function = (Func<GenericResourceExpanded, TOperation>)asGeneric.Invoke(null, new object[] { context, new ArmClientOptions() });
            var resource = new GenericResourceExpanded();
            resource.Location = location;
            resource.Tags = tags ?? new Dictionary<string, string>();
            return function.DynamicInvoke(new object[] { resource });
        }
    }
}